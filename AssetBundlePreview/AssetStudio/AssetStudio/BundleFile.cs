using System.IO;
using System.Linq;
using K4os.Compression.LZ4;

namespace AssetStudio;

public class BundleFile
{
	public class Header
	{
		public string signature;

		public uint version;

		public string unityVersion;

		public string unityRevision;

		public long size;

		public uint compressedBlocksInfoSize;

		public uint uncompressedBlocksInfoSize;

		public ArchiveFlags flags;
	}

	public class StorageBlock
	{
		public uint compressedSize;

		public uint uncompressedSize;

		public StorageBlockFlags flags;
	}

	public class Node
	{
		public long offset;

		public long size;

		public uint flags;

		public string path;
	}

	public Header m_Header;

	private StorageBlock[] m_BlocksInfo;

	private Node[] m_DirectoryInfo;

	public StreamFile[] fileList;

	public BundleFile(FileReader reader)
	{
		m_Header = new Header();
		m_Header.signature = reader.ReadStringToNull();
		m_Header.version = reader.ReadUInt32();
		m_Header.unityVersion = reader.ReadStringToNull();
		m_Header.unityRevision = reader.ReadStringToNull();
		switch (m_Header.signature)
		{
		default:
			return;
		case "UnityWeb":
		case "UnityRaw":
			if (m_Header.version != 6)
			{
				ReadHeaderAndBlocksInfo(reader);
				using Stream blocksStream2 = CreateBlocksStream(reader.FullPath);
				ReadBlocksAndDirectory(reader, blocksStream2);
				ReadFiles(blocksStream2, reader.FullPath);
				return;
			}
			break;
		case "UnityFS":
			break;
		}
		ReadHeader(reader);
		ReadBlocksInfoAndDirectory(reader);
		using Stream blocksStream = CreateBlocksStream(reader.FullPath);
		ReadBlocks(reader, blocksStream);
		ReadFiles(blocksStream, reader.FullPath);
	}

	private void ReadHeaderAndBlocksInfo(EndianBinaryReader reader)
	{
		if (m_Header.version >= 4)
		{
			reader.ReadBytes(16);
			reader.ReadUInt32();
		}
		reader.ReadUInt32();
		m_Header.size = reader.ReadUInt32();
		reader.ReadUInt32();
		int levelCount = reader.ReadInt32();
		m_BlocksInfo = new StorageBlock[1];
		for (int i = 0; i < levelCount; i++)
		{
			StorageBlock storageBlock = new StorageBlock
			{
				compressedSize = reader.ReadUInt32(),
				uncompressedSize = reader.ReadUInt32()
			};
			if (i == levelCount - 1)
			{
				m_BlocksInfo[0] = storageBlock;
			}
		}
		if (m_Header.version >= 2)
		{
			reader.ReadUInt32();
		}
		if (m_Header.version >= 3)
		{
			reader.ReadUInt32();
		}
		reader.Position = m_Header.size;
	}

	private Stream CreateBlocksStream(string path)
	{
		long uncompressedSizeSum = m_BlocksInfo.Sum((StorageBlock x) => x.uncompressedSize);
		if (uncompressedSizeSum >= int.MaxValue)
		{
			return new FileStream(path + ".temp", FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose);
		}
		return new MemoryStream((int)uncompressedSizeSum);
	}

	private void ReadBlocksAndDirectory(EndianBinaryReader reader, Stream blocksStream)
	{
		bool isCompressed = m_Header.signature == "UnityWeb";
		StorageBlock[] blocksInfo = m_BlocksInfo;
		foreach (StorageBlock blockInfo in blocksInfo)
		{
			byte[] uncompressedBytes = reader.ReadBytes((int)blockInfo.compressedSize);
			if (isCompressed)
			{
				using MemoryStream memoryStream = new MemoryStream(uncompressedBytes);
				using MemoryStream decompressStream = SevenZipHelper.StreamDecompress(memoryStream);
				uncompressedBytes = decompressStream.ToArray();
			}
			blocksStream.Write(uncompressedBytes, 0, uncompressedBytes.Length);
		}
		blocksStream.Position = 0L;
		EndianBinaryReader blocksReader = new EndianBinaryReader(blocksStream);
		int nodesCount = blocksReader.ReadInt32();
		m_DirectoryInfo = new Node[nodesCount];
		for (int i = 0; i < nodesCount; i++)
		{
			m_DirectoryInfo[i] = new Node
			{
				path = blocksReader.ReadStringToNull(),
				offset = blocksReader.ReadUInt32(),
				size = blocksReader.ReadUInt32()
			};
		}
	}

