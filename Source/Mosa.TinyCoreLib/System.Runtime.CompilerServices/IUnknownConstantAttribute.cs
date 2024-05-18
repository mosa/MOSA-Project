namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
public sealed class IUnknownConstantAttribute : CustomConstantAttribute
{
	public override object Value
	{
		get
		{
			throw null;
		}
	}
}
