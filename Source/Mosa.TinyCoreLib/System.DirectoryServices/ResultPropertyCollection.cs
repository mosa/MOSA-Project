using System.Collections;

namespace System.DirectoryServices;

public class ResultPropertyCollection : DictionaryBase
{
	public ResultPropertyValueCollection this[string name]
	{
		get
		{
			throw null;
		}
	}

	public ICollection PropertyNames
	{
		get
		{
			throw null;
		}
	}

	public ICollection Values
	{
		get
		{
			throw null;
		}
	}

	internal ResultPropertyCollection()
	{
	}

	public bool Contains(string propertyName)
	{
		throw null;
	}

	public void CopyTo(ResultPropertyValueCollection[] array, int index)
	{
	}
}
