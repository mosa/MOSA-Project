namespace System.Runtime.InteropServices.Marshalling;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
public sealed class MarshalUsingAttribute : Attribute
{
	public const string ReturnsCountValue = "return-value";

	public Type? NativeType
	{
		get
		{
			throw null;
		}
	}

	public string CountElementName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ConstantElementCount
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ElementIndirectionDepth
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MarshalUsingAttribute()
	{
	}

	public MarshalUsingAttribute(Type nativeType)
	{
	}
}
