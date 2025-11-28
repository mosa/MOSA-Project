using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public abstract class HttpContent : IDisposable
{
	public HttpContentHeaders Headers
	{
		get
		{
			throw null;
		}
	}

	public void CopyTo(Stream stream, TransportContext? context, CancellationToken cancellationToken)
	{
	}

	public Task CopyToAsync(Stream stream)
	{
		throw null;
	}

	public Task CopyToAsync(Stream stream, TransportContext? context)
	{
		throw null;
	}

	public Task CopyToAsync(Stream stream, TransportContext? context, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task CopyToAsync(Stream stream, CancellationToken cancellationToken)
	{
		throw null;
	}

	protected virtual Stream CreateContentReadStream(CancellationToken cancellationToken)
	{
		throw null;
	}

	protected virtual Task<Stream> CreateContentReadStreamAsync()
	{
		throw null;
	}

	protected virtual Task<Stream> CreateContentReadStreamAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public Task LoadIntoBufferAsync()
	{
		throw null;
	}

	public Task LoadIntoBufferAsync(long maxBufferSize)
	{
		throw null;
	}

	public Task<byte[]> ReadAsByteArrayAsync()
	{
		throw null;
	}

	public Task<byte[]> ReadAsByteArrayAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public Stream ReadAsStream()
	{
		throw null;
	}

	public Stream ReadAsStream(CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<Stream> ReadAsStreamAsync()
	{
		throw null;
	}

	public Task<Stream> ReadAsStreamAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task<string> ReadAsStringAsync()
	{
		throw null;
	}

	public Task<string> ReadAsStringAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	protected virtual void SerializeToStream(Stream stream, TransportContext? context, CancellationToken cancellationToken)
	{
	}

	protected abstract Task SerializeToStreamAsync(Stream stream, TransportContext? context);

	protected virtual Task SerializeToStreamAsync(Stream stream, TransportContext? context, CancellationToken cancellationToken)
	{
		throw null;
	}

	protected internal abstract bool TryComputeLength(out long length);
}
