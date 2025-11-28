using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace System.Runtime.InteropServices;

[UnsupportedOSPlatform("android")]
[UnsupportedOSPlatform("browser")]
[UnsupportedOSPlatform("ios")]
[UnsupportedOSPlatform("tvos")]
[CLSCompliant(false)]
public abstract class ComWrappers
{
	public struct ComInterfaceEntry
	{
		public Guid IID;

		public IntPtr Vtable;
	}

	public struct ComInterfaceDispatch
	{
		public IntPtr Vtable;

		public unsafe static T GetInstance<T>(ComInterfaceDispatch* dispatchPtr) where T : class
		{
			throw null;
		}
	}

	public static bool TryGetComInstance(object obj, out IntPtr unknown)
	{
		throw null;
	}

	public static bool TryGetObject(IntPtr unknown, [NotNullWhen(true)] out object? obj)
	{
		throw null;
	}

	public IntPtr GetOrCreateComInterfaceForObject(object instance, CreateComInterfaceFlags flags)
	{
		throw null;
	}

	protected unsafe abstract ComInterfaceEntry* ComputeVtables(object obj, CreateComInterfaceFlags flags, out int count);

	public object GetOrCreateObjectForComInstance(IntPtr externalComObject, CreateObjectFlags flags)
	{
		throw null;
	}

	protected abstract object? CreateObject(IntPtr externalComObject, CreateObjectFlags flags);

	public object GetOrRegisterObjectForComInstance(IntPtr externalComObject, CreateObjectFlags flags, object wrapper)
	{
		throw null;
	}

	public object GetOrRegisterObjectForComInstance(IntPtr externalComObject, CreateObjectFlags flags, object wrapper, IntPtr inner)
	{
		throw null;
	}

	protected abstract void ReleaseObjects(IEnumerable objects);

	public static void RegisterForTrackerSupport(ComWrappers instance)
	{
	}

	[SupportedOSPlatform("windows")]
	public static void RegisterForMarshalling(ComWrappers instance)
	{
	}

	public static void GetIUnknownImpl(out IntPtr fpQueryInterface, out IntPtr fpAddRef, out IntPtr fpRelease)
	{
		throw null;
	}
}
