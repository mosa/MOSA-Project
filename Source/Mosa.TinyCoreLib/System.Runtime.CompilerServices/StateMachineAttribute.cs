namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class StateMachineAttribute : Attribute
{
	public Type StateMachineType
	{
		get
		{
			throw null;
		}
	}

	public StateMachineAttribute(Type stateMachineType)
	{
	}
}
