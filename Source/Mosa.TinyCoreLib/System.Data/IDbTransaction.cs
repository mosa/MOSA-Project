namespace System.Data;

public interface IDbTransaction : IDisposable
{
	IDbConnection? Connection { get; }

	IsolationLevel IsolationLevel { get; }

	void Commit();

	void Rollback();
}
