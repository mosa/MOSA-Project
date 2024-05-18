namespace System.Reflection;

public abstract class ReflectionContext
{
	public virtual TypeInfo GetTypeForObject(object value)
	{
		throw null;
	}

	public abstract Assembly MapAssembly(Assembly assembly);

	public abstract TypeInfo MapType(TypeInfo type);
}
