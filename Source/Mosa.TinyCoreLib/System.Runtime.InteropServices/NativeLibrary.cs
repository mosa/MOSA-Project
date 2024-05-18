using System.Reflection;

namespace System.Runtime.InteropServices;

public static class NativeLibrary
{
	public static void Free(IntPtr handle)
	{
	}

	public static IntPtr GetMainProgramHandle()
	{
		throw null;
	}

	public static IntPtr GetExport(IntPtr handle, string name)
	{
		throw null;
	}

	public static IntPtr Load(string libraryPath)
	{
		throw null;
	}

	public static IntPtr Load(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
	{
		throw null;
	}

	public static void SetDllImportResolver(Assembly assembly, DllImportResolver resolver)
	{
	}

	public static bool TryGetExport(IntPtr handle, string name, out IntPtr address)
	{
		throw null;
	}

	public static bool TryLoad(string libraryPath, out IntPtr handle)
	{
		throw null;
	}

	public static bool TryLoad(string libraryName, Assembly assembly, DllImportSearchPath? searchPath, out IntPtr handle)
	{
		throw null;
	}
}
