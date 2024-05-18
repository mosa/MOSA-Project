using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public class AggregateException : Exception
{
	public ReadOnlyCollection<Exception> InnerExceptions
	{
		get
		{
			throw null;
		}
	}

	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public AggregateException()
	{
	}

	public AggregateException(IEnumerable<Exception> innerExceptions)
	{
	}

	public AggregateException(params Exception[] innerExceptions)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected AggregateException(SerializationInfo info, StreamingContext context)
	{
	}

	public AggregateException(string? message)
	{
	}

	public AggregateException(string? message, IEnumerable<Exception> innerExceptions)
	{
	}

	public AggregateException(string? message, Exception innerException)
	{
	}

	public AggregateException(string? message, params Exception[] innerExceptions)
	{
	}

	public AggregateException Flatten()
	{
		throw null;
	}

	public override Exception GetBaseException()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public void Handle(Func<Exception, bool> predicate)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
