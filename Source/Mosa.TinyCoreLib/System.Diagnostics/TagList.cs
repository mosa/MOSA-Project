using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Diagnostics;

public struct TagList : IList<KeyValuePair<string, object?>>, ICollection<KeyValuePair<string, object?>>, IEnumerable<KeyValuePair<string, object?>>, IEnumerable, IReadOnlyList<KeyValuePair<string, object?>>, IReadOnlyCollection<KeyValuePair<string, object?>>
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct Enumerator : IEnumerator<KeyValuePair<string, object?>>, IEnumerator, IDisposable
	{
		public KeyValuePair<string, object?> Current
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
			throw null;
		}

		public bool MoveNext()
		{
			throw null;
		}

		public void Reset()
		{
			throw null;
		}
	}

	public readonly int Count
	{
		get
		{
			throw null;
		}
	}

	public readonly bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public KeyValuePair<string, object?> this[int index]
	{
		readonly get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public TagList(ReadOnlySpan<KeyValuePair<string, object?>> tagList)
	{
		this = default(TagList);
		throw null;
	}

	public void Add(string key, object? value)
	{
		throw null;
	}

	public void Add(KeyValuePair<string, object?> tag)
	{
		throw null;
	}

	public readonly void CopyTo(Span<KeyValuePair<string, object?>> tags)
	{
		throw null;
	}

	public void Insert(int index, KeyValuePair<string, object?> item)
	{
		throw null;
	}

	public void RemoveAt(int index)
	{
		throw null;
	}

	public void Clear()
	{
		throw null;
	}

	public readonly bool Contains(KeyValuePair<string, object?> item)
	{
		throw null;
	}

	public readonly void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
	{
		throw null;
	}

	public bool Remove(KeyValuePair<string, object?> item)
	{
		throw null;
	}

	public readonly IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
	{
		throw null;
	}

	readonly IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public readonly int IndexOf(KeyValuePair<string, object?> item)
	{
		throw null;
	}
}
