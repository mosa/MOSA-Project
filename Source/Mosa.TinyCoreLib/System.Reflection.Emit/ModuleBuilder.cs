using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

public abstract class ModuleBuilder : Module
{
	public override Assembly Assembly
	{
		get
		{
			throw null;
		}
	}

	[RequiresAssemblyFiles("Returns <Unknown> for modules with no file path")]
	public override string FullyQualifiedName
	{
		get
		{
			throw null;
		}
	}

	public override int MDStreamVersion
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

	public override Guid ModuleVersionId
	{
		get
		{
			throw null;
		}
	}

	[RequiresAssemblyFiles("Returns <Unknown> for modules with no file path")]
	public override string Name
	{
		get
		{
			throw null;
		}
	}

	public override string ScopeName
	{
		get
		{
			throw null;
		}
	}

	public void CreateGlobalFunctions()
	{
	}

	protected abstract void CreateGlobalFunctionsCore();

	public EnumBuilder DefineEnum(string name, TypeAttributes visibility, Type underlyingType)
	{
		throw null;
	}

	protected abstract EnumBuilder DefineEnumCore(string name, TypeAttributes visibility, Type underlyingType);

	public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? parameterTypes)
	{
		throw null;
	}

	public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? requiredReturnTypeCustomModifiers, Type[]? optionalReturnTypeCustomModifiers, Type[]? parameterTypes, Type[][]? requiredParameterTypeCustomModifiers, Type[][]? optionalParameterTypeCustomModifiers)
	{
		throw null;
	}

	protected abstract MethodBuilder DefineGlobalMethodCore(string name, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? requiredReturnTypeCustomModifiers, Type[]? optionalReturnTypeCustomModifiers, Type[]? parameterTypes, Type[][]? requiredParameterTypeCustomModifiers, Type[][]? optionalParameterTypeCustomModifiers);

	public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, Type? returnType, Type[]? parameterTypes)
	{
		throw null;
	}

	public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
	{
		throw null;
	}

	protected abstract FieldBuilder DefineInitializedDataCore(string name, byte[] data, FieldAttributes attributes);

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
	protected abstract MethodBuilder DefinePInvokeMethodCore(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet);

	public TypeBuilder DefineType(string name)
	{
		throw null;
	}

	public TypeBuilder DefineType(string name, TypeAttributes attr)
	{
		throw null;
	}

	public TypeBuilder DefineType(string name, TypeAttributes attr, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? parent)
	{
		throw null;
	}

	public TypeBuilder DefineType(string name, TypeAttributes attr, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? parent, int typesize)
	{
		throw null;
	}

	public TypeBuilder DefineType(string name, TypeAttributes attr, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? parent, PackingSize packsize)
	{
		throw null;
	}

	public TypeBuilder DefineType(string name, TypeAttributes attr, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? parent, PackingSize packingSize, int typesize)
	{
		throw null;
	}

	public TypeBuilder DefineType(string name, TypeAttributes attr, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? parent, Type[]? interfaces)
	{
		throw null;
	}

	protected abstract TypeBuilder DefineTypeCore(string name, TypeAttributes attr, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type? parent, Type[]? interfaces, PackingSize packingSize, int typesize);

	public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
	{
		throw null;
	}

	protected abstract FieldBuilder DefineUninitializedDataCore(string name, int size, FieldAttributes attributes);

	public override bool Equals(object? obj)
	{
		throw null;
	}

	public MethodInfo GetArrayMethod(Type arrayClass, string methodName, CallingConventions callingConvention, Type? returnType, Type[]? parameterTypes)
	{
		throw null;
	}

	protected abstract MethodInfo GetArrayMethodCore(Type arrayClass, string methodName, CallingConventions callingConvention, Type? returnType, Type[]? parameterTypes);

	public override object[] GetCustomAttributes(bool inherit)
	{
		throw null;
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw null;
	}

	public override IList<CustomAttributeData> GetCustomAttributesData()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Fields might be removed")]
	public override FieldInfo? GetField(string name, BindingFlags bindingAttr)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Fields might be removed")]
	public override FieldInfo[] GetFields(BindingFlags bindingFlags)
	{
		throw null;
	}

	public abstract int GetFieldMetadataToken(FieldInfo field);

	public override int GetHashCode()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Methods might be removed")]
	protected override MethodInfo? GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	public abstract int GetMethodMetadataToken(ConstructorInfo constructor);

	public abstract int GetMethodMetadataToken(MethodInfo method);

	[RequiresUnreferencedCode("Methods might be removed")]
	public override MethodInfo[] GetMethods(BindingFlags bindingFlags)
	{
		throw null;
	}

	public override void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public override Type? GetType(string className)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public override Type? GetType(string className, bool ignoreCase)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public override Type? GetType(string className, bool throwOnError, bool ignoreCase)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public override Type[] GetTypes()
	{
		throw null;
	}

	public abstract int GetTypeMetadataToken(Type type);

	public abstract int GetSignatureMetadataToken(SignatureHelper signature);

	public abstract int GetStringMetadataToken(string stringConstant);

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw null;
	}

	public override bool IsResource()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public override FieldInfo? ResolveField(int metadataToken, Type[]? genericTypeArguments, Type[]? genericMethodArguments)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public override MemberInfo? ResolveMember(int metadataToken, Type[]? genericTypeArguments, Type[]? genericMethodArguments)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public override MethodBase? ResolveMethod(int metadataToken, Type[]? genericTypeArguments, Type[]? genericMethodArguments)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public override byte[] ResolveSignature(int metadataToken)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public override string ResolveString(int metadataToken)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public override Type ResolveType(int metadataToken, Type[]? genericTypeArguments, Type[]? genericMethodArguments)
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
