using System.Collections;

namespace System.Configuration;

public class SettingsPropertyCollection : ICollection, IEnumerable, ICloneable
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	public SettingsProperty this[string name]
	{
		get
		{
			throw null;
		}
	}

	public object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public void Add(SettingsProperty property)
	{
	}

	public void Clear()
	{
	}

	public object Clone()
	{
		throw null;
	}

	public void CopyTo(Array array, int index)
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	protected virtual void OnAdd(SettingsProperty property)
	{
	}

	protected virtual void OnAddComplete(SettingsProperty property)
	{
	}

	protected virtual void OnClear()
	{
	}

	protected virtual void OnClearComplete()
	{
	}

	protected virtual void OnRemove(SettingsProperty property)
	{
	}

	protected virtual void OnRemoveComplete(SettingsProperty property)
	{
	}

	public void Remove(string name)
	{
	}

	public void SetReadOnly()
	{
	}
}
