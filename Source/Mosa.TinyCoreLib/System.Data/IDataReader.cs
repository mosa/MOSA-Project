namespace System.Data;

public interface IDataReader : IDataRecord, IDisposable
{
	int Depth { get; }

	bool IsClosed { get; }

	int RecordsAffected { get; }

	void Close();

	DataTable? GetSchemaTable();

	bool NextResult();

	bool Read();
}
