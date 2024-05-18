using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System.Reflection;

public abstract class Module : ICustomAttributeProvider, ISerializable
{
	public static readonly TypeFilter FilterTypeName;

	public static readonly TypeFilter FilterTypeNameIgnoreCase;

	public virtual Assembly Assembly
	{
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<CustomAttributeData> CustomAttributes
	{
		get
		{
			throw null;
		}
	}

	[RequiresAssemblyFiles("Returns <Unknown> for modules with no file path")]
	public virtual string FullyQualifiedName
	{
		get
		{
			throw null;
		}
	}

	public virtual int MDStreamVersion
	{
		get
		{
			throw null;
		}
	}

	public virtual int MetadataToken
	{
		get
		{
			throw null;
		}
	}

	public ModuleHandle ModuleHandle
	{
		get
		{
			throw null;
		}
	}

	public virtual Guid ModuleVersionId
	{
		get
		{
			throw null;
		}
	}

	[RequiresAssemblyFiles("Returns <Unknown> for modules with no file path")]
	public virtual string Name
	{
		get
		{
			throw null;
		}
	}

	public virtual string ScopeName
	{
		get
		{
			throw null;
		}
	}

	public override bool Equals(object? o)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public virtual Type[] FindTypes(TypeFilter? filter, object? filterCriteria)
	{
		throw null;
	}

	public virtual object[] GetCustomAttributes(bool inherit)
	{
		throw null;
	}

	public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw null;
	}

	public virtual IList<CustomAttributeData> GetCustomAttributesData()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Fields might be removed")]
	public FieldInfo? GetField(string name)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Fields might be removed")]
	public virtual FieldInfo? GetField(string name, BindingFlags bindingAttr)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Fields might be removed")]
	public FieldInfo[] GetFields()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Fields might be removed")]
	public virtual FieldInfo[] GetFields(BindingFlags bindingFlags)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Methods might be removed")]
	public MethodInfo? GetMethod(string name)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Methods might be removed")]
	public MethodInfo? GetMethod(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Methods might be removed")]
	public MethodInfo? GetMethod(string name, Type[] types)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Methods might be removed")]
	protected virtual MethodInfo? GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Methods might be removed")]
	public MethodInfo[] GetMethods()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Methods might be removed")]
	public virtual MethodInfo[] GetMethods(BindingFlags bindingFlags)
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public virtual void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public virtual Type? GetType(string className)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public virtual Type? GetType(string className, bool ignoreCase)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public virtual Type? GetType(string className, bool throwOnError, bool ignoreCase)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Types might be removed")]
	public virtual Type[] GetTypes()
	{
		throw null;
	}

	public virtual bool IsDefined(Type attributeType, bool inherit)
	{
		throw null;
	}

	public virtual bool IsResource()
	{
		throw null;
	}

	public static bool operator ==(Module? left, Module? right)
	{
		throw null;
	}

	public static bool operator !=(Module? left, Module? right)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public FieldInfo? ResolveField(int metadataToken)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public virtual FieldInfo? ResolveField(int metadataToken, Type[]? genericTypeArguments, Type[]? genericMethodArguments)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public MemberInfo? ResolveMember(int metadataToken)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public virtual MemberInfo? ResolveMember(int metadataToken, Type[]? genericTypeArguments, Type[]? genericMethodArguments)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public MethodBase? ResolveMethod(int metadataToken)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public virtual MethodBase? ResolveMethod(int metadataToken, Type[]? genericTypeArguments, Type[]? genericMethodArguments)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public virtual byte[] ResolveSignature(int metadataToken)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public virtual string ResolveString(int metadataToken)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public Type ResolveType(int metadataToken)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Trimming changes metadata tokens")]
	public virtual Type ResolveType(int metadataToken, Type[]? genericTypeArguments, Type[]? genericMethodArguments)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
