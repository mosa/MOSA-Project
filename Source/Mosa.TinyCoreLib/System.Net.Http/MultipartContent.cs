using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public class MultipartContent : HttpContent, IEnumerable<HttpContent>, IEnumerable
{
	public HeaderEncodingSelector<HttpContent>? HeaderEncodingSelector
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MultipartContent()
	{
	}

	public MultipartContent(string subtype)
	{
	}

	public MultipartContent(string subtype, string boundary)
	{
	}

	public virtual void Add(HttpContent content)
	{
	}

	protected override Stream CreateContentReadStream(CancellationToken cancellationToken)
	{
		throw null;
	}

	protected override Task<Stream> CreateContentReadStreamAsync()
	{
		throw null;
	}

	protected override Task<Stream> CreateContentReadStreamAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public IEnumerator<HttpContent> GetEnumerator()
	{
		throw null;
	}

	protected override void SerializeToStream(Stream stream, TransportContext? context, CancellationToken cancellationToken)
	{
	}

	protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
	{
		throw null;
	}

	protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context, CancellationToken cancellationToken)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	protected internal override bool TryComputeLength(out long length)
	{
		throw null;
	}
}
