using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AssetStudio.PInvoke;

public static class DllLoader
{
	private static class Win32
	{
		private const uint LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 4096u;

		private const uint LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 256u;

		internal static void LoadDll(string dllDir, string dllName)
		{
			string dllFileName = dllName + ".dll";
			if (LoadLibraryEx(Path.Combine(dllDir, dllFileName), IntPtr.Zero, 4352u) == IntPtr.Zero)
			{
				Win32Exception exception = new Win32Exception(Marshal.GetLastWin32Error());
				throw new DllNotFoundException(exception.Message, exception);
			}
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr LoadLibraryEx(string lpLibFileName, IntPtr hFile, uint dwFlags);
	}

	private static class Posix
	{
		private const int RTLD_LAZY = 1;

		private const int RTLD_NOW = 2;

		private const int RTLD_GLOBAL = 256;

		internal static void LoadDll(string dllDir, string dllName)
		{
			string dllExtension;
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				dllExtension = ".so";
			}
			else
			{
				if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					throw new NotSupportedException();
				}
				dllExtension = ".dylib";
			}
			string dllFileName = "lib" + dllName + dllExtension;
			if (DlOpen(Path.Combine(dllDir, dllFileName), 258) == IntPtr.Zero)
			{
				throw new DllNotFoundException(Marshal.PtrToStringAnsi(DlError()));
			}
		}

		[DllImport("libdl", EntryPoint = "dlopen")]
		private static extern IntPtr DlOpen([MarshalAs(UnmanagedType.LPStr)] string fileName, int flags);

		[DllImport("libdl", EntryPoint = "dlerror")]
		private static extern IntPtr DlError();
	}

	public static void PreloadDll(string dllName)
	{
		string dllDir = GetDirectedDllDirectory();
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			Win32.LoadDll(dllDir, dllName);
		}
		else
		{
			Posix.LoadDll(dllDir, dllName);
		}
	}

	private static string GetDirectedDllDirectory()
	{
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		return directoryName;
	}
}
