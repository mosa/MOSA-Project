namespace System.Collections;

public sealed class BitArray : ICollection, IEnumerable, ICloneable
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

	public bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	public bool this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Length
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public BitArray(bool[] values)
	{
	}

	public BitArray(byte[] bytes)
	{
	}

	public BitArray(BitArray bits)
	{
	}

	public BitArray(int length)
	{
	}

	public BitArray(int length, bool defaultValue)
	{
	}

	public BitArray(int[] values)
	{
	}

	public BitArray And(BitArray value)
	{
		throw null;
	}

	public object Clone()
	{
		throw null;
	}

	public void CopyTo(Array array, int index)
	{
	}

	public bool Get(int index)
	{
		throw null;
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public bool HasAllSet()
	{
		throw null;
	}

	public bool HasAnySet()
	{
		throw null;
	}

	public BitArray LeftShift(int count)
	{
		throw null;
	}

	public BitArray Not()
	{
		throw null;
	}

	public BitArray Or(BitArray value)
	{
		throw null;
	}

	public BitArray RightShift(int count)
	{
		throw null;
	}

	public void Set(int index, bool value)
	{
	}

	public void SetAll(bool value)
	{
	}

	public BitArray Xor(BitArray value)
	{
		throw null;
	}
}
