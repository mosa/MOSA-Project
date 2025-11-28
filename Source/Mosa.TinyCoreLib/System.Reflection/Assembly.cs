using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Assemblies;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;

namespace System.Reflection;

public abstract class Assembly : ICustomAttributeProvider, ISerializable
{
	[Obsolete("Assembly.CodeBase and Assembly.EscapedCodeBase are only included for .NET Framework compatibility. Use Assembly.Location.", DiagnosticId = "SYSLIB0012", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[RequiresAssemblyFiles("This member throws an exception for assemblies embedded in a single-file app")]
	public virtual string? CodeBase
	{
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<CustomAttributeData> CustomAttributes
	{
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<TypeInfo> DefinedTypes
	{
		[RequiresUnreferencedCode("Types might be removed")]
		get
		{
			throw null;
		}
	}

	public virtual MethodInfo? EntryPoint
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Assembly.CodeBase and Assembly.EscapedCodeBase are only included for .NET Framework compatibility. Use Assembly.Location.", DiagnosticId = "SYSLIB0012", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[RequiresAssemblyFiles("This member throws an exception for assemblies embedded in a single-file app")]
	public virtual string EscapedCodeBase
	{
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<Type> ExportedTypes
	{
		[RequiresUnreferencedCode("Types might be removed")]
		get
		{
			throw null;
		}
	}

	public virtual string? FullName
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("The Global Assembly Cache is not supported.", DiagnosticId = "SYSLIB0005", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public virtual bool GlobalAssemblyCache
	{
		get
		{
			throw null;
		}
	}

	public virtual long HostContext
	{
		get
		{
			throw null;
		}
	}

	public virtual string ImageRuntimeVersion
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsCollectible
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsDynamic
	{
		get
		{
			throw null;
		}
	}

	public bool IsFullyTrusted
	{
		get
		{
			throw null;
		}
	}

	public virtual string Location
	{
		get
		{
			throw null;
		}
	}

	public virtual Module ManifestModule
	{
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<Module> Modules
	{
		get
		{
			throw null;
		}
	}

	public virtual bool ReflectionOnly
	{
		get
		{
			throw null;
		}
	}

	public virtual SecurityRuleSet SecurityRuleSet
	{
		get
		{
			throw null;
		}
	}

	public virtual event ModuleResolveEventHandler? ModuleResolve
	{
		add
		{
		}
		remove
		{
		}
	}

	[RequiresUnreferencedCode("Assembly.CreateInstance is not supported with trimming. Use Type.GetType instead.")]
	public object? CreateInstance(string typeName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Assembly.CreateInstance is not supported with trimming. Use Type.GetType instead.")]
	public object? CreateInstance(string typeName, bool ignoreCase)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Assembly.CreateInstance is not supported with trimming. Use Type.GetType instead.")]
	public virtual object? CreateInstance(string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder? binder, object[]? args, CultureInfo? culture, object[]? activationAttributes)
	{
		throw null;
	}

	public static string CreateQualifiedName(string? assemblyName, string? typeName)
	{
		throw null;
	}

	public override bool Equals(object? o)
	{
		throw null;
	}

	public static Assembly? GetAssembly(Type type)
	{
		throw null;
	}

	public static Assembly GetCallingAssembly()
	{
		throw null;
	}

	public virtual object[] GetCustomAttributes(bool inherit)
	{
		throw null;
	}

	public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw null;
	}

	public virtual IList<CustomAttributeData> GetCustomAttributesData()
	{
		throw null;
	}

	public static Assembly? GetEntryAssembly()
	{
		throw null;
	}

	public static Assembly GetExecutingAssembly()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public virtual Type[] GetExportedTypes()
	{
		throw null;
	}

	[RequiresAssemblyFiles("This member throws an exception for assemblies embedded in a single-file app")]
	public virtual FileStream? GetFile(string name)
	{
		throw null;
	}

	[RequiresAssemblyFiles("This member throws an exception for assemblies embedded in a single-file app")]
	public virtual FileStream[] GetFiles()
	{
		throw null;
	}

	[RequiresAssemblyFiles("This member throws an exception for assemblies embedded in a single-file app")]
	public virtual FileStream[] GetFiles(bool getResourceModules)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public virtual Type[] GetForwardedTypes()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public Module[] GetLoadedModules()
	{
		throw null;
	}

	public virtual Module[] GetLoadedModules(bool getResourceModules)
	{
		throw null;
	}

	public virtual ManifestResourceInfo? GetManifestResourceInfo(string resourceName)
	{
		throw null;
	}

	public virtual string[] GetManifestResourceNames()
	{
		throw null;
	}

	public virtual Stream? GetManifestResourceStream(string name)
	{
		throw null;
	}

	public virtual Stream? GetManifestResourceStream(Type type, string name)
	{
		throw null;
	}

	public virtual Module? GetModule(string name)
	{
		throw null;
	}

	public Module[] GetModules()
	{
		throw null;
	}

	public virtual Module[] GetModules(bool getResourceModules)
	{
		throw null;
	}

	public virtual AssemblyName GetName()
	{
		throw null;
	}

	public virtual AssemblyName GetName(bool copiedName)
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	[RequiresUnreferencedCode("Assembly references might be removed")]
	public virtual AssemblyName[] GetReferencedAssemblies()
	{
		throw null;
	}

	public virtual Assembly GetSatelliteAssembly(CultureInfo culture)
	{
		throw null;
	}

	public virtual Assembly GetSatelliteAssembly(CultureInfo culture, Version? version)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public virtual Type? GetType(string name)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public virtual Type? GetType(string name, bool throwOnError)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public virtual Type? GetType(string name, bool throwOnError, bool ignoreCase)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public virtual Type[] GetTypes()
	{
		throw null;
	}

	public virtual bool IsDefined(Type attributeType, bool inherit)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public static Assembly Load(byte[] rawAssembly)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public static Assembly Load(byte[] rawAssembly, byte[]? rawSymbolStore)
	{
		throw null;
	}

	public static Assembly Load(AssemblyName assemblyRef)
	{
		throw null;
	}

	public static Assembly Load(string assemblyString)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public static Assembly LoadFile(string path)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public static Assembly LoadFrom(string assemblyFile)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public static Assembly LoadFrom(string assemblyFile, byte[]? hashValue, System.Configuration.Assemblies.AssemblyHashAlgorithm hashAlgorithm)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded module depends on might be removed")]
	public Module LoadModule(string moduleName, byte[]? rawModule)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded module depends on might be removed")]
	public virtual Module LoadModule(string moduleName, byte[]? rawModule, byte[]? rawSymbolStore)
	{
		throw null;
	}

	[Obsolete("Assembly.LoadWithPartialName has been deprecated. Use Assembly.Load() instead.")]
	public static Assembly? LoadWithPartialName(string partialName)
	{
		throw null;
	}

	public static bool operator ==(Assembly? left, Assembly? right)
	{
		throw null;
	}

	public static bool operator !=(Assembly? left, Assembly? right)
	{
		throw null;
	}

	[Obsolete("ReflectionOnly loading is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0018", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public static Assembly ReflectionOnlyLoad(byte[] rawAssembly)
	{
		throw null;
	}

	[Obsolete("ReflectionOnly loading is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0018", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public static Assembly ReflectionOnlyLoad(string assemblyString)
	{
		throw null;
	}

	[Obsolete("ReflectionOnly loading is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0018", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public static Assembly ReflectionOnlyLoadFrom(string assemblyFile)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public static Assembly UnsafeLoadFrom(string assemblyFile)
	{
		throw null;
	}
}
