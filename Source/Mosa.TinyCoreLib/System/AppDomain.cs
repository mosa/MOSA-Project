using System.Configuration.Assemblies;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Principal;

namespace System;

public sealed class AppDomain : MarshalByRefObject
{
	public string BaseDirectory
	{
		get
		{
			throw null;
		}
	}

	public static AppDomain CurrentDomain
	{
		get
		{
			throw null;
		}
	}

	public string? DynamicDirectory
	{
		get
		{
			throw null;
		}
	}

	public string FriendlyName
	{
		get
		{
			throw null;
		}
	}

	public int Id
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

	public bool IsHomogenous
	{
		get
		{
			throw null;
		}
	}

	public static bool MonitoringIsEnabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long MonitoringSurvivedMemorySize
	{
		get
		{
			throw null;
		}
	}

	public static long MonitoringSurvivedProcessMemorySize
	{
		get
		{
			throw null;
		}
	}

	public long MonitoringTotalAllocatedMemorySize
	{
		get
		{
			throw null;
		}
	}

	public TimeSpan MonitoringTotalProcessorTime
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public PermissionSet PermissionSet
	{
		get
		{
			throw null;
		}
	}

	public string? RelativeSearchPath
	{
		get
		{
			throw null;
		}
	}

	public AppDomainSetup SetupInformation
	{
		get
		{
			throw null;
		}
	}

	public bool ShadowCopyFiles
	{
		get
		{
			throw null;
		}
	}

	public event AssemblyLoadEventHandler? AssemblyLoad
	{
		add
		{
		}
		remove
		{
		}
	}

	public event ResolveEventHandler? AssemblyResolve
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler? DomainUnload
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<FirstChanceExceptionEventArgs>? FirstChanceException
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler? ProcessExit
	{
		add
		{
		}
		remove
		{
		}
	}

	public event ResolveEventHandler? ReflectionOnlyAssemblyResolve
	{
		add
		{
		}
		remove
		{
		}
	}

	public event ResolveEventHandler? ResourceResolve
	{
		add
		{
		}
		remove
		{
		}
	}

	public event ResolveEventHandler? TypeResolve
	{
		add
		{
		}
		remove
		{
		}
	}

	public event UnhandledExceptionEventHandler? UnhandledException
	{
		add
		{
		}
		remove
		{
		}
	}

	internal AppDomain()
	{
	}

	[Obsolete("AppDomain.AppendPrivatePath has been deprecated and is not supported.")]
	public void AppendPrivatePath(string? path)
	{
	}

	public string ApplyPolicy(string assemblyName)
	{
		throw null;
	}

	[Obsolete("AppDomain.ClearPrivatePath has been deprecated and is not supported.")]
	public void ClearPrivatePath()
	{
	}

	[Obsolete("AppDomain.ClearShadowCopyPath has been deprecated and is not supported.")]
	public void ClearShadowCopyPath()
	{
	}

	[Obsolete("Creating and unloading AppDomains is not supported and throws an exception.", DiagnosticId = "SYSLIB0024", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static AppDomain CreateDomain(string friendlyName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public ObjectHandle? CreateInstance(string assemblyName, string typeName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public ObjectHandle? CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder? binder, object?[]? args, CultureInfo? culture, object?[]? activationAttributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public ObjectHandle? CreateInstance(string assemblyName, string typeName, object?[]? activationAttributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public object? CreateInstanceAndUnwrap(string assemblyName, string typeName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public object? CreateInstanceAndUnwrap(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder? binder, object?[]? args, CultureInfo? culture, object?[]? activationAttributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public object? CreateInstanceAndUnwrap(string assemblyName, string typeName, object?[]? activationAttributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public ObjectHandle? CreateInstanceFrom(string assemblyFile, string typeName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public ObjectHandle? CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder? binder, object?[]? args, CultureInfo? culture, object?[]? activationAttributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public ObjectHandle? CreateInstanceFrom(string assemblyFile, string typeName, object?[]? activationAttributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public object? CreateInstanceFromAndUnwrap(string assemblyFile, string typeName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public object? CreateInstanceFromAndUnwrap(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder? binder, object?[]? args, CultureInfo? culture, object?[]? activationAttributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Type and its constructor could be removed")]
	public object? CreateInstanceFromAndUnwrap(string assemblyFile, string typeName, object?[]? activationAttributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public int ExecuteAssembly(string assemblyFile)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public int ExecuteAssembly(string assemblyFile, string?[]? args)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public int ExecuteAssembly(string assemblyFile, string?[]? args, byte[]? hashValue, System.Configuration.Assemblies.AssemblyHashAlgorithm hashAlgorithm)
	{
		throw null;
	}

	public int ExecuteAssemblyByName(AssemblyName assemblyName, params string?[]? args)
	{
		throw null;
	}

	public int ExecuteAssemblyByName(string assemblyName)
	{
		throw null;
	}

	public int ExecuteAssemblyByName(string assemblyName, params string?[]? args)
	{
		throw null;
	}

	public Assembly[] GetAssemblies()
	{
		throw null;
	}

	[Obsolete("AppDomain.GetCurrentThreadId has been deprecated because it does not provide a stable Id when managed threads are running on fibers (aka lightweight threads). To get a stable identifier for a managed thread, use the ManagedThreadId property on Thread instead.")]
	public static int GetCurrentThreadId()
	{
		throw null;
	}

	public object? GetData(string name)
	{
		throw null;
	}

	public bool? IsCompatibilitySwitchSet(string value)
	{
		throw null;
	}

	public bool IsDefaultAppDomain()
	{
		throw null;
	}

	public bool IsFinalizingForUnload()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public Assembly Load(byte[] rawAssembly)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
	public Assembly Load(byte[] rawAssembly, byte[]? rawSymbolStore)
	{
		throw null;
	}

	public Assembly Load(AssemblyName assemblyRef)
	{
		throw null;
	}

	public Assembly Load(string assemblyString)
	{
		throw null;
	}

	public Assembly[] ReflectionOnlyGetAssemblies()
	{
		throw null;
	}

	[Obsolete("AppDomain.SetCachePath has been deprecated and is not supported.")]
	public void SetCachePath(string? path)
	{
	}

	public void SetData(string name, object? data)
	{
	}

	[Obsolete("AppDomain.SetDynamicBase has been deprecated and is not supported.")]
	public void SetDynamicBase(string? path)
	{
	}

	public void SetPrincipalPolicy(PrincipalPolicy policy)
	{
	}

	[Obsolete("AppDomain.SetShadowCopyFiles has been deprecated and is not supported.")]
	public void SetShadowCopyFiles()
	{
	}

	[Obsolete("AppDomain.SetShadowCopyPath has been deprecated and is not supported.")]
	public void SetShadowCopyPath(string? path)
	{
	}

	public void SetThreadPrincipal(IPrincipal principal)
	{
	}

	public override string ToString()
	{
		throw null;
	}

	[Obsolete("Creating and unloading AppDomains is not supported and throws an exception.", DiagnosticId = "SYSLIB0024", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static void Unload(AppDomain domain)
	{
	}
}
