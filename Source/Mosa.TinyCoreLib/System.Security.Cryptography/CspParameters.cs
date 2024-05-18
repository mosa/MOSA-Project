using System.Runtime.Versioning;

namespace System.Security.Cryptography;

[SupportedOSPlatform("windows")]
public sealed class CspParameters
{
	public string? KeyContainerName;

	public int KeyNumber;

	public string? ProviderName;

	public int ProviderType;

	public CspProviderFlags Flags
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[CLSCompliant(false)]
	public SecureString? KeyPassword
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IntPtr ParentWindowHandle
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CspParameters()
	{
	}

	public CspParameters(int dwTypeIn)
	{
	}

	public CspParameters(int dwTypeIn, string? strProviderNameIn)
	{
	}

	public CspParameters(int dwTypeIn, string? strProviderNameIn, string? strContainerNameIn)
	{
	}
}
