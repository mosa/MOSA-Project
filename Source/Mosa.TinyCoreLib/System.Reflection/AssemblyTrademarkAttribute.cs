namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyTrademarkAttribute(string trademark) : Attribute
{
	public string Trademark { get; } = trademark;
}
