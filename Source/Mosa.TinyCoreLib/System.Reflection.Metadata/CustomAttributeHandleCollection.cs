using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct CustomAttributeHandleCollection : IEnumerable<CustomAttributeHandle>, IEnumerable, IReadOnlyCollection<CustomAttributeHandle>
{
	public struct Enumerator : IEnumerator<CustomAttributeHandle>, IEnumerator, IDisposable
	{
		private object _dummy;

		private int _dummyPrimitive;

		public CustomAttributeHandle Current
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

	IEnumerator<CustomAttributeHandle> IEnumerable<CustomAttributeHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
