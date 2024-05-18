using System.IO;

namespace System.Net.Http;

public sealed class SocketsHttpPlaintextStreamFilterContext
{
	public Stream PlaintextStream
	{
		get
		{
			throw null;
		}
	}

	public Version NegotiatedHttpVersion
	{
		get
		{
			throw null;
		}
	}

	public HttpRequestMessage InitialRequestMessage
	{
		get
		{
			throw null;
		}
	}

	internal SocketsHttpPlaintextStreamFilterContext()
	{
	}
}
