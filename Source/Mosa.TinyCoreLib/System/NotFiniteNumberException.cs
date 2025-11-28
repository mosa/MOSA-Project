using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class NotFiniteNumberException : ArithmeticException
{
	public double OffendingNumber
	{
		get
		{
			throw null;
		}
	}

	public NotFiniteNumberException()
	{
	}

	public NotFiniteNumberException(double offendingNumber)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected NotFiniteNumberException(SerializationInfo info, StreamingContext context)
	{
	}

	public NotFiniteNumberException(string? message)
	{
	}

	public NotFiniteNumberException(string? message, double offendingNumber)
	{
	}

	public NotFiniteNumberException(string? message, double offendingNumber, Exception? innerException)
	{
	}

	public NotFiniteNumberException(string? message, Exception? innerException)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
