using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

public interface ICustomTypeDescriptor
{
	AttributeCollection GetAttributes();

	string? GetClassName();

	string? GetComponentName();

	[RequiresUnreferencedCode("Generic TypeConverters may require the generic types to be annotated. For example, NullableConverter requires the underlying type to be DynamicallyAccessedMembers All.")]
	TypeConverter? GetConverter();

	[RequiresUnreferencedCode("The built-in EventDescriptor implementation uses Reflection which requires unreferenced code.")]
	EventDescriptor? GetDefaultEvent();

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
	PropertyDescriptor? GetDefaultProperty();

	[RequiresUnreferencedCode("Editors registered in TypeDescriptor.AddEditorTable may be trimmed.")]
	object? GetEditor(Type editorBaseType);

	EventDescriptorCollection GetEvents();

	[RequiresUnreferencedCode("The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	EventDescriptorCollection GetEvents(Attribute[]? attributes);

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
	PropertyDescriptorCollection GetProperties();

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	PropertyDescriptorCollection GetProperties(Attribute[]? attributes);

	object? GetPropertyOwner(PropertyDescriptor? pd);
}
