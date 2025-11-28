namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
public sealed class DecimalConstantAttribute : Attribute
{
	public decimal Value { get; }

	public DecimalConstantAttribute(byte scale, byte sign, int hi, int mid, int low)
		=> Value = new decimal(low, mid, hi, sign != 0, scale);

	[CLSCompliant(false)]
	public DecimalConstantAttribute(byte scale, byte sign, uint hi, uint mid, uint low)
		=> Value = new decimal((int)low, (int)mid, (int)hi, sign != 0, scale);
}
