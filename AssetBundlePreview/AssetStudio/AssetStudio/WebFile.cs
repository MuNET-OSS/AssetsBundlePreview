using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AssetStudio;

public class WebFile
{
	private class WebData
	{
		public int dataOffset;

		public int dataLength;

		public string path;
	}

	public StreamFile[] fileList;

	public WebFile(EndianBinaryReader reader)
	{
		reader.Endian = EndianType.LittleEndian;
		reader.ReadStringToNull();
		int headLength = reader.ReadInt32();
		List<WebData> dataList = new List<WebData>();
		while (reader.BaseStream.Position < headLength)
		{
			WebData data2 = new WebData
			{
				dataOffset = reader.ReadInt32(),
				dataLength = reader.ReadInt32()
			};
			int pathLength = reader.ReadInt32();
			data2.path = Encoding.UTF8.GetString(reader.ReadBytes(pathLength));
			dataList.Add(data2);
		}
		fileList = new StreamFile[dataList.Count];
		for (int i = 0; i < dataList.Count; i++)
		{
			WebData data = dataList[i];
			StreamFile file = new StreamFile
			{
				path = data.path,
				fileName = Path.GetFileName(data.path)
			};
			reader.BaseStream.Position = data.dataOffset;
			file.stream = new MemoryStream(reader.ReadBytes(data.dataLength));
			fileList[i] = file;
		}
	}
}
