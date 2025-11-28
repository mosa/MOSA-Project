using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

[EditorBrowsable(EditorBrowsableState.Never)]
public class PasswordDeriveBytes : DeriveBytes
{
	public string HashName
	{
		get
		{
			throw null;
		}
		[RequiresUnreferencedCode("The hash implementation might be removed. Ensure the referenced hash algorithm is not trimmed.")]
		set
		{
		}
	}

	public int IterationCount
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[]? Salt
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PasswordDeriveBytes(byte[] password, byte[]? salt)
	{
	}

	public PasswordDeriveBytes(byte[] password, byte[]? salt, CspParameters? cspParams)
	{
	}

	[RequiresUnreferencedCode("The hash implementation might be removed. Ensure the referenced hash algorithm is not trimmed.")]
	public PasswordDeriveBytes(byte[] password, byte[]? salt, string hashName, int iterations)
	{
	}

	[RequiresUnreferencedCode("The hash implementation might be removed. Ensure the referenced hash algorithm is not trimmed.")]
	public PasswordDeriveBytes(byte[] password, byte[]? salt, string hashName, int iterations, CspParameters? cspParams)
	{
	}

	public PasswordDeriveBytes(string strPassword, byte[]? rgbSalt)
	{
	}

	public PasswordDeriveBytes(string strPassword, byte[]? rgbSalt, CspParameters? cspParams)
	{
	}

	[RequiresUnreferencedCode("The hash implementation might be removed. Ensure the referenced hash algorithm is not trimmed.")]
	public PasswordDeriveBytes(string strPassword, byte[]? rgbSalt, string strHashName, int iterations)
	{
	}

	[RequiresUnreferencedCode("The hash implementation might be removed. Ensure the referenced hash algorithm is not trimmed.")]
	public PasswordDeriveBytes(string strPassword, byte[]? rgbSalt, string strHashName, int iterations, CspParameters? cspParams)
	{
	}

	[SupportedOSPlatform("windows")]
	public byte[] CryptDeriveKey(string? algname, string? alghashname, int keySize, byte[] rgbIV)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	[Obsolete("Rfc2898DeriveBytes replaces PasswordDeriveBytes for deriving key material from a password and is preferred in new applications.")]
	public override byte[] GetBytes(int cb)
	{
		throw null;
	}

	public override void Reset()
	{
	}
}
