namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
public sealed class AsyncMethodBuilderAttribute : Attribute
{
	public Type BuilderType
	{
		get
		{
			throw null;
		}
	}

	public AsyncMethodBuilderAttribute(Type builderType)
	{
	}
}
