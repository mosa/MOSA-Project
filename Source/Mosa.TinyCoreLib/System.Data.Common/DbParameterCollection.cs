using System.Collections;
using System.ComponentModel;

namespace System.Data.Common;

public abstract class DbParameterCollection : MarshalByRefObject, ICollection, IEnumerable, IList, IDataParameterCollection
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public abstract int Count { get; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual bool IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	public DbParameter this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DbParameter this[string parameterName]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract object SyncRoot { get; }

	object? IList.this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	object IDataParameterCollection.this[string parameterName]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	int IList.Add(object? value)
	{
		throw null;
	}

	public abstract int Add(object value);

	public abstract void AddRange(Array values);

	public abstract void Clear();

	bool IList.Contains(object? value)
	{
		throw null;
	}

	public abstract bool Contains(object value);

	public abstract bool Contains(string value);

	public abstract void CopyTo(Array array, int index);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract IEnumerator GetEnumerator();

	protected abstract DbParameter GetParameter(int index);

	protected abstract DbParameter GetParameter(string parameterName);

	int IList.IndexOf(object? value)
	{
		throw null;
	}

	public abstract int IndexOf(object value);

	public abstract int IndexOf(string parameterName);

	void IList.Insert(int index, object? value)
	{
		throw null;
	}

	public abstract void Insert(int index, object value);

	void IList.Remove(object? value)
	{
		throw null;
	}

	public abstract void Remove(object value);

	public abstract void RemoveAt(int index);

	public abstract void RemoveAt(string parameterName);

	protected abstract void SetParameter(int index, DbParameter value);

	protected abstract void SetParameter(string parameterName, DbParameter value);
}
