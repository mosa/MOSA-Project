namespace System.Net.Http.Metrics;

public sealed class HttpMetricsEnrichmentContext
{
	public HttpRequestMessage Request
	{
		get
		{
			throw null;
		}
	}

	public HttpResponseMessage? Response
	{
		get
		{
			throw null;
		}
	}

	public Exception? Exception
	{
		get
		{
			throw null;
		}
	}

	internal HttpMetricsEnrichmentContext()
	{
	}

	public void AddCustomTag(string name, object? value)
	{
		throw null;
	}

	public static void AddCallback(HttpRequestMessage request, Action<HttpMetricsEnrichmentContext> callback)
	{
		throw null;
	}
}
