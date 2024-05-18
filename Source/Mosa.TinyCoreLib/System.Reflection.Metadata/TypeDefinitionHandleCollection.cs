using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct TypeDefinitionHandleCollection : IEnumerable<TypeDefinitionHandle>, IEnumerable, IReadOnlyCollection<TypeDefinitionHandle>
{
	public struct Enumerator : IEnumerator<TypeDefinitionHandle>, IEnumerator, IDisposable
	{
		private int _dummyPrimitive;

		public TypeDefinitionHandle Current
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

	IEnumerator<TypeDefinitionHandle> IEnumerable<TypeDefinitionHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
