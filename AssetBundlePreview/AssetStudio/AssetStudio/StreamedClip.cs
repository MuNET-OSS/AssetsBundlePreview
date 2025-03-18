using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssetStudio;

public class StreamedClip
{
	public class StreamedCurveKey
	{
		public int index;

		public float[] coeff;

		public float value;

		public float outSlope;

		public float inSlope;

		public StreamedCurveKey(BinaryReader reader)
		{
			index = reader.ReadInt32();
			coeff = reader.ReadSingleArray(4);
			outSlope = coeff[2];
			value = coeff[3];
		}

		public float CalculateNextInSlope(float dx, StreamedCurveKey rhs)
		{
			if (coeff[0] == 0f && coeff[1] == 0f && coeff[2] == 0f)
			{
				return float.PositiveInfinity;
			}
			dx = Math.Max(dx, 0.0001f);
			float dy = rhs.value - value;
			float length = 1f / (dx * dx);
			float d1 = outSlope * dx;
			return (dy + dy + dy - d1 - d1 - coeff[1] / length) / dx;
		}
	}

	public class StreamedFrame
	{
		public float time;

		public StreamedCurveKey[] keyList;

		public StreamedFrame(BinaryReader reader)
		{
			time = reader.ReadSingle();
			int numKeys = reader.ReadInt32();
			keyList = new StreamedCurveKey[numKeys];
			for (int i = 0; i < numKeys; i++)
			{
				keyList[i] = new StreamedCurveKey(reader);
			}
		}
	}

	public uint[] data;

	public uint curveCount;

	public StreamedClip(ObjectReader reader)
	{
		data = reader.ReadUInt32Array();
		curveCount = reader.ReadUInt32();
	}

	public List<StreamedFrame> ReadData()
	{
		List<StreamedFrame> frameList = new List<StreamedFrame>();
		byte[] buffer = new byte[data.Length * 4];
		Buffer.BlockCopy(data, 0, buffer, 0, buffer.Length);
		using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
		{
			while (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				frameList.Add(new StreamedFrame(reader));
			}
		}
		for (int frameIndex = 2; frameIndex < frameList.Count - 1; frameIndex++)
		{
			StreamedFrame frame = frameList[frameIndex];
			StreamedCurveKey[] keyList = frame.keyList;
			foreach (StreamedCurveKey curveKey in keyList)
			{
				for (int i = frameIndex - 1; i >= 0; i--)
				{
					StreamedFrame preFrame = frameList[i];
					StreamedCurveKey preCurveKey = preFrame.keyList.FirstOrDefault((StreamedCurveKey x) => x.index == curveKey.index);
					if (preCurveKey != null)
					{
						curveKey.inSlope = preCurveKey.CalculateNextInSlope(frame.time - preFrame.time, curveKey);
						break;
					}
				}
			}
		}
		return frameList;
	}
}
