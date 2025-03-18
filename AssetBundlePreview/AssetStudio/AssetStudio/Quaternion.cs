using System;
using System.Runtime.InteropServices;

namespace AssetStudio;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Quaternion : IEquatable<Quaternion>
{
	public float X;

	public float Y;

	public float Z;

	public float W;

	private const float kEpsilon = 1E-06f;

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
				_ => throw new ArgumentOutOfRangeException("index", "Invalid Quaternion index!"), 
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
				throw new ArgumentOutOfRangeException("index", "Invalid Quaternion index!");
			}
		}
	}

	public Quaternion(float x, float y, float z, float w)
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}

	public override int GetHashCode()
	{
		return X.GetHashCode() ^ (Y.GetHashCode() << 2) ^ (Z.GetHashCode() >> 2) ^ (W.GetHashCode() >> 1);
	}

	public override bool Equals(object other)
	{
		if (!(other is Quaternion))
		{
			return false;
		}
		return Equals((Quaternion)other);
	}

	public bool Equals(Quaternion other)
	{
		if (X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z))
		{
			return W.Equals(other.W);
		}
		return false;
	}

	public static float Dot(Quaternion a, Quaternion b)
	{
		return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
	}

	private static bool IsEqualUsingDot(float dot)
	{
		return dot > 0.999999f;
	}

	public static bool operator ==(Quaternion lhs, Quaternion rhs)
	{
		return IsEqualUsingDot(Dot(lhs, rhs));
	}

	public static bool operator !=(Quaternion lhs, Quaternion rhs)
	{
		return !(lhs == rhs);
	}
}
