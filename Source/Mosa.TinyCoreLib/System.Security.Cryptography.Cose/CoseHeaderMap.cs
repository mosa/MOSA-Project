using System.Collections;
using System.Collections.Generic;

namespace System.Security.Cryptography.Cose;

public sealed class CoseHeaderMap : ICollection<KeyValuePair<CoseHeaderLabel, CoseHeaderValue>>, IEnumerable<KeyValuePair<CoseHeaderLabel, CoseHeaderValue>>, IEnumerable, IDictionary<CoseHeaderLabel, CoseHeaderValue>, IReadOnlyCollection<KeyValuePair<CoseHeaderLabel, CoseHeaderValue>>, IReadOnlyDictionary<CoseHeaderLabel, CoseHeaderValue>
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public CoseHeaderValue this[CoseHeaderLabel key]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ICollection<CoseHeaderLabel> Keys
	{
		get
		{
			throw null;
		}
	}

	IEnumerable<CoseHeaderLabel> IReadOnlyDictionary<CoseHeaderLabel, CoseHeaderValue>.Keys
	{
		get
		{
			throw null;
		}
	}

	IEnumerable<CoseHeaderValue> IReadOnlyDictionary<CoseHeaderLabel, CoseHeaderValue>.Values
	{
		get
		{
			throw null;
		}
	}

	public ICollection<CoseHeaderValue> Values
	{
		get
		{
			throw null;
		}
	}

	public void Add(KeyValuePair<CoseHeaderLabel, CoseHeaderValue> item)
	{
	}

	public void Add(CoseHeaderLabel label, byte[] value)
	{
	}

	public void Add(CoseHeaderLabel label, int value)
	{
	}

	public void Add(CoseHeaderLabel label, ReadOnlySpan<byte> value)
	{
	}

	public void Add(CoseHeaderLabel key, CoseHeaderValue value)
	{
	}

	public void Add(CoseHeaderLabel label, string value)
	{
	}

	public void Clear()
	{
	}

	public bool Contains(KeyValuePair<CoseHeaderLabel, CoseHeaderValue> item)
	{
		throw null;
	}

	public bool ContainsKey(CoseHeaderLabel key)
	{
		throw null;
	}

	public void CopyTo(KeyValuePair<CoseHeaderLabel, CoseHeaderValue>[] array, int arrayIndex)
	{
	}

	public IEnumerator<KeyValuePair<CoseHeaderLabel, CoseHeaderValue>> GetEnumerator()
	{
		throw null;
	}

	public byte[] GetValueAsBytes(CoseHeaderLabel label)
	{
		throw null;
	}

	public int GetValueAsBytes(CoseHeaderLabel label, Span<byte> destination)
	{
		throw null;
	}

	public int GetValueAsInt32(CoseHeaderLabel label)
	{
		throw null;
	}

	public string GetValueAsString(CoseHeaderLabel label)
	{
		throw null;
	}

	public bool Remove(KeyValuePair<CoseHeaderLabel, CoseHeaderValue> item)
	{
		throw null;
	}

	public bool Remove(CoseHeaderLabel label)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public bool TryGetValue(CoseHeaderLabel key, out CoseHeaderValue value)
	{
		throw null;
	}
}
