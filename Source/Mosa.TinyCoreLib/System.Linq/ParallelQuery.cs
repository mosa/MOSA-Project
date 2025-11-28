using System.Collections;
using System.Collections.Generic;

namespace System.Linq;

public class ParallelQuery : IEnumerable
{
	internal ParallelQuery()
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
public class ParallelQuery<TSource> : ParallelQuery, IEnumerable<TSource>, IEnumerable
{
	internal ParallelQuery()
	{
	}

	public virtual IEnumerator<TSource> GetEnumerator()
	{
		throw null;
	}
}
