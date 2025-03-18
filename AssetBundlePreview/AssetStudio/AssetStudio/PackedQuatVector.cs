using System;

namespace AssetStudio;

public class PackedQuatVector
{
	public uint m_NumItems;

	public byte[] m_Data;

	public PackedQuatVector(ObjectReader reader)
	{
		m_NumItems = reader.ReadUInt32();
		int numData = reader.ReadInt32();
		m_Data = reader.ReadBytes(numData);
		reader.AlignStream();
	}

	public Quaternion[] UnpackQuats()
	{
		Quaternion[] data = new Quaternion[m_NumItems];
		int indexPos = 0;
		int bitPos = 0;
		for (int i = 0; i < m_NumItems; i++)
		{
			uint flags = 0u;
			int bits = 0;
			while (bits < 3)
			{
				flags |= (uint)(m_Data[indexPos] >> bitPos << bits);
				int num2 = Math.Min(3 - bits, 8 - bitPos);
				bitPos += num2;
				bits += num2;
				if (bitPos == 8)
				{
					indexPos++;
					bitPos = 0;
				}
			}
			flags &= 7u;
			Quaternion q = default(Quaternion);
			float sum = 0f;
			for (int j = 0; j < 4; j++)
			{
				if ((flags & 3) == j)
				{
					continue;
				}
				int bitSize = ((((flags & 3) + 1) % 4 == j) ? 9 : 10);
				uint x = 0u;
				bits = 0;
				while (bits < bitSize)
				{
					x |= (uint)(m_Data[indexPos] >> bitPos << bits);
					int num = Math.Min(bitSize - bits, 8 - bitPos);
					bitPos += num;
					bits += num;
					if (bitPos == 8)
					{
						indexPos++;
						bitPos = 0;
					}
				}
				x &= (uint)((1 << bitSize) - 1);
				q[j] = (float)x / (0.5f * (float)((1 << bitSize) - 1)) - 1f;
				sum += q[j] * q[j];
			}
			int lastComponent = (int)(flags & 3);
			q[lastComponent] = (float)Math.Sqrt(1f - sum);
			if ((flags & 4u) != 0)
			{
				q[lastComponent] = 0f - q[lastComponent];
			}
			data[i] = q;
		}
		return data;
	}
}
