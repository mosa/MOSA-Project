namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.All)]
internal sealed class DecoratedNameAttribute : Attribute
{
	public DecoratedNameAttribute(string decoratedName)
	{
	}
}
