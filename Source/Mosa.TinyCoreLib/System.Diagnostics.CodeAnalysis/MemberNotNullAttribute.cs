namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class MemberNotNullAttribute : Attribute
{
	public string[] Members
	{
		get
		{
			throw null;
		}
	}

	public MemberNotNullAttribute(string member)
	{
	}

	public MemberNotNullAttribute(params string[] members)
	{
	}
}
