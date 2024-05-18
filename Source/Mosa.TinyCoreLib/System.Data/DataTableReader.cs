using System.Collections;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace System.Data;

public sealed class DataTableReader : DbDataReader
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

	public override object this[int ordinal]
	{
		get
		{
			throw null;
		}
	}

	public override object this[string name]
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

	public DataTableReader(DataTable dataTable)
	{
	}

	public DataTableReader(DataTable[] dataTables)
	{
	}

	public override void Close()
	{
	}

	public override bool GetBoolean(int ordinal)
	{
		throw null;
	}

	public override byte GetByte(int ordinal)
	{
		throw null;
	}

	public override long GetBytes(int ordinal, long dataIndex, byte[]? buffer, int bufferIndex, int length)
	{
		throw null;
	}

	public override char GetChar(int ordinal)
	{
		throw null;
	}

	public override long GetChars(int ordinal, long dataIndex, char[]? buffer, int bufferIndex, int length)
	{
		throw null;
	}

	public override string GetDataTypeName(int ordinal)
	{
		throw null;
	}

	public override DateTime GetDateTime(int ordinal)
	{
		throw null;
	}

	public override decimal GetDecimal(int ordinal)
	{
		throw null;
	}

	public override double GetDouble(int ordinal)
	{
		throw null;
	}

	public override IEnumerator GetEnumerator()
	{
		throw null;
	}

	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)]
	public override Type GetFieldType(int ordinal)
	{
		throw null;
	}

	public override float GetFloat(int ordinal)
	{
		throw null;
	}

	public override Guid GetGuid(int ordinal)
	{
		throw null;
	}

	public override short GetInt16(int ordinal)
	{
		throw null;
	}

	public override int GetInt32(int ordinal)
	{
		throw null;
	}

	public override long GetInt64(int ordinal)
	{
		throw null;
	}

	public override string GetName(int ordinal)
	{
		throw null;
	}

	public override int GetOrdinal(string name)
	{
		throw null;
	}

	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)]
	public override Type GetProviderSpecificFieldType(int ordinal)
	{
		throw null;
	}

	public override object GetProviderSpecificValue(int ordinal)
	{
		throw null;
	}

	public override int GetProviderSpecificValues(object[] values)
	{
		throw null;
	}

	public override DataTable GetSchemaTable()
	{
		throw null;
	}

	public override string GetString(int ordinal)
	{
		throw null;
	}

	public override object GetValue(int ordinal)
	{
		throw null;
	}

	public override int GetValues(object[] values)
	{
		throw null;
	}

	public override bool IsDBNull(int ordinal)
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
