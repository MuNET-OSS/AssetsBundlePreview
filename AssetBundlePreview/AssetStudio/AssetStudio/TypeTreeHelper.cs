using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace AssetStudio;

public static class TypeTreeHelper
{
	public static string ReadTypeString(TypeTree m_Type, ObjectReader reader)
	{
		reader.Reset();
		StringBuilder sb = new StringBuilder();
		List<TypeTreeNode> m_Nodes = m_Type.m_Nodes;
		for (int i = 0; i < m_Nodes.Count; i++)
		{
			ReadStringValue(sb, m_Nodes, reader, ref i);
		}
		long readed = reader.Position - reader.byteStart;
		if (readed != reader.byteSize)
		{
			Logger.Info($"Error while read type, read {readed} bytes but expected {reader.byteSize} bytes");
		}
		return sb.ToString();
	}

	private static void ReadStringValue(StringBuilder sb, List<TypeTreeNode> m_Nodes, BinaryReader reader, ref int i)
	{
		TypeTreeNode typeTreeNode = m_Nodes[i];
		int level = typeTreeNode.m_Level;
		string varTypeStr = typeTreeNode.m_Type;
		string varNameStr = typeTreeNode.m_Name;
		object value = null;
		bool append = true;
		bool align = (typeTreeNode.m_MetaFlag & 0x4000) != 0;
		switch (varTypeStr)
		{
		case "SInt8":
			value = reader.ReadSByte();
			break;
		case "UInt8":
			value = reader.ReadByte();
			break;
		case "char":
			value = BitConverter.ToChar(reader.ReadBytes(2), 0);
			break;
		case "short":
		case "SInt16":
			value = reader.ReadInt16();
			break;
		case "UInt16":
		case "unsigned short":
			value = reader.ReadUInt16();
			break;
		case "int":
		case "SInt32":
			value = reader.ReadInt32();
			break;
		case "UInt32":
		case "unsigned int":
		case "Type*":
			value = reader.ReadUInt32();
			break;
		case "long long":
		case "SInt64":
			value = reader.ReadInt64();
			break;
		case "UInt64":
		case "unsigned long long":
		case "FileSize":
			value = reader.ReadUInt64();
			break;
		case "float":
			value = reader.ReadSingle();
			break;
		case "double":
			value = reader.ReadDouble();
			break;
		case "bool":
			value = reader.ReadBoolean();
			break;
		case "string":
		{
			append = false;
			string str = reader.ReadAlignedString();
			sb.AppendFormat("{0}{1} {2} = \"{3}\"\r\n", new string('\t', level), varTypeStr, varNameStr, str);
			List<TypeTreeNode> toSkip = GetNodes(m_Nodes, i);
			i += toSkip.Count - 1;
			break;
		}
		case "map":
		{
			if (((uint)m_Nodes[i + 1].m_MetaFlag & 0x4000u) != 0)
			{
				align = true;
			}
			append = false;
			sb.AppendFormat("{0}{1} {2}\r\n", new string('\t', level), varTypeStr, varNameStr);
			sb.AppendFormat("{0}{1} {2}\r\n", new string('\t', level + 1), "Array", "Array");
			int size3 = reader.ReadInt32();
			sb.AppendFormat("{0}{1} {2} = {3}\r\n", new string('\t', level + 1), "int", "size", size3);
			List<TypeTreeNode> map = GetNodes(m_Nodes, i);
			i += map.Count - 1;
			List<TypeTreeNode> first = GetNodes(map, 4);
			int next = 4 + first.Count;
			List<TypeTreeNode> second = GetNodes(map, next);
			for (int l = 0; l < size3; l++)
			{
				sb.AppendFormat("{0}[{1}]\r\n", new string('\t', level + 2), l);
				sb.AppendFormat("{0}{1} {2}\r\n", new string('\t', level + 2), "pair", "data");
				int tmp2 = 0;
				int tmp3 = 0;
				ReadStringValue(sb, first, reader, ref tmp2);
				ReadStringValue(sb, second, reader, ref tmp3);
			}
			break;
		}
		case "TypelessData":
		{
			append = false;
			int size2 = reader.ReadInt32();
			reader.ReadBytes(size2);
			i += 2;
			sb.AppendFormat("{0}{1} {2}\r\n", new string('\t', level), varTypeStr, varNameStr);
			sb.AppendFormat("{0}{1} {2} = {3}\r\n", new string('\t', level), "int", "size", size2);
			break;
		}
		default:
			if (i < m_Nodes.Count - 1 && m_Nodes[i + 1].m_Type == "Array")
			{
				if (((uint)m_Nodes[i + 1].m_MetaFlag & 0x4000u) != 0)
				{
					align = true;
				}
				append = false;
				sb.AppendFormat("{0}{1} {2}\r\n", new string('\t', level), varTypeStr, varNameStr);
				sb.AppendFormat("{0}{1} {2}\r\n", new string('\t', level + 1), "Array", "Array");
				int size = reader.ReadInt32();
				sb.AppendFormat("{0}{1} {2} = {3}\r\n", new string('\t', level + 1), "int", "size", size);
				List<TypeTreeNode> vector = GetNodes(m_Nodes, i);
				i += vector.Count - 1;
				for (int j = 0; j < size; j++)
				{
					sb.AppendFormat("{0}[{1}]\r\n", new string('\t', level + 2), j);
					int tmp = 3;
					ReadStringValue(sb, vector, reader, ref tmp);
				}
			}
			else
			{
				append = false;
				sb.AppendFormat("{0}{1} {2}\r\n", new string('\t', level), varTypeStr, varNameStr);
				List<TypeTreeNode> @class = GetNodes(m_Nodes, i);
				i += @class.Count - 1;
				for (int k = 1; k < @class.Count; k++)
				{
					ReadStringValue(sb, @class, reader, ref k);
				}
			}
			break;
		}
		if (append)
		{
			sb.AppendFormat("{0}{1} {2} = {3}\r\n", new string('\t', level), varTypeStr, varNameStr, value);
		}
		if (align)
		{
			reader.AlignStream();
		}
	}

