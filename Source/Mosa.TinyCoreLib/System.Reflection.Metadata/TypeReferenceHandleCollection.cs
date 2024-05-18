using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct TypeReferenceHandleCollection : IEnumerable<TypeReferenceHandle>, IEnumerable, IReadOnlyCollection<TypeReferenceHandle>
{
	public struct Enumerator : IEnumerator<TypeReferenceHandle>, IEnumerator, IDisposable
	{
		private int _dummyPrimitive;

		public TypeReferenceHandle Current
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

	IEnumerator<TypeReferenceHandle> IEnumerable<TypeReferenceHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
