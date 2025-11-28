using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace System.Dynamic;

public sealed class ExpandoObject : ICollection<KeyValuePair<string, object?>>, IEnumerable<KeyValuePair<string, object?>>, IEnumerable, IDictionary<string, object?>, INotifyPropertyChanged, IDynamicMetaObjectProvider
{
	int ICollection<KeyValuePair<string, object>>.Count
	{
		get
		{
			throw null;
		}
	}

	bool ICollection<KeyValuePair<string, object>>.IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	object? IDictionary<string, object>.this[string key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	ICollection<string> IDictionary<string, object>.Keys
	{
		get
		{
			throw null;
		}
	}

	ICollection<object?> IDictionary<string, object>.Values
	{
		get
		{
			throw null;
		}
	}

	event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
	{
	}

	void ICollection<KeyValuePair<string, object>>.Clear()
	{
	}

	bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
	{
		throw null;
	}

	void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
	{
	}

	bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
	{
		throw null;
	}

	void IDictionary<string, object>.Add(string key, object? value)
	{
	}

	bool IDictionary<string, object>.ContainsKey(string key)
	{
		throw null;
	}

	bool IDictionary<string, object>.Remove(string key)
	{
		throw null;
	}

	bool IDictionary<string, object>.TryGetValue(string key, out object? value)
	{
		throw null;
	}

	IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
	{
		throw null;
	}
}
