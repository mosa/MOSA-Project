using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ReadOnlyStringCollection : ReadOnlyCollectionBase
{
	public string this[int index]
	{
		get
		{
			throw null;
		}
	}

	internal ReadOnlyStringCollection()
	{
	}

	public bool Contains(string value)
	{
		throw null;
	}

	public void CopyTo(string[] values, int index)
	{
	}

	public int IndexOf(string value)
	{
		throw null;
	}
}
