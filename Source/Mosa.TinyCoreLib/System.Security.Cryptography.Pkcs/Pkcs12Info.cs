using System.Collections.ObjectModel;

namespace System.Security.Cryptography.Pkcs;

public sealed class Pkcs12Info
{
	public ReadOnlyCollection<Pkcs12SafeContents> AuthenticatedSafe
	{
		get
		{
			throw null;
		}
	}

	public Pkcs12IntegrityMode IntegrityMode
	{
		get
		{
			throw null;
		}
	}

	internal Pkcs12Info()
	{
	}

	public static Pkcs12Info Decode(ReadOnlyMemory<byte> encodedBytes, out int bytesConsumed, bool skipCopy = false)
	{
		throw null;
	}

	public bool VerifyMac(ReadOnlySpan<char> password)
	{
		throw null;
	}

	public bool VerifyMac(string? password)
	{
		throw null;
	}
}
