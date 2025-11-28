using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System.Threading.Tasks;

public static class TaskAsyncEnumerableExtensions
{
	public static ConfiguredAsyncDisposable ConfigureAwait(this IAsyncDisposable source, bool continueOnCapturedContext)
	{
		throw null;
	}

	public static ConfiguredCancelableAsyncEnumerable<T> ConfigureAwait<T>(this IAsyncEnumerable<T> source, bool continueOnCapturedContext)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static IEnumerable<T> ToBlockingEnumerable<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static ConfiguredCancelableAsyncEnumerable<T> WithCancellation<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken)
	{
		throw null;
	}
}
