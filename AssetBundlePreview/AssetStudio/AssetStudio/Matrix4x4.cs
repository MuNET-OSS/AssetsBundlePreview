using System;
using System.Runtime.InteropServices;

namespace AssetStudio;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Matrix4x4 : IEquatable<Matrix4x4>
{
	public float M00;

	public float M10;

	public float M20;

	public float M30;

	public float M01;

	public float M11;

	public float M21;

	public float M31;

	public float M02;

	public float M12;

	public float M22;

	public float M32;

	public float M03;

	public float M13;

	public float M23;

	public float M33;

	public float this[int row, int column]
	{
		get
		{
			return this[row + column * 4];
		}
		set
		{
			this[row + column * 4] = value;
		}
	}

	public float this[int index]
	{
		get
		{
			return index switch
			{
				0 => M00, 
				1 => M10, 
				2 => M20, 
				3 => M30, 
				4 => M01, 
				5 => M11, 
				6 => M21, 
				7 => M31, 
				8 => M02, 
				9 => M12, 
				10 => M22, 
				11 => M32, 
				12 => M03, 
				13 => M13, 
				14 => M23, 
				15 => M33, 
				_ => throw new ArgumentOutOfRangeException("index", "Invalid Matrix4x4 index!"), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				M00 = value;
				break;
			case 1:
				M10 = value;
				break;
			case 2:
				M20 = value;
				break;
			case 3:
				M30 = value;
				break;
			case 4:
				M01 = value;
				break;
			case 5:
				M11 = value;
				break;
			case 6:
				M21 = value;
				break;
			case 7:
				M31 = value;
				break;
			case 8:
				M02 = value;
				break;
			case 9:
				M12 = value;
				break;
			case 10:
				M22 = value;
				break;
			case 11:
				M32 = value;
				break;
			case 12:
				M03 = value;
				break;
			case 13:
				M13 = value;
				break;
			case 14:
				M23 = value;
				break;
			case 15:
				M33 = value;
				break;
			default:
				throw new ArgumentOutOfRangeException("index", "Invalid Matrix4x4 index!");
			}
		}
	}

