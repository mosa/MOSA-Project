using System.Collections.Generic;

namespace System.Reflection.Context;

public abstract class CustomReflectionContext : ReflectionContext
{
	protected CustomReflectionContext()
	{
	}

	protected CustomReflectionContext(ReflectionContext source)
	{
	}

	protected virtual IEnumerable<PropertyInfo> AddProperties(Type type)
	{
		throw null;
	}

	protected PropertyInfo CreateProperty(Type propertyType, string name, Func<object, object?>? getter, Action<object, object?>? setter)
	{
		throw null;
	}

	protected PropertyInfo CreateProperty(Type propertyType, string name, Func<object, object?>? getter, Action<object, object?>? setter, IEnumerable<Attribute>? propertyCustomAttributes, IEnumerable<Attribute>? getterCustomAttributes, IEnumerable<Attribute>? setterCustomAttributes)
	{
		throw null;
	}

	protected virtual IEnumerable<object> GetCustomAttributes(MemberInfo member, IEnumerable<object> declaredAttributes)
	{
		throw null;
	}

	protected virtual IEnumerable<object> GetCustomAttributes(ParameterInfo parameter, IEnumerable<object> declaredAttributes)
	{
		throw null;
	}

	public override Assembly MapAssembly(Assembly assembly)
	{
		throw null;
	}

	public override TypeInfo MapType(TypeInfo type)
	{
		throw null;
	}
}
