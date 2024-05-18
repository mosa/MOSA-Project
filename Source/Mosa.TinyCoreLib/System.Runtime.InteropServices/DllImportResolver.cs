using System.Reflection;

namespace System.Runtime.InteropServices;

public delegate IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath);
