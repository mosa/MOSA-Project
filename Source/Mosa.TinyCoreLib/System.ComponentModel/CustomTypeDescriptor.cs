using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

public abstract class CustomTypeDescriptor : ICustomTypeDescriptor
{
	protected CustomTypeDescriptor()
	{
	}

	protected CustomTypeDescriptor(ICustomTypeDescriptor? parent)
	{
	}

	public virtual AttributeCollection GetAttributes()
	{
		throw null;
	}

	public virtual string? GetClassName()
	{
		throw null;
	}

	public virtual string? GetComponentName()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Generic TypeConverters may require the generic types to be annotated. For example, NullableConverter requires the underlying type to be DynamicallyAccessedMembers All.")]
	public virtual TypeConverter? GetConverter()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The built-in EventDescriptor implementation uses Reflection which requires unreferenced code.")]
	public virtual EventDescriptor? GetDefaultEvent()
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
	public virtual PropertyDescriptor? GetDefaultProperty()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Editors registered in TypeDescriptor.AddEditorTable may be trimmed.")]
	public virtual object? GetEditor(Type editorBaseType)
	{
		throw null;
	}

	public virtual EventDescriptorCollection GetEvents()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	public virtual EventDescriptorCollection GetEvents(Attribute[]? attributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
	public virtual PropertyDescriptorCollection GetProperties()
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	public virtual PropertyDescriptorCollection GetProperties(Attribute[]? attributes)
	{
		throw null;
	}

	public virtual object? GetPropertyOwner(PropertyDescriptor? pd)
	{
		throw null;
	}
}
