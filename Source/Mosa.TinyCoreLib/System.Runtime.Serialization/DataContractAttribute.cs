namespace System.Runtime.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
public sealed class DataContractAttribute : Attribute
{
	public bool IsNameSetExplicitly
	{
		get
		{
			throw null;
		}
	}

	public bool IsNamespaceSetExplicitly
	{
		get
		{
			throw null;
		}
	}

	public bool IsReference
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsReferenceSetExplicitly
	{
		get
		{
			throw null;
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

	public string? Namespace
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
