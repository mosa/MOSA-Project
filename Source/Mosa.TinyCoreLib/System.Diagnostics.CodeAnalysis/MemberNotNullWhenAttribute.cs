namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class MemberNotNullWhenAttribute : Attribute
{
	public string[] Members
	{
		get
		{
			throw null;
		}
	}

	public bool ReturnValue
	{
		get
		{
			throw null;
		}
	}

	public MemberNotNullWhenAttribute(bool returnValue, string member)
	{
	}

	public MemberNotNullWhenAttribute(bool returnValue, params string[] members)
	{
	}
}
