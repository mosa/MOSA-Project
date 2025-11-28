using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct GenericParameterHandleCollection : IEnumerable<GenericParameterHandle>, IEnumerable, IReadOnlyCollection<GenericParameterHandle>, IReadOnlyList<GenericParameterHandle>
{
	public struct Enumerator : IEnumerator<GenericParameterHandle>, IEnumerator, IDisposable
	{
		private int _dummyPrimitive;

		public GenericParameterHandle Current
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

	public GenericParameterHandle this[int index]
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

	IEnumerator<GenericParameterHandle> IEnumerable<GenericParameterHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
