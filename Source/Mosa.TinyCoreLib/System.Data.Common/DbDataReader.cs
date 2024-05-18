using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Common;

public abstract class DbDataReader : MarshalByRefObject, IEnumerable, IDataReader, IDataRecord, IDisposable, IAsyncDisposable
{
	public abstract int Depth { get; }

	public abstract int FieldCount { get; }

	public abstract bool HasRows { get; }

	public abstract bool IsClosed { get; }

	public abstract object this[int ordinal] { get; }

	public abstract object this[string name] { get; }

	public abstract int RecordsAffected { get; }

	public virtual int VisibleFieldCount
	{
		get
		{
			throw null;
		}
	}

	public virtual void Close()
	{
	}

	public virtual Task CloseAsync()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual ValueTask DisposeAsync()
	{
		throw null;
	}

	public abstract bool GetBoolean(int ordinal);

	public abstract byte GetByte(int ordinal);

	public abstract long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length);

	public abstract char GetChar(int ordinal);

	public abstract long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public DbDataReader GetData(int ordinal)
	{
		throw null;
	}

	public abstract string GetDataTypeName(int ordinal);

	public abstract DateTime GetDateTime(int ordinal);

	protected virtual DbDataReader GetDbDataReader(int ordinal)
	{
		throw null;
	}

	public abstract decimal GetDecimal(int ordinal);

	public abstract double GetDouble(int ordinal);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract IEnumerator GetEnumerator();

	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)]
	public abstract Type GetFieldType(int ordinal);

	public Task<T> GetFieldValueAsync<T>(int ordinal)
	{
		throw null;
	}

	public virtual Task<T> GetFieldValueAsync<T>(int ordinal, CancellationToken cancellationToken)
	{
		throw null;
	}

	public virtual T GetFieldValue<T>(int ordinal)
	{
		throw null;
	}

	public abstract float GetFloat(int ordinal);

	public abstract Guid GetGuid(int ordinal);

	public abstract short GetInt16(int ordinal);

	public abstract int GetInt32(int ordinal);

	public abstract long GetInt64(int ordinal);

	public abstract string GetName(int ordinal);

	public abstract int GetOrdinal(string name);

	[EditorBrowsable(EditorBrowsableState.Never)]
	[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)]
	public virtual Type GetProviderSpecificFieldType(int ordinal)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual object GetProviderSpecificValue(int ordinal)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual int GetProviderSpecificValues(object[] values)
	{
		throw null;
	}

	public virtual DataTable? GetSchemaTable()
	{
		throw null;
	}

	public virtual Task<DataTable?> GetSchemaTableAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual Task<ReadOnlyCollection<DbColumn>> GetColumnSchemaAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual Stream GetStream(int ordinal)
	{
		throw null;
	}

	public abstract string GetString(int ordinal);

	public virtual TextReader GetTextReader(int ordinal)
	{
		throw null;
	}

	public abstract object GetValue(int ordinal);

	public abstract int GetValues(object[] values);

	public abstract bool IsDBNull(int ordinal);

	public Task<bool> IsDBNullAsync(int ordinal)
	{
		throw null;
	}

	public virtual Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken)
	{
		throw null;
	}

	public abstract bool NextResult();

	public Task<bool> NextResultAsync()
	{
		throw null;
	}

	public virtual Task<bool> NextResultAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public abstract bool Read();

	public Task<bool> ReadAsync()
	{
		throw null;
	}

	public virtual Task<bool> ReadAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	IDataReader IDataRecord.GetData(int ordinal)
	{
		throw null;
	}
}
