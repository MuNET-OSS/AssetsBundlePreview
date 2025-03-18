using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace AssetStudio.PInvoke;

public class Utf8StringHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	private static readonly UTF8Encoding Utf8;

	static Utf8StringHandle()
	{
		Utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
	}

	public unsafe Utf8StringHandle(string str)
		: base(ownsHandle: true)
	{
		IntPtr buffer;
		if (str == null)
		{
			buffer = IntPtr.Zero;
		}
		else if (str.Length == 0)
		{
			buffer = Marshal.AllocHGlobal(1);
			*(sbyte*)(void*)buffer = 0;
		}
		else
		{
			byte[] strBuffer = new byte[Utf8.GetByteCount(str) + 1];
			Utf8.GetBytes(str, 0, str.Length, strBuffer, 0);
			buffer = Marshal.AllocHGlobal(strBuffer.Length);
			Marshal.Copy(strBuffer, 0, buffer, strBuffer.Length);
		}
		SetHandle(buffer);
	}

	public unsafe static string ReadUtf8StringFromPointer(IntPtr lpstr)
	{
		if (lpstr == IntPtr.Zero || lpstr == new IntPtr(-1))
		{
			return null;
		}
		int byteCount = 0;
		for (byte* p = (byte*)lpstr.ToPointer(); *p != 0; p++)
		{
			byteCount++;
		}
		if (byteCount == 0)
		{
			return string.Empty;
		}
		byte[] strBuffer = new byte[byteCount];
		Marshal.Copy(lpstr, strBuffer, 0, byteCount);
		return Utf8.GetString(strBuffer);
	}

	protected override bool ReleaseHandle()
	{
		if (!IsInvalid)
		{
			Marshal.FreeHGlobal(handle);
		}
		return true;
	}
}
