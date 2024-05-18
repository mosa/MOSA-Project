using System.Collections;
using System.Data.Common;

namespace System.Data.Odbc;

public sealed class OdbcDataReader : DbDataReader
{
	public override int Depth
	{
		get
		{
			throw null;
		}
	}

	public override int FieldCount
	{
		get
		{
			throw null;
		}
	}

	public override bool HasRows
	{
		get
		{
			throw null;
		}
	}

	public override bool IsClosed
	{
		get
		{
			throw null;
		}
	}

	public override object this[int i]
	{
		get
		{
			throw null;
		}
	}

	public override object this[string value]
	{
		get
		{
			throw null;
		}
	}

	public override int RecordsAffected
	{
		get
		{
			throw null;
		}
	}

	internal OdbcDataReader()
	{
	}

	public override void Close()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override bool GetBoolean(int i)
	{
		throw null;
	}

	public override byte GetByte(int i)
	{
		throw null;
	}

	public override long GetBytes(int i, long dataIndex, byte[]? buffer, int bufferIndex, int length)
	{
		throw null;
	}

	public override char GetChar(int i)
	{
		throw null;
	}

	public override long GetChars(int i, long dataIndex, char[]? buffer, int bufferIndex, int length)
	{
		throw null;
	}

	public override string GetDataTypeName(int i)
	{
		throw null;
	}

	public DateTime GetDate(int i)
	{
		throw null;
	}

	public override DateTime GetDateTime(int i)
	{
		throw null;
	}

	public override decimal GetDecimal(int i)
	{
		throw null;
	}

	public override double GetDouble(int i)
	{
		throw null;
	}

	public override IEnumerator GetEnumerator()
	{
		throw null;
	}

	public override Type GetFieldType(int i)
	{
		throw null;
	}

	public override float GetFloat(int i)
	{
		throw null;
	}

	public override Guid GetGuid(int i)
	{
		throw null;
	}

	public override short GetInt16(int i)
	{
		throw null;
	}

	public override int GetInt32(int i)
	{
		throw null;
	}

	public override long GetInt64(int i)
	{
		throw null;
	}

	public override string GetName(int i)
	{
		throw null;
	}

	public override int GetOrdinal(string value)
	{
		throw null;
	}

	public override DataTable? GetSchemaTable()
	{
		throw null;
	}

	public override string GetString(int i)
	{
		throw null;
	}

	public TimeSpan GetTime(int i)
	{
		throw null;
	}

	public override object GetValue(int i)
	{
		throw null;
	}

	public override int GetValues(object[] values)
	{
		throw null;
	}

	public override bool IsDBNull(int i)
	{
		throw null;
	}

	public override bool NextResult()
	{
		throw null;
	}

	public override bool Read()
	{
		throw null;
	}
}
