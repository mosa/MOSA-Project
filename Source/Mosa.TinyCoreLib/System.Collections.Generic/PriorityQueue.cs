using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Collections.Generic;

public class PriorityQueue<TElement, TPriority>
{
	public sealed class UnorderedItemsCollection : IEnumerable<(TElement Element, TPriority Priority)>, IEnumerable, IReadOnlyCollection<(TElement Element, TPriority Priority)>, ICollection
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Enumerator : IEnumerator<(TElement Element, TPriority Priority)>, IEnumerator, IDisposable
		{
			(TElement Element, TPriority Priority) IEnumerator<(TElement Element, TPriority Priority)>.Current
			{
				get
				{
					throw null;
				}
			}

			public (TElement Element, TPriority Priority) Current
			{
				get
				{
					throw null;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					throw null;
				}
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				throw null;
			}

			void IEnumerator.Reset()
			{
			}
		}

		public int Count
		{
			get
			{
				throw null;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				throw null;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				throw null;
			}
		}

		internal UnorderedItemsCollection(PriorityQueue<TElement, TPriority> queue)
		{
		}

		void ICollection.CopyTo(Array array, int index)
		{
		}

		public Enumerator GetEnumerator()
		{
			throw null;
		}

		IEnumerator<(TElement Element, TPriority Priority)> IEnumerable<(TElement Element, TPriority Priority)>.GetEnumerator()
		{
			throw null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}
	}

	public IComparer<TPriority> Comparer
	{
		get
		{
			throw null;
		}
	}

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public UnorderedItemsCollection UnorderedItems
	{
		get
		{
			throw null;
		}
	}

	public PriorityQueue()
	{
	}

	public PriorityQueue(IComparer<TPriority>? comparer)
	{
	}

	public PriorityQueue(IEnumerable<(TElement Element, TPriority Priority)> items)
	{
	}

	public PriorityQueue(IEnumerable<(TElement Element, TPriority Priority)> items, IComparer<TPriority>? comparer)
	{
	}

	public PriorityQueue(int initialCapacity)
	{
	}

	public PriorityQueue(int initialCapacity, IComparer<TPriority>? comparer)
	{
	}

	public void Clear()
	{
	}

	public TElement Dequeue()
	{
		throw null;
	}

	public TElement DequeueEnqueue(TElement element, TPriority priority)
	{
		throw null;
	}

	public void Enqueue(TElement element, TPriority priority)
	{
	}

	public TElement EnqueueDequeue(TElement element, TPriority priority)
	{
		throw null;
	}

	public void EnqueueRange(IEnumerable<(TElement Element, TPriority Priority)> items)
	{
	}

	public void EnqueueRange(IEnumerable<TElement> elements, TPriority priority)
	{
	}

	public int EnsureCapacity(int capacity)
	{
		throw null;
	}

	public TElement Peek()
	{
		throw null;
	}

	public void TrimExcess()
	{
	}

	public bool TryDequeue([MaybeNullWhen(false)] out TElement element, [MaybeNullWhen(false)] out TPriority priority)
	{
		throw null;
	}

	public bool TryPeek([MaybeNullWhen(false)] out TElement element, [MaybeNullWhen(false)] out TPriority priority)
	{
		throw null;
	}
}
