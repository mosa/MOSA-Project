using System.Reflection;

namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
public class CatalogReflectionContextAttribute : Attribute
{
	public CatalogReflectionContextAttribute(Type reflectionContextType)
	{
	}

	public ReflectionContext CreateReflectionContext()
	{
		throw null;
	}
}
