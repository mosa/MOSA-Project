using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.Reflection.Emit;

public abstract class EnumBuilder : TypeInfo
{
	public override Assembly Assembly
	{
		get
		{
			throw null;
		}
	}

	public override string? AssemblyQualifiedName
	{
		get
		{
			throw null;
		}
	}

	public override Type? BaseType
	{
		get
		{
			throw null;
		}
	}

	public override Type? DeclaringType
	{
		get
		{
			throw null;
		}
	}

	public override string? FullName
	{
		get
		{
			throw null;
		}
	}

	public override Guid GUID
	{
		get
		{
			throw null;
		}
	}

	public override bool IsByRefLike
	{
		get
		{
			throw null;
		}
	}

	public override bool IsConstructedGenericType
	{
		get
		{
			throw null;
		}
	}

	public override bool IsSZArray
	{
		get
		{
			throw null;
		}
	}

	public override bool IsTypeDefinition
	{
		get
		{
			throw null;
		}
	}

	public override Module Module
	{
		get
		{
			throw null;
		}
	}

	public override string Name
	{
		get
		{
			throw null;
		}
	}

	public override string? Namespace
	{
		get
		{
			throw null;
		}
	}

	public override Type? ReflectedType
	{
		get
		{
			throw null;
		}
	}

	public override RuntimeTypeHandle TypeHandle
	{
		get
		{
			throw null;
		}
	}

	public FieldBuilder UnderlyingField
	{
		get
		{
			throw null;
		}
	}

	protected abstract FieldBuilder UnderlyingFieldCore { get; }

	public override Type UnderlyingSystemType
	{
		get
		{
			throw null;
		}
	}

	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	public Type CreateType()
	{
		throw null;
	}

	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	public TypeInfo CreateTypeInfo()
	{
		throw null;
	}

	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	protected abstract TypeInfo CreateTypeInfoCore();

	public FieldBuilder DefineLiteral(string literalName, object? literalValue)
	{
		throw null;
	}

	protected abstract FieldBuilder DefineLiteralCore(string literalName, object? literalValue);

	protected override TypeAttributes GetAttributeFlagsImpl()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
	protected override ConstructorInfo? GetConstructorImpl(BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
	public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
	{
		throw null;
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		throw null;
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw null;
	}

	public override Type? GetElementType()
	{
		throw null;
	}

	public override Type GetEnumUnderlyingType()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
	public override EventInfo? GetEvent(string name, BindingFlags bindingAttr)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
	public override EventInfo[] GetEvents()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
	public override EventInfo[] GetEvents(BindingFlags bindingAttr)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)]
	public override FieldInfo? GetField(string name, BindingFlags bindingAttr)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)]
	public override FieldInfo[] GetFields(BindingFlags bindingAttr)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
	public override Type? GetInterface(string name, bool ignoreCase)
	{
		throw null;
	}

	public override InterfaceMapping GetInterfaceMap([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] Type interfaceType)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
	public override Type[] GetInterfaces()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
	public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
	public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	protected override MethodInfo? GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes)]
	public override Type? GetNestedType(string name, BindingFlags bindingAttr)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes)]
	public override Type[] GetNestedTypes(BindingFlags bindingAttr)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)]
	public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)]
	protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[]? types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	protected override bool HasElementTypeImpl()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	public override object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, CultureInfo? culture, string[]? namedParameters)
	{
		throw null;
	}

	protected override bool IsArrayImpl()
	{
		throw null;
	}

	public override bool IsAssignableFrom([NotNullWhen(true)] TypeInfo? typeInfo)
	{
		throw null;
	}

	protected override bool IsByRefImpl()
	{
		throw null;
	}

	protected override bool IsCOMObjectImpl()
	{
		throw null;
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw null;
	}

	protected override bool IsPointerImpl()
	{
		throw null;
	}

	protected override bool IsPrimitiveImpl()
	{
		throw null;
	}

	protected override bool IsValueTypeImpl()
	{
		throw null;
	}

	[RequiresDynamicCode("The code for an array of the specified type might not be available.")]
	public override Type MakeArrayType()
	{
		throw null;
	}

	[RequiresDynamicCode("The code for an array of the specified type might not be available.")]
	public override Type MakeArrayType(int rank)
	{
		throw null;
	}

	public override Type MakeByRefType()
	{
		throw null;
	}

	public override Type MakePointerType()
	{
		throw null;
	}

	public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
	{
	}

	public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
	{
	}

	protected abstract void SetCustomAttributeCore(ConstructorInfo con, ReadOnlySpan<byte> binaryAttribute);
}
