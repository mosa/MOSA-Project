using System.Collections;

namespace System.Diagnostics;

public class ProcessThreadCollection : ReadOnlyCollectionBase
{
	public ProcessThread this[int index]
	{
		get
		{
			throw null;
		}
	}

	protected ProcessThreadCollection()
	{
	}

	public ProcessThreadCollection(ProcessThread[] processThreads)
	{
	}

	public int Add(ProcessThread thread)
	{
		throw null;
	}

	public bool Contains(ProcessThread thread)
	{
		throw null;
	}

	public void CopyTo(ProcessThread[] array, int index)
	{
	}

	public int IndexOf(ProcessThread thread)
	{
		throw null;
	}

	public void Insert(int index, ProcessThread thread)
	{
	}

	public void Remove(ProcessThread thread)
	{
	}
}
