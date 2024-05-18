namespace System.Net.Http;

public sealed class HttpProtocolException : HttpIOException
{
	public long ErrorCode
	{
		get
		{
			throw null;
		}
	}

	public HttpProtocolException(long errorCode, string? message, Exception? innerException)
		: base(HttpRequestError.Unknown)
	{
	}
}
