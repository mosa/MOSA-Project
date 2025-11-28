using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace System;

public abstract class Type : MemberInfo, IReflect
{
	public static readonly char Delimiter;

	public static readonly Type[] EmptyTypes;

	public static readonly MemberFilter FilterAttribute;

	public static readonly MemberFilter FilterName;

	public static readonly MemberFilter FilterNameIgnoreCase;

	public static readonly object Missing;

	public abstract Assembly Assembly { get; }

	public abstract string? AssemblyQualifiedName { get; }

	public TypeAttributes Attributes
	{
		get
		{
			throw null;
		}
	}

	public abstract Type? BaseType { get; }

	public virtual bool ContainsGenericParameters
	{
		get
		{
			throw null;
		}
	}

	public virtual MethodBase? DeclaringMethod
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

	public static Binder DefaultBinder
	{
		get
		{
			throw null;
		}
	}

	public abstract string? FullName { get; }

	public virtual GenericParameterAttributes GenericParameterAttributes
	{
		get
		{
			throw null;
		}
	}

	public virtual int GenericParameterPosition
	{
		get
		{
			throw null;
		}
	}

	public virtual Type[] GenericTypeArguments
	{
		get
		{
			throw null;
		}
	}

	public abstract Guid GUID { get; }

	public bool HasElementType
	{
		get
		{
			throw null;
		}
	}

	public bool IsAbstract
	{
		get
		{
			throw null;
		}
	}

	public bool IsAnsiClass
	{
		get
		{
			throw null;
		}
	}

	public bool IsArray
	{
		get
		{
			throw null;
		}
	}

	public bool IsAutoClass
	{
		get
		{
			throw null;
		}
	}

	public bool IsAutoLayout
	{
		get
		{
			throw null;
		}
	}

	public bool IsByRef
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsByRefLike
	{
		get
		{
			throw null;
		}
	}

	public bool IsClass
	{
		get
		{
			throw null;
		}
	}

	public bool IsCOMObject
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsConstructedGenericType
	{
		get
		{
			throw null;
		}
	}

	public bool IsContextful
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsEnum
	{
		get
		{
			throw null;
		}
	}

	public bool IsExplicitLayout
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsFunctionPointer
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsGenericMethodParameter
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsGenericParameter
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsGenericType
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsGenericTypeDefinition
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsGenericTypeParameter
	{
		get
		{
			throw null;
		}
	}

	public bool IsImport
	{
		get
		{
			throw null;
		}
	}

	public bool IsInterface
	{
		get
		{
			throw null;
		}
	}

	public bool IsLayoutSequential
	{
		get
		{
			throw null;
		}
	}

	public bool IsMarshalByRef
	{
		get
		{
			throw null;
		}
	}

	public bool IsNested
	{
		get
		{
			throw null;
		}
	}

	public bool IsNestedAssembly
	{
		get
		{
			throw null;
		}
	}

	public bool IsNestedFamANDAssem
	{
		get
		{
			throw null;
		}
	}

	public bool IsNestedFamily
	{
		get
		{
			throw null;
		}
	}

	public bool IsNestedFamORAssem
	{
		get
		{
			throw null;
		}
	}

	public bool IsNestedPrivate
	{
		get
		{
			throw null;
		}
	}

	public bool IsNestedPublic
	{
		get
		{
			throw null;
		}
	}

	public bool IsNotPublic
	{
		get
		{
			throw null;
		}
	}

	public bool IsPointer
	{
		get
		{
			throw null;
		}
	}

	public bool IsPrimitive
	{
		get
		{
			throw null;
		}
	}

	public bool IsPublic
	{
		get
		{
			throw null;
		}
	}

	public bool IsSealed
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsSecurityCritical
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsSecuritySafeCritical
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsSecurityTransparent
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public virtual bool IsSerializable
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsSignatureType
	{
		get
		{
			throw null;
		}
	}

	public bool IsSpecialName
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsSZArray
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsTypeDefinition
	{
		get
		{
			throw null;
		}
	}

	public bool IsUnicodeClass
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsUnmanagedFunctionPointer
	{
		get
		{
			throw null;
		}
	}

	public bool IsValueType
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsVariableBoundArray
	{
		get
		{
			throw null;
		}
	}

	public bool IsVisible
	{
		get
		{
			throw null;
		}
	}

	public override MemberTypes MemberType
	{
		get
		{
			throw null;
		}
	}

	public new abstract Module Module { get; }

	public abstract string? Namespace { get; }

	public override Type? ReflectedType
	{
		get
		{
			throw null;
		}
	}

	public virtual StructLayoutAttribute? StructLayoutAttribute
	{
		get
		{
			throw null;
		}
	}

	public virtual RuntimeTypeHandle TypeHandle
	{
		get
		{
			throw null;
		}
	}

	public ConstructorInfo? TypeInitializer
	{
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
		get
		{
			throw null;
		}
	}

	public abstract Type UnderlyingSystemType { get; }

	public override bool Equals(object? o)
	{
		throw null;
	}

