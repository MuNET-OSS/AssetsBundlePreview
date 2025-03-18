using System.IO;
using System.Runtime.InteropServices;
using AssetStudio.FbxInterop;
using AssetStudio.PInvoke;

namespace AssetStudio;

public static class Fbx
{
	public static class Exporter
	{
		public static void Export(string path, IImported imported, bool eulerFilter, float filterPrecision, bool allNodes, bool skins, bool animation, bool blendShape, bool castToBone, float boneSize, bool exportAllUvsAsDiffuseMaps, float scaleFactor, int versionIndex, bool isAscii)
		{
			DirectoryInfo dir = new FileInfo(path).Directory;
			if (!dir.Exists)
			{
				dir.Create();
			}
			string currentDir = Directory.GetCurrentDirectory();
			Directory.SetCurrentDirectory(dir.FullName);
			using (FbxExporter exporter = new FbxExporter(Path.GetFileName(path), imported, allNodes, skins, castToBone, boneSize, exportAllUvsAsDiffuseMaps, scaleFactor, versionIndex, isAscii))
			{
				exporter.Initialize();
				exporter.ExportAll(blendShape, animation, eulerFilter, filterPrecision);
			}
			Directory.SetCurrentDirectory(currentDir);
		}
	}

	static Fbx()
	{
		DllLoader.PreloadDll("AssetStudioFBXNative");
	}

	public static Vector3 QuaternionToEuler(Quaternion q)
	{
		AsUtilQuaternionToEuler(q.X, q.Y, q.Z, q.W, out var x, out var y, out var z);
		return new Vector3(x, y, z);
	}

	public static Quaternion EulerToQuaternion(Vector3 v)
	{
		AsUtilEulerToQuaternion(v.X, v.Y, v.Z, out var x, out var y, out var z, out var w);
		return new Quaternion(x, y, z, w);
	}

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsUtilQuaternionToEuler(float qx, float qy, float qz, float qw, out float vx, out float vy, out float vz);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsUtilEulerToQuaternion(float vx, float vy, float vz, out float qx, out float qy, out float qz, out float qw);
}
