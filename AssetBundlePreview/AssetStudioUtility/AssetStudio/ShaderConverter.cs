using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using K4os.Compression.LZ4;

namespace AssetStudio;

public static class ShaderConverter
{
	private static string header = "//////////////////////////////////////////\n//\n// NOTE: This is *not* a valid shader file\n//\n///////////////////////////////////////////\n";

	public static string Convert(this Shader shader)
	{
		if (shader.m_SubProgramBlob != null)
		{
			byte[] decompressedBytes = new byte[shader.decompressedSize];
			LZ4Codec.Decode(shader.m_SubProgramBlob, decompressedBytes);
			using BinaryReader blobReader = new BinaryReader(new MemoryStream(decompressedBytes));
			ShaderProgram program = new ShaderProgram(blobReader, shader.version);
			program.Read(blobReader, 0);
			return header + program.Export(Encoding.UTF8.GetString(shader.m_Script));
		}
		if (shader.compressedBlob != null)
		{
			return header + ConvertSerializedShader(shader);
		}
		return header + Encoding.UTF8.GetString(shader.m_Script);
	}

	private static string ConvertSerializedShader(Shader shader)
	{
		int length = shader.platforms.Length;
		ShaderProgram[] shaderPrograms = new ShaderProgram[length];
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < shader.offsets[i].Length; j++)
			{
				uint offset = shader.offsets[i][j];
				uint compressedLength = shader.compressedLengths[i][j];
				uint decompressedLength = shader.decompressedLengths[i][j];
				byte[] decompressedBytes = new byte[decompressedLength];
				LZ4Codec.Decode(shader.compressedBlob, (int)offset, (int)compressedLength, decompressedBytes, 0, (int)decompressedLength);
				using BinaryReader blobReader = new BinaryReader(new MemoryStream(decompressedBytes));
				if (j == 0)
				{
					shaderPrograms[i] = new ShaderProgram(blobReader, shader.version);
				}
				shaderPrograms[i].Read(blobReader, j);
			}
		}
		return ConvertSerializedShader(shader.m_ParsedForm, shader.platforms, shaderPrograms);
	}

	private static string ConvertSerializedShader(SerializedShader m_ParsedForm, ShaderCompilerPlatform[] platforms, ShaderProgram[] shaderPrograms)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("Shader \"" + m_ParsedForm.m_Name + "\" {\n");
		sb.Append(ConvertSerializedProperties(m_ParsedForm.m_PropInfo));
		SerializedSubShader[] subShaders = m_ParsedForm.m_SubShaders;
		foreach (SerializedSubShader m_SubShader in subShaders)
		{
			sb.Append(ConvertSerializedSubShader(m_SubShader, platforms, shaderPrograms));
		}
		if (!string.IsNullOrEmpty(m_ParsedForm.m_FallbackName))
		{
			sb.Append("Fallback \"" + m_ParsedForm.m_FallbackName + "\"\n");
		}
		if (!string.IsNullOrEmpty(m_ParsedForm.m_CustomEditorName))
		{
			sb.Append("CustomEditor \"" + m_ParsedForm.m_CustomEditorName + "\"\n");
		}
		sb.Append("}");
		return sb.ToString();
	}

	private static string ConvertSerializedSubShader(SerializedSubShader m_SubShader, ShaderCompilerPlatform[] platforms, ShaderProgram[] shaderPrograms)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("SubShader {\n");
		if (m_SubShader.m_LOD != 0)
		{
			sb.Append($" LOD {m_SubShader.m_LOD}\n");
		}
		sb.Append(ConvertSerializedTagMap(m_SubShader.m_Tags, 1));
		SerializedPass[] passes = m_SubShader.m_Passes;
		foreach (SerializedPass m_Passe in passes)
		{
			sb.Append(ConvertSerializedPass(m_Passe, platforms, shaderPrograms));
		}
		sb.Append("}\n");
		return sb.ToString();
	}

	private static string ConvertSerializedPass(SerializedPass m_Passe, ShaderCompilerPlatform[] platforms, ShaderProgram[] shaderPrograms)
	{
		StringBuilder sb = new StringBuilder();
		switch (m_Passe.m_Type)
		{
		case PassType.Normal:
			sb.Append(" Pass ");
			break;
		case PassType.Use:
			sb.Append(" UsePass ");
			break;
		case PassType.Grab:
			sb.Append(" GrabPass ");
			break;
		}
		if (m_Passe.m_Type == PassType.Use)
		{
			sb.Append("\"" + m_Passe.m_UseName + "\"\n");
		}
		else
		{
			sb.Append("{\n");
			if (m_Passe.m_Type == PassType.Grab)
			{
				if (!string.IsNullOrEmpty(m_Passe.m_TextureName))
				{
					sb.Append("  \"" + m_Passe.m_TextureName + "\"\n");
				}
			}
			else
			{
				sb.Append(ConvertSerializedShaderState(m_Passe.m_State));
				if (m_Passe.progVertex.m_SubPrograms.Length != 0)
				{
					sb.Append("Program \"vp\" {\n");
					sb.Append(ConvertSerializedSubPrograms(m_Passe.progVertex.m_SubPrograms, platforms, shaderPrograms));
					sb.Append("}\n");
				}
				if (m_Passe.progFragment.m_SubPrograms.Length != 0)
				{
					sb.Append("Program \"fp\" {\n");
					sb.Append(ConvertSerializedSubPrograms(m_Passe.progFragment.m_SubPrograms, platforms, shaderPrograms));
					sb.Append("}\n");
				}
				if (m_Passe.progGeometry.m_SubPrograms.Length != 0)
				{
					sb.Append("Program \"gp\" {\n");
					sb.Append(ConvertSerializedSubPrograms(m_Passe.progGeometry.m_SubPrograms, platforms, shaderPrograms));
					sb.Append("}\n");
				}
				if (m_Passe.progHull.m_SubPrograms.Length != 0)
				{
					sb.Append("Program \"hp\" {\n");
					sb.Append(ConvertSerializedSubPrograms(m_Passe.progHull.m_SubPrograms, platforms, shaderPrograms));
					sb.Append("}\n");
				}
				if (m_Passe.progDomain.m_SubPrograms.Length != 0)
				{
					sb.Append("Program \"dp\" {\n");
					sb.Append(ConvertSerializedSubPrograms(m_Passe.progDomain.m_SubPrograms, platforms, shaderPrograms));
					sb.Append("}\n");
				}
				SerializedProgram progRayTracing = m_Passe.progRayTracing;
				if (progRayTracing != null && progRayTracing.m_SubPrograms.Length != 0)
				{
					sb.Append("Program \"rtp\" {\n");
					sb.Append(ConvertSerializedSubPrograms(m_Passe.progRayTracing.m_SubPrograms, platforms, shaderPrograms));
					sb.Append("}\n");
				}
			}
			sb.Append("}\n");
		}
		return sb.ToString();
	}

	private static string ConvertSerializedSubPrograms(SerializedSubProgram[] m_SubPrograms, ShaderCompilerPlatform[] platforms, ShaderProgram[] shaderPrograms)
	{
		StringBuilder sb = new StringBuilder();
		foreach (IGrouping<uint, SerializedSubProgram> item in from x in m_SubPrograms
			group x by x.m_BlobIndex)
		{
			foreach (IGrouping<ShaderGpuProgramType, SerializedSubProgram> program in from x in item
				group x by x.m_GpuProgramType)
			{
				for (int i = 0; i < platforms.Length; i++)
				{
					ShaderCompilerPlatform platform = platforms[i];
					if (!CheckGpuProgramUsable(platform, program.Key))
					{
						continue;
					}
					List<SerializedSubProgram> list = program.ToList();
					bool isTier = list.Count > 1;
					foreach (SerializedSubProgram subProgram in list)
					{
						sb.Append("SubProgram \"" + GetPlatformString(platform) + " ");
						if (isTier)
						{
							sb.Append($"hw_tier{subProgram.m_ShaderHardwareTier:00} ");
						}
						sb.Append("\" {\n");
						sb.Append(shaderPrograms[i].m_SubPrograms[subProgram.m_BlobIndex].Export());
						sb.Append("\n}\n");
					}
					break;
				}
			}
		}
		return sb.ToString();
	}

	private static string ConvertSerializedShaderState(SerializedShaderState m_State)
	{
		StringBuilder sb = new StringBuilder();
		if (!string.IsNullOrEmpty(m_State.m_Name))
		{
			sb.Append("  Name \"" + m_State.m_Name + "\"\n");
		}
		if (m_State.m_LOD != 0)
		{
			sb.Append($"  LOD {m_State.m_LOD}\n");
		}
		sb.Append(ConvertSerializedTagMap(m_State.m_Tags, 2));
		sb.Append(ConvertSerializedShaderRTBlendState(m_State.rtBlend, m_State.rtSeparateBlend));
		if (m_State.alphaToMask.val > 0f)
		{
			sb.Append("  AlphaToMask On\n");
		}
		SerializedShaderFloatValue zClip = m_State.zClip;
		if (zClip == null || zClip.val != 1f)
		{
			sb.Append("  ZClip Off\n");
		}
		if (m_State.zTest.val != 4f)
		{
			sb.Append("  ZTest ");
			float val = m_State.zTest.val;
			if (val <= 3f)
			{
				if (val <= 1f)
				{
					if (val != 0f)
					{
						if (val == 1f)
						{
							sb.Append("Never");
						}
					}
					else
					{
						sb.Append("Off");
					}
				}
				else if (val != 2f)
				{
					if (val == 3f)
					{
						sb.Append("Equal");
					}
				}
				else
				{
					sb.Append("Less");
				}
			}
			else if (val <= 6f)
			{
				if (val != 5f)
				{
					if (val == 6f)
					{
						sb.Append("NotEqual");
					}
				}
				else
				{
					sb.Append("Greater");
				}
			}
			else if (val != 7f)
			{
				if (val == 8f)
				{
					sb.Append("Always");
				}
			}
			else
			{
				sb.Append("GEqual");
			}
			sb.Append("\n");
		}
		if (m_State.zWrite.val != 1f)
		{
			sb.Append("  ZWrite Off\n");
		}
		if (m_State.culling.val != 2f)
		{
			sb.Append("  Cull ");
			float val = m_State.culling.val;
			if (val != 0f)
			{
				if (val == 1f)
				{
					sb.Append("Front");
				}
			}
			else
			{
				sb.Append("Off");
			}
			sb.Append("\n");
		}
		if (m_State.offsetFactor.val != 0f || m_State.offsetUnits.val != 0f)
		{
			sb.Append($"  Offset {m_State.offsetFactor.val}, {m_State.offsetUnits.val}\n");
		}
		if (m_State.stencilRef.val != 0f || m_State.stencilReadMask.val != 255f || m_State.stencilWriteMask.val != 255f || m_State.stencilOp.pass.val != 0f || m_State.stencilOp.fail.val != 0f || m_State.stencilOp.zFail.val != 0f || m_State.stencilOp.comp.val != 8f || m_State.stencilOpFront.pass.val != 0f || m_State.stencilOpFront.fail.val != 0f || m_State.stencilOpFront.zFail.val != 0f || m_State.stencilOpFront.comp.val != 8f || m_State.stencilOpBack.pass.val != 0f || m_State.stencilOpBack.fail.val != 0f || m_State.stencilOpBack.zFail.val != 0f || m_State.stencilOpBack.comp.val != 8f)
		{
			sb.Append("  Stencil {\n");
			if (m_State.stencilRef.val != 0f)
			{
				sb.Append($"   Ref {m_State.stencilRef.val}\n");
			}
			if (m_State.stencilReadMask.val != 255f)
			{
				sb.Append($"   ReadMask {m_State.stencilReadMask.val}\n");
			}
			if (m_State.stencilWriteMask.val != 255f)
			{
				sb.Append($"   WriteMask {m_State.stencilWriteMask.val}\n");
			}
			if (m_State.stencilOp.pass.val != 0f || m_State.stencilOp.fail.val != 0f || m_State.stencilOp.zFail.val != 0f || m_State.stencilOp.comp.val != 8f)
			{
				sb.Append(ConvertSerializedStencilOp(m_State.stencilOp, ""));
			}
			if (m_State.stencilOpFront.pass.val != 0f || m_State.stencilOpFront.fail.val != 0f || m_State.stencilOpFront.zFail.val != 0f || m_State.stencilOpFront.comp.val != 8f)
			{
				sb.Append(ConvertSerializedStencilOp(m_State.stencilOpFront, "Front"));
			}
			if (m_State.stencilOpBack.pass.val != 0f || m_State.stencilOpBack.fail.val != 0f || m_State.stencilOpBack.zFail.val != 0f || m_State.stencilOpBack.comp.val != 8f)
			{
				sb.Append(ConvertSerializedStencilOp(m_State.stencilOpBack, "Back"));
			}
			sb.Append("  }\n");
		}
		if (m_State.fogMode != FogMode.Unknown || m_State.fogColor.x.val != 0f || m_State.fogColor.y.val != 0f || m_State.fogColor.z.val != 0f || m_State.fogColor.w.val != 0f || m_State.fogDensity.val != 0f || m_State.fogStart.val != 0f || m_State.fogEnd.val != 0f)
		{
			sb.Append("  Fog {\n");
			if (m_State.fogMode != FogMode.Unknown)
			{
				sb.Append("   Mode ");
				switch (m_State.fogMode)
				{
				case FogMode.Disabled:
					sb.Append("Off");
					break;
				case FogMode.Linear:
					sb.Append("Linear");
					break;
				case FogMode.Exp:
					sb.Append("Exp");
					break;
				case FogMode.Exp2:
					sb.Append("Exp2");
					break;
				}
				sb.Append("\n");
			}
			if (m_State.fogColor.x.val != 0f || m_State.fogColor.y.val != 0f || m_State.fogColor.z.val != 0f || m_State.fogColor.w.val != 0f)
			{
				sb.AppendFormat("   Color ({0},{1},{2},{3})\n", m_State.fogColor.x.val.ToString(CultureInfo.InvariantCulture), m_State.fogColor.y.val.ToString(CultureInfo.InvariantCulture), m_State.fogColor.z.val.ToString(CultureInfo.InvariantCulture), m_State.fogColor.w.val.ToString(CultureInfo.InvariantCulture));
			}
			if (m_State.fogDensity.val != 0f)
			{
				sb.Append("   Density " + m_State.fogDensity.val.ToString(CultureInfo.InvariantCulture) + "\n");
			}
			if (m_State.fogStart.val != 0f || m_State.fogEnd.val != 0f)
			{
				sb.Append("   Range " + m_State.fogStart.val.ToString(CultureInfo.InvariantCulture) + ", " + m_State.fogEnd.val.ToString(CultureInfo.InvariantCulture) + "\n");
			}
			sb.Append("  }\n");
		}
		if (m_State.lighting)
		{
			sb.Append("  Lighting " + (m_State.lighting ? "On" : "Off") + "\n");
		}
		sb.Append($"  GpuProgramID {m_State.gpuProgramID}\n");
		return sb.ToString();
	}

	private static string ConvertSerializedStencilOp(SerializedStencilOp stencilOp, string suffix)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("   Comp" + suffix + " " + ConvertStencilComp(stencilOp.comp) + "\n");
		sb.Append("   Pass" + suffix + " " + ConvertStencilOp(stencilOp.pass) + "\n");
		sb.Append("   Fail" + suffix + " " + ConvertStencilOp(stencilOp.fail) + "\n");
		sb.Append("   ZFail" + suffix + " " + ConvertStencilOp(stencilOp.zFail) + "\n");
		return sb.ToString();
	}

	private static string ConvertStencilOp(SerializedShaderFloatValue op)
	{
		float val = op.val;
		if (val <= 3f)
		{
			if (val <= 1f)
			{
				if (val != 0f && val == 1f)
				{
					return "Zero";
				}
			}
			else
			{
				if (val == 2f)
				{
					return "Replace";
				}
				if (val == 3f)
				{
					return "IncrSat";
				}
			}
		}
		else if (val <= 5f)
		{
			if (val == 4f)
			{
				return "DecrSat";
			}
			if (val == 5f)
			{
				return "Invert";
			}
		}
		else
		{
			if (val == 6f)
			{
				return "IncrWrap";
			}
			if (val == 7f)
			{
				return "DecrWrap";
			}
		}
		return "Keep";
	}

	private static string ConvertStencilComp(SerializedShaderFloatValue comp)
	{
		float val = comp.val;
		if (val <= 3f)
		{
			if (val <= 1f)
			{
				if (val == 0f)
				{
					return "Disabled";
				}
				if (val == 1f)
				{
					return "Never";
				}
			}
			else
			{
				if (val == 2f)
				{
					return "Less";
				}
				if (val == 3f)
				{
					return "Equal";
				}
			}
		}
		else if (val <= 5f)
		{
			if (val == 4f)
			{
				return "LEqual";
			}
			if (val == 5f)
			{
				return "Greater";
			}
		}
		else
		{
			if (val == 6f)
			{
				return "NotEqual";
			}
			if (val == 7f)
			{
				return "GEqual";
			}
			if (val == 8f)
			{
			}
		}
		return "Always";
	}

	private static string ConvertSerializedShaderRTBlendState(SerializedShaderRTBlendState[] rtBlend, bool rtSeparateBlend)
	{
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < rtBlend.Length; i++)
		{
			SerializedShaderRTBlendState blend = rtBlend[i];
			if (blend.srcBlend.val != 1f || blend.destBlend.val != 0f || blend.srcBlendAlpha.val != 1f || blend.destBlendAlpha.val != 0f)
			{
				sb.Append("  Blend ");
				if (i != 0 || rtSeparateBlend)
				{
					sb.Append($"{i} ");
				}
				sb.Append(ConvertBlendFactor(blend.srcBlend) + " " + ConvertBlendFactor(blend.destBlend));
				if (blend.srcBlendAlpha.val != 1f || blend.destBlendAlpha.val != 0f)
				{
					sb.Append(", " + ConvertBlendFactor(blend.srcBlendAlpha) + " " + ConvertBlendFactor(blend.destBlendAlpha));
				}
				sb.Append("\n");
			}
			if (blend.blendOp.val != 0f || blend.blendOpAlpha.val != 0f)
			{
				sb.Append("  BlendOp ");
				if (i != 0 || rtSeparateBlend)
				{
					sb.Append($"{i} ");
				}
				sb.Append(ConvertBlendOp(blend.blendOp));
				if (blend.blendOpAlpha.val != 0f)
				{
					sb.Append(", " + ConvertBlendOp(blend.blendOpAlpha));
				}
				sb.Append("\n");
			}
			int val = (int)blend.colMask.val;
			if (val == 15)
			{
				continue;
			}
			sb.Append("  ColorMask ");
			if (val == 0)
			{
				sb.Append(0);
			}
			else
			{
				if (((uint)val & 2u) != 0)
				{
					sb.Append("R");
				}
				if (((uint)val & 4u) != 0)
				{
					sb.Append("G");
				}
				if (((uint)val & 8u) != 0)
				{
					sb.Append("B");
				}
				if (((uint)val & (true ? 1u : 0u)) != 0)
				{
					sb.Append("A");
				}
			}
			sb.Append($" {i}\n");
		}
		return sb.ToString();
	}

	private static string ConvertBlendOp(SerializedShaderFloatValue op)
	{
		float val = op.val;
		if (val <= 9f)
		{
			if (val <= 4f)
			{
				if (val <= 1f)
				{
					if (val != 0f && val == 1f)
					{
						return "Sub";
					}
				}
				else
				{
					if (val == 2f)
					{
						return "RevSub";
					}
					if (val == 3f)
					{
						return "Min";
					}
					if (val == 4f)
					{
						return "Max";
					}
				}
			}
			else if (val <= 6f)
			{
				if (val == 5f)
				{
					return "LogicalClear";
				}
				if (val == 6f)
				{
					return "LogicalSet";
				}
			}
			else
			{
				if (val == 7f)
				{
					return "LogicalCopy";
				}
				if (val == 8f)
				{
					return "LogicalCopyInverted";
				}
				if (val == 9f)
				{
					return "LogicalNoop";
				}
			}
		}
		else if (val <= 14f)
		{
			if (val <= 11f)
			{
				if (val == 10f)
				{
					return "LogicalInvert";
				}
				if (val == 11f)
				{
					return "LogicalAnd";
				}
			}
			else
			{
				if (val == 12f)
				{
					return "LogicalNand";
				}
				if (val == 13f)
				{
					return "LogicalOr";
				}
				if (val == 14f)
				{
					return "LogicalNor";
				}
			}
		}
		else if (val <= 17f)
		{
			if (val == 15f)
			{
				return "LogicalXor";
			}
			if (val == 16f)
			{
				return "LogicalEquiv";
			}
			if (val == 17f)
			{
				return "LogicalAndReverse";
			}
		}
		else
		{
			if (val == 18f)
			{
				return "LogicalAndInverted";
			}
			if (val == 19f)
			{
				return "LogicalOrReverse";
			}
			if (val == 20f)
			{
				return "LogicalOrInverted";
			}
		}
		return "Add";
	}

	private static string ConvertBlendFactor(SerializedShaderFloatValue factor)
	{
		float val = factor.val;
		if (val <= 4f)
		{
			if (val <= 1f)
			{
				if (val == 0f)
				{
					return "Zero";
				}
				if (val == 1f)
				{
				}
			}
			else
			{
				if (val == 2f)
				{
					return "DstColor";
				}
				if (val == 3f)
				{
					return "SrcColor";
				}
				if (val == 4f)
				{
					return "OneMinusDstColor";
				}
			}
		}
		else if (val <= 7f)
		{
			if (val == 5f)
			{
				return "SrcAlpha";
			}
			if (val == 6f)
			{
				return "OneMinusSrcColor";
			}
			if (val == 7f)
			{
				return "DstAlpha";
			}
		}
		else
		{
			if (val == 8f)
			{
				return "OneMinusDstAlpha";
			}
			if (val == 9f)
			{
				return "SrcAlphaSaturate";
			}
			if (val == 10f)
			{
				return "OneMinusSrcAlpha";
			}
		}
		return "One";
	}

	private static string ConvertSerializedTagMap(SerializedTagMap m_Tags, int intent)
	{
		StringBuilder sb = new StringBuilder();
		if (m_Tags.tags.Length != 0)
		{
			sb.Append(new string(' ', intent));
			sb.Append("Tags { ");
			KeyValuePair<string, string>[] tags = m_Tags.tags;
			for (int i = 0; i < tags.Length; i++)
			{
				KeyValuePair<string, string> pair = tags[i];
				sb.Append("\"" + pair.Key + "\" = \"" + pair.Value + "\" ");
			}
			sb.Append("}\n");
		}
		return sb.ToString();
	}

	private static string ConvertSerializedProperties(SerializedProperties m_PropInfo)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("Properties {\n");
		SerializedProperty[] props = m_PropInfo.m_Props;
		foreach (SerializedProperty m_Prop in props)
		{
			sb.Append(ConvertSerializedProperty(m_Prop));
		}
		sb.Append("}\n");
		return sb.ToString();
	}

	private static string ConvertSerializedProperty(SerializedProperty m_Prop)
	{
		StringBuilder sb = new StringBuilder();
		string[] attributes = m_Prop.m_Attributes;
		foreach (string m_Attribute in attributes)
		{
			sb.Append("[" + m_Attribute + "] ");
		}
		sb.Append(m_Prop.m_Name + " (\"" + m_Prop.m_Description + "\", ");
		switch (m_Prop.m_Type)
		{
		case SerializedPropertyType.Color:
			sb.Append("Color");
			break;
		case SerializedPropertyType.Vector:
			sb.Append("Vector");
			break;
		case SerializedPropertyType.Float:
			sb.Append("Float");
			break;
		case SerializedPropertyType.Range:
			sb.Append($"Range({m_Prop.m_DefValue[1]}, {m_Prop.m_DefValue[2]})");
			break;
		case SerializedPropertyType.Texture:
			switch (m_Prop.m_DefTexture.m_TexDim)
			{
			case TextureDimension.Any:
				sb.Append("any");
				break;
			case TextureDimension.Tex2D:
				sb.Append("2D");
				break;
			case TextureDimension.Tex3D:
				sb.Append("3D");
				break;
			case TextureDimension.Cube:
				sb.Append("Cube");
				break;
			case TextureDimension.Tex2DArray:
				sb.Append("2DArray");
				break;
			case TextureDimension.CubeArray:
				sb.Append("CubeArray");
				break;
			}
			break;
		}
		sb.Append(") = ");
		switch (m_Prop.m_Type)
		{
		case SerializedPropertyType.Color:
		case SerializedPropertyType.Vector:
			sb.Append($"({m_Prop.m_DefValue[0]},{m_Prop.m_DefValue[1]},{m_Prop.m_DefValue[2]},{m_Prop.m_DefValue[3]})");
			break;
		case SerializedPropertyType.Float:
		case SerializedPropertyType.Range:
			sb.Append(m_Prop.m_DefValue[0]);
			break;
		case SerializedPropertyType.Texture:
			sb.Append("\"" + m_Prop.m_DefTexture.m_DefaultName + "\" { }");
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		sb.Append("\n");
		return sb.ToString();
	}

	private static bool CheckGpuProgramUsable(ShaderCompilerPlatform platform, ShaderGpuProgramType programType)
	{
		switch (platform)
		{
		case ShaderCompilerPlatform.GL:
			return programType == ShaderGpuProgramType.GLLegacy;
		case ShaderCompilerPlatform.D3D9:
			if (programType != ShaderGpuProgramType.DX9VertexSM20 && programType != ShaderGpuProgramType.DX9VertexSM30 && programType != ShaderGpuProgramType.DX9PixelSM20)
			{
				return programType == ShaderGpuProgramType.DX9PixelSM30;
			}
			return true;
		case ShaderCompilerPlatform.Xbox360:
		case ShaderCompilerPlatform.PS3:
		case ShaderCompilerPlatform.PSP2:
		case ShaderCompilerPlatform.PS4:
		case ShaderCompilerPlatform.XboxOne:
		case ShaderCompilerPlatform.N3DS:
		case ShaderCompilerPlatform.WiiU:
		case ShaderCompilerPlatform.Switch:
		case ShaderCompilerPlatform.XboxOneD3D12:
		case ShaderCompilerPlatform.GameCoreXboxOne:
		case ShaderCompilerPlatform.GameCoreScarlett:
		case ShaderCompilerPlatform.PS5:
			if (programType != ShaderGpuProgramType.ConsoleVS && programType != ShaderGpuProgramType.ConsoleFS && programType != ShaderGpuProgramType.ConsoleHS && programType != ShaderGpuProgramType.ConsoleDS)
			{
				return programType == ShaderGpuProgramType.ConsoleGS;
			}
			return true;
		case ShaderCompilerPlatform.PS5NGGC:
			return programType == ShaderGpuProgramType.PS5NGGC;
		case ShaderCompilerPlatform.D3D11:
			if (programType != ShaderGpuProgramType.DX11VertexSM40 && programType != ShaderGpuProgramType.DX11VertexSM50 && programType != ShaderGpuProgramType.DX11PixelSM40 && programType != ShaderGpuProgramType.DX11PixelSM50 && programType != ShaderGpuProgramType.DX11GeometrySM40 && programType != ShaderGpuProgramType.DX11GeometrySM50 && programType != ShaderGpuProgramType.DX11HullSM50)
			{
				return programType == ShaderGpuProgramType.DX11DomainSM50;
			}
			return true;
		case ShaderCompilerPlatform.GLES20:
			return programType == ShaderGpuProgramType.GLES;
		case ShaderCompilerPlatform.NaCl:
			throw new NotSupportedException();
		case ShaderCompilerPlatform.Flash:
			throw new NotSupportedException();
		case ShaderCompilerPlatform.D3D11_9x:
			if (programType != ShaderGpuProgramType.DX10Level9Vertex)
			{
				return programType == ShaderGpuProgramType.DX10Level9Pixel;
			}
			return true;
		case ShaderCompilerPlatform.GLES3Plus:
			if (programType != ShaderGpuProgramType.GLES31AEP && programType != ShaderGpuProgramType.GLES31)
			{
				return programType == ShaderGpuProgramType.GLES3;
			}
			return true;
		case ShaderCompilerPlatform.PSM:
			throw new NotSupportedException();
		case ShaderCompilerPlatform.Metal:
			if (programType != ShaderGpuProgramType.MetalVS)
			{
				return programType == ShaderGpuProgramType.MetalFS;
			}
			return true;
		case ShaderCompilerPlatform.OpenGLCore:
			if (programType != ShaderGpuProgramType.GLCore32 && programType != ShaderGpuProgramType.GLCore41)
			{
				return programType == ShaderGpuProgramType.GLCore43;
			}
			return true;
		case ShaderCompilerPlatform.Vulkan:
			return programType == ShaderGpuProgramType.SPIRV;
		default:
			throw new NotSupportedException();
		}
	}

	public static string GetPlatformString(ShaderCompilerPlatform platform)
	{
		return platform switch
		{
			ShaderCompilerPlatform.GL => "openGL", 
			ShaderCompilerPlatform.D3D9 => "d3d9", 
			ShaderCompilerPlatform.Xbox360 => "xbox360", 
			ShaderCompilerPlatform.PS3 => "ps3", 
			ShaderCompilerPlatform.D3D11 => "d3d11", 
			ShaderCompilerPlatform.GLES20 => "gles", 
			ShaderCompilerPlatform.NaCl => "glesdesktop", 
			ShaderCompilerPlatform.Flash => "flash", 
			ShaderCompilerPlatform.D3D11_9x => "d3d11_9x", 
			ShaderCompilerPlatform.GLES3Plus => "gles3", 
			ShaderCompilerPlatform.PSP2 => "psp2", 
			ShaderCompilerPlatform.PS4 => "ps4", 
			ShaderCompilerPlatform.XboxOne => "xboxone", 
			ShaderCompilerPlatform.PSM => "psm", 
			ShaderCompilerPlatform.Metal => "metal", 
			ShaderCompilerPlatform.OpenGLCore => "glcore", 
			ShaderCompilerPlatform.N3DS => "n3ds", 
			ShaderCompilerPlatform.WiiU => "wiiu", 
			ShaderCompilerPlatform.Vulkan => "vulkan", 
			ShaderCompilerPlatform.Switch => "switch", 
			ShaderCompilerPlatform.XboxOneD3D12 => "xboxone_d3d12", 
			ShaderCompilerPlatform.GameCoreXboxOne => "xboxone", 
			ShaderCompilerPlatform.GameCoreScarlett => "xbox_scarlett", 
			ShaderCompilerPlatform.PS5 => "ps5", 
			ShaderCompilerPlatform.PS5NGGC => "ps5_nggc", 
			_ => "unknown", 
		};
	}
}
