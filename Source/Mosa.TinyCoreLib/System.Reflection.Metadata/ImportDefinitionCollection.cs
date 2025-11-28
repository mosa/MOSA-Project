using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct ImportDefinitionCollection : IEnumerable<ImportDefinition>, IEnumerable
{
	public struct Enumerator : IEnumerator<ImportDefinition>, IEnumerator, IDisposable
	{
		private int _dummyPrimitive;

		public ImportDefinition Current
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

	IEnumerator<ImportDefinition> IEnumerable<ImportDefinition>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
