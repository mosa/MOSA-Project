using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace System.Data.Common;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class DbConnectionStringBuilder : ICollection, IEnumerable, IDictionary, ICustomTypeDescriptor
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DesignOnly(true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool BrowsableConnectionString
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	public string ConnectionString
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

	[Browsable(false)]
	public virtual int Count
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public virtual bool IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public virtual object this[string keyword]
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

	[Browsable(false)]
	public virtual ICollection Keys
	{
		get
		{
			throw null;
		}
	}

	bool ICollection.IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	object ICollection.SyncRoot
	{
		get
		{
			throw null;
		}
	}

	object? IDictionary.this[object keyword]
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
	public virtual ICollection Values
	{
		get
		{
			throw null;
		}
	}

	public DbConnectionStringBuilder()
	{
	}

	public DbConnectionStringBuilder(bool useOdbcRules)
	{
	}

	public void Add(string keyword, object value)
	{
	}

	public static void AppendKeyValuePair(StringBuilder builder, string keyword, string? value)
	{
	}

	public static void AppendKeyValuePair(StringBuilder builder, string keyword, string? value, bool useOdbcRules)
	{
	}

	public virtual void Clear()
	{
	}

	protected internal void ClearPropertyDescriptors()
	{
	}

	public virtual bool ContainsKey(string keyword)
	{
		throw null;
	}

	public virtual bool EquivalentTo(DbConnectionStringBuilder connectionStringBuilder)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
	protected virtual void GetProperties(Hashtable propertyDescriptors)
	{
	}

	public virtual bool Remove(string keyword)
	{
		throw null;
	}

	public virtual bool ShouldSerialize(string keyword)
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}

	void IDictionary.Add(object keyword, object? value)
	{
	}

	bool IDictionary.Contains(object keyword)
	{
		throw null;
	}

	IDictionaryEnumerator IDictionary.GetEnumerator()
	{
		throw null;
	}

	void IDictionary.Remove(object keyword)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	AttributeCollection ICustomTypeDescriptor.GetAttributes()
	{
		throw null;
	}

	string? ICustomTypeDescriptor.GetClassName()
	{
		throw null;
	}

	string? ICustomTypeDescriptor.GetComponentName()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Generic TypeConverters may require the generic types to be annotated. For example, NullableConverter requires the underlying type to be DynamicallyAccessedMembers All.")]
	TypeConverter ICustomTypeDescriptor.GetConverter()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The built-in EventDescriptor implementation uses Reflection which requires unreferenced code.")]
	EventDescriptor? ICustomTypeDescriptor.GetDefaultEvent()
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
	PropertyDescriptor? ICustomTypeDescriptor.GetDefaultProperty()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Editors registered in TypeDescriptor.AddEditorTable may be trimmed.")]
	object? ICustomTypeDescriptor.GetEditor(Type editorBaseType)
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

	public override string ToString()
	{
		throw null;
	}

	public virtual bool TryGetValue(string keyword, [NotNullWhen(true)] out object? value)
	{
		throw null;
	}
}
