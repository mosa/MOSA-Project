using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Threading.Tasks.Dataflow;

public interface IReceivableSourceBlock<TOutput> : IDataflowBlock, ISourceBlock<TOutput>
{
	bool TryReceive(Predicate<TOutput>? filter, [MaybeNullWhen(false)] out TOutput item);

	bool TryReceiveAll([NotNullWhen(true)] out IList<TOutput>? items);
}
