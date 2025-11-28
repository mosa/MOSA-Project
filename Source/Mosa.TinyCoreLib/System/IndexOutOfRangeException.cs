namespace System;

public sealed class IndexOutOfRangeException : SystemException
{
	public IndexOutOfRangeException()
		: base(Internal.Exceptions.IndexOutOfRangeException) {}

	public IndexOutOfRangeException(string? message)
		: base(message) {}

	public IndexOutOfRangeException(string? message, Exception? innerException)
		: base(message, innerException) {}
}
