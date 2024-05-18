namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class CppInlineNamespaceAttribute : Attribute
{
	public CppInlineNamespaceAttribute(string dottedName)
	{
	}
}
