namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class AsyncStateMachineAttribute : StateMachineAttribute
{
	public AsyncStateMachineAttribute(Type stateMachineType)
		: base(null)
	{
	}
}
