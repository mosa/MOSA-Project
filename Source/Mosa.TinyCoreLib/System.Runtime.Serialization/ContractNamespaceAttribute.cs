namespace System.Runtime.Serialization;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module, Inherited = false, AllowMultiple = true)]
public sealed class ContractNamespaceAttribute : Attribute
{
	public string? ClrNamespace
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string ContractNamespace
	{
		get
		{
			throw null;
		}
	}

	public ContractNamespaceAttribute(string contractNamespace)
	{
	}
}
