using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct ImportScopeCollection : IEnumerable<ImportScopeHandle>, IEnumerable, IReadOnlyCollection<ImportScopeHandle>
{
	public struct Enumerator : IEnumerator<ImportScopeHandle>, IEnumerator, IDisposable
	{
		private object _dummy;

		private int _dummyPrimitive;

		public ImportScopeHandle Current
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

	IEnumerator<ImportScopeHandle> IEnumerable<ImportScopeHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
