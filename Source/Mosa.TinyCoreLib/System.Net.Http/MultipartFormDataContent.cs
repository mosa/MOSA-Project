using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public class MultipartFormDataContent : MultipartContent
{
	public MultipartFormDataContent()
	{
	}

	public MultipartFormDataContent(string boundary)
	{
	}

	public override void Add(HttpContent content)
	{
	}

	public void Add(HttpContent content, string name)
	{
	}

	public void Add(HttpContent content, string name, string fileName)
	{
	}

	protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context, CancellationToken cancellationToken)
	{
		throw null;
	}
}
