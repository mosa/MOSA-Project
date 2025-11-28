namespace System.Runtime.InteropServices.Marshalling;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public sealed class CustomMarshallerAttribute : Attribute
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct GenericPlaceholder
	{
	}

	public Type ManagedType
	{
		get
		{
			throw null;
		}
	}

	public MarshalMode MarshalMode
	{
		get
		{
			throw null;
		}
	}

	public Type MarshallerType
	{
		get
		{
			throw null;
		}
	}

	public CustomMarshallerAttribute(Type managedType, MarshalMode marshalMode, Type marshallerType)
	{
	}
}
