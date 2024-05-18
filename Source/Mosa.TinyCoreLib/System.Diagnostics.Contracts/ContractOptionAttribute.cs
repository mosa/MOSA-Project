namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class ContractOptionAttribute : Attribute
{
	public string Category
	{
		get
		{
			throw null;
		}
	}

	public bool Enabled
	{
		get
		{
			throw null;
		}
	}

	public string Setting
	{
		get
		{
			throw null;
		}
	}

	public string? Value
	{
		get
		{
			throw null;
		}
	}

	public ContractOptionAttribute(string category, string setting, bool enabled)
	{
	}

	public ContractOptionAttribute(string category, string setting, string value)
	{
	}
}
