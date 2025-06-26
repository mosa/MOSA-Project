using System.Collections.Generic;

namespace System.Reflection;

public class CustomAttributeData
{
	public virtual Type AttributeType { get; }

	public virtual ConstructorInfo Constructor
	{
		get
		{
			throw null;
		}
	}

	public virtual IList<CustomAttributeTypedArgument> ConstructorArguments { get; }

	public virtual IList<CustomAttributeNamedArgument> NamedArguments { get; }

	protected CustomAttributeData()
	{
	}

	public override bool Equals(object? obj)
	{
		throw null;
	}

	public static IList<CustomAttributeData> GetCustomAttributes(Assembly target)
	{
		throw null;
	}

	public static IList<CustomAttributeData> GetCustomAttributes(MemberInfo target)
	{
		throw null;
	}

	public static IList<CustomAttributeData> GetCustomAttributes(Module target)
	{
		throw null;
	}

	public static IList<CustomAttributeData> GetCustomAttributes(ParameterInfo target)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
