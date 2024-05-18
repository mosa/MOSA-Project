using System.Collections.Generic;

namespace System.Linq;

public class OrderedParallelQuery<TSource> : ParallelQuery<TSource>
{
	internal OrderedParallelQuery()
	{
	}

	public override IEnumerator<TSource> GetEnumerator()
	{
		throw null;
	}
}
