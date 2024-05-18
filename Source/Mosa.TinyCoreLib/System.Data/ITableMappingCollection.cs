using System.Collections;

namespace System.Data;

public interface ITableMappingCollection : ICollection, IEnumerable, IList
{
	object this[string index] { get; set; }

	ITableMapping Add(string sourceTableName, string dataSetTableName);

	bool Contains(string? sourceTableName);

	ITableMapping GetByDataSetTable(string dataSetTableName);

	int IndexOf(string? sourceTableName);

	void RemoveAt(string sourceTableName);
}
