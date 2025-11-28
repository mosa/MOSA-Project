using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Context;

namespace System.ComponentModel.Composition.Registration;

public class RegistrationBuilder : CustomReflectionContext
{
	public PartBuilder ForType(Type type)
	{
		throw null;
	}

	public PartBuilder ForTypesDerivedFrom(Type type)
	{
		throw null;
	}

	public PartBuilder<T> ForTypesDerivedFrom<T>()
	{
		throw null;
	}

	public PartBuilder ForTypesMatching(Predicate<Type> typeFilter)
	{
		throw null;
	}

	public PartBuilder<T> ForTypesMatching<T>(Predicate<Type> typeFilter)
	{
		throw null;
	}

	public PartBuilder<T> ForType<T>()
	{
		throw null;
	}

	protected override IEnumerable<object> GetCustomAttributes(MemberInfo member, IEnumerable<object> declaredAttributes)
	{
		throw null;
	}

	protected override IEnumerable<object> GetCustomAttributes(ParameterInfo parameter, IEnumerable<object> declaredAttributes)
	{
		throw null;
	}
}
