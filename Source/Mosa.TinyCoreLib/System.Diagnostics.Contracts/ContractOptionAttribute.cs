namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
[Conditional("CONTRACTS_FULL")]
public sealed class ContractOptionAttribute : Attribute
{
	public string Category { get; }

	public bool Enabled { get; }

	public string Setting { get; }

	public string? Value { get; }

	public ContractOptionAttribute(string category, string setting, bool enabled)
	{
		Category = category;
		Setting = setting;
		Enabled = enabled;
	}

	public ContractOptionAttribute(string category, string setting, string value)
	{
		Category = category;
		Setting = setting;
		Value = value;
	}
}
