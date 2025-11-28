using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct AssemblyFileHandleCollection : IEnumerable<AssemblyFileHandle>, IEnumerable, IReadOnlyCollection<AssemblyFileHandle>
{
	public struct Enumerator : IEnumerator<AssemblyFileHandle>, IEnumerator, IDisposable
	{
		private int _dummyPrimitive;

		public AssemblyFileHandle Current
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

	IEnumerator<AssemblyFileHandle> IEnumerable<AssemblyFileHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
