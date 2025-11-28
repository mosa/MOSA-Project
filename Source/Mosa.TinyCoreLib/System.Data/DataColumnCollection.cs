using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data;

[DefaultEvent("CollectionChanged")]
[Editor("Microsoft.VSDesigner.Data.Design.ColumnsCollectionEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
public sealed class DataColumnCollection : InternalDataCollectionBase
{
	public DataColumn this[int index]
	{
		get
		{
			throw null;
		}
	}

	public DataColumn? this[string name]
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

	internal DataColumnCollection()
	{
	}

	public DataColumn Add()
	{
		throw null;
	}

	public void Add(DataColumn column)
	{
	}

	public DataColumn Add(string? columnName)
	{
		throw null;
	}

	public DataColumn Add(string? columnName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] Type type)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members might be trimmed for some data types or expressions.")]
	public DataColumn Add(string? columnName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] Type type, string expression)
	{
		throw null;
	}

	public void AddRange(DataColumn[] columns)
	{
	}

	public bool CanRemove(DataColumn? column)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public bool Contains(string name)
	{
		throw null;
	}

	public void CopyTo(DataColumn[] array, int index)
	{
	}

	public int IndexOf(DataColumn? column)
	{
		throw null;
	}

	public int IndexOf(string? columnName)
	{
		throw null;
	}

	public void Remove(DataColumn column)
	{
	}

	public void Remove(string name)
	{
	}

	public void RemoveAt(int index)
	{
	}
}
