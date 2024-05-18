namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
public sealed class RequiredAttributeAttribute : Attribute
{
	public Type RequiredContract
	{
		get
		{
			throw null;
		}
	}

	public RequiredAttributeAttribute(Type requiredContract)
	{
	}
}
