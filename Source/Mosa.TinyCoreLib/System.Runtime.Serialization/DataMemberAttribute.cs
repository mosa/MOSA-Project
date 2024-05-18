namespace System.Runtime.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class DataMemberAttribute : Attribute
{
	public bool EmitDefaultValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsNameSetExplicitly
	{
		get
		{
			throw null;
		}
	}

	public bool IsRequired
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Order
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}
}
