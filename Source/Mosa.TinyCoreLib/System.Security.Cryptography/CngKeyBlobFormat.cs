using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

public sealed class CngKeyBlobFormat : IEquatable<CngKeyBlobFormat>
{
	public static CngKeyBlobFormat EccFullPrivateBlob
	{
		get
		{
			throw null;
		}
	}

	public static CngKeyBlobFormat EccFullPublicBlob
	{
		get
		{
			throw null;
		}
	}

	public static CngKeyBlobFormat EccPrivateBlob
	{
		get
		{
			throw null;
		}
	}

	public static CngKeyBlobFormat EccPublicBlob
	{
		get
		{
			throw null;
		}
	}

	public string Format
	{
		get
		{
			throw null;
		}
	}

	public static CngKeyBlobFormat GenericPrivateBlob
	{
		get
		{
			throw null;
		}
	}

	public static CngKeyBlobFormat GenericPublicBlob
	{
		get
		{
			throw null;
		}
	}

	public static CngKeyBlobFormat OpaqueTransportBlob
	{
		get
		{
			throw null;
		}
	}

	public static CngKeyBlobFormat Pkcs8PrivateBlob
	{
		get
		{
			throw null;
		}
	}

	public CngKeyBlobFormat(string format)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals([NotNullWhen(true)] CngKeyBlobFormat? other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(CngKeyBlobFormat? left, CngKeyBlobFormat? right)
	{
		throw null;
	}

	public static bool operator !=(CngKeyBlobFormat? left, CngKeyBlobFormat? right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
