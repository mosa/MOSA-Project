using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines;

public static class StreamPipeExtensions
{
	public static Task CopyToAsync(this Stream source, PipeWriter destination, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
