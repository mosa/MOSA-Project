using System.Collections;
using System.Collections.Generic;

namespace System.Net.Http.Headers;

public readonly struct HeaderStringValues : IEnumerable<string>, IEnumerable, IReadOnlyCollection<string>
{
	public struct Enumerator : IEnumerator<string>, IEnumerator, IDisposable
	{
		private object _dummy;

		private int _dummyPrimitive;

		public string Current
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

	IEnumerator<string> IEnumerable<string>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
