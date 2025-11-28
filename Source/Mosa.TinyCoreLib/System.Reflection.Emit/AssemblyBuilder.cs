using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

namespace System.Reflection.Emit;

public abstract class AssemblyBuilder : Assembly
{
	[Obsolete("Assembly.CodeBase and Assembly.EscapedCodeBase are only included for .NET Framework compatibility. Use Assembly.Location instead.", DiagnosticId = "SYSLIB0012", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[RequiresAssemblyFiles("This member throws an exception for assemblies embedded in a single-file app")]
	public override string? CodeBase
	{
		get
		{
			throw null;
		}
	}

	public override MethodInfo? EntryPoint
	{
		get
		{
			throw null;
		}
	}

	public override string? FullName
	{
		get
		{
			throw null;
		}
	}

	public override long HostContext
	{
		get
		{
			throw null;
		}
	}

	public override bool IsCollectible
	{
		get
		{
			throw null;
		}
	}

	public override bool IsDynamic
	{
		get
		{
			throw null;
		}
	}

	public override string Location
	{
		get
		{
			throw null;
		}
	}

	public override Module ManifestModule
	{
		get
		{
			throw null;
		}
	}

	public override bool ReflectionOnly
	{
		get
		{
			throw null;
		}
	}

	[RequiresDynamicCode("Defining a dynamic assembly requires dynamic code.")]
	public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
	{
		throw null;
	}

	[RequiresDynamicCode("Defining a dynamic assembly requires dynamic code.")]
	public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, IEnumerable<CustomAttributeBuilder>? assemblyAttributes)
	{
		throw null;
	}

	public ModuleBuilder DefineDynamicModule(string name)
	{
		throw null;
	}

	protected abstract ModuleBuilder DefineDynamicModuleCore(string name);

	public override bool Equals(object? obj)
	{
		throw null;
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		throw null;
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw null;
	}

	public override IList<CustomAttributeData> GetCustomAttributesData()
	{
		throw null;
	}

	public ModuleBuilder? GetDynamicModule(string name)
	{
		throw null;
	}

	protected abstract ModuleBuilder? GetDynamicModuleCore(string name);

	[RequiresUnreferencedCode("Types might be removed")]
	public override Type[] GetExportedTypes()
	{
		throw null;
	}

	[RequiresAssemblyFiles("This member throws an exception for assemblies embedded in a single-file app")]
	public override FileStream GetFile(string name)
	{
		throw null;
	}

	[RequiresAssemblyFiles("This member throws an exception for assemblies embedded in a single-file app")]
	public override FileStream[] GetFiles(bool getResourceModules)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override Module[] GetLoadedModules(bool getResourceModules)
	{
		throw null;
	}

	public override ManifestResourceInfo? GetManifestResourceInfo(string resourceName)
	{
		throw null;
	}

	public override string[] GetManifestResourceNames()
	{
		throw null;
	}

	public override Stream? GetManifestResourceStream(string name)
	{
		throw null;
	}

	public override Stream? GetManifestResourceStream(Type type, string name)
	{
		throw null;
	}

	public override Module? GetModule(string name)
	{
		throw null;
	}

	public override Module[] GetModules(bool getResourceModules)
	{
		throw null;
	}

	public override AssemblyName GetName(bool copiedName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Assembly references might be removed")]
	public override AssemblyName[] GetReferencedAssemblies()
	{
		throw null;
	}

	public override Assembly GetSatelliteAssembly(CultureInfo culture)
	{
		throw null;
	}

	public override Assembly GetSatelliteAssembly(CultureInfo culture, Version? version)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public override Type? GetType(string name, bool throwOnError, bool ignoreCase)
	{
		throw null;
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw null;
	}

	public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
	{
	}

	public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
	{
	}

	protected abstract void SetCustomAttributeCore(ConstructorInfo con, ReadOnlySpan<byte> binaryAttribute);
}
