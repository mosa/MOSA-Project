namespace System.Reflection;

public sealed class TargetInvocationException : ApplicationException
{
	public TargetInvocationException(Exception? inner)
	{
	}

	public TargetInvocationException(string? message, Exception? inner)
	{
	}
}
