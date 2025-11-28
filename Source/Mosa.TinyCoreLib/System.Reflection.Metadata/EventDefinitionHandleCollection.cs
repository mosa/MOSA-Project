using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct EventDefinitionHandleCollection : IEnumerable<EventDefinitionHandle>, IEnumerable, IReadOnlyCollection<EventDefinitionHandle>
{
	public struct Enumerator : IEnumerator<EventDefinitionHandle>, IEnumerator, IDisposable
	{
		private object _dummy;

		private int _dummyPrimitive;

		public EventDefinitionHandle Current
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

	IEnumerator<EventDefinitionHandle> IEnumerable<EventDefinitionHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
