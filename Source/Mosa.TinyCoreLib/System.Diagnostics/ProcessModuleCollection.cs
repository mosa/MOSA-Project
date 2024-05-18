using System.Collections;

namespace System.Diagnostics;

public class ProcessModuleCollection : ReadOnlyCollectionBase
{
	public ProcessModule this[int index]
	{
		get
		{
			throw null;
		}
	}

	protected ProcessModuleCollection()
	{
	}

	public ProcessModuleCollection(ProcessModule[] processModules)
	{
	}

	public bool Contains(ProcessModule module)
	{
		throw null;
	}

	public void CopyTo(ProcessModule[] array, int index)
	{
	}

	public int IndexOf(ProcessModule module)
	{
		throw null;
	}
}
