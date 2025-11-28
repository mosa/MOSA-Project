namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class IteratorStateMachineAttribute : StateMachineAttribute
{
	public IteratorStateMachineAttribute(Type stateMachineType)
		: base(null)
	{
	}
}