	public void ReadFiles(Stream blocksStream, string path)
	{
		fileList = new StreamFile[m_DirectoryInfo.Length];
		for (int i = 0; i < m_DirectoryInfo.Length; i++)
		{
			Node node = m_DirectoryInfo[i];
			StreamFile file = new StreamFile();
			fileList[i] = file;
			file.path = node.path;
			file.fileName = Path.GetFileName(node.path);
			if (node.size >= int.MaxValue)
			{
				string extractPath = path + "_unpacked" + Path.DirectorySeparatorChar;
				Directory.CreateDirectory(extractPath);
				file.stream = new FileStream(extractPath + file.fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
			}
			else
			{
				file.stream = new MemoryStream((int)node.size);
			}
			blocksStream.Position = node.offset;
			blocksStream.CopyTo(file.stream, node.size);
			file.stream.Position = 0L;
		}
	}

	private void ReadHeader(EndianBinaryReader reader)
	{
		m_Header.size = reader.ReadInt64();
		m_Header.compressedBlocksInfoSize = reader.ReadUInt32();
		m_Header.uncompressedBlocksInfoSize = reader.ReadUInt32();
		m_Header.flags = (ArchiveFlags)reader.ReadUInt32();
		if (m_Header.signature != "UnityFS")
		{
			reader.ReadByte();
		}
	}

	private void ReadBlocksInfoAndDirectory(EndianBinaryReader reader)
	{
		if (m_Header.version >= 7)
		{
			reader.AlignStream(16);
		}
		byte[] blocksInfoBytes;
		if ((m_Header.flags & ArchiveFlags.BlocksInfoAtTheEnd) != 0)
		{
			long position = reader.Position;
			reader.Position = reader.BaseStream.Length - m_Header.compressedBlocksInfoSize;
			blocksInfoBytes = reader.ReadBytes((int)m_Header.compressedBlocksInfoSize);
			reader.Position = position;
		}
		else
		{
			blocksInfoBytes = reader.ReadBytes((int)m_Header.compressedBlocksInfoSize);
		}
		uint uncompressedSize = m_Header.uncompressedBlocksInfoSize;
		CompressionType compressionType = (CompressionType)(m_Header.flags & ArchiveFlags.CompressionTypeMask);
		MemoryStream blocksInfoUncompresseddStream;
		switch (compressionType)
		{
		case CompressionType.None:
			blocksInfoUncompresseddStream = new MemoryStream(blocksInfoBytes);
			break;
		case CompressionType.Lzma:
		{
			blocksInfoUncompresseddStream = new MemoryStream((int)uncompressedSize);
			using (MemoryStream blocksInfoCompressedStream = new MemoryStream(blocksInfoBytes))
			{
				SevenZipHelper.StreamDecompress(blocksInfoCompressedStream, blocksInfoUncompresseddStream, m_Header.compressedBlocksInfoSize, m_Header.uncompressedBlocksInfoSize);
			}
			blocksInfoUncompresseddStream.Position = 0L;
			break;
		}
		case CompressionType.Lz4:
		case CompressionType.Lz4HC:
		{
			byte[] uncompressedBytes = new byte[uncompressedSize];
			int numWrite = LZ4Codec.Decode(blocksInfoBytes, uncompressedBytes);
			if (numWrite != uncompressedSize)
			{
				throw new IOException($"Lz4 decompression error, write {numWrite} bytes but expected {uncompressedSize} bytes");
			}
			blocksInfoUncompresseddStream = new MemoryStream(uncompressedBytes);
			break;
		}
		default:
			throw new IOException($"Unsupported compression type {compressionType}");
		}
		using (EndianBinaryReader blocksInfoReader = new EndianBinaryReader(blocksInfoUncompresseddStream))
		{
			blocksInfoReader.ReadBytes(16);
			int blocksInfoCount = blocksInfoReader.ReadInt32();
			m_BlocksInfo = new StorageBlock[blocksInfoCount];
			for (int j = 0; j < blocksInfoCount; j++)
			{
				m_BlocksInfo[j] = new StorageBlock
				{
					uncompressedSize = blocksInfoReader.ReadUInt32(),
					compressedSize = blocksInfoReader.ReadUInt32(),
					flags = (StorageBlockFlags)blocksInfoReader.ReadUInt16()
				};
			}
			int nodesCount = blocksInfoReader.ReadInt32();
			m_DirectoryInfo = new Node[nodesCount];
			for (int i = 0; i < nodesCount; i++)
			{
				m_DirectoryInfo[i] = new Node
				{
					offset = blocksInfoReader.ReadInt64(),
					size = blocksInfoReader.ReadInt64(),
					flags = blocksInfoReader.ReadUInt32(),
					path = blocksInfoReader.ReadStringToNull()
				};
			}
		}
		if ((m_Header.flags & ArchiveFlags.BlockInfoNeedPaddingAtStart) != 0)
		{
			reader.AlignStream(16);
		}
	}

	private void ReadBlocks(EndianBinaryReader reader, Stream blocksStream)
	{
		StorageBlock[] blocksInfo = m_BlocksInfo;
		foreach (StorageBlock blockInfo in blocksInfo)
		{
			CompressionType compressionType = (CompressionType)(blockInfo.flags & StorageBlockFlags.CompressionTypeMask);
			switch (compressionType)
			{
			case CompressionType.None:
				reader.BaseStream.CopyTo(blocksStream, blockInfo.compressedSize);
				break;
			case CompressionType.Lzma:
				SevenZipHelper.StreamDecompress(reader.BaseStream, blocksStream, blockInfo.compressedSize, blockInfo.uncompressedSize);
				break;
			case CompressionType.Lz4:
			case CompressionType.Lz4HC:
			{
				int compressedSize = (int)blockInfo.compressedSize;
				byte[] compressedBytes = BigArrayPool<byte>.Shared.Rent(compressedSize);
				reader.Read(compressedBytes, 0, compressedSize);
				int uncompressedSize = (int)blockInfo.uncompressedSize;
				byte[] uncompressedBytes = BigArrayPool<byte>.Shared.Rent(uncompressedSize);
				int numWrite = LZ4Codec.Decode(compressedBytes, 0, compressedSize, uncompressedBytes, 0, uncompressedSize);
				if (numWrite != uncompressedSize)
				{
					throw new IOException($"Lz4 decompression error, write {numWrite} bytes but expected {uncompressedSize} bytes");
				}
				blocksStream.Write(uncompressedBytes, 0, uncompressedSize);
				BigArrayPool<byte>.Shared.Return(compressedBytes);
				BigArrayPool<byte>.Shared.Return(uncompressedBytes);
				break;
			}
			default:
				throw new IOException($"Unsupported compression type {compressionType}");
			}
		}
		blocksStream.Position = 0L;
	}
}
