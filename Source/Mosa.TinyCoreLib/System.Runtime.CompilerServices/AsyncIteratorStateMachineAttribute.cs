namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class AsyncIteratorStateMachineAttribute : StateMachineAttribute
{
	public AsyncIteratorStateMachineAttribute(Type stateMachineType)
		: base(null)
	{
	}
}
