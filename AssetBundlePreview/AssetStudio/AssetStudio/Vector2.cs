using System;
using System.Runtime.InteropServices;

namespace AssetStudio;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Vector2 : IEquatable<Vector2>
{
	public float X;

	public float Y;

	private const float kEpsilon = 1E-05f;

	public float this[int index]
	{
		get
		{
			return index switch
			{
				0 => X, 
				1 => Y, 
				_ => throw new ArgumentOutOfRangeException("index", "Invalid Vector2 index!"), 
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
			default:
				throw new ArgumentOutOfRangeException("index", "Invalid Vector2 index!");
			}
		}
	}

	public static Vector2 Zero => default(Vector2);

	public Vector2(float x, float y)
	{
		X = x;
		Y = y;
	}

	public override int GetHashCode()
	{
		return X.GetHashCode() ^ (Y.GetHashCode() << 2);
	}

	public override bool Equals(object other)
	{
		if (!(other is Vector2))
		{
			return false;
		}
		return Equals((Vector2)other);
	}

	public bool Equals(Vector2 other)
	{
		if (X.Equals(other.X))
		{
			return Y.Equals(other.Y);
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
		}
		else
		{
			X = 0f;
			Y = 0f;
		}
	}

	public float Length()
	{
		return (float)Math.Sqrt(LengthSquared());
	}

	public float LengthSquared()
	{
		return X * X + Y * Y;
	}

	public static Vector2 operator +(Vector2 a, Vector2 b)
	{
		return new Vector2(a.X + b.X, a.Y + b.Y);
	}

	public static Vector2 operator -(Vector2 a, Vector2 b)
	{
		return new Vector2(a.X - b.X, a.Y - b.Y);
	}

	public static Vector2 operator *(Vector2 a, Vector2 b)
	{
		return new Vector2(a.X * b.X, a.Y * b.Y);
	}

	public static Vector2 operator /(Vector2 a, Vector2 b)
	{
		return new Vector2(a.X / b.X, a.Y / b.Y);
	}

	public static Vector2 operator -(Vector2 a)
	{
		return new Vector2(0f - a.X, 0f - a.Y);
	}

	public static Vector2 operator *(Vector2 a, float d)
	{
		return new Vector2(a.X * d, a.Y * d);
	}

	public static Vector2 operator *(float d, Vector2 a)
	{
		return new Vector2(a.X * d, a.Y * d);
	}

	public static Vector2 operator /(Vector2 a, float d)
	{
		return new Vector2(a.X / d, a.Y / d);
	}

	public static bool operator ==(Vector2 lhs, Vector2 rhs)
	{
		return (lhs - rhs).LengthSquared() < 9.9999994E-11f;
	}

	public static bool operator !=(Vector2 lhs, Vector2 rhs)
	{
		return !(lhs == rhs);
	}

	public static implicit operator Vector3(Vector2 v)
	{
		return new Vector3(v.X, v.Y, 0f);
	}

	public static implicit operator Vector4(Vector2 v)
	{
		return new Vector4(v.X, v.Y, 0f, 0f);
	}
}