	public virtual bool Equals(Type? o)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
	public virtual Type[] FindInterfaces(TypeFilter filter, object? filterCriteria)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	public virtual MemberInfo[] FindMembers(MemberTypes memberType, BindingFlags bindingAttr, MemberFilter? filter, object? filterCriteria)
	{
		throw null;
	}

	public virtual int GetArrayRank()
	{
		throw null;
	}

	protected abstract TypeAttributes GetAttributeFlagsImpl();

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
	public ConstructorInfo? GetConstructor(BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
	public ConstructorInfo? GetConstructor(BindingFlags bindingAttr, Binder? binder, Type[] types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
	public ConstructorInfo? GetConstructor(BindingFlags bindingAttr, Type[] types)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
	public ConstructorInfo? GetConstructor(Type[] types)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
	protected abstract ConstructorInfo? GetConstructorImpl(BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers);

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
	public ConstructorInfo[] GetConstructors()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
	public abstract ConstructorInfo[] GetConstructors(BindingFlags bindingAttr);

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicEvents)]
	public virtual MemberInfo[] GetDefaultMembers()
	{
		throw null;
	}

	public abstract Type? GetElementType();

	public virtual string? GetEnumName(object value)
	{
		throw null;
	}

	public virtual string[] GetEnumNames()
	{
		throw null;
	}

	public virtual Type GetEnumUnderlyingType()
	{
		throw null;
	}

	[RequiresDynamicCode("It might not be possible to create an array of the enum type at runtime. Use Enum.GetValues<T> or the GetEnumValuesAsUnderlyingType method instead.")]
	public virtual Array GetEnumValues()
	{
		throw null;
	}

