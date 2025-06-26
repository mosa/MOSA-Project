namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
public sealed class GuidAttribute(string guid) : Attribute
{
	public string Value { get; } = guid;
}
