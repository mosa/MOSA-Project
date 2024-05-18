using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct MethodImplementationHandleCollection : IEnumerable<MethodImplementationHandle>, IEnumerable, IReadOnlyCollection<MethodImplementationHandle>
{
	public struct Enumerator : IEnumerator<MethodImplementationHandle>, IEnumerator, IDisposable
	{
		private int _dummyPrimitive;

		public MethodImplementationHandle Current
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

	IEnumerator<MethodImplementationHandle> IEnumerable<MethodImplementationHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
