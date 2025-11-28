using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct SequencePointCollection : IEnumerable<SequencePoint>, IEnumerable
{
	public struct Enumerator : IEnumerator<SequencePoint>, IEnumerator, IDisposable
	{
		private int _dummyPrimitive;

		public SequencePoint Current
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

		public void Reset()
		{
		}

		void IDisposable.Dispose()
		{
		}
	}

	private readonly int _dummyPrimitive;

	public Enumerator GetEnumerator()
	{
		throw null;
	}

	IEnumerator<SequencePoint> IEnumerable<SequencePoint>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
