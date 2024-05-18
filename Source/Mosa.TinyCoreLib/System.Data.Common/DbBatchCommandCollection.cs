using System.Collections;
using System.Collections.Generic;

namespace System.Data.Common;

public abstract class DbBatchCommandCollection : IList<DbBatchCommand>, ICollection<DbBatchCommand>, IEnumerable<DbBatchCommand>, IEnumerable
{
	public abstract int Count { get; }

	public abstract bool IsReadOnly { get; }

	public DbBatchCommand this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public abstract IEnumerator<DbBatchCommand> GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public abstract void Add(DbBatchCommand item);

	public abstract void Clear();

	public abstract bool Contains(DbBatchCommand item);

	public abstract void CopyTo(DbBatchCommand[] array, int arrayIndex);

	public abstract bool Remove(DbBatchCommand item);

	public abstract int IndexOf(DbBatchCommand item);

	public abstract void Insert(int index, DbBatchCommand item);

	public abstract void RemoveAt(int index);

	protected abstract DbBatchCommand GetBatchCommand(int index);

	protected abstract void SetBatchCommand(int index, DbBatchCommand batchCommand);
}
