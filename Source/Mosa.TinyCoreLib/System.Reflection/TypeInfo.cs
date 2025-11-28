using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Reflection;

public abstract class TypeInfo : Type, IReflectableType
{
	public virtual IEnumerable<ConstructorInfo> DeclaredConstructors
	{
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<EventInfo> DeclaredEvents
	{
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<FieldInfo> DeclaredFields
	{
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)]
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<MemberInfo> DeclaredMembers
	{
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<MethodInfo> DeclaredMethods
	{
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<TypeInfo> DeclaredNestedTypes
	{
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes)]
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<PropertyInfo> DeclaredProperties
	{
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)]
		get
		{
			throw null;
		}
	}

	public virtual Type[] GenericTypeParameters
	{
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<Type> ImplementedInterfaces
	{
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
		get
		{
			throw null;
		}
	}

	public virtual Type AsType()
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
	public virtual EventInfo? GetDeclaredEvent(string name)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)]
	public virtual FieldInfo? GetDeclaredField(string name)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	public virtual MethodInfo? GetDeclaredMethod(string name)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
	public virtual IEnumerable<MethodInfo> GetDeclaredMethods(string name)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes)]
	public virtual TypeInfo? GetDeclaredNestedType(string name)
	{
		throw null;
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)]
	public virtual PropertyInfo? GetDeclaredProperty(string name)
	{
		throw null;
	}

	public virtual bool IsAssignableFrom([NotNullWhen(true)] TypeInfo? typeInfo)
	{
		throw null;
	}

	TypeInfo IReflectableType.GetTypeInfo()
	{
		throw null;
	}
}
