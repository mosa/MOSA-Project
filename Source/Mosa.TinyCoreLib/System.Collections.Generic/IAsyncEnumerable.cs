using System.Threading;

namespace System.Collections.Generic;

public interface IAsyncEnumerable<out T>
{
	IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken));
}
