using System.Collections;
using System.Collections.Generic;

namespace System.Reflection.Metadata;

public readonly struct ManifestResourceHandleCollection : IEnumerable<ManifestResourceHandle>, IEnumerable, IReadOnlyCollection<ManifestResourceHandle>
{
	public struct Enumerator : IEnumerator<ManifestResourceHandle>, IEnumerator, IDisposable
	{
		private int _dummyPrimitive;

		public ManifestResourceHandle Current
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

	IEnumerator<ManifestResourceHandle> IEnumerable<ManifestResourceHandle>.GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
