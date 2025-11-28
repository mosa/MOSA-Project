using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public sealed class ReadOnlyMemoryContent : HttpContent
{
	public ReadOnlyMemoryContent(ReadOnlyMemory<byte> content)
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

	protected internal override bool TryComputeLength(out long length)
	{
		throw null;
	}
}