	public Matrix4x4(float[] values)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		if (values.Length != 16)
		{
			throw new ArgumentOutOfRangeException("values", "There must be sixteen and only sixteen input values for Matrix.");
		}
		M00 = values[0];
		M10 = values[1];
		M20 = values[2];
		M30 = values[3];
		M01 = values[4];
		M11 = values[5];
		M21 = values[6];
		M31 = values[7];
		M02 = values[8];
		M12 = values[9];
		M22 = values[10];
		M32 = values[11];
		M03 = values[12];
		M13 = values[13];
		M23 = values[14];
		M33 = values[15];
	}

	public override int GetHashCode()
	{
		return GetColumn(0).GetHashCode() ^ (GetColumn(1).GetHashCode() << 2) ^ (GetColumn(2).GetHashCode() >> 2) ^ (GetColumn(3).GetHashCode() >> 1);
	}

	public override bool Equals(object other)
	{
		if (!(other is Matrix4x4))
		{
			return false;
		}
		return Equals((Matrix4x4)other);
	}

	public bool Equals(Matrix4x4 other)
	{
		if (GetColumn(0).Equals(other.GetColumn(0)) && GetColumn(1).Equals(other.GetColumn(1)) && GetColumn(2).Equals(other.GetColumn(2)))
		{
			return GetColumn(3).Equals(other.GetColumn(3));
		}
		return false;
	}

	public Vector4 GetColumn(int index)
	{
		return index switch
		{
			0 => new Vector4(M00, M10, M20, M30), 
			1 => new Vector4(M01, M11, M21, M31), 
			2 => new Vector4(M02, M12, M22, M32), 
			3 => new Vector4(M03, M13, M23, M33), 
			_ => throw new IndexOutOfRangeException("Invalid column index!"), 
		};
	}

	public Vector4 GetRow(int index)
	{
		return index switch
		{
			0 => new Vector4(M00, M01, M02, M03), 
			1 => new Vector4(M10, M11, M12, M13), 
			2 => new Vector4(M20, M21, M22, M23), 
			3 => new Vector4(M30, M31, M32, M33), 
			_ => throw new IndexOutOfRangeException("Invalid row index!"), 
		};
	}

	public static Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs)
	{
		Matrix4x4 res = default(Matrix4x4);
		res.M00 = lhs.M00 * rhs.M00 + lhs.M01 * rhs.M10 + lhs.M02 * rhs.M20 + lhs.M03 * rhs.M30;
		res.M01 = lhs.M00 * rhs.M01 + lhs.M01 * rhs.M11 + lhs.M02 * rhs.M21 + lhs.M03 * rhs.M31;
		res.M02 = lhs.M00 * rhs.M02 + lhs.M01 * rhs.M12 + lhs.M02 * rhs.M22 + lhs.M03 * rhs.M32;
		res.M03 = lhs.M00 * rhs.M03 + lhs.M01 * rhs.M13 + lhs.M02 * rhs.M23 + lhs.M03 * rhs.M33;
		res.M10 = lhs.M10 * rhs.M00 + lhs.M11 * rhs.M10 + lhs.M12 * rhs.M20 + lhs.M13 * rhs.M30;
		res.M11 = lhs.M10 * rhs.M01 + lhs.M11 * rhs.M11 + lhs.M12 * rhs.M21 + lhs.M13 * rhs.M31;
		res.M12 = lhs.M10 * rhs.M02 + lhs.M11 * rhs.M12 + lhs.M12 * rhs.M22 + lhs.M13 * rhs.M32;
		res.M13 = lhs.M10 * rhs.M03 + lhs.M11 * rhs.M13 + lhs.M12 * rhs.M23 + lhs.M13 * rhs.M33;
		res.M20 = lhs.M20 * rhs.M00 + lhs.M21 * rhs.M10 + lhs.M22 * rhs.M20 + lhs.M23 * rhs.M30;
		res.M21 = lhs.M20 * rhs.M01 + lhs.M21 * rhs.M11 + lhs.M22 * rhs.M21 + lhs.M23 * rhs.M31;
		res.M22 = lhs.M20 * rhs.M02 + lhs.M21 * rhs.M12 + lhs.M22 * rhs.M22 + lhs.M23 * rhs.M32;
		res.M23 = lhs.M20 * rhs.M03 + lhs.M21 * rhs.M13 + lhs.M22 * rhs.M23 + lhs.M23 * rhs.M33;
		res.M30 = lhs.M30 * rhs.M00 + lhs.M31 * rhs.M10 + lhs.M32 * rhs.M20 + lhs.M33 * rhs.M30;
		res.M31 = lhs.M30 * rhs.M01 + lhs.M31 * rhs.M11 + lhs.M32 * rhs.M21 + lhs.M33 * rhs.M31;
		res.M32 = lhs.M30 * rhs.M02 + lhs.M31 * rhs.M12 + lhs.M32 * rhs.M22 + lhs.M33 * rhs.M32;
		res.M33 = lhs.M30 * rhs.M03 + lhs.M31 * rhs.M13 + lhs.M32 * rhs.M23 + lhs.M33 * rhs.M33;
		return res;
	}

	public static bool operator ==(Matrix4x4 lhs, Matrix4x4 rhs)
	{
		if (lhs.GetColumn(0) == rhs.GetColumn(0) && lhs.GetColumn(1) == rhs.GetColumn(1) && lhs.GetColumn(2) == rhs.GetColumn(2))
		{
			return lhs.GetColumn(3) == rhs.GetColumn(3);
		}
		return false;
	}

	public static bool operator !=(Matrix4x4 lhs, Matrix4x4 rhs)
	{
		return !(lhs == rhs);
	}

	public static Matrix4x4 Scale(Vector3 vector)
	{
		Matrix4x4 i = default(Matrix4x4);
		i.M00 = vector.X;
		i.M01 = 0f;
		i.M02 = 0f;
		i.M03 = 0f;
		i.M10 = 0f;
		i.M11 = vector.Y;
		i.M12 = 0f;
		i.M13 = 0f;
		i.M20 = 0f;
		i.M21 = 0f;
		i.M22 = vector.Z;
		i.M23 = 0f;
		i.M30 = 0f;
		i.M31 = 0f;
		i.M32 = 0f;
		i.M33 = 1f;
		return i;
	}

	public static Matrix4x4 Translate(Vector3 vector)
	{
		Matrix4x4 i = default(Matrix4x4);
		i.M00 = 1f;
		i.M01 = 0f;
		i.M02 = 0f;
		i.M03 = vector.X;
		i.M10 = 0f;
		i.M11 = 1f;
		i.M12 = 0f;
		i.M13 = vector.Y;
		i.M20 = 0f;
		i.M21 = 0f;
		i.M22 = 1f;
		i.M23 = vector.Z;
		i.M30 = 0f;
		i.M31 = 0f;
		i.M32 = 0f;
		i.M33 = 1f;
		return i;
	}

	public static Matrix4x4 Rotate(Quaternion q)
	{
		float x = q.X * 2f;
		float y = q.Y * 2f;
		float z = q.Z * 2f;
		float xx = q.X * x;
		float yy = q.Y * y;
		float zz = q.Z * z;
		float xy = q.X * y;
		float xz = q.X * z;
		float yz = q.Y * z;
		float wx = q.W * x;
		float wy = q.W * y;
		float wz = q.W * z;
		Matrix4x4 i = default(Matrix4x4);
		i.M00 = 1f - (yy + zz);
		i.M10 = xy + wz;
		i.M20 = xz - wy;
		i.M30 = 0f;
		i.M01 = xy - wz;
		i.M11 = 1f - (xx + zz);
		i.M21 = yz + wx;
		i.M31 = 0f;
		i.M02 = xz + wy;
		i.M12 = yz - wx;
		i.M22 = 1f - (xx + yy);
		i.M32 = 0f;
		i.M03 = 0f;
		i.M13 = 0f;
		i.M23 = 0f;
		i.M33 = 1f;
		return i;
	}
}
