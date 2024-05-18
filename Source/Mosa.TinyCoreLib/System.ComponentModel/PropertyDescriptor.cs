using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

public abstract class PropertyDescriptor : MemberDescriptor
{
	public abstract Type ComponentType { get; }

	public virtual TypeConverter Converter
	{
		[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
		get
		{
			throw null;
		}
	}

	public virtual bool IsLocalizable
	{
		get
		{
			throw null;
		}
	}

	public abstract bool IsReadOnly { get; }

	public abstract Type PropertyType { get; }

	public DesignerSerializationVisibility SerializationVisibility
	{
		get
		{
			throw null;
		}
	}

	public virtual bool SupportsChangeEvents
	{
		get
		{
			throw null;
		}
	}

	protected PropertyDescriptor(MemberDescriptor descr)
		: base((string)null)
	{
	}

	protected PropertyDescriptor(MemberDescriptor descr, Attribute[]? attrs)
		: base((string)null)
	{
	}

	protected PropertyDescriptor(string name, Attribute[]? attrs)
		: base((string)null)
	{
	}

	public virtual void AddValueChanged(object component, EventHandler handler)
	{
	}

	public abstract bool CanResetValue(object component);

	protected object? CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	protected override void FillAttributes(IList attributeList)
	{
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered.")]
	public PropertyDescriptorCollection GetChildProperties()
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	public PropertyDescriptorCollection GetChildProperties(Attribute[] filter)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered. The Type of instance cannot be statically discovered.")]
	public PropertyDescriptorCollection GetChildProperties(object instance)
	{
		throw null;
	}

	[RequiresUnreferencedCode("PropertyDescriptor's PropertyType cannot be statically discovered. The Type of instance cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	public virtual PropertyDescriptorCollection GetChildProperties(object? instance, Attribute[]? filter)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Editors registered in TypeDescriptor.AddEditorTable may be trimmed. PropertyDescriptor's PropertyType cannot be statically discovered.")]
	public virtual object? GetEditor(Type editorBaseType)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	protected override object? GetInvocationTarget(Type type, object instance)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Calls ComponentType.Assembly.GetType on the non-fully qualified typeName, which the trimmer cannot recognize.")]
	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
	protected Type? GetTypeFromName([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] string? typeName)
	{
		throw null;
	}

	public abstract object? GetValue(object? component);

	protected internal EventHandler? GetValueChangedHandler(object component)
	{
		throw null;
	}

	protected virtual void OnValueChanged(object? component, EventArgs e)
	{
	}

	public virtual void RemoveValueChanged(object component, EventHandler handler)
	{
	}

	public abstract void ResetValue(object component);

	public abstract void SetValue(object? component, object? value);

	public abstract bool ShouldSerializeValue(object component);
}
