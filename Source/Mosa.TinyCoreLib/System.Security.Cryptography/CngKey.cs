using System.Runtime.Versioning;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography;

public sealed class CngKey : IDisposable
{
	public CngAlgorithm Algorithm
	{
		get
		{
			throw null;
		}
	}

	public CngAlgorithmGroup? AlgorithmGroup
	{
		get
		{
			throw null;
		}
	}

	public CngExportPolicies ExportPolicy
	{
		get
		{
			throw null;
		}
	}

	public SafeNCryptKeyHandle Handle
	{
		get
		{
			throw null;
		}
	}

	public bool IsEphemeral
	{
		get
		{
			throw null;
		}
	}

	public bool IsMachineKey
	{
		get
		{
			throw null;
		}
	}

	public string? KeyName
	{
		get
		{
			throw null;
		}
	}

	public int KeySize
	{
		get
		{
			throw null;
		}
	}

	public CngKeyUsages KeyUsage
	{
		get
		{
			throw null;
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

	public CngProvider? Provider
	{
		get
		{
			throw null;
		}
	}

	public SafeNCryptProviderHandle ProviderHandle
	{
		get
		{
			throw null;
		}
	}

	public CngUIPolicy UIPolicy
	{
		get
		{
			throw null;
		}
	}

	public string? UniqueName
	{
		get
		{
			throw null;
		}
	}

	internal CngKey()
	{
	}

	[SupportedOSPlatform("windows")]
	public static CngKey Create(CngAlgorithm algorithm)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static CngKey Create(CngAlgorithm algorithm, string? keyName)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static CngKey Create(CngAlgorithm algorithm, string? keyName, CngKeyCreationParameters? creationParameters)
	{
		throw null;
	}

	public void Delete()
	{
	}

	public void Dispose()
	{
	}

	[SupportedOSPlatform("windows")]
	public static bool Exists(string keyName)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static bool Exists(string keyName, CngProvider provider)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static bool Exists(string keyName, CngProvider provider, CngKeyOpenOptions options)
	{
		throw null;
	}

	public byte[] Export(CngKeyBlobFormat format)
	{
		throw null;
	}

	public CngProperty GetProperty(string name, CngPropertyOptions options)
	{
		throw null;
	}

	public bool HasProperty(string name, CngPropertyOptions options)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static CngKey Import(byte[] keyBlob, CngKeyBlobFormat format)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static CngKey Import(byte[] keyBlob, CngKeyBlobFormat format, CngProvider provider)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static CngKey Open(SafeNCryptKeyHandle keyHandle, CngKeyHandleOpenOptions keyHandleOpenOptions)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static CngKey Open(string keyName)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static CngKey Open(string keyName, CngProvider provider)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static CngKey Open(string keyName, CngProvider provider, CngKeyOpenOptions openOptions)
	{
		throw null;
	}

	public void SetProperty(CngProperty property)
	{
	}
}
