using System.Collections;
using System.Collections.Generic;

namespace System;

public sealed class CharEnumerator : IEnumerator<char>, IEnumerator, IDisposable, ICloneable
{
	public char Current
	{
		get
		{
			throw null;
		}
	}

	object? IEnumerator.Current
	{
		get
		{
			throw null;
		}
	}

	internal CharEnumerator()
	{
	}

	public object Clone()
	{
		throw null;
	}

	public void Dispose()
	{
	}

	public bool MoveNext()
	{
		throw null;
	}

	public void Reset()
	{
	}
}
