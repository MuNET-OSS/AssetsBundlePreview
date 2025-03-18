using System;
using System.Runtime.InteropServices;

namespace AssetStudio;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Vector4 : IEquatable<Vector4>
{
	public float X;

	public float Y;

	public float Z;

	public float W;

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
				3 => W, 
				_ => throw new ArgumentOutOfRangeException("index", "Invalid Vector4 index!"), 
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
			case 3:
				W = value;
				break;
			default:
				throw new ArgumentOutOfRangeException("index", "Invalid Vector4 index!");
			}
		}
	}

	public static Vector4 Zero => default(Vector4);

	public Vector4(float x, float y, float z, float w)
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}

	public Vector4(Vector3 value, float w)
	{
		X = value.X;
		Y = value.Y;
		Z = value.Z;
		W = w;
	}

	public override int GetHashCode()
	{
		return X.GetHashCode() ^ (Y.GetHashCode() << 2) ^ (Z.GetHashCode() >> 2) ^ (W.GetHashCode() >> 1);
	}

	public override bool Equals(object other)
	{
		if (!(other is Vector4))
		{
			return false;
		}
		return Equals((Vector4)other);
	}

	public bool Equals(Vector4 other)
	{
		if (X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z))
		{
			return W.Equals(other.W);
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
			W *= invNorm;
		}
		else
		{
			X = 0f;
			Y = 0f;
			Z = 0f;
			W = 0f;
		}
	}

	public float Length()
	{
		return (float)Math.Sqrt(LengthSquared());
	}

	public float LengthSquared()
	{
		return X * X + Y * Y + Z * Z + W * W;
	}

	public static Vector4 operator +(Vector4 a, Vector4 b)
	{
		return new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
	}

	public static Vector4 operator -(Vector4 a, Vector4 b)
	{
		return new Vector4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
	}

	public static Vector4 operator -(Vector4 a)
	{
		return new Vector4(0f - a.X, 0f - a.Y, 0f - a.Z, 0f - a.W);
	}

	public static Vector4 operator *(Vector4 a, float d)
	{
		return new Vector4(a.X * d, a.Y * d, a.Z * d, a.W * d);
	}

	public static Vector4 operator *(float d, Vector4 a)
	{
		return new Vector4(a.X * d, a.Y * d, a.Z * d, a.W * d);
	}

	public static Vector4 operator /(Vector4 a, float d)
	{
		return new Vector4(a.X / d, a.Y / d, a.Z / d, a.W / d);
	}

	public static bool operator ==(Vector4 lhs, Vector4 rhs)
	{
		return (lhs - rhs).LengthSquared() < 9.9999994E-11f;
	}

	public static bool operator !=(Vector4 lhs, Vector4 rhs)
	{
		return !(lhs == rhs);
	}

	public static implicit operator Vector2(Vector4 v)
	{
		return new Vector2(v.X, v.Y);
	}

	public static implicit operator Vector3(Vector4 v)
	{
		return new Vector3(v.X, v.Y, v.Z);
	}

	public static implicit operator Color(Vector4 v)
	{
		return new Color(v.X, v.Y, v.Z, v.W);
	}
}
