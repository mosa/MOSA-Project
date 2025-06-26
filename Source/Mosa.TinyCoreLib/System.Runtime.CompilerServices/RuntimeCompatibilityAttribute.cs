namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
public sealed class RuntimeCompatibilityAttribute : Attribute
{
	public bool WrapNonExceptionThrows { get; set; }
}
