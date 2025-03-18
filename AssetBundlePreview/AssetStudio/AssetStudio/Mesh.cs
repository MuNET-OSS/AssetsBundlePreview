using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AssetStudio;

public sealed class Mesh : NamedObject
{
	private bool m_Use16BitIndices = true;

	public SubMesh[] m_SubMeshes;

	private uint[] m_IndexBuffer;

	public BlendShapeData m_Shapes;

	public Matrix4x4[] m_BindPose;

	public uint[] m_BoneNameHashes;

	public int m_VertexCount;

	public float[] m_Vertices;

	public BoneWeights4[] m_Skin;

	public float[] m_Normals;

	public float[] m_Colors;

	public float[] m_UV0;

	public float[] m_UV1;

	public float[] m_UV2;

	public float[] m_UV3;

	public float[] m_UV4;

	public float[] m_UV5;

	public float[] m_UV6;

	public float[] m_UV7;

	public float[] m_Tangents;

	private VertexData m_VertexData;

	private CompressedMesh m_CompressedMesh;

	private StreamingInfo m_StreamData;

	public List<uint> m_Indices = new List<uint>();

	public Mesh(ObjectReader reader)
		: base(reader)
	{
		if (version[0] < 3 || (version[0] == 3 && version[1] < 5))
		{
			m_Use16BitIndices = reader.ReadInt32() > 0;
		}
		if (version[0] == 2 && version[1] <= 5)
		{
			int m_IndexBuffer_size2 = reader.ReadInt32();
			if (m_Use16BitIndices)
			{
				m_IndexBuffer = new uint[m_IndexBuffer_size2 / 2];
				for (int l = 0; l < m_IndexBuffer_size2 / 2; l++)
				{
					m_IndexBuffer[l] = reader.ReadUInt16();
				}
				reader.AlignStream();
			}
			else
			{
				m_IndexBuffer = reader.ReadUInt32Array(m_IndexBuffer_size2 / 4);
			}
		}
		int m_SubMeshesSize = reader.ReadInt32();
		m_SubMeshes = new SubMesh[m_SubMeshesSize];
		for (int k = 0; k < m_SubMeshesSize; k++)
		{
			m_SubMeshes[k] = new SubMesh(reader);
		}
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 1))
		{
			m_Shapes = new BlendShapeData(reader);
		}
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 3))
		{
			m_BindPose = reader.ReadMatrixArray();
			m_BoneNameHashes = reader.ReadUInt32Array();
			reader.ReadUInt32();
		}
		if (version[0] > 2 || (version[0] == 2 && version[1] >= 6))
		{
			if (version[0] >= 2019)
			{
				int m_BonesAABBSize = reader.ReadInt32();
				MinMaxAABB[] m_BonesAABB = new MinMaxAABB[m_BonesAABBSize];
				for (int j = 0; j < m_BonesAABBSize; j++)
				{
					m_BonesAABB[j] = new MinMaxAABB(reader);
				}
				reader.ReadUInt32Array();
			}
			byte m_MeshCompression = reader.ReadByte();
			if (version[0] >= 4)
			{
				if (version[0] < 5)
				{
					reader.ReadByte();
				}
				reader.ReadBoolean();
				reader.ReadBoolean();
				reader.ReadBoolean();
			}
			reader.AlignStream();
			if (version[0] > 2017 || (version[0] == 2017 && version[1] >= 4) || (version[0] == 2017 && version[1] == 3 && version[2] == 1 && buildType.IsPatch) || (version[0] == 2017 && version[1] == 3 && m_MeshCompression == 0))
			{
				int m_IndexFormat = reader.ReadInt32();
				m_Use16BitIndices = m_IndexFormat == 0;
			}
			int m_IndexBuffer_size = reader.ReadInt32();
			if (m_Use16BitIndices)
			{
				m_IndexBuffer = new uint[m_IndexBuffer_size / 2];
				for (int i = 0; i < m_IndexBuffer_size / 2; i++)
				{
					m_IndexBuffer[i] = reader.ReadUInt16();
				}
				reader.AlignStream();
			}
			else
			{
				m_IndexBuffer = reader.ReadUInt32Array(m_IndexBuffer_size / 4);
			}
		}
		if (version[0] < 3 || (version[0] == 3 && version[1] < 5))
		{
			m_VertexCount = reader.ReadInt32();
			m_Vertices = reader.ReadSingleArray(m_VertexCount * 3);
			m_Skin = new BoneWeights4[reader.ReadInt32()];
			for (int s = 0; s < m_Skin.Length; s++)
			{
				m_Skin[s] = new BoneWeights4(reader);
			}
			m_BindPose = reader.ReadMatrixArray();
			m_UV0 = reader.ReadSingleArray(reader.ReadInt32() * 2);
			m_UV1 = reader.ReadSingleArray(reader.ReadInt32() * 2);
			if (version[0] == 2 && version[1] <= 5)
			{
				int m_TangentSpace_size = reader.ReadInt32();
				m_Normals = new float[m_TangentSpace_size * 3];
				m_Tangents = new float[m_TangentSpace_size * 4];
				for (int v2 = 0; v2 < m_TangentSpace_size; v2++)
				{
					m_Normals[v2 * 3] = reader.ReadSingle();
					m_Normals[v2 * 3 + 1] = reader.ReadSingle();
					m_Normals[v2 * 3 + 2] = reader.ReadSingle();
					m_Tangents[v2 * 3] = reader.ReadSingle();
					m_Tangents[v2 * 3 + 1] = reader.ReadSingle();
					m_Tangents[v2 * 3 + 2] = reader.ReadSingle();
					m_Tangents[v2 * 3 + 3] = reader.ReadSingle();
				}
			}
			else
			{
				m_Tangents = reader.ReadSingleArray(reader.ReadInt32() * 4);
				m_Normals = reader.ReadSingleArray(reader.ReadInt32() * 3);
			}
		}
		else
		{
			if (version[0] < 2018 || (version[0] == 2018 && version[1] < 2))
			{
				m_Skin = new BoneWeights4[reader.ReadInt32()];
				for (int s2 = 0; s2 < m_Skin.Length; s2++)
				{
					m_Skin[s2] = new BoneWeights4(reader);
				}
			}
			if (version[0] == 3 || (version[0] == 4 && version[1] <= 2))
			{
				m_BindPose = reader.ReadMatrixArray();
			}
			m_VertexData = new VertexData(reader);
		}
		if (version[0] > 2 || (version[0] == 2 && version[1] >= 6))
		{
			m_CompressedMesh = new CompressedMesh(reader);
		}
		reader.Position += 24L;
		if (version[0] < 3 || (version[0] == 3 && version[1] <= 4))
		{
			int m_Colors_size = reader.ReadInt32();
			m_Colors = new float[m_Colors_size * 4];
			for (int v = 0; v < m_Colors_size * 4; v++)
			{
				m_Colors[v] = (float)(int)reader.ReadByte() / 255f;
			}
			int m_CollisionTriangles_size = reader.ReadInt32();
			reader.Position += m_CollisionTriangles_size * 4;
			reader.ReadInt32();
		}
		reader.ReadInt32();
		if (version[0] > 2022 || (version[0] == 2022 && version[1] >= 1))
		{
			reader.ReadInt32();
		}
		if (version[0] >= 5)
		{
			reader.ReadUInt8Array();
			reader.AlignStream();
			reader.ReadUInt8Array();
			reader.AlignStream();
		}
		if (version[0] > 2018 || (version[0] == 2018 && version[1] >= 2))
		{
			_ = new float[2]
			{
				reader.ReadSingle(),
				reader.ReadSingle()
			};
		}
		if (version[0] > 2018 || (version[0] == 2018 && version[1] >= 3))
		{
			reader.AlignStream();
			m_StreamData = new StreamingInfo(reader);
		}
		ProcessData();
	}

	private void ProcessData()
	{
		if (!string.IsNullOrEmpty(m_StreamData?.path) && m_VertexData.m_VertexCount != 0)
		{
			ResourceReader resourceReader = new ResourceReader(m_StreamData.path, assetsFile, m_StreamData.offset, m_StreamData.size);
			m_VertexData.m_DataSize = resourceReader.GetData();
		}
		if (version[0] > 3 || (version[0] == 3 && version[1] >= 5))
		{
			ReadVertexData();
		}
		if (version[0] > 2 || (version[0] == 2 && version[1] >= 6))
		{
			DecompressCompressedMesh();
		}
		GetTriangles();
	}

	private void ReadVertexData()
	{
		m_VertexCount = (int)m_VertexData.m_VertexCount;
		for (int chn = 0; chn < m_VertexData.m_Channels.Length; chn++)
		{
			ChannelInfo m_Channel = m_VertexData.m_Channels[chn];
			if (m_Channel.dimension <= 0)
			{
				continue;
			}
			StreamInfo m_Stream = m_VertexData.m_Streams[m_Channel.stream];
			if (!new BitArray(new int[1] { (int)m_Stream.channelMask }).Get(chn))
			{
				continue;
			}
			if (version[0] < 2018 && chn == 2 && m_Channel.format == 2)
			{
				m_Channel.dimension = 4;
			}
			MeshHelper.VertexFormat vertexFormat = MeshHelper.ToVertexFormat(m_Channel.format, version);
			int componentByteSize = (int)MeshHelper.GetFormatSize(vertexFormat);
			byte[] componentBytes = new byte[m_VertexCount * m_Channel.dimension * componentByteSize];
			for (int v = 0; v < m_VertexCount; v++)
			{
				int vertexOffset = (int)(m_Stream.offset + m_Channel.offset) + (int)m_Stream.stride * v;
				for (int d = 0; d < m_Channel.dimension; d++)
				{
					int componentOffset = vertexOffset + componentByteSize * d;
					Buffer.BlockCopy(m_VertexData.m_DataSize, componentOffset, componentBytes, componentByteSize * (v * m_Channel.dimension + d), componentByteSize);
				}
			}
			if (reader.Endian == EndianType.BigEndian && componentByteSize > 1)
			{
				for (int k = 0; k < componentBytes.Length / componentByteSize; k++)
				{
					byte[] buff = new byte[componentByteSize];
					Buffer.BlockCopy(componentBytes, k * componentByteSize, buff, 0, componentByteSize);
					buff = buff.Reverse().ToArray();
					Buffer.BlockCopy(buff, 0, componentBytes, k * componentByteSize, componentByteSize);
				}
			}
			int[] componentsIntArray = null;
			float[] componentsFloatArray = null;
			if (MeshHelper.IsIntFormat(vertexFormat))
			{
				componentsIntArray = MeshHelper.BytesToIntArray(componentBytes, vertexFormat);
			}
			else
			{
				componentsFloatArray = MeshHelper.BytesToFloatArray(componentBytes, vertexFormat);
			}
			if (version[0] >= 2018)
			{
				switch (chn)
				{
				case 0:
					m_Vertices = componentsFloatArray;
					break;
				case 1:
					m_Normals = componentsFloatArray;
					break;
				case 2:
					m_Tangents = componentsFloatArray;
					break;
				case 3:
					m_Colors = componentsFloatArray;
					break;
				case 4:
					m_UV0 = componentsFloatArray;
					break;
				case 5:
					m_UV1 = componentsFloatArray;
					break;
				case 6:
					m_UV2 = componentsFloatArray;
					break;
				case 7:
					m_UV3 = componentsFloatArray;
					break;
				case 8:
					m_UV4 = componentsFloatArray;
					break;
				case 9:
					m_UV5 = componentsFloatArray;
					break;
				case 10:
					m_UV6 = componentsFloatArray;
					break;
				case 11:
					m_UV7 = componentsFloatArray;
					break;
				case 12:
				{
					if (m_Skin == null)
					{
						InitMSkin();
					}
					for (int j = 0; j < m_VertexCount; j++)
					{
						for (int m = 0; m < m_Channel.dimension; m++)
						{
							m_Skin[j].weight[m] = componentsFloatArray[j * m_Channel.dimension + m];
						}
					}
					break;
				}
				case 13:
				{
					if (m_Skin == null)
					{
						InitMSkin();
					}
					for (int i = 0; i < m_VertexCount; i++)
					{
						for (int l = 0; l < m_Channel.dimension; l++)
						{
							m_Skin[i].boneIndex[l] = componentsIntArray[i * m_Channel.dimension + l];
						}
					}
					break;
				}
				}
				continue;
			}
			switch (chn)
			{
			case 0:
				m_Vertices = componentsFloatArray;
				break;
			case 1:
				m_Normals = componentsFloatArray;
				break;
			case 2:
				m_Colors = componentsFloatArray;
				break;
			case 3:
				m_UV0 = componentsFloatArray;
				break;
			case 4:
				m_UV1 = componentsFloatArray;
				break;
			case 5:
				if (version[0] >= 5)
				{
					m_UV2 = componentsFloatArray;
				}
				else
				{
					m_Tangents = componentsFloatArray;
				}
				break;
			case 6:
				m_UV3 = componentsFloatArray;
				break;
			case 7:
				m_Tangents = componentsFloatArray;
				break;
			}
		}
	}

	private void DecompressCompressedMesh()
	{
		if (m_CompressedMesh.m_Vertices.m_NumItems != 0)
		{
			m_VertexCount = (int)m_CompressedMesh.m_Vertices.m_NumItems / 3;
			m_Vertices = m_CompressedMesh.m_Vertices.UnpackFloats(3, 12);
		}
		if (m_CompressedMesh.m_UV.m_NumItems != 0)
		{
			uint m_UVInfo = m_CompressedMesh.m_UVInfo;
			if (m_UVInfo != 0)
			{
				int uvSrcOffset = 0;
				for (int uv = 0; uv < 8; uv++)
				{
					uint texCoordBits = m_UVInfo >> uv * 4;
					texCoordBits &= 0xFu;
					if ((texCoordBits & 4u) != 0)
					{
						int uvDim = (int)(1 + (texCoordBits & 3));
						float[] m_UV = m_CompressedMesh.m_UV.UnpackFloats(uvDim, uvDim * 4, uvSrcOffset, m_VertexCount);
						SetUV(uv, m_UV);
						uvSrcOffset += uvDim * m_VertexCount;
					}
				}
			}
			else
			{
				m_UV0 = m_CompressedMesh.m_UV.UnpackFloats(2, 8, 0, m_VertexCount);
				if (m_CompressedMesh.m_UV.m_NumItems >= m_VertexCount * 4)
				{
					m_UV1 = m_CompressedMesh.m_UV.UnpackFloats(2, 8, m_VertexCount * 2, m_VertexCount);
				}
			}
		}
		if (version[0] < 5 && m_CompressedMesh.m_BindPoses.m_NumItems != 0)
		{
			m_BindPose = new Matrix4x4[m_CompressedMesh.m_BindPoses.m_NumItems / 16];
			float[] m_BindPoses_Unpacked = m_CompressedMesh.m_BindPoses.UnpackFloats(16, 64);
			float[] buffer = new float[16];
			for (int l = 0; l < m_BindPose.Length; l++)
			{
				Array.Copy(m_BindPoses_Unpacked, l * 16, buffer, 0, 16);
				m_BindPose[l] = new Matrix4x4(buffer);
			}
		}
		if (m_CompressedMesh.m_Normals.m_NumItems != 0)
		{
			float[] normalData = m_CompressedMesh.m_Normals.UnpackFloats(2, 8);
			int[] signs2 = m_CompressedMesh.m_NormalSigns.UnpackInts();
			m_Normals = new float[m_CompressedMesh.m_Normals.m_NumItems / 2 * 3];
			for (int k = 0; k < m_CompressedMesh.m_Normals.m_NumItems / 2; k++)
			{
				float x3 = normalData[k * 2];
				float y2 = normalData[k * 2 + 1];
				float zsqr2 = 1f - x3 * x3 - y2 * y2;
				float z2;
				if (zsqr2 >= 0f)
				{
					z2 = (float)Math.Sqrt(zsqr2);
				}
				else
				{
					z2 = 0f;
					Vector3 normal = new Vector3(x3, y2, z2);
					normal.Normalize();
					x3 = normal.X;
					y2 = normal.Y;
					z2 = normal.Z;
				}
				if (signs2[k] == 0)
				{
					z2 = 0f - z2;
				}
				m_Normals[k * 3] = x3;
				m_Normals[k * 3 + 1] = y2;
				m_Normals[k * 3 + 2] = z2;
			}
		}
		if (m_CompressedMesh.m_Tangents.m_NumItems != 0)
		{
			float[] tangentData = m_CompressedMesh.m_Tangents.UnpackFloats(2, 8);
			int[] signs = m_CompressedMesh.m_TangentSigns.UnpackInts();
			m_Tangents = new float[m_CompressedMesh.m_Tangents.m_NumItems / 2 * 4];
			for (int j = 0; j < m_CompressedMesh.m_Tangents.m_NumItems / 2; j++)
			{
				float x2 = tangentData[j * 2];
				float y = tangentData[j * 2 + 1];
				float zsqr = 1f - x2 * x2 - y * y;
				float z;
				if (zsqr >= 0f)
				{
					z = (float)Math.Sqrt(zsqr);
				}
				else
				{
					z = 0f;
					Vector3 vector3f = new Vector3(x2, y, z);
					vector3f.Normalize();
					x2 = vector3f.X;
					y = vector3f.Y;
					z = vector3f.Z;
				}
				if (signs[j * 2] == 0)
				{
					z = 0f - z;
				}
				float w = ((signs[j * 2 + 1] > 0) ? 1f : (-1f));
				m_Tangents[j * 4] = x2;
				m_Tangents[j * 4 + 1] = y;
				m_Tangents[j * 4 + 2] = z;
				m_Tangents[j * 4 + 3] = w;
			}
		}
		if (version[0] >= 5 && m_CompressedMesh.m_FloatColors.m_NumItems != 0)
		{
			m_Colors = m_CompressedMesh.m_FloatColors.UnpackFloats(1, 4);
		}
		if (m_CompressedMesh.m_Weights.m_NumItems != 0)
		{
			int[] weights = m_CompressedMesh.m_Weights.UnpackInts();
			int[] boneIndices = m_CompressedMesh.m_BoneIndices.UnpackInts();
			InitMSkin();
			int bonePos = 0;
			int boneIndexPos = 0;
			int m = 0;
			int sum = 0;
			for (int i = 0; i < m_CompressedMesh.m_Weights.m_NumItems; i++)
			{
				m_Skin[bonePos].weight[m] = (float)weights[i] / 31f;
				m_Skin[bonePos].boneIndex[m] = boneIndices[boneIndexPos++];
				m++;
				sum += weights[i];
				if (sum >= 31)
				{
					for (; m < 4; m++)
					{
						m_Skin[bonePos].weight[m] = 0f;
						m_Skin[bonePos].boneIndex[m] = 0;
					}
					bonePos++;
					m = 0;
					sum = 0;
				}
				else if (m == 3)
				{
					m_Skin[bonePos].weight[m] = (float)(31 - sum) / 31f;
					m_Skin[bonePos].boneIndex[m] = boneIndices[boneIndexPos++];
					bonePos++;
					m = 0;
					sum = 0;
				}
			}
		}
		if (m_CompressedMesh.m_Triangles.m_NumItems != 0)
		{
			m_IndexBuffer = Array.ConvertAll(m_CompressedMesh.m_Triangles.UnpackInts(), (int x) => (uint)x);
		}
		PackedIntVector colors = m_CompressedMesh.m_Colors;
		if (colors != null && colors.m_NumItems != 0)
		{
			m_CompressedMesh.m_Colors.m_NumItems *= 4u;
			m_CompressedMesh.m_Colors.m_BitSize /= 4;
			int[] tempColors = m_CompressedMesh.m_Colors.UnpackInts();
			m_Colors = new float[m_CompressedMesh.m_Colors.m_NumItems];
			for (int v = 0; v < m_CompressedMesh.m_Colors.m_NumItems; v++)
			{
				m_Colors[v] = (float)tempColors[v] / 255f;
			}
		}
	}

	private void GetTriangles()
	{
		SubMesh[] subMeshes = m_SubMeshes;
		foreach (SubMesh m_SubMesh in subMeshes)
		{
			uint firstIndex = m_SubMesh.firstByte / 2;
			if (!m_Use16BitIndices)
			{
				firstIndex /= 2;
			}
			uint indexCount = m_SubMesh.indexCount;
			GfxPrimitiveType topology = m_SubMesh.topology;
			if (topology == GfxPrimitiveType.Triangles)
			{
				for (int i = 0; i < indexCount; i += 3)
				{
					m_Indices.Add(m_IndexBuffer[firstIndex + i]);
					m_Indices.Add(m_IndexBuffer[firstIndex + i + 1]);
					m_Indices.Add(m_IndexBuffer[firstIndex + i + 2]);
				}
				continue;
			}
			if (version[0] >= 4)
			{
				switch (topology)
				{
				case GfxPrimitiveType.TriangleStrip:
					break;
				case GfxPrimitiveType.Quads:
				{
					for (int q = 0; q < indexCount; q += 4)
					{
						m_Indices.Add(m_IndexBuffer[firstIndex + q]);
						m_Indices.Add(m_IndexBuffer[firstIndex + q + 1]);
						m_Indices.Add(m_IndexBuffer[firstIndex + q + 2]);
						m_Indices.Add(m_IndexBuffer[firstIndex + q]);
						m_Indices.Add(m_IndexBuffer[firstIndex + q + 2]);
						m_Indices.Add(m_IndexBuffer[firstIndex + q + 3]);
					}
					m_SubMesh.indexCount = indexCount / 2 * 3;
					continue;
				}
				default:
					throw new NotSupportedException("Failed getting triangles. Submesh topology is lines or points.");
				}
			}
			uint triIndex = 0u;
			for (int j = 0; j < indexCount - 2; j++)
			{
				uint a = m_IndexBuffer[firstIndex + j];
				uint b = m_IndexBuffer[firstIndex + j + 1];
				uint c = m_IndexBuffer[firstIndex + j + 2];
				if (a != b && a != c && b != c)
				{
					if ((j & 1) == 1)
					{
						m_Indices.Add(b);
						m_Indices.Add(a);
					}
					else
					{
						m_Indices.Add(a);
						m_Indices.Add(b);
					}
					m_Indices.Add(c);
					triIndex += 3;
				}
			}
			m_SubMesh.indexCount = triIndex;
		}
	}

	private void InitMSkin()
	{
		m_Skin = new BoneWeights4[m_VertexCount];
		for (int i = 0; i < m_VertexCount; i++)
		{
			m_Skin[i] = new BoneWeights4();
		}
	}

	private void SetUV(int uv, float[] m_UV)
	{
		switch (uv)
		{
		case 0:
			m_UV0 = m_UV;
			break;
		case 1:
			m_UV1 = m_UV;
			break;
		case 2:
			m_UV2 = m_UV;
			break;
		case 3:
			m_UV3 = m_UV;
			break;
		case 4:
			m_UV4 = m_UV;
			break;
		case 5:
			m_UV5 = m_UV;
			break;
		case 6:
			m_UV6 = m_UV;
			break;
		case 7:
			m_UV7 = m_UV;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public float[] GetUV(int uv)
	{
		return uv switch
		{
			0 => m_UV0, 
			1 => m_UV1, 
			2 => m_UV2, 
			3 => m_UV3, 
			4 => m_UV4, 
			5 => m_UV5, 
			6 => m_UV6, 
			7 => m_UV7, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}
}
