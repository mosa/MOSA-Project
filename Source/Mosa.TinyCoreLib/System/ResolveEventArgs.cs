using System.Reflection;

namespace System;

public class ResolveEventArgs : EventArgs
{
	public string Name
	{
		get
		{
			throw null;
		}
	}

	public Assembly? RequestingAssembly
	{
		get
		{
			throw null;
		}
	}

	public ResolveEventArgs(string name)
	{
	}

	public ResolveEventArgs(string name, Assembly? requestingAssembly)
	{
	}
}
