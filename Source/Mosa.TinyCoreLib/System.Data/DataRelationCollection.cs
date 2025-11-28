using System.ComponentModel;

namespace System.Data;

[DefaultEvent("CollectionChanged")]
[DefaultProperty("Table")]
[Editor("Microsoft.VSDesigner.Data.Design.DataRelationCollectionEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public abstract class DataRelationCollection : InternalDataCollectionBase
{
	public abstract DataRelation this[int index] { get; }

	public abstract DataRelation? this[string? name] { get; }

	public event CollectionChangeEventHandler? CollectionChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public virtual DataRelation Add(DataColumn parentColumn, DataColumn childColumn)
	{
		throw null;
	}

	public virtual DataRelation Add(DataColumn[] parentColumns, DataColumn[] childColumns)
	{
		throw null;
	}

	public void Add(DataRelation relation)
	{
	}

	public virtual DataRelation Add(string? name, DataColumn parentColumn, DataColumn childColumn)
	{
		throw null;
	}

	public virtual DataRelation Add(string? name, DataColumn parentColumn, DataColumn childColumn, bool createConstraints)
	{
		throw null;
	}

	public virtual DataRelation Add(string? name, DataColumn[] parentColumns, DataColumn[] childColumns)
	{
		throw null;
	}

	public virtual DataRelation Add(string? name, DataColumn[] parentColumns, DataColumn[] childColumns, bool createConstraints)
	{
		throw null;
	}

	protected virtual void AddCore(DataRelation relation)
	{
	}

	public virtual void AddRange(DataRelation[]? relations)
	{
	}

	public virtual bool CanRemove(DataRelation? relation)
	{
		throw null;
	}

	public virtual void Clear()
	{
	}

	public virtual bool Contains(string? name)
	{
		throw null;
	}

	public void CopyTo(DataRelation[] array, int index)
	{
	}

	protected abstract DataSet GetDataSet();

	public virtual int IndexOf(DataRelation? relation)
	{
		throw null;
	}

	public virtual int IndexOf(string? relationName)
	{
		throw null;
	}

	protected virtual void OnCollectionChanged(CollectionChangeEventArgs ccevent)
	{
	}

	protected virtual void OnCollectionChanging(CollectionChangeEventArgs ccevent)
	{
	}

	public void Remove(DataRelation relation)
	{
	}

	public void Remove(string name)
	{
	}

	public void RemoveAt(int index)
	{
	}

	protected virtual void RemoveCore(DataRelation relation)
	{
	}
}
