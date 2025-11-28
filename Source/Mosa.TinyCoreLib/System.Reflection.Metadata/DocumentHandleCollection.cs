using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct DocumentHandleCollection : IEnumerable<DocumentHandle>, IEnumerable, IReadOnlyCollection<DocumentHandle>
{
	public struct Enumerator : IEnumerator<DocumentHandle>, IEnumerator, IDisposable
	{
		private object _dummy;

		private int _dummyPrimitive;

		public DocumentHandle Current
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

		public bool MoveNext()
		{
			throw null;
		}

		void IEnumerator.Reset()
		{
		}

		void IDisposable.Dispose()
		{
		}
	}

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	IEnumerator<DocumentHandle> IEnumerable<DocumentHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
