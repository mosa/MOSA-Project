using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.ComponentModel;

public sealed class TypeDescriptor
{
	[Obsolete("TypeDescriptor.ComNativeDescriptorHandler has been deprecated. Use a type description provider to supply type information for COM types instead.")]
	public static IComNativeDescriptorHandler? ComNativeDescriptorHandler
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static Type ComObjectType
	{
		[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		get
		{
			throw null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static Type InterfaceType
	{
		[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
		get
		{
			throw null;
		}
	}

	public static event RefreshEventHandler? Refreshed
	{
		add
		{
		}
		remove
		{
		}
	}

	internal TypeDescriptor()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static TypeDescriptionProvider AddAttributes(object instance, params Attribute[] attributes)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static TypeDescriptionProvider AddAttributes(Type type, params Attribute[] attributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Types specified in table may be trimmed, or have their static construtors trimmed.")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void AddEditorTable(Type editorBaseType, Hashtable table)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void AddProvider(TypeDescriptionProvider provider, object instance)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void AddProvider(TypeDescriptionProvider provider, Type type)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void AddProviderTransparent(TypeDescriptionProvider provider, object instance)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void AddProviderTransparent(TypeDescriptionProvider provider, Type type)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void CreateAssociation(object primary, object secondary)
	{
	}

	[RequiresUnreferencedCode("The Type of component cannot be statically discovered.")]
	public static IDesigner? CreateDesigner(IComponent component, Type designerBaseType)
	{
		throw null;
	}

	public static EventDescriptor CreateEvent([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType, EventDescriptor oldEventDescriptor, params Attribute[] attributes)
	{
		throw null;
	}

	public static EventDescriptor CreateEvent([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType, string name, Type type, params Attribute[] attributes)
	{
		throw null;
	}

	public static object? CreateInstance(IServiceProvider? provider, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type objectType, Type[]? argTypes, object?[]? args)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
	public static PropertyDescriptor CreateProperty([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType, PropertyDescriptor oldPropertyDescriptor, params Attribute[] attributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
	public static PropertyDescriptor CreateProperty([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType, string name, Type type, params Attribute[] attributes)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static object GetAssociation(Type type, object primary)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of component cannot be statically discovered.")]
	public static AttributeCollection GetAttributes(object component)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of component cannot be statically discovered.")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static AttributeCollection GetAttributes(object component, bool noCustomTypeDesc)
	{
		throw null;
	}

	public static AttributeCollection GetAttributes([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of component cannot be statically discovered.")]
	public static string? GetClassName(object component)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of component cannot be statically discovered.")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static string? GetClassName(object component, bool noCustomTypeDesc)
	{
		throw null;
	}

	public static string? GetClassName([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of component cannot be statically discovered.")]
	public static string? GetComponentName(object component)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of component cannot be statically discovered.")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static string? GetComponentName(object component, bool noCustomTypeDesc)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Generic TypeConverters may require the generic types to be annotated. For example, NullableConverter requires the underlying type to be DynamicallyAccessedMembers All. The Type of component cannot be statically discovered.")]
	public static TypeConverter GetConverter(object component)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Generic TypeConverters may require the generic types to be annotated. For example, NullableConverter requires the underlying type to be DynamicallyAccessedMembers All. The Type of component cannot be statically discovered.")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static TypeConverter GetConverter(object component, bool noCustomTypeDesc)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Generic TypeConverters may require the generic types to be annotated. For example, NullableConverter requires the underlying type to be DynamicallyAccessedMembers All.")]
	public static TypeConverter GetConverter([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The built-in EventDescriptor implementation uses Reflection which requires unreferenced code. The Type of component cannot be statically discovered.")]
	public static EventDescriptor? GetDefaultEvent(object component)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The built-in EventDescriptor implementation uses Reflection which requires unreferenced code. The Type of component cannot be statically discovered.")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static EventDescriptor? GetDefaultEvent(object component, bool noCustomTypeDesc)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The built-in EventDescriptor implementation uses Reflection which requires unreferenced code.")]
	public static EventDescriptor? GetDefaultEvent([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered. The Type of component cannot be statically discovered.")]
	public static PropertyDescriptor? GetDefaultProperty(object component)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered. The Type of component cannot be statically discovered.")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static PropertyDescriptor? GetDefaultProperty(object component, bool noCustomTypeDesc)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
	public static PropertyDescriptor? GetDefaultProperty([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Editors registered in TypeDescriptor.AddEditorTable may be trimmed. The Type of component cannot be statically discovered.")]
	public static object? GetEditor(object component, Type editorBaseType)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Editors registered in TypeDescriptor.AddEditorTable may be trimmed. The Type of component cannot be statically discovered.")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static object? GetEditor(object component, Type editorBaseType, bool noCustomTypeDesc)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Editors registered in TypeDescriptor.AddEditorTable may be trimmed.")]
	public static object? GetEditor([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type, Type editorBaseType)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of component cannot be statically discovered.")]
	public static EventDescriptorCollection GetEvents(object component)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of component cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	public static EventDescriptorCollection GetEvents(object component, Attribute[] attributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of component cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static EventDescriptorCollection GetEvents(object component, Attribute[]? attributes, bool noCustomTypeDesc)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of component cannot be statically discovered.")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static EventDescriptorCollection GetEvents(object component, bool noCustomTypeDesc)
	{
		throw null;
	}

	public static EventDescriptorCollection GetEvents([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	public static EventDescriptorCollection GetEvents([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType, Attribute[] attributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of component cannot be statically discovered.")]
	public static string? GetFullComponentName(object component)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered. The Type of component cannot be statically discovered.")]
	public static PropertyDescriptorCollection GetProperties(object component)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered. The Type of component cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	public static PropertyDescriptorCollection GetProperties(object component, Attribute[]? attributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered. The Type of component cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	public static PropertyDescriptorCollection GetProperties(object component, Attribute[]? attributes, bool noCustomTypeDesc)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered. The Type of component cannot be statically discovered.")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static PropertyDescriptorCollection GetProperties(object component, bool noCustomTypeDesc)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
	public static PropertyDescriptorCollection GetProperties([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	public static PropertyDescriptorCollection GetProperties([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType, Attribute[]? attributes)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static TypeDescriptionProvider GetProvider(object instance)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static TypeDescriptionProvider GetProvider(Type type)
	{
		throw null;
	}

	[RequiresUnreferencedCode("GetReflectionType is not trim compatible because the Type of object cannot be statically discovered.")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static Type GetReflectionType(object instance)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)]
	public static Type GetReflectionType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type type)
	{
		throw null;
	}

	public static void Refresh(object component)
	{
	}

	public static void Refresh(Assembly assembly)
	{
	}

	public static void Refresh(Module module)
	{
	}

	public static void Refresh(Type type)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void RemoveAssociation(object primary, object secondary)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void RemoveAssociations(object primary)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void RemoveProvider(TypeDescriptionProvider provider, object instance)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void RemoveProvider(TypeDescriptionProvider provider, Type type)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void RemoveProviderTransparent(TypeDescriptionProvider provider, object instance)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static void RemoveProviderTransparent(TypeDescriptionProvider provider, Type type)
	{
	}

	public static void SortDescriptorArray(IList infos)
	{
	}
}
