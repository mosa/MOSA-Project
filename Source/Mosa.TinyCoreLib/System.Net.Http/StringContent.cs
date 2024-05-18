using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public class StringContent : ByteArrayContent
{
	public StringContent(string content)
		: base(null)
	{
	}

	public StringContent(string content, MediaTypeHeaderValue mediaType)
		: base(null)
	{
	}

	public StringContent(string content, Encoding? encoding)
		: base(null)
	{
	}

	public StringContent(string content, Encoding? encoding, MediaTypeHeaderValue mediaType)
		: base(null)
	{
	}

	public StringContent(string content, Encoding? encoding, string mediaType)
		: base(null)
	{
	}

	protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context, CancellationToken cancellationToken)
	{
		throw null;
	}
}
