namespace System.Reflection;

public sealed class NullabilityInfo
{
	public NullabilityInfo? ElementType
	{
		get
		{
			throw null;
		}
	}

	public NullabilityInfo[] GenericTypeArguments
	{
		get
		{
			throw null;
		}
	}

	public NullabilityState ReadState
	{
		get
		{
			throw null;
		}
	}

	public Type Type
	{
		get
		{
			throw null;
		}
	}

	public NullabilityState WriteState
	{
		get
		{
			throw null;
		}
	}

	internal NullabilityInfo()
	{
	}
}
