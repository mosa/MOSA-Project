using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

public abstract class TypeDescriptionProvider
{
	protected TypeDescriptionProvider()
	{
	}

	protected TypeDescriptionProvider(TypeDescriptionProvider parent)
	{
	}

	public virtual object? CreateInstance(IServiceProvider? provider, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type objectType, Type[]? argTypes, object?[]? args)
	{
		throw null;
	}

	public virtual IDictionary? GetCache(object instance)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of instance cannot be statically discovered.")]
	public virtual ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance)
	{
		throw null;
	}

	protected internal virtual IExtenderProvider[] GetExtenderProviders(object instance)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of component cannot be statically discovered.")]
	public virtual string? GetFullComponentName(object component)
	{
		throw null;
	}

	[RequiresUnreferencedCode("GetReflectionType is not trim compatible because the Type of object cannot be statically discovered.")]
	public Type GetReflectionType(object instance)
	{
		throw null;
	}

	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)]
	public Type GetReflectionType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type objectType)
	{
		throw null;
	}

	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)]
	public virtual Type GetReflectionType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type objectType, object? instance)
	{
		throw null;
	}

	public virtual Type GetRuntimeType(Type reflectionType)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of instance cannot be statically discovered.")]
	public ICustomTypeDescriptor? GetTypeDescriptor(object instance)
	{
		throw null;
	}

	public ICustomTypeDescriptor? GetTypeDescriptor([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type objectType)
	{
		throw null;
	}

	public virtual ICustomTypeDescriptor? GetTypeDescriptor([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type objectType, object? instance)
	{
		throw null;
	}

	public virtual bool IsSupportedType(Type type)
	{
		throw null;
	}
}
