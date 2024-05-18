using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

public abstract class TypeBuilder : TypeInfo
{
	public const int UnspecifiedTypeSize = 0;

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

	public override MethodBase? DeclaringMethod
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

	public override GenericParameterAttributes GenericParameterAttributes
	{
		get
		{
			throw null;
		}
	}

	public override int GenericParameterPosition
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

	public override bool IsGenericParameter
	{
		get
		{
			throw null;
		}
	}

	public override bool IsGenericType
	{
		get
		{
			throw null;
		}
	}

	public override bool IsGenericTypeDefinition
	{
		get
		{
			throw null;
		}
	}

	public override bool IsSecurityCritical
	{
		get
		{
			throw null;
		}
	}

	public override bool IsSecuritySafeCritical
	{
		get
		{
			throw null;
		}
	}

	public override bool IsSecurityTransparent
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

	public override int MetadataToken
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

	public PackingSize PackingSize
	{
		get
		{
			throw null;
		}
	}

	protected abstract PackingSize PackingSizeCore { get; }

	public override Type? ReflectedType
	{
		get
		{
			throw null;
		}
	}

	public int Size
	{
		get
		{
			throw null;
		}
	}

	protected abstract int SizeCore { get; }

	public override RuntimeTypeHandle TypeHandle
	{
		get
		{
			throw null;
		}
	}

	public override Type UnderlyingSystemType
	{
		get
		{
			throw null;
		}
	}

	public void AddInterfaceImplementation([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type interfaceType)
	{
	}

	protected abstract void AddInterfaceImplementationCore([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type interfaceType);

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

	public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[]? parameterTypes)
	{
		throw null;
	}

	public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[]? parameterTypes, Type[][]? requiredCustomModifiers, Type[][]? optionalCustomModifiers)
	{
		throw null;
	}

	protected abstract ConstructorBuilder DefineConstructorCore(MethodAttributes attributes, CallingConventions callingConvention, Type[]? parameterTypes, Type[][]? requiredCustomModifiers, Type[][]? optionalCustomModifiers);

	public ConstructorBuilder DefineDefaultConstructor(MethodAttributes attributes)
	{
		throw null;
	}

	protected abstract ConstructorBuilder DefineDefaultConstructorCore(MethodAttributes attributes);

	public EventBuilder DefineEvent(string name, EventAttributes attributes, Type eventtype)
	{
		throw null;
	}

	protected abstract EventBuilder DefineEventCore(string name, EventAttributes attributes, Type eventtype);

	public FieldBuilder DefineField(string fieldName, Type type, FieldAttributes attributes)
	{
		throw null;
	}

	public FieldBuilder DefineField(string fieldName, Type type, Type[]? requiredCustomModifiers, Type[]? optionalCustomModifiers, FieldAttributes attributes)
	{
		throw null;
	}

	protected abstract FieldBuilder DefineFieldCore(string fieldName, Type type, Type[]? requiredCustomModifiers, Type[]? optionalCustomModifiers, FieldAttributes attributes);

	public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
	{
		throw null;
	}

	protected abstract GenericTypeParameterBuilder[] DefineGenericParametersCore(params string[] names);

	public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
	{
		throw null;
	}

	protected abstract FieldBuilder DefineInitializedDataCore(string name, byte[] data, FieldAttributes attributes);

	public MethodBuilder DefineMethod(string name, MethodAttributes attributes)
	{
		throw null;
	}

