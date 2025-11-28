using System.Collections.Generic;

namespace System.Data;

public static class DataRowComparer
{
	public static DataRowComparer<DataRow> Default
	{
		get
		{
			throw null;
		}
	}
}
public sealed class DataRowComparer<TRow> : IEqualityComparer<TRow> where TRow : DataRow
{
	public static DataRowComparer<TRow> Default
	{
		get
		{
			throw null;
		}
	}

	internal DataRowComparer()
	{
	}

	public bool Equals(TRow? leftRow, TRow? rightRow)
	{
		throw null;
	}

	public int GetHashCode(TRow row)
	{
		throw null;
	}
}
