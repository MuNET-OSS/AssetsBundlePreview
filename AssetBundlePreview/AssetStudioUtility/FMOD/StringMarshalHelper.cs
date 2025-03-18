using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FMOD;

internal class StringMarshalHelper
{
	internal static void NativeToBuilder(StringBuilder builder, IntPtr nativeMem)
	{
		byte[] bytes = new byte[builder.Capacity];
		Marshal.Copy(nativeMem, bytes, 0, builder.Capacity);
		int strlen = Array.IndexOf(bytes, (byte)0);
		if (strlen > 0)
		{
			string str = Encoding.UTF8.GetString(bytes, 0, strlen);
			builder.Append(str);
		}
	}
}
