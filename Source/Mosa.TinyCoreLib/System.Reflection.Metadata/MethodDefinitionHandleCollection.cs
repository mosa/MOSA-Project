using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct MethodDefinitionHandleCollection : IEnumerable<MethodDefinitionHandle>, IEnumerable, IReadOnlyCollection<MethodDefinitionHandle>
{
	public struct Enumerator : IEnumerator<MethodDefinitionHandle>, IEnumerator, IDisposable
	{
		private object _dummy;

		private int _dummyPrimitive;

		public MethodDefinitionHandle Current
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

	IEnumerator<MethodDefinitionHandle> IEnumerable<MethodDefinitionHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
