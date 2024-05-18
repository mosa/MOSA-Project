using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data;

public class DataRowView : ICustomTypeDescriptor, IDataErrorInfo, IEditableObject, INotifyPropertyChanged
{
	public DataView DataView
	{
		get
		{
			throw null;
		}
	}

	public bool IsEdit
	{
		get
		{
			throw null;
		}
	}

	public bool IsNew
	{
		get
		{
			throw null;
		}
	}

	public object this[int ndx]
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public object this[string property]
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public DataRow Row
	{
		get
		{
			throw null;
		}
	}

	public DataRowVersion RowVersion
	{
		get
		{
			throw null;
		}
	}

	string IDataErrorInfo.Error
	{
		get
		{
			throw null;
		}
	}

	string IDataErrorInfo.this[string colName]
	{
		get
		{
			throw null;
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	internal DataRowView()
	{
	}

	public void BeginEdit()
	{
	}

	public void CancelEdit()
	{
	}

	public DataView CreateChildView(DataRelation relation)
	{
		throw null;
	}

	public DataView CreateChildView(DataRelation relation, bool followParent)
	{
		throw null;
	}

	public DataView CreateChildView(string relationName)
	{
		throw null;
	}

	public DataView CreateChildView(string relationName, bool followParent)
	{
		throw null;
	}

	public void Delete()
	{
	}

	public void EndEdit()
	{
	}

	public override bool Equals(object? other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	AttributeCollection ICustomTypeDescriptor.GetAttributes()
	{
		throw null;
	}

	string ICustomTypeDescriptor.GetClassName()
	{
		throw null;
	}

	string ICustomTypeDescriptor.GetComponentName()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Generic TypeConverters may require the generic types to be annotated. For example, NullableConverter requires the underlying type to be DynamicallyAccessedMembers All.")]
	TypeConverter ICustomTypeDescriptor.GetConverter()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The built-in EventDescriptor implementation uses Reflection which requires unreferenced code.")]
	EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
	PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Editors registered in TypeDescriptor.AddEditorTable may be trimmed.")]
	object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
	{
		throw null;
	}

	EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[]? attributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
	PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[]? attributes)
	{
		throw null;
	}

	object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor? pd)
	{
		throw null;
	}
}
