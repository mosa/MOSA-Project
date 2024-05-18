namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
public sealed class DebuggerNonUserCodeAttribute : Attribute
{
}
