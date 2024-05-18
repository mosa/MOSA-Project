using System.Collections;
using System.Collections.Generic;

namespace System.Linq;

public class Lookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>, IEnumerable, ILookup<TKey, TElement>
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<TElement> this[TKey key]
	{
		get
		{
			throw null;
		}
	}

	internal Lookup()
	{
	}

	public IEnumerable<TResult> ApplyResultSelector<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
	{
		throw null;
	}

	public bool Contains(TKey key)
	{
		throw null;
	}

	public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
