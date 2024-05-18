namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public sealed class InterpolatedStringHandlerArgumentAttribute : Attribute
{
	public string[] Arguments
	{
		get
		{
			throw null;
		}
	}

	public InterpolatedStringHandlerArgumentAttribute(string argument)
	{
	}

	public InterpolatedStringHandlerArgumentAttribute(params string[] arguments)
	{
	}
}
