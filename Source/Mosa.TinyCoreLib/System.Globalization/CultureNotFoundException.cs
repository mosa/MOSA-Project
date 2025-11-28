using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Globalization;

public class CultureNotFoundException : ArgumentException
{
	public virtual int? InvalidCultureId
	{
		get
		{
			throw null;
		}
	}

	public virtual string? InvalidCultureName
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

	public CultureNotFoundException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected CultureNotFoundException(SerializationInfo info, StreamingContext context)
	{
	}

	public CultureNotFoundException(string? message)
	{
	}

	public CultureNotFoundException(string? message, Exception? innerException)
	{
	}

	public CultureNotFoundException(string? message, int invalidCultureId, Exception? innerException)
	{
	}

	public CultureNotFoundException(string? paramName, int invalidCultureId, string? message)
	{
	}

	public CultureNotFoundException(string? paramName, string? message)
	{
	}

	public CultureNotFoundException(string? message, string? invalidCultureName, Exception? innerException)
	{
	}

	public CultureNotFoundException(string? paramName, string? invalidCultureName, string? message)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
