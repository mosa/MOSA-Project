using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct GenericParameterConstraintHandleCollection : IEnumerable<GenericParameterConstraintHandle>, IEnumerable, IReadOnlyCollection<GenericParameterConstraintHandle>, IReadOnlyList<GenericParameterConstraintHandle>
{
	public struct Enumerator : IEnumerator<GenericParameterConstraintHandle>, IEnumerator, IDisposable
	{
		private int _dummyPrimitive;

		public GenericParameterConstraintHandle Current
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

	public GenericParameterConstraintHandle this[int index]
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

	IEnumerator<GenericParameterConstraintHandle> IEnumerable<GenericParameterConstraintHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
