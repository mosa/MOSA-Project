using System.Collections;
using System.Collections.Generic;

namespace System.Linq;

public interface IOrderedEnumerable<out TElement> : IEnumerable<TElement>, IEnumerable
{
	IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey>? comparer, bool descending);
}
