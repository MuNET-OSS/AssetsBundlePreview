using System;
using System.Runtime.InteropServices;

namespace AssetStudio;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Vector3 : IEquatable<Vector3>
{
	public float X;

	public float Y;

	public float Z;

	private const float kEpsilon = 1E-05f;

	public float this[int index]
	{
		get
		{
			return index switch
			{
				0 => X, 
				1 => Y, 
				2 => Z, 
				_ => throw new ArgumentOutOfRangeException("index", "Invalid Vector3 index!"), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				X = value;
				break;
			case 1:
				Y = value;
				break;
			case 2:
				Z = value;
				break;
			default:
				throw new ArgumentOutOfRangeException("index", "Invalid Vector3 index!");
			}
		}
	}

	public static Vector3 Zero => default(Vector3);

	public static Vector3 One => new Vector3(1f, 1f, 1f);

	public Vector3(float x, float y, float z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public override int GetHashCode()
	{
		return X.GetHashCode() ^ (Y.GetHashCode() << 2) ^ (Z.GetHashCode() >> 2);
	}

	public override bool Equals(object other)
	{
		if (!(other is Vector3))
		{
			return false;
		}
		return Equals((Vector3)other);
	}

	public bool Equals(Vector3 other)
	{
		if (X.Equals(other.X) && Y.Equals(other.Y))
		{
			return Z.Equals(other.Z);
		}
		return false;
	}

	public void Normalize()
	{
		float length = Length();
		if (length > 1E-05f)
		{
			float invNorm = 1f / length;
			X *= invNorm;
			Y *= invNorm;
			Z *= invNorm;
		}
		else
		{
			X = 0f;
			Y = 0f;
			Z = 0f;
		}
	}

	public float Length()
	{
		return (float)Math.Sqrt(LengthSquared());
	}

	public float LengthSquared()
	{
		return X * X + Y * Y + Z * Z;
	}

	public static Vector3 operator +(Vector3 a, Vector3 b)
	{
		return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
	}

	public static Vector3 operator -(Vector3 a, Vector3 b)
	{
		return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
	}

	public static Vector3 operator -(Vector3 a)
	{
		return new Vector3(0f - a.X, 0f - a.Y, 0f - a.Z);
	}

	public static Vector3 operator *(Vector3 a, float d)
	{
		return new Vector3(a.X * d, a.Y * d, a.Z * d);
	}

	public static Vector3 operator *(float d, Vector3 a)
	{
		return new Vector3(a.X * d, a.Y * d, a.Z * d);
	}

	public static Vector3 operator /(Vector3 a, float d)
	{
		return new Vector3(a.X / d, a.Y / d, a.Z / d);
	}

	public static bool operator ==(Vector3 lhs, Vector3 rhs)
	{
		return (lhs - rhs).LengthSquared() < 9.9999994E-11f;
	}

	public static bool operator !=(Vector3 lhs, Vector3 rhs)
	{
		return !(lhs == rhs);
	}

	public static implicit operator Vector2(Vector3 v)
	{
		return new Vector2(v.X, v.Y);
	}

	public static implicit operator Vector4(Vector3 v)
	{
		return new Vector4(v.X, v.Y, v.Z, 0f);
	}
}
