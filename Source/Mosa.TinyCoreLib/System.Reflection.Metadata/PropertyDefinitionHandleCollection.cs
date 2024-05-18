using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct PropertyDefinitionHandleCollection : IEnumerable<PropertyDefinitionHandle>, IEnumerable, IReadOnlyCollection<PropertyDefinitionHandle>
{
	public struct Enumerator : IEnumerator<PropertyDefinitionHandle>, IEnumerator, IDisposable
	{
		private object _dummy;

		private int _dummyPrimitive;

		public PropertyDefinitionHandle Current
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

	IEnumerator<PropertyDefinitionHandle> IEnumerable<PropertyDefinitionHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
