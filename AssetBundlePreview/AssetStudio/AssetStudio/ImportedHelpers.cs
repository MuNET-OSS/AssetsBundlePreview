using System.Collections.Generic;

namespace AssetStudio;

public static class ImportedHelpers
{
	public static ImportedMesh FindMesh(string path, List<ImportedMesh> importedMeshList)
	{
		foreach (ImportedMesh mesh in importedMeshList)
		{
			if (mesh.Path == path)
			{
				return mesh;
			}
		}
		return null;
	}

	public static ImportedMaterial FindMaterial(string name, List<ImportedMaterial> importedMats)
	{
		foreach (ImportedMaterial mat in importedMats)
		{
			if (mat.Name == name)
			{
				return mat;
			}
		}
		return null;
	}

	public static ImportedTexture FindTexture(string name, List<ImportedTexture> importedTextureList)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		foreach (ImportedTexture tex in importedTextureList)
		{
			if (tex.Name == name)
			{
				return tex;
			}
		}
		return null;
	}
}
