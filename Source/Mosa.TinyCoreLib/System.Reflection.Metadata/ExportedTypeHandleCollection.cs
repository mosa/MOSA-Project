using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct ExportedTypeHandleCollection : IEnumerable<ExportedTypeHandle>, IEnumerable, IReadOnlyCollection<ExportedTypeHandle>
{
	public struct Enumerator : IEnumerator<ExportedTypeHandle>, IEnumerator, IDisposable
	{
		private int _dummyPrimitive;

		public ExportedTypeHandle Current
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

	IEnumerator<ExportedTypeHandle> IEnumerable<ExportedTypeHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
