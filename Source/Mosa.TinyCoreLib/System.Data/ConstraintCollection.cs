using System.Collections;
using System.ComponentModel;

namespace System.Data;

[DefaultEvent("CollectionChanged")]
[Editor("Microsoft.VSDesigner.Data.Design.ConstraintsCollectionEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public sealed class ConstraintCollection : InternalDataCollectionBase
{
	public Constraint this[int index]
	{
		get
		{
			throw null;
		}
	}

	public Constraint? this[string? name]
	{
		get
		{
			throw null;
		}
	}

	protected override ArrayList List
	{
		get
		{
			throw null;
		}
	}

	public event CollectionChangeEventHandler? CollectionChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	internal ConstraintCollection()
	{
	}

	public void Add(Constraint constraint)
	{
	}

	public Constraint Add(string? name, DataColumn column, bool primaryKey)
	{
		throw null;
	}

	public Constraint Add(string? name, DataColumn primaryKeyColumn, DataColumn foreignKeyColumn)
	{
		throw null;
	}

	public Constraint Add(string? name, DataColumn[] columns, bool primaryKey)
	{
		throw null;
	}

	public Constraint Add(string? name, DataColumn[] primaryKeyColumns, DataColumn[] foreignKeyColumns)
	{
		throw null;
	}

	public void AddRange(Constraint[]? constraints)
	{
	}

	public bool CanRemove(Constraint constraint)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public bool Contains(string? name)
	{
		throw null;
	}

	public void CopyTo(Constraint[] array, int index)
	{
	}

	public int IndexOf(Constraint? constraint)
	{
		throw null;
	}

	public int IndexOf(string? constraintName)
	{
		throw null;
	}

	public void Remove(Constraint constraint)
	{
	}

	public void Remove(string name)
	{
	}

	public void RemoveAt(int index)
	{
	}
}
