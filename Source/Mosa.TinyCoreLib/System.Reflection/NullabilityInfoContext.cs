namespace System.Reflection;

public sealed class NullabilityInfoContext
{
	public NullabilityInfo Create(EventInfo eventInfo)
	{
		throw null;
	}

	public NullabilityInfo Create(FieldInfo fieldInfo)
	{
		throw null;
	}

	public NullabilityInfo Create(ParameterInfo parameterInfo)
	{
		throw null;
	}

	public NullabilityInfo Create(PropertyInfo propertyInfo)
	{
		throw null;
	}
}
