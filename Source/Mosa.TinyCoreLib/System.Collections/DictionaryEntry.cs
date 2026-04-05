using System.ComponentModel;

namespace System.Collections;

public struct DictionaryEntry(object key, object? value)
{
	public object Key { get; set; } = key;

	public object? Value { get; set; } = value;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Deconstruct(out object key, out object? value)
	{
		key = Key;
		value = Value;
	}
}
