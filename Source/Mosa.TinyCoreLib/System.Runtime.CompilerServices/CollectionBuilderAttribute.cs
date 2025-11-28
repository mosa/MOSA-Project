namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false)]
public sealed class CollectionBuilderAttribute : Attribute
{
	public Type BuilderType
	{
		get
		{
			throw null;
		}
	}

	public string MethodName
	{
		get
		{
			throw null;
		}
	}

	public CollectionBuilderAttribute(Type builderType, string methodName)
	{
	}
}
