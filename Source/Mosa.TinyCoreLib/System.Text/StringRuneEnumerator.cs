using System.Collections;
using System.Collections.Generic;

namespace System.Text;

public struct StringRuneEnumerator : IEnumerable<Rune>, IEnumerable, IEnumerator<Rune>, IEnumerator, IDisposable
{
	private object _dummy;

	private int _dummyPrimitive;

	public Rune Current
	{
		get
		{
			throw null;
		}
	}

	object? IEnumerator.Current
	{
		get
		{
			throw null;
		}
	}

	public StringRuneEnumerator GetEnumerator()
	{
		throw null;
	}

	public bool MoveNext()
	{
		throw null;
	}

	IEnumerator<Rune> IEnumerable<Rune>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
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
