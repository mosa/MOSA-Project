using System.ComponentModel;
using System.Configuration.Assemblies;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Reflection;

public sealed class AssemblyName : ICloneable, IDeserializationCallback, ISerializable
{
	[Obsolete("AssemblyName.CodeBase and AssemblyName.EscapedCodeBase are obsolete. Using them for loading an assembly is not supported.", DiagnosticId = "SYSLIB0044", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public string? CodeBase
	{
		[RequiresAssemblyFiles("The code will return an empty string for assemblies embedded in a single-file app")]
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public AssemblyContentType ContentType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CultureInfo? CultureInfo
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? CultureName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[RequiresAssemblyFiles("The code will return an empty string for assemblies embedded in a single-file app")]
	[Obsolete("AssemblyName.CodeBase and AssemblyName.EscapedCodeBase are obsolete. Using them for loading an assembly is not supported.", DiagnosticId = "SYSLIB0044", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public string? EscapedCodeBase
	{
		get
		{
			throw null;
		}
	}

	public AssemblyNameFlags Flags
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string FullName
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("AssemblyName members HashAlgorithm, ProcessorArchitecture, and VersionCompatibility are obsolete and not supported.", DiagnosticId = "SYSLIB0037", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public System.Configuration.Assemblies.AssemblyHashAlgorithm HashAlgorithm
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("Strong name signing is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0017", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public StrongNameKeyPair? KeyPair
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("AssemblyName members HashAlgorithm, ProcessorArchitecture, and VersionCompatibility are obsolete and not supported.", DiagnosticId = "SYSLIB0037", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public ProcessorArchitecture ProcessorArchitecture
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Version? Version
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("AssemblyName members HashAlgorithm, ProcessorArchitecture, and VersionCompatibility are obsolete and not supported.", DiagnosticId = "SYSLIB0037", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public AssemblyVersionCompatibility VersionCompatibility
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public AssemblyName()
	{
	}

	public AssemblyName(string assemblyName)
	{
	}

	public object Clone()
	{
		throw null;
	}

	public static AssemblyName GetAssemblyName(string assemblyFile)
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public byte[]? GetPublicKey()
	{
		throw null;
	}

	public byte[]? GetPublicKeyToken()
	{
		throw null;
	}

	public void OnDeserialization(object? sender)
	{
	}

	public static bool ReferenceMatchesDefinition(AssemblyName? reference, AssemblyName? definition)
	{
		throw null;
	}

	public void SetPublicKey(byte[]? publicKey)
	{
	}

	public void SetPublicKeyToken(byte[]? publicKeyToken)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
