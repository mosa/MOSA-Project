namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
public sealed class DecimalConstantAttribute : Attribute
{
	public decimal Value
	{
		get
		{
			throw null;
		}
	}

	public DecimalConstantAttribute(byte scale, byte sign, int hi, int mid, int low)
	{
	}

	[CLSCompliant(false)]
	public DecimalConstantAttribute(byte scale, byte sign, uint hi, uint mid, uint low)
	{
	}
}
