using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct MemberReferenceHandleCollection : IEnumerable<MemberReferenceHandle>, IEnumerable, IReadOnlyCollection<MemberReferenceHandle>
{
	public struct Enumerator : IEnumerator<MemberReferenceHandle>, IEnumerator, IDisposable
	{
		private int _dummyPrimitive;

		public MemberReferenceHandle Current
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

	IEnumerator<MemberReferenceHandle> IEnumerable<MemberReferenceHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