	public virtual Array GetEnumValuesAsUnderlyingType()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
	public EventInfo? GetEvent(string name)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
	public abstract EventInfo? GetEvent(string name, BindingFlags bindingAttr);

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
	public virtual EventInfo[] GetEvents()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
	public abstract EventInfo[] GetEvents(BindingFlags bindingAttr);

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)]
	public FieldInfo? GetField(string name)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)]
	public abstract FieldInfo? GetField(string name, BindingFlags bindingAttr);

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)]
	public FieldInfo[] GetFields()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)]
	public abstract FieldInfo[] GetFields(BindingFlags bindingAttr);

	public virtual Type[] GetFunctionPointerCallingConventions()
	{
		throw null;
	}

	public virtual Type[] GetFunctionPointerParameterTypes()
	{
		throw null;
	}

	public virtual Type GetFunctionPointerReturnType()
	{
		throw null;
	}

	public virtual Type[] GetGenericArguments()
	{
		throw null;
	}

	public virtual Type[] GetGenericParameterConstraints()
	{
		throw null;
	}

	public virtual Type GetGenericTypeDefinition()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
	public Type? GetInterface(string name)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
	public abstract Type? GetInterface(string name, bool ignoreCase);

	public virtual InterfaceMapping GetInterfaceMap([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] Type interfaceType)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
	public abstract Type[] GetInterfaces();

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicEvents)]
	public MemberInfo[] GetMember(string name)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
	public virtual MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
	public virtual MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicEvents)]
	public MemberInfo[] GetMembers()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
	public abstract MemberInfo[] GetMembers(BindingFlags bindingAttr);

	public virtual MemberInfo GetMemberWithSameMetadataDefinitionAs(MemberInfo member)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
	public MethodInfo? GetMethod(string name)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	public MethodInfo? GetMethod(string name, int genericParameterCount, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	public MethodInfo? GetMethod(string name, int genericParameterCount, BindingFlags bindingAttr, Binder? binder, Type[] types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
	public MethodInfo? GetMethod(string name, int genericParameterCount, Type[] types)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
	public MethodInfo? GetMethod(string name, int genericParameterCount, Type[] types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	public MethodInfo? GetMethod(string name, BindingFlags bindingAttr)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	public MethodInfo? GetMethod(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	public MethodInfo? GetMethod(string name, BindingFlags bindingAttr, Binder? binder, Type[] types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	public MethodInfo? GetMethod(string name, BindingFlags bindingAttr, Type[] types)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
	public MethodInfo? GetMethod(string name, Type[] types)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
	public MethodInfo? GetMethod(string name, Type[] types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	protected virtual MethodInfo? GetMethodImpl(string name, int genericParameterCount, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	protected abstract MethodInfo? GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers);

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
	public MethodInfo[] GetMethods()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	public abstract MethodInfo[] GetMethods(BindingFlags bindingAttr);

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes)]
	public Type? GetNestedType(string name)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes)]
	public abstract Type? GetNestedType(string name, BindingFlags bindingAttr);

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes)]
	public Type[] GetNestedTypes()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes)]
	public abstract Type[] GetNestedTypes(BindingFlags bindingAttr);

	public virtual Type[] GetOptionalCustomModifiers()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
	public PropertyInfo[] GetProperties()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)]
	public abstract PropertyInfo[] GetProperties(BindingFlags bindingAttr);

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
	public PropertyInfo? GetProperty(string name)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)]
	public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)]
	public PropertyInfo? GetProperty(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[] types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
	public PropertyInfo? GetProperty(string name, Type? returnType)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
	public PropertyInfo? GetProperty(string name, Type? returnType, Type[] types)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
	public PropertyInfo? GetProperty(string name, Type? returnType, Type[] types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
	public PropertyInfo? GetProperty(string name, Type[] types)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)]
	protected abstract PropertyInfo? GetPropertyImpl(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[]? types, ParameterModifier[]? modifiers);

	public virtual Type[] GetRequiredCustomModifiers()
	{
		throw null;
	}

	public new Type GetType()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The type might be removed")]
	public static Type? GetType(string typeName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The type might be removed")]
	public static Type? GetType(string typeName, bool throwOnError)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The type might be removed")]
	public static Type? GetType(string typeName, bool throwOnError, bool ignoreCase)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The type might be removed")]
	public static Type? GetType(string typeName, Func<AssemblyName, Assembly?>? assemblyResolver, Func<Assembly?, string, bool, Type?>? typeResolver)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The type might be removed")]
	public static Type? GetType(string typeName, Func<AssemblyName, Assembly?>? assemblyResolver, Func<Assembly?, string, bool, Type?>? typeResolver, bool throwOnError)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The type might be removed")]
	public static Type? GetType(string typeName, Func<AssemblyName, Assembly?>? assemblyResolver, Func<Assembly?, string, bool, Type?>? typeResolver, bool throwOnError, bool ignoreCase)
	{
		throw null;
	}

	public static Type[] GetTypeArray(object[] args)
	{
		throw null;
	}

	public static TypeCode GetTypeCode(Type? type)
	{
		throw null;
	}

	protected virtual TypeCode GetTypeCodeImpl()
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static Type? GetTypeFromCLSID(Guid clsid)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static Type? GetTypeFromCLSID(Guid clsid, bool throwOnError)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static Type? GetTypeFromCLSID(Guid clsid, string? server)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static Type? GetTypeFromCLSID(Guid clsid, string? server, bool throwOnError)
	{
		throw null;
	}

	public static Type? GetTypeFromHandle(RuntimeTypeHandle handle)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static Type? GetTypeFromProgID(string progID)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static Type? GetTypeFromProgID(string progID, bool throwOnError)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static Type? GetTypeFromProgID(string progID, string? server)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static Type? GetTypeFromProgID(string progID, string? server, bool throwOnError)
	{
		throw null;
	}

	public static RuntimeTypeHandle GetTypeHandle(object o)
	{
		throw null;
	}

	protected abstract bool HasElementTypeImpl();

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	public object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	public object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, CultureInfo? culture)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
	public abstract object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, CultureInfo? culture, string[]? namedParameters);

	protected abstract bool IsArrayImpl();

	public virtual bool IsAssignableFrom([NotNullWhen(true)] Type? c)
	{
		throw null;
	}

	public bool IsAssignableTo([NotNullWhen(true)] Type? targetType)
	{
		throw null;
	}

	protected abstract bool IsByRefImpl();

	protected abstract bool IsCOMObjectImpl();

	protected virtual bool IsContextfulImpl()
	{
		throw null;
	}

	public virtual bool IsEnumDefined(object value)
	{
		throw null;
	}

	public virtual bool IsEquivalentTo([NotNullWhen(true)] Type? other)
	{
		throw null;
	}

	public virtual bool IsInstanceOfType([NotNullWhen(true)] object? o)
	{
		throw null;
	}

	protected virtual bool IsMarshalByRefImpl()
	{
		throw null;
	}

	protected abstract bool IsPointerImpl();

	protected abstract bool IsPrimitiveImpl();

	public virtual bool IsSubclassOf(Type c)
	{
		throw null;
	}

	protected virtual bool IsValueTypeImpl()
	{
		throw null;
	}

	[RequiresDynamicCode("The code for an array of the specified type might not be available.")]
	public virtual Type MakeArrayType()
	{
		throw null;
	}

	[RequiresDynamicCode("The code for an array of the specified type might not be available.")]
	public virtual Type MakeArrayType(int rank)
	{
		throw null;
	}

	public virtual Type MakeByRefType()
	{
		throw null;
	}

	public static Type MakeGenericMethodParameter(int position)
	{
		throw null;
	}

	public static Type MakeGenericSignatureType(Type genericTypeDefinition, params Type[] typeArguments)
	{
		throw null;
	}

	[RequiresDynamicCode("The native code for this instantiation might not be available at runtime.")]
	[RequiresUnreferencedCode("If some of the generic arguments are annotated (either with DynamicallyAccessedMembersAttribute, or generic constraints), trimming can't validate that the requirements of those annotations are met.")]
	public virtual Type MakeGenericType(params Type[] typeArguments)
	{
		throw null;
	}

	public virtual Type MakePointerType()
	{
		throw null;
	}

	public static bool operator ==(Type? left, Type? right)
	{
		throw null;
	}

	public static bool operator !=(Type? left, Type? right)
	{
		throw null;
	}

	[Obsolete("ReflectionOnly loading is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0018", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static Type? ReflectionOnlyGetType(string typeName, bool throwIfNotFound, bool ignoreCase)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
