namespace System;

[Obsolete("ExecutionEngineException previously indicated an unspecified fatal error in the runtime. The runtime no longer raises this exception so this type is obsolete.")]
public sealed class ExecutionEngineException : SystemException
{
	public ExecutionEngineException()
	{
	}

	public ExecutionEngineException(string? message)
	{
	}

	public ExecutionEngineException(string? message, Exception? innerException)
	{
	}
}
