using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Data.Common;

public abstract class DbDataRecord : ICustomTypeDescriptor, IDataRecord
{
	public abstract int FieldCount { get; }

	public abstract object this[int i] { get; }

	public abstract object this[string name] { get; }

	public abstract bool GetBoolean(int i);

	public abstract byte GetByte(int i);

	public abstract long GetBytes(int i, long dataIndex, byte[]? buffer, int bufferIndex, int length);

	public abstract char GetChar(int i);

	public abstract long GetChars(int i, long dataIndex, char[]? buffer, int bufferIndex, int length);

	public IDataReader GetData(int i)
	{
		throw null;
	}

	public abstract string GetDataTypeName(int i);

	public abstract DateTime GetDateTime(int i);

	protected virtual DbDataReader GetDbDataReader(int i)
	{
		throw null;
	}

	public abstract decimal GetDecimal(int i);

	public abstract double GetDouble(int i);

	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)]
	public abstract Type GetFieldType(int i);

	public abstract float GetFloat(int i);

	public abstract Guid GetGuid(int i);

	public abstract short GetInt16(int i);

	public abstract int GetInt32(int i);

	public abstract long GetInt64(int i);

	public abstract string GetName(int i);

	public abstract int GetOrdinal(string name);

	public abstract string GetString(int i);

	public abstract object GetValue(int i);

	public abstract int GetValues(object[] values);

	public abstract bool IsDBNull(int i);

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