	public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention)
	{
		throw null;
	}

	public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? parameterTypes)
	{
		throw null;
	}

	public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? returnTypeRequiredCustomModifiers, Type[]? returnTypeOptionalCustomModifiers, Type[]? parameterTypes, Type[][]? parameterTypeRequiredCustomModifiers, Type[][]? parameterTypeOptionalCustomModifiers)
	{
		throw null;
	}

	protected abstract MethodBuilder DefineMethodCore(string name, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? returnTypeRequiredCustomModifiers, Type[]? returnTypeOptionalCustomModifiers, Type[]? parameterTypes, Type[][]? parameterTypeRequiredCustomModifiers, Type[][]? parameterTypeOptionalCustomModifiers);

	public MethodBuilder DefineMethod(string name, MethodAttributes attributes, Type? returnType, Type[]? parameterTypes)
	{
		throw null;
	}

	public void DefineMethodOverride(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration)
	{
	}

	protected abstract void DefineMethodOverrideCore(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration);

	public TypeBuilder DefineNestedType(string name)
	{
		throw null;
	}

	public TypeBuilder DefineNestedType(string name, TypeAttributes attr)
	{
		throw null;
	}

	public TypeBuilder DefineNestedType(string name, TypeAttributes attr, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? parent)
	{
		throw null;
	}

	public TypeBuilder DefineNestedType(string name, TypeAttributes attr, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? parent, int typeSize)
	{
		throw null;
	}

	public TypeBuilder DefineNestedType(string name, TypeAttributes attr, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? parent, PackingSize packSize)
	{
		throw null;
	}

	public TypeBuilder DefineNestedType(string name, TypeAttributes attr, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? parent, PackingSize packSize, int typeSize)
	{
		throw null;
	}

	public TypeBuilder DefineNestedType(string name, TypeAttributes attr, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? parent, Type[]? interfaces)
	{
		throw null;
	}

	protected abstract TypeBuilder DefineNestedTypeCore(string name, TypeAttributes attr, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? parent, Type[]? interfaces, PackingSize packSize, int typeSize);

	[RequiresUnreferencedCode("P/Invoke marshalling may dynamically access members that could be trimmed.")]
	public MethodBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
	{
		throw null;
	}

	[RequiresUnreferencedCode("P/Invoke marshalling may dynamically access members that could be trimmed.")]
	public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
	{
		throw null;
	}

	[RequiresUnreferencedCode("P/Invoke marshalling may dynamically access members that could be trimmed.")]
	public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? returnTypeRequiredCustomModifiers, Type[]? returnTypeOptionalCustomModifiers, Type[]? parameterTypes, Type[][]? parameterTypeRequiredCustomModifiers, Type[][]? parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
	{
		throw null;
	}

	[RequiresUnreferencedCode("P/Invoke marshalling may dynamically access members that could be trimmed.")]
	protected abstract MethodBuilder DefinePInvokeMethodCore(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? returnTypeRequiredCustomModifiers, Type[]? returnTypeOptionalCustomModifiers, Type[]? parameterTypes, Type[][]? parameterTypeRequiredCustomModifiers, Type[][]? parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet);

	public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[]? parameterTypes)
	{
		throw null;
	}

	public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[]? returnTypeRequiredCustomModifiers, Type[]? returnTypeOptionalCustomModifiers, Type[]? parameterTypes, Type[][]? parameterTypeRequiredCustomModifiers, Type[][]? parameterTypeOptionalCustomModifiers)
	{
		throw null;
	}

	public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[]? parameterTypes)
	{
		throw null;
	}

	public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[]? returnTypeRequiredCustomModifiers, Type[]? returnTypeOptionalCustomModifiers, Type[]? parameterTypes, Type[][]? parameterTypeRequiredCustomModifiers, Type[][]? parameterTypeOptionalCustomModifiers)
	{
		throw null;
	}

	protected abstract PropertyBuilder DefinePropertyCore(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[]? returnTypeRequiredCustomModifiers, Type[]? returnTypeOptionalCustomModifiers, Type[]? parameterTypes, Type[][]? parameterTypeRequiredCustomModifiers, Type[][]? parameterTypeOptionalCustomModifiers);

	public ConstructorBuilder DefineTypeInitializer()
	{
		throw null;
	}

	protected abstract ConstructorBuilder DefineTypeInitializerCore();

	public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
	{
		throw null;
	}

	protected abstract FieldBuilder DefineUninitializedDataCore(string name, int size, FieldAttributes attributes);

	protected override TypeAttributes GetAttributeFlagsImpl()
	{
		throw null;
	}

	public static ConstructorInfo GetConstructor(Type type, ConstructorInfo constructor)
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

	public override Type GetElementType()
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

	public static FieldInfo GetField(Type type, FieldInfo field)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)]
	public override FieldInfo[] GetFields(BindingFlags bindingAttr)
	{
		throw null;
	}

	public override Type[] GetGenericArguments()
	{
		throw null;
	}

	public override Type GetGenericTypeDefinition()
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

	public static MethodInfo GetMethod(Type type, MethodInfo method)
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

	public override bool IsAssignableFrom([NotNullWhen(true)] Type? c)
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

	public bool IsCreated()
	{
		throw null;
	}

	protected abstract bool IsCreatedCore();

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

	public override bool IsSubclassOf(Type c)
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

	[RequiresDynamicCode("The native code for this instantiation might not be available at runtime.")]
	[RequiresUnreferencedCode("If some of the generic arguments are annotated (either with DynamicallyAccessedMembersAttribute, or generic constraints), trimming can't validate that the requirements of those annotations are met.")]
	public override Type MakeGenericType(params Type[] typeArguments)
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

	public void SetParent([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? parent)
	{
	}

	protected abstract void SetParentCore([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? parent);

	public override string ToString()
	{
		throw null;
	}
}
