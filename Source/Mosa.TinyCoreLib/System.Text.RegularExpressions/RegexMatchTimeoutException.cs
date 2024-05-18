using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Text.RegularExpressions;

public class RegexMatchTimeoutException : TimeoutException, ISerializable
{
	public string Input
	{
		get
		{
			throw null;
		}
	}

	public TimeSpan MatchTimeout
	{
		get
		{
			throw null;
		}
	}

	public string Pattern
	{
		get
		{
			throw null;
		}
	}

	public RegexMatchTimeoutException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected RegexMatchTimeoutException(SerializationInfo info, StreamingContext context)
	{
	}

	public RegexMatchTimeoutException(string message)
	{
	}

	public RegexMatchTimeoutException(string message, Exception inner)
	{
	}

	public RegexMatchTimeoutException(string regexInput, string regexPattern, TimeSpan matchTimeout)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
