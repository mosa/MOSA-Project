using System.Diagnostics.CodeAnalysis;

namespace System.Data;

public interface IDbCommand : IDisposable
{
	string CommandText
	{
		get; [param: AllowNull]
		set;
	}

	int CommandTimeout { get; set; }

	CommandType CommandType { get; set; }

	IDbConnection? Connection { get; set; }

	IDataParameterCollection Parameters { get; }

	IDbTransaction? Transaction { get; set; }

	UpdateRowSource UpdatedRowSource { get; set; }

	void Cancel();

	IDbDataParameter CreateParameter();

	int ExecuteNonQuery();

	IDataReader ExecuteReader();

	IDataReader ExecuteReader(CommandBehavior behavior);

	object? ExecuteScalar();

	void Prepare();
}
