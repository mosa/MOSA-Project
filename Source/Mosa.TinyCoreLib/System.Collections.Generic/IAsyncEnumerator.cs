using System.Threading.Tasks;

namespace System.Collections.Generic;

public interface IAsyncEnumerator<out T> : IAsyncDisposable
{
	T Current { get; }

	ValueTask<bool> MoveNextAsync();
}
