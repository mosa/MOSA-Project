using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

public sealed class RSASignaturePadding : IEquatable<RSASignaturePadding>
{
	public RSASignaturePaddingMode Mode
	{
		get
		{
			throw null;
		}
	}

	public static RSASignaturePadding Pkcs1
	{
		get
		{
			throw null;
		}
	}

	public static RSASignaturePadding Pss
	{
		get
		{
			throw null;
		}
	}

	internal RSASignaturePadding()
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals([NotNullWhen(true)] RSASignaturePadding? other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(RSASignaturePadding? left, RSASignaturePadding? right)
	{
		throw null;
	}

	public static bool operator !=(RSASignaturePadding? left, RSASignaturePadding? right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
