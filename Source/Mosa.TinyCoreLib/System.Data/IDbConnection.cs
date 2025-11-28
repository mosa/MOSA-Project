using System.Diagnostics.CodeAnalysis;

namespace System.Data;

public interface IDbConnection : IDisposable
{
	string ConnectionString
	{
		get; [param: AllowNull]
		set;
	}

	int ConnectionTimeout { get; }

	string Database { get; }

	ConnectionState State { get; }

	IDbTransaction BeginTransaction();

	IDbTransaction BeginTransaction(IsolationLevel il);

	void ChangeDatabase(string databaseName);

	void Close();

	IDbCommand CreateCommand();

	void Open();
}
