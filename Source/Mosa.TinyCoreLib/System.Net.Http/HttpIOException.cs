using System.IO;

namespace System.Net.Http;

public class HttpIOException : IOException
{
	public HttpRequestError HttpRequestError
	{
		get
		{
			throw null;
		}
	}

	public HttpIOException(HttpRequestError httpRequestError, string? message = null, Exception? innerException = null)
	{
	}
}
