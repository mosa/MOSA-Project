namespace System.Net.Http;

public class HttpRequestException : Exception
{
	public HttpRequestError HttpRequestError
	{
		get
		{
			throw null;
		}
	}

	public HttpStatusCode? StatusCode
	{
		get
		{
			throw null;
		}
	}

	public HttpRequestException()
	{
	}

	public HttpRequestException(string? message)
	{
	}

	public HttpRequestException(string? message, Exception? inner)
	{
	}

	public HttpRequestException(string? message, Exception? inner, HttpStatusCode? statusCode)
	{
	}

	public HttpRequestException(HttpRequestError httpRequestError, string? message = null, Exception? inner = null, HttpStatusCode? statusCode = null)
	{
	}
}
