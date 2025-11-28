using System.Collections;
using System.Collections.Generic;

namespace System.Data;

public abstract class EnumerableRowCollection : IEnumerable
{
	internal EnumerableRowCollection()
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
public class EnumerableRowCollection<TRow> : EnumerableRowCollection, IEnumerable<TRow>, IEnumerable
{
	internal EnumerableRowCollection()
	{
	}

	public IEnumerator<TRow> GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
