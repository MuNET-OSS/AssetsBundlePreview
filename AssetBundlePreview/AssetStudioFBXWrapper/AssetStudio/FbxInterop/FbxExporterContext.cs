using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AssetStudio.PInvoke;

namespace AssetStudio.FbxInterop;

internal sealed class FbxExporterContext : IDisposable
{
	private IntPtr _pContext;

	private readonly Dictionary<ImportedFrame, IntPtr> _frameToNode;

	private readonly List<KeyValuePair<string, IntPtr>> _createdMaterials;

	private readonly Dictionary<string, IntPtr> _createdTextures;

	public bool IsDisposed { get; private set; }

	public FbxExporterContext()
	{
		_pContext = AsFbxCreateContext();
		_frameToNode = new Dictionary<ImportedFrame, IntPtr>();
		_createdMaterials = new List<KeyValuePair<string, IntPtr>>();
		_createdTextures = new Dictionary<string, IntPtr>();
	}

	~FbxExporterContext()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		if (!IsDisposed)
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}

	private void Dispose(bool disposing)
	{
		IsDisposed = true;
		_frameToNode.Clear();
		_createdMaterials.Clear();
		_createdTextures.Clear();
		AsFbxDisposeContext(ref _pContext);
	}

	private void EnsureNotDisposed()
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException("FbxExporterContext");
		}
	}

	internal void Initialize(string fileName, float scaleFactor, int versionIndex, bool isAscii, bool is60Fps)
	{
		EnsureNotDisposed();
		if (!AsFbxInitializeContext(_pContext, fileName, scaleFactor, versionIndex, isAscii, is60Fps, out var errorMessage))
		{
			throw new ApplicationException("Failed to initialize FbxExporter: " + errorMessage);
		}
	}

	internal void SetFramePaths(HashSet<string> framePaths)
	{
		EnsureNotDisposed();
		if (framePaths != null && framePaths.Count != 0)
		{
			string[] framePathArray = new List<string>(framePaths).ToArray();
			AsFbxSetFramePaths(_pContext, framePathArray);
		}
	}

	internal void ExportScene()
	{
		EnsureNotDisposed();
		AsFbxExportScene(_pContext);
	}

	internal void ExportFrame(List<ImportedMesh> meshList, List<ImportedFrame> meshFrames, ImportedFrame rootFrame)
	{
		IntPtr rootNode = AsFbxGetSceneRootNode(_pContext);
		Stack<IntPtr> nodeStack = new Stack<IntPtr>();
		Stack<ImportedFrame> frameStack = new Stack<ImportedFrame>();
		nodeStack.Push(rootNode);
		frameStack.Push(rootFrame);
		while (nodeStack.Count > 0)
		{
			IntPtr parentNode = nodeStack.Pop();
			ImportedFrame frame = frameStack.Pop();
			IntPtr pContext = _pContext;
			string path = frame.Path;
			string name = frame.Name;
			Vector3 localPosition = frame.LocalPosition;
			Vector3 localRotation = frame.LocalRotation;
			Vector3 localScale = frame.LocalScale;
			IntPtr childNode = AsFbxExportSingleFrame(pContext, parentNode, path, name, in localPosition, in localRotation, in localScale);
			if (meshList != null && ImportedHelpers.FindMesh(frame.Path, meshList) != null)
			{
				meshFrames.Add(frame);
			}
			_frameToNode.Add(frame, childNode);
			for (int i = frame.Count - 1; i >= 0; i--)
			{
				nodeStack.Push(childNode);
				frameStack.Push(frame[i]);
			}
		}
	}

	internal void SetJointsNode(ImportedFrame rootFrame, HashSet<string> bonePaths, bool castToBone, float boneSize)
	{
		Stack<ImportedFrame> frameStack = new Stack<ImportedFrame>();
		frameStack.Push(rootFrame);
		while (frameStack.Count > 0)
		{
			ImportedFrame frame = frameStack.Pop();
			if (_frameToNode.TryGetValue(frame, out var node))
			{
				if (castToBone)
				{
					AsFbxSetJointsNode_CastToBone(_pContext, node, boneSize);
				}
				else if (bonePaths.Contains(frame.Path))
				{
					AsFbxSetJointsNode_BoneInPath(_pContext, node, boneSize);
				}
				else
				{
					AsFbxSetJointsNode_Generic(_pContext, node);
				}
			}
			for (int i = frame.Count - 1; i >= 0; i--)
			{
				frameStack.Push(frame[i]);
			}
		}
	}

	internal void PrepareMaterials(int materialCount, int textureCount)
	{
		AsFbxPrepareMaterials(_pContext, materialCount, textureCount);
	}

	internal void ExportMeshFromFrame(ImportedFrame rootFrame, ImportedFrame meshFrame, List<ImportedMesh> meshList, List<ImportedMaterial> materialList, List<ImportedTexture> textureList, bool exportSkins, bool exportAllUvsAsDiffuseMaps)
	{
		IntPtr meshNode = _frameToNode[meshFrame];
		ImportedMesh mesh = ImportedHelpers.FindMesh(meshFrame.Path, meshList);
		ExportMesh(rootFrame, materialList, textureList, meshNode, mesh, exportSkins, exportAllUvsAsDiffuseMaps);
	}

	private IntPtr ExportTexture(ImportedTexture texture)
	{
		if (texture == null)
		{
			return IntPtr.Zero;
		}
		if (_createdTextures.ContainsKey(texture.Name))
		{
			return _createdTextures[texture.Name];
		}
		IntPtr pTex = AsFbxCreateTexture(_pContext, texture.Name);
		_createdTextures.Add(texture.Name, pTex);
		using BinaryWriter writer = new BinaryWriter(new FileInfo(texture.Name).Create());
		writer.Write(texture.Data);
		return pTex;
	}

	private unsafe void ExportMesh(ImportedFrame rootFrame, List<ImportedMaterial> materialList, List<ImportedTexture> textureList, IntPtr frameNode, ImportedMesh importedMesh, bool exportSkins, bool exportAllUvsAsDiffuseMaps)
	{
		List<ImportedBone> boneList = importedMesh.BoneList;
		int totalBoneCount = 0;
		bool hasBones = false;
		if (exportSkins && boneList != null && boneList.Count > 0)
		{
			totalBoneCount = boneList.Count;
			hasBones = true;
		}
		IntPtr pClusterArray = IntPtr.Zero;
		try
		{
			if (hasBones)
			{
				pClusterArray = AsFbxMeshCreateClusterArray(totalBoneCount);
				foreach (ImportedBone bone in boneList)
				{
					if (bone.Path != null)
					{
						ImportedFrame frame = rootFrame.FindFrameByPath(bone.Path);
						IntPtr boneNode = _frameToNode[frame];
						IntPtr cluster = AsFbxMeshCreateCluster(_pContext, boneNode);
						AsFbxMeshAddCluster(pClusterArray, cluster);
					}
					else
					{
						AsFbxMeshAddCluster(pClusterArray, IntPtr.Zero);
					}
				}
			}
			IntPtr mesh = AsFbxMeshCreateMesh(_pContext, frameNode);
			AsFbxMeshInitControlPoints(mesh, importedMesh.VertexList.Count);
			if (importedMesh.hasNormal)
			{
				AsFbxMeshCreateElementNormal(mesh);
			}
			for (int i = 0; i < importedMesh.hasUV.Length; i++)
			{
				if (importedMesh.hasUV[i])
				{
					if (i == 1 && !exportAllUvsAsDiffuseMaps)
					{
						AsFbxMeshCreateNormalMapUV(mesh, 1);
					}
					else
					{
						AsFbxMeshCreateDiffuseUV(mesh, i);
					}
				}
			}
			if (importedMesh.hasTangent)
			{
				AsFbxMeshCreateElementTangent(mesh);
			}
			if (importedMesh.hasColor)
			{
				AsFbxMeshCreateElementVertexColor(mesh);
			}
			AsFbxMeshCreateElementMaterial(mesh);
			foreach (ImportedSubmesh meshObj in importedMesh.SubmeshList)
			{
				int materialIndex = 0;
				ImportedMaterial mat = ImportedHelpers.FindMaterial(meshObj.Material, materialList);
				if (mat != null)
				{
					int foundMat = _createdMaterials.FindIndex((KeyValuePair<string, IntPtr> kv) => kv.Key == mat.Name);
					IntPtr pMat;
					if (foundMat >= 0)
					{
						pMat = _createdMaterials[foundMat].Value;
					}
					else
					{
						Color diffuse = mat.Diffuse;
						Color ambient = mat.Ambient;
						Color emissive = mat.Emissive;
						Color specular = mat.Specular;
						Color reflection = mat.Reflection;
						pMat = AsFbxCreateMaterial(_pContext, mat.Name, in diffuse, in ambient, in emissive, in specular, in reflection, mat.Shininess, mat.Transparency);
						_createdMaterials.Add(new KeyValuePair<string, IntPtr>(mat.Name, pMat));
					}
					materialIndex = AsFbxAddMaterialToFrame(frameNode, pMat);
					bool hasTexture = false;
					foreach (ImportedMaterialTexture texture in mat.Textures)
					{
						ImportedTexture tex = ImportedHelpers.FindTexture(texture.Name, textureList);
						IntPtr pTexture = ExportTexture(tex);
						if (pTexture != IntPtr.Zero)
						{
							int dest = texture.Dest;
							if ((uint)dest <= 3u)
							{
								AsFbxLinkTexture(texture.Dest, pTexture, pMat, texture.Offset.X, texture.Offset.Y, texture.Scale.X, texture.Scale.Y);
								hasTexture = true;
							}
						}
					}
					if (hasTexture)
					{
						AsFbxSetFrameShadingModeToTextureShading(frameNode);
					}
				}
				foreach (ImportedFace face in meshObj.FaceList)
				{
					int index0 = face.VertexIndices[0] + meshObj.BaseVertex;
					int index1 = face.VertexIndices[1] + meshObj.BaseVertex;
					int index2 = face.VertexIndices[2] + meshObj.BaseVertex;
					AsFbxMeshAddPolygon(mesh, materialIndex, index0, index1, index2);
				}
			}
			List<ImportedVertex> vertexList = importedMesh.VertexList;
			int vertexCount = vertexList.Count;
			for (int k = 0; k < vertexCount; k++)
			{
				ImportedVertex importedVertex = vertexList[k];
				Vector3 vertex = importedVertex.Vertex;
				AsFbxMeshSetControlPoint(mesh, k, vertex.X, vertex.Y, vertex.Z);
				if (importedMesh.hasNormal)
				{
					Vector3 normal = importedVertex.Normal;
					AsFbxMeshElementNormalAdd(mesh, 0, normal.X, normal.Y, normal.Z);
				}
				for (int uvIndex = 0; uvIndex < importedMesh.hasUV.Length; uvIndex++)
				{
					if (importedMesh.hasUV[uvIndex])
					{
						float[] uv = importedVertex.UV[uvIndex];
						AsFbxMeshElementUVAdd(mesh, uvIndex, uv[0], uv[1]);
					}
				}
				if (importedMesh.hasTangent)
				{
					Vector4 tangent = importedVertex.Tangent;
					AsFbxMeshElementTangentAdd(mesh, 0, tangent.X, tangent.Y, tangent.Z, tangent.W);
				}
				if (importedMesh.hasColor)
				{
					Color color = importedVertex.Color;
					AsFbxMeshElementVertexColorAdd(mesh, 0, color.R, color.G, color.B, color.A);
				}
				if (!hasBones || importedVertex.BoneIndices == null)
				{
					continue;
				}
				int[] boneIndices = importedVertex.BoneIndices;
				float[] boneWeights = importedVertex.Weights;
				for (int l = 0; l < 4; l++)
				{
					if (boneIndices[l] < totalBoneCount && boneWeights[l] > 0f)
					{
						AsFbxMeshSetBoneWeight(pClusterArray, boneIndices[l], k, boneWeights[l]);
					}
				}
			}
			if (!hasBones)
			{
				return;
			}
			IntPtr pSkinContext = IntPtr.Zero;
			try
			{
				pSkinContext = AsFbxMeshCreateSkinContext(_pContext, frameNode);
				float* boneMatrix = stackalloc float[16];
				for (int j = 0; j < totalBoneCount; j++)
				{
					if (FbxClusterArray_HasItemAt(pClusterArray, j))
					{
						Matrix4x4 m = boneList[j].Matrix;
						CopyMatrix4x4(in m, boneMatrix);
						AsFbxMeshSkinAddCluster(pSkinContext, pClusterArray, j, boneMatrix);
					}
				}
				AsFbxMeshAddDeformer(pSkinContext, mesh);
			}
			finally
			{
				AsFbxMeshDisposeSkinContext(ref pSkinContext);
			}
		}
		finally
		{
			AsFbxMeshDisposeClusterArray(ref pClusterArray);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static void CopyMatrix4x4(in Matrix4x4 matrix, float* buffer)
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				int index = IndexFrom4x4(i, j);
				buffer[index] = matrix[i, j];
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int IndexFrom4x4(int m, int n)
	{
		return 4 * m + n;
	}

	internal void ExportAnimations(ImportedFrame rootFrame, List<ImportedKeyframedAnimation> animationList, bool eulerFilter, float filterPrecision)
	{
		if (animationList == null || animationList.Count == 0)
		{
			return;
		}
		IntPtr pAnimContext = IntPtr.Zero;
		try
		{
			pAnimContext = AsFbxAnimCreateContext(eulerFilter);
			for (int i = 0; i < animationList.Count; i++)
			{
				ImportedKeyframedAnimation importedAnimation = animationList[i];
				AsFbxAnimPrepareStackAndLayer(takeName: (importedAnimation.Name == null) ? ("Take" + i) : importedAnimation.Name, pContext: _pContext, pAnimContext: pAnimContext);
				ExportKeyframedAnimation(rootFrame, importedAnimation, pAnimContext, filterPrecision);
			}
		}
		finally
		{
			AsFbxAnimDisposeContext(ref pAnimContext);
		}
	}

	private void ExportKeyframedAnimation(ImportedFrame rootFrame, ImportedKeyframedAnimation parser, IntPtr pAnimContext, float filterPrecision)
	{
		foreach (ImportedAnimationKeyframedTrack track in parser.TrackList)
		{
			if (track.Path == null)
			{
				continue;
			}
			ImportedFrame frame = rootFrame.FindFrameByPath(track.Path);
			if (frame == null)
			{
				continue;
			}
			IntPtr pNode = _frameToNode[frame];
			AsFbxAnimLoadCurves(pNode, pAnimContext);
			AsFbxAnimBeginKeyModify(pAnimContext);
			foreach (ImportedKeyframe<Vector3> scaling in track.Scalings)
			{
				Vector3 value3 = scaling.value;
				AsFbxAnimAddScalingKey(pAnimContext, scaling.time, value3.X, value3.Y, value3.Z);
			}
			foreach (ImportedKeyframe<Vector3> rotation in track.Rotations)
			{
				Vector3 value2 = rotation.value;
				AsFbxAnimAddRotationKey(pAnimContext, rotation.time, value2.X, value2.Y, value2.Z);
			}
			foreach (ImportedKeyframe<Vector3> translation in track.Translations)
			{
				Vector3 value = translation.value;
				AsFbxAnimAddTranslationKey(pAnimContext, translation.time, value.X, value.Y, value.Z);
			}
			AsFbxAnimEndKeyModify(pAnimContext);
			AsFbxAnimApplyEulerFilter(pAnimContext, filterPrecision);
			ImportedBlendShape blendShape = track.BlendShape;
			if (blendShape == null)
			{
				continue;
			}
			int channelCount = AsFbxAnimGetCurrentBlendShapeChannelCount(pAnimContext, pNode);
			if (channelCount <= 0)
			{
				continue;
			}
			for (int channelIndex = 0; channelIndex < channelCount; channelIndex++)
			{
				if (!AsFbxAnimIsBlendShapeChannelMatch(pAnimContext, channelIndex, blendShape.ChannelName))
				{
					continue;
				}
				AsFbxAnimBeginBlendShapeAnimCurve(pAnimContext, channelIndex);
				foreach (ImportedKeyframe<float> keyframe in blendShape.Keyframes)
				{
					AsFbxAnimAddBlendShapeKeyframe(pAnimContext, keyframe.time, keyframe.value);
				}
				AsFbxAnimEndBlendShapeAnimCurve(pAnimContext);
			}
		}
	}

	internal void ExportMorphs(ImportedFrame rootFrame, List<ImportedMorph> morphList)
	{
		if (morphList == null || morphList.Count == 0)
		{
			return;
		}
		foreach (ImportedMorph morph in morphList)
		{
			ImportedFrame frame = rootFrame.FindFrameByPath(morph.Path);
			if (frame == null)
			{
				continue;
			}
			IntPtr pNode = _frameToNode[frame];
			IntPtr pMorphContext = IntPtr.Zero;
			try
			{
				pMorphContext = AsFbxMorphCreateContext();
				AsFbxMorphInitializeContext(_pContext, pMorphContext, pNode);
				foreach (ImportedMorphChannel channel in morph.Channels)
				{
					AsFbxMorphAddBlendShapeChannel(_pContext, pMorphContext, channel.Name);
					for (int i = 0; i < channel.KeyframeList.Count; i++)
					{
						ImportedMorphKeyframe keyframe = channel.KeyframeList[i];
						AsFbxMorphAddBlendShapeChannelShape(_pContext, pMorphContext, keyframe.Weight, (i == 0) ? channel.Name : $"{channel.Name}_{i + 1}");
						AsFbxMorphCopyBlendShapeControlPoints(pMorphContext);
						foreach (ImportedMorphVertex vertex2 in keyframe.VertexList)
						{
							Vector3 v2 = vertex2.Vertex.Vertex;
							AsFbxMorphSetBlendShapeVertex(pMorphContext, vertex2.Index, v2.X, v2.Y, v2.Z);
						}
						if (!keyframe.hasNormals)
						{
							continue;
						}
						AsFbxMorphCopyBlendShapeControlPointsNormal(pMorphContext);
						foreach (ImportedMorphVertex vertex in keyframe.VertexList)
						{
							Vector3 v = vertex.Vertex.Normal;
							AsFbxMorphSetBlendShapeVertexNormal(pMorphContext, vertex.Index, v.X, v.Y, v.Z);
						}
					}
				}
			}
			finally
			{
				AsFbxMorphDisposeContext(ref pMorphContext);
			}
		}
	}

	[DllImport("AssetStudioFBXNative")]
	private static extern IntPtr AsFbxCreateContext();

	private static bool AsFbxInitializeContext(IntPtr context, string fileName, float scaleFactor, int versionIndex, bool isAscii, bool is60Fps, out string errorMessage)
	{
		bool b;
		IntPtr pErrMsg;
		using (Utf8StringHandle fileNameUtf8 = new Utf8StringHandle(fileName))
		{
			b = AsFbxInitializeContext(context, fileNameUtf8.DangerousGetHandle(), scaleFactor, versionIndex, isAscii, is60Fps, out pErrMsg);
		}
		errorMessage = Utf8StringHandle.ReadUtf8StringFromPointer(pErrMsg);
		return b;
	}

	[DllImport("AssetStudioFBXNative")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool AsFbxInitializeContext(IntPtr context, IntPtr strFileName, float scaleFactor, int versionIndex, [MarshalAs(UnmanagedType.Bool)] bool isAscii, [MarshalAs(UnmanagedType.Bool)] bool is60Fps, out IntPtr strErrorMessage);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxDisposeContext(ref IntPtr ppContext);

	private static void AsFbxSetFramePaths(IntPtr context, string[] framePaths)
	{
		int framePathCount = framePaths.Length;
		if (framePathCount == 0)
		{
			AsFbxSetFramePaths(context, Array.Empty<IntPtr>(), 0);
			return;
		}
		Utf8StringHandle[] utf8Paths = new Utf8StringHandle[framePathCount];
		try
		{
			for (int j = 0; j < framePathCount; j++)
			{
				utf8Paths[j] = new Utf8StringHandle(framePaths[j]);
			}
			IntPtr[] pathPointers = new IntPtr[framePathCount];
			for (int i = 0; i < framePathCount; i++)
			{
				pathPointers[i] = utf8Paths[i].DangerousGetHandle();
			}
			AsFbxSetFramePaths(context, pathPointers, framePathCount);
		}
		finally
		{
			Utf8StringHandle[] array = utf8Paths;
			for (int k = 0; k < array.Length; k++)
			{
				array[k]?.Dispose();
			}
		}
	}

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxSetFramePaths(IntPtr context, [MarshalAs(UnmanagedType.LPArray)] IntPtr[] strFramePaths, int count);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxExportScene(IntPtr context);

	[DllImport("AssetStudioFBXNative")]
	private static extern IntPtr AsFbxGetSceneRootNode(IntPtr context);

	private static IntPtr AsFbxExportSingleFrame(IntPtr context, IntPtr parentNode, string framePath, string frameName, in Vector3 localPosition, in Vector3 localRotation, in Vector3 localScale)
	{
		using Utf8StringHandle framePathUtf8 = new Utf8StringHandle(framePath);
		using Utf8StringHandle frameNameUtf8 = new Utf8StringHandle(frameName);
		return AsFbxExportSingleFrame(context, parentNode, framePathUtf8.DangerousGetHandle(), frameNameUtf8.DangerousGetHandle(), localPosition.X, localPosition.Y, localPosition.Z, localRotation.X, localRotation.Y, localRotation.Z, localScale.X, localScale.Y, localScale.Z);
	}

	[DllImport("AssetStudioFBXNative")]
	private static extern IntPtr AsFbxExportSingleFrame(IntPtr context, IntPtr parentNode, IntPtr strFramePath, IntPtr strFrameName, float localPositionX, float localPositionY, float localPositionZ, float localRotationX, float localRotationY, float localRotationZ, float localScaleX, float localScaleY, float localScaleZ);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxSetJointsNode_CastToBone(IntPtr context, IntPtr node, float boneSize);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxSetJointsNode_BoneInPath(IntPtr context, IntPtr node, float boneSize);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxSetJointsNode_Generic(IntPtr context, IntPtr node);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxPrepareMaterials(IntPtr context, int materialCount, int textureCount);

	private static IntPtr AsFbxCreateTexture(IntPtr context, string matTexName)
	{
		using Utf8StringHandle matTexNameUtf8 = new Utf8StringHandle(matTexName);
		return AsFbxCreateTexture(context, matTexNameUtf8.DangerousGetHandle());
	}

	[DllImport("AssetStudioFBXNative")]
	private static extern IntPtr AsFbxCreateTexture(IntPtr context, IntPtr strMatTexName);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxLinkTexture(int dest, IntPtr texture, IntPtr material, float offsetX, float offsetY, float scaleX, float scaleY);

	[DllImport("AssetStudioFBXNative")]
	private static extern IntPtr AsFbxMeshCreateClusterArray(int boneCount);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshDisposeClusterArray(ref IntPtr ppArray);

	[DllImport("AssetStudioFBXNative")]
	private static extern IntPtr AsFbxMeshCreateCluster(IntPtr context, IntPtr boneNode);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshAddCluster(IntPtr array, IntPtr cluster);

	[DllImport("AssetStudioFBXNative")]
	private static extern IntPtr AsFbxMeshCreateMesh(IntPtr context, IntPtr frameNode);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshInitControlPoints(IntPtr mesh, int vertexCount);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshCreateElementNormal(IntPtr mesh);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshCreateDiffuseUV(IntPtr mesh, int uv);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshCreateNormalMapUV(IntPtr mesh, int uv);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshCreateElementTangent(IntPtr mesh);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshCreateElementVertexColor(IntPtr mesh);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshCreateElementMaterial(IntPtr mesh);

	private static IntPtr AsFbxCreateMaterial(IntPtr pContext, string matName, in Color diffuse, in Color ambient, in Color emissive, in Color specular, in Color reflection, float shininess, float transparency)
	{
		using Utf8StringHandle matNameUtf8 = new Utf8StringHandle(matName);
		return AsFbxCreateMaterial(pContext, matNameUtf8.DangerousGetHandle(), diffuse.R, diffuse.G, diffuse.B, ambient.R, ambient.G, ambient.B, emissive.R, emissive.G, emissive.B, specular.R, specular.G, specular.B, reflection.R, reflection.G, reflection.B, shininess, transparency);
	}

	[DllImport("AssetStudioFBXNative")]
	private static extern IntPtr AsFbxCreateMaterial(IntPtr pContext, IntPtr pMatName, float diffuseR, float diffuseG, float diffuseB, float ambientR, float ambientG, float ambientB, float emissiveR, float emissiveG, float emissiveB, float specularR, float specularG, float specularB, float reflectR, float reflectG, float reflectB, float shininess, float transparency);

	[DllImport("AssetStudioFBXNative")]
	private static extern int AsFbxAddMaterialToFrame(IntPtr frameNode, IntPtr material);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxSetFrameShadingModeToTextureShading(IntPtr frameNode);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshSetControlPoint(IntPtr mesh, int index, float x, float y, float z);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshAddPolygon(IntPtr mesh, int materialIndex, int index0, int index1, int index2);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshElementNormalAdd(IntPtr mesh, int elementIndex, float x, float y, float z);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshElementUVAdd(IntPtr mesh, int elementIndex, float u, float v);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshElementTangentAdd(IntPtr mesh, int elementIndex, float x, float y, float z, float w);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshElementVertexColorAdd(IntPtr mesh, int elementIndex, float r, float g, float b, float a);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshSetBoneWeight(IntPtr pClusterArray, int boneIndex, int vertexIndex, float weight);

	[DllImport("AssetStudioFBXNative")]
	private static extern IntPtr AsFbxMeshCreateSkinContext(IntPtr context, IntPtr frameNode);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshDisposeSkinContext(ref IntPtr ppSkinContext);

	[DllImport("AssetStudioFBXNative")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool FbxClusterArray_HasItemAt(IntPtr pClusterArray, int index);

	[DllImport("AssetStudioFBXNative")]
	private unsafe static extern void AsFbxMeshSkinAddCluster(IntPtr pSkinContext, IntPtr pClusterArray, int index, float* pBoneMatrix);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMeshAddDeformer(IntPtr pSkinContext, IntPtr pMesh);

	[DllImport("AssetStudioFBXNative")]
	private static extern IntPtr AsFbxAnimCreateContext([MarshalAs(UnmanagedType.Bool)] bool eulerFilter);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxAnimDisposeContext(ref IntPtr ppAnimContext);

	private static void AsFbxAnimPrepareStackAndLayer(IntPtr pContext, IntPtr pAnimContext, string takeName)
	{
		using Utf8StringHandle takeNameUtf8 = new Utf8StringHandle(takeName);
		AsFbxAnimPrepareStackAndLayer(pContext, pAnimContext, takeNameUtf8.DangerousGetHandle());
	}

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxAnimPrepareStackAndLayer(IntPtr pContext, IntPtr pAnimContext, IntPtr strTakeName);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxAnimLoadCurves(IntPtr pNode, IntPtr pAnimContext);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxAnimBeginKeyModify(IntPtr pAnimContext);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxAnimEndKeyModify(IntPtr pAnimContext);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxAnimAddScalingKey(IntPtr pAnimContext, float time, float x, float y, float z);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxAnimAddRotationKey(IntPtr pAnimContext, float time, float x, float y, float z);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxAnimAddTranslationKey(IntPtr pAnimContext, float time, float x, float y, float z);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxAnimApplyEulerFilter(IntPtr pAnimContext, float filterPrecision);

	[DllImport("AssetStudioFBXNative")]
	private static extern int AsFbxAnimGetCurrentBlendShapeChannelCount(IntPtr pAnimContext, IntPtr pNode);

	private static bool AsFbxAnimIsBlendShapeChannelMatch(IntPtr pAnimContext, int channelIndex, string channelName)
	{
		using Utf8StringHandle channelNameUtf8 = new Utf8StringHandle(channelName);
		return AsFbxAnimIsBlendShapeChannelMatch(pAnimContext, channelIndex, channelNameUtf8.DangerousGetHandle());
	}

	[DllImport("AssetStudioFBXNative")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool AsFbxAnimIsBlendShapeChannelMatch(IntPtr pAnimContext, int channelIndex, IntPtr strChannelName);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxAnimBeginBlendShapeAnimCurve(IntPtr pAnimContext, int channelIndex);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxAnimEndBlendShapeAnimCurve(IntPtr pAnimContext);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxAnimAddBlendShapeKeyframe(IntPtr pAnimContext, float time, float value);

	[DllImport("AssetStudioFBXNative")]
	private static extern IntPtr AsFbxMorphCreateContext();

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMorphInitializeContext(IntPtr pContext, IntPtr pMorphContext, IntPtr pNode);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMorphDisposeContext(ref IntPtr ppMorphContext);

	private static void AsFbxMorphAddBlendShapeChannel(IntPtr pContext, IntPtr pMorphContext, string channelName)
	{
		using Utf8StringHandle channelNameUtf8 = new Utf8StringHandle(channelName);
		AsFbxMorphAddBlendShapeChannel(pContext, pMorphContext, channelNameUtf8.DangerousGetHandle());
	}

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMorphAddBlendShapeChannel(IntPtr pContext, IntPtr pMorphContext, IntPtr strChannelName);

	private static void AsFbxMorphAddBlendShapeChannelShape(IntPtr pContext, IntPtr pMorphContext, float weight, string shapeName)
	{
		using Utf8StringHandle shapeNameUtf8 = new Utf8StringHandle(shapeName);
		AsFbxMorphAddBlendShapeChannelShape(pContext, pMorphContext, weight, shapeNameUtf8.DangerousGetHandle());
	}

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMorphAddBlendShapeChannelShape(IntPtr pContext, IntPtr pMorphContext, float weight, IntPtr strShapeName);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMorphCopyBlendShapeControlPoints(IntPtr pMorphContext);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMorphSetBlendShapeVertex(IntPtr pMorphContext, uint index, float x, float y, float z);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMorphCopyBlendShapeControlPointsNormal(IntPtr pMorphContext);

	[DllImport("AssetStudioFBXNative")]
	private static extern void AsFbxMorphSetBlendShapeVertexNormal(IntPtr pMorphContext, uint index, float x, float y, float z);
}
