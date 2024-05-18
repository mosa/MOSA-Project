namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public sealed class MetadataViewImplementationAttribute : Attribute
{
	public Type? ImplementationType
	{
		get
		{
			throw null;
		}
	}

	public MetadataViewImplementationAttribute(Type? implementationType)
	{
	}
}
