namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
public sealed class ComAliasNameAttribute : Attribute
{
	public string Value
	{
		get
		{
			throw null;
		}
	}

	public ComAliasNameAttribute(string alias)
	{
	}
}
