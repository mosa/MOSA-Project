namespace System.Runtime.InteropServices.Marshalling;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate)]
public sealed class NativeMarshallingAttribute : Attribute
{
	public Type NativeType
	{
		get
		{
			throw null;
		}
	}

	public NativeMarshallingAttribute(Type nativeType)
	{
	}
}