	public static OrderedDictionary ReadType(TypeTree m_Types, ObjectReader reader)
	{
		reader.Reset();
		OrderedDictionary obj = new OrderedDictionary();
		List<TypeTreeNode> m_Nodes = m_Types.m_Nodes;
		for (int i = 1; i < m_Nodes.Count; i++)
		{
			string varNameStr = m_Nodes[i].m_Name;
			obj[varNameStr] = ReadValue(m_Nodes, reader, ref i);
		}
		long readed = reader.Position - reader.byteStart;
		if (readed != reader.byteSize)
		{
			Logger.Info($"Error while read type, read {readed} bytes but expected {reader.byteSize} bytes");
		}
		return obj;
	}

	private static object ReadValue(List<TypeTreeNode> m_Nodes, BinaryReader reader, ref int i)
	{
		TypeTreeNode typeTreeNode = m_Nodes[i];
		string varTypeStr = typeTreeNode.m_Type;
		bool align = (typeTreeNode.m_MetaFlag & 0x4000) != 0;
		object value;
		switch (varTypeStr)
		{
		case "SInt8":
			value = reader.ReadSByte();
			break;
		case "UInt8":
			value = reader.ReadByte();
			break;
		case "char":
			value = BitConverter.ToChar(reader.ReadBytes(2), 0);
			break;
		case "short":
		case "SInt16":
			value = reader.ReadInt16();
			break;
		case "UInt16":
		case "unsigned short":
			value = reader.ReadUInt16();
			break;
		case "int":
		case "SInt32":
			value = reader.ReadInt32();
			break;
		case "UInt32":
		case "unsigned int":
		case "Type*":
			value = reader.ReadUInt32();
			break;
		case "long long":
		case "SInt64":
			value = reader.ReadInt64();
			break;
		case "UInt64":
		case "unsigned long long":
		case "FileSize":
			value = reader.ReadUInt64();
			break;
		case "float":
			value = reader.ReadSingle();
			break;
		case "double":
			value = reader.ReadDouble();
			break;
		case "bool":
			value = reader.ReadBoolean();
			break;
		case "string":
		{
			value = reader.ReadAlignedString();
			List<TypeTreeNode> toSkip = GetNodes(m_Nodes, i);
			i += toSkip.Count - 1;
			break;
		}
		case "map":
		{
			if (((uint)m_Nodes[i + 1].m_MetaFlag & 0x4000u) != 0)
			{
				align = true;
			}
			List<TypeTreeNode> map = GetNodes(m_Nodes, i);
			i += map.Count - 1;
			List<TypeTreeNode> first = GetNodes(map, 4);
			int next = 4 + first.Count;
			List<TypeTreeNode> second = GetNodes(map, next);
			int size3 = reader.ReadInt32();
			List<KeyValuePair<object, object>> dic = new List<KeyValuePair<object, object>>(size3);
			for (int l = 0; l < size3; l++)
			{
				int tmp2 = 0;
				int tmp3 = 0;
				dic.Add(new KeyValuePair<object, object>(ReadValue(first, reader, ref tmp2), ReadValue(second, reader, ref tmp3)));
			}
			value = dic;
			break;
		}
		case "TypelessData":
		{
			int size2 = reader.ReadInt32();
			value = reader.ReadBytes(size2);
			i += 2;
			break;
		}
		default:
			if (i < m_Nodes.Count - 1 && m_Nodes[i + 1].m_Type == "Array")
			{
				if (((uint)m_Nodes[i + 1].m_MetaFlag & 0x4000u) != 0)
				{
					align = true;
				}
				List<TypeTreeNode> vector = GetNodes(m_Nodes, i);
				i += vector.Count - 1;
				int size = reader.ReadInt32();
				List<object> list = new List<object>(size);
				for (int j = 0; j < size; j++)
				{
					int tmp = 3;
					list.Add(ReadValue(vector, reader, ref tmp));
				}
				value = list;
			}
			else
			{
				List<TypeTreeNode> @class = GetNodes(m_Nodes, i);
				i += @class.Count - 1;
				OrderedDictionary obj = new OrderedDictionary();
				for (int k = 1; k < @class.Count; k++)
				{
					string name = @class[k].m_Name;
					obj[name] = ReadValue(@class, reader, ref k);
				}
				value = obj;
			}
			break;
		}
		if (align)
		{
			reader.AlignStream();
		}
		return value;
	}

	private static List<TypeTreeNode> GetNodes(List<TypeTreeNode> m_Nodes, int index)
	{
		List<TypeTreeNode> nodes = new List<TypeTreeNode>();
		nodes.Add(m_Nodes[index]);
		int level = m_Nodes[index].m_Level;
		for (int i = index + 1; i < m_Nodes.Count; i++)
		{
			TypeTreeNode member = m_Nodes[i];
			if (member.m_Level <= level)
			{
				return nodes;
			}
			nodes.Add(member);
		}
		return nodes;
	}
}
