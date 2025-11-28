namespace System.Reflection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
public sealed class DefaultMemberAttribute(string memberName) : Attribute
{
	public string MemberName { get; } = memberName;
}
