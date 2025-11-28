namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
public sealed class MarshalAsAttribute : Attribute
{
	public UnmanagedType ArraySubType;

	public int IidParameterIndex;

	public string? MarshalCookie;

	public string? MarshalType;

	public Type? MarshalTypeRef;

	public VarEnum SafeArraySubType;

	public Type? SafeArrayUserDefinedSubType;

	public int SizeConst;

	public short SizeParamIndex;

	public UnmanagedType Value
	{
		get
		{
			throw null;
		}
	}

	public MarshalAsAttribute(short unmanagedType)
	{
	}

	public MarshalAsAttribute(UnmanagedType unmanagedType)
	{
	}
}
