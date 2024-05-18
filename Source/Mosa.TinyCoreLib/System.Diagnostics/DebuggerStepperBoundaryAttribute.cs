namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
public sealed class DebuggerStepperBoundaryAttribute : Attribute
{
}
