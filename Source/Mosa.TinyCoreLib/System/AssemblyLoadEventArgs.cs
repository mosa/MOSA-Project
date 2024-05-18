using System.Reflection;

namespace System;

public class AssemblyLoadEventArgs : EventArgs
{
	public Assembly LoadedAssembly
	{
		get
		{
			throw null;
		}
	}

	public AssemblyLoadEventArgs(Assembly loadedAssembly)
	{
	}
}
