namespace System.Reflection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
public sealed class DefaultMemberAttribute : Attribute
{
	public string MemberName
	{
		get
		{
			throw null;
		}
	}

	public DefaultMemberAttribute(string memberName)
	{
	}
}
