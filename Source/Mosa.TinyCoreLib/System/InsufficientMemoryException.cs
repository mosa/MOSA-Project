namespace System;

public sealed class InsufficientMemoryException : OutOfMemoryException
{
	public InsufficientMemoryException()
	{
	}

	public InsufficientMemoryException(string? message)
	{
	}

	public InsufficientMemoryException(string? message, Exception? innerException)
	{
	}
}
