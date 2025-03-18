using System;
using System.Runtime.InteropServices;
using System.Text;
using FMOD;

namespace AssetStudio;

public class AudioClipConverter
{
	private AudioClip m_AudioClip;

	public bool IsSupport
	{
		get
		{
			if (m_AudioClip.version[0] < 5)
			{
				switch (m_AudioClip.m_Type)
				{
				case FMODSoundType.AIFF:
				case FMODSoundType.IT:
				case FMODSoundType.MOD:
				case FMODSoundType.S3M:
				case FMODSoundType.XM:
				case FMODSoundType.XMA:
				case FMODSoundType.AUDIOQUEUE:
					return true;
				default:
					return false;
				}
			}
			AudioCompressionFormat compressionFormat = m_AudioClip.m_CompressionFormat;
			if ((uint)compressionFormat <= 3u || compressionFormat == AudioCompressionFormat.XMA)
			{
				return true;
			}
			return false;
		}
	}

	public AudioClipConverter(AudioClip audioClip)
	{
		m_AudioClip = audioClip;
	}

	public byte[] ConvertToWav()
	{
		byte[] m_AudioData = m_AudioClip.m_AudioData.GetData();
		if (m_AudioData == null || m_AudioData.Length == 0)
		{
			return null;
		}
		CREATESOUNDEXINFO exinfo = default(CREATESOUNDEXINFO);
		if (Factory.System_Create(out var system) != 0)
		{
			return null;
		}
		if (system.init(1, INITFLAGS.NORMAL, IntPtr.Zero) != 0)
		{
			return null;
		}
		exinfo.cbsize = Marshal.SizeOf(exinfo);
		exinfo.length = (uint)m_AudioClip.m_Size;
		if (system.createSound(m_AudioData, MODE.OPENMEMORY, ref exinfo, out var sound) != 0)
		{
			return null;
		}
		if (sound.getNumSubSounds(out var numsubsounds) != 0)
		{
			return null;
		}
		byte[] buff;
		if (numsubsounds > 0)
		{
			if (sound.getSubSound(0, out var subsound) != 0)
			{
				return null;
			}
			buff = SoundToWav(subsound);
			subsound.release();
		}
		else
		{
			buff = SoundToWav(sound);
		}
		sound.release();
		system.release();
		return buff;
	}

	public byte[] SoundToWav(Sound sound)
	{
		if (sound.getFormat(out var _, out var _, out var channels, out var bits) != 0)
		{
			return null;
		}
		if (sound.getDefaults(out var frequency, out var _) != 0)
		{
			return null;
		}
		int sampleRate = (int)frequency;
		if (sound.getLength(out var length, TIMEUNIT.PCMBYTES) != 0)
		{
			return null;
		}
		if (sound.@lock(0u, length, out var ptr1, out var ptr2, out var len1, out var len2) != 0)
		{
			return null;
		}
		byte[] buffer = new byte[len1 + 44];
		Encoding.UTF8.GetBytes("RIFF").CopyTo(buffer, 0);
		BitConverter.GetBytes(len1 + 36).CopyTo(buffer, 4);
		Encoding.UTF8.GetBytes("WAVEfmt ").CopyTo(buffer, 8);
		BitConverter.GetBytes(16).CopyTo(buffer, 16);
		BitConverter.GetBytes((short)1).CopyTo(buffer, 20);
		BitConverter.GetBytes((short)channels).CopyTo(buffer, 22);
		BitConverter.GetBytes(sampleRate).CopyTo(buffer, 24);
		BitConverter.GetBytes(sampleRate * channels * bits / 8).CopyTo(buffer, 28);
		BitConverter.GetBytes((short)(channels * bits / 8)).CopyTo(buffer, 32);
		BitConverter.GetBytes((short)bits).CopyTo(buffer, 34);
		Encoding.UTF8.GetBytes("data").CopyTo(buffer, 36);
		BitConverter.GetBytes(len1).CopyTo(buffer, 40);
		Marshal.Copy(ptr1, buffer, 44, (int)len1);
		if (sound.unlock(ptr1, ptr2, len1, len2) != 0)
		{
			return null;
		}
		return buffer;
	}

	public string GetExtensionName()
	{
		if (m_AudioClip.version[0] < 5)
		{
			switch (m_AudioClip.m_Type)
			{
			case FMODSoundType.ACC:
				return ".m4a";
			case FMODSoundType.AIFF:
				return ".aif";
			case FMODSoundType.IT:
				return ".it";
			case FMODSoundType.MOD:
				return ".mod";
			case FMODSoundType.MPEG:
				return ".mp3";
			case FMODSoundType.OGGVORBIS:
				return ".ogg";
			case FMODSoundType.S3M:
				return ".s3m";
			case FMODSoundType.WAV:
				return ".wav";
			case FMODSoundType.XM:
				return ".xm";
			case FMODSoundType.XMA:
				return ".wav";
			case FMODSoundType.VAG:
				return ".vag";
			case FMODSoundType.AUDIOQUEUE:
				return ".fsb";
			}
		}
		else
		{
			switch (m_AudioClip.m_CompressionFormat)
			{
			case AudioCompressionFormat.PCM:
				return ".fsb";
			case AudioCompressionFormat.Vorbis:
				return ".fsb";
			case AudioCompressionFormat.ADPCM:
				return ".fsb";
			case AudioCompressionFormat.MP3:
				return ".fsb";
			case AudioCompressionFormat.PSMVAG:
				return ".fsb";
			case AudioCompressionFormat.HEVAG:
				return ".fsb";
			case AudioCompressionFormat.XMA:
				return ".fsb";
			case AudioCompressionFormat.AAC:
				return ".m4a";
			case AudioCompressionFormat.GCADPCM:
				return ".fsb";
			case AudioCompressionFormat.ATRAC9:
				return ".fsb";
			}
		}
		return ".AudioClip";
	}
}
