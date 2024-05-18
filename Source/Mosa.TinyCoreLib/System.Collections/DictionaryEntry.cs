using System.ComponentModel;

namespace System.Collections;

public struct DictionaryEntry
{
	private object _dummy;

	private int _dummyPrimitive;

	public object Key
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object? Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DictionaryEntry(object key, object? value)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Deconstruct(out object key, out object? value)
	{
		throw null;
	}
}
