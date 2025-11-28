using System.Collections.ObjectModel;

namespace System.Data.Common;

public interface IDbColumnSchemaGenerator
{
	ReadOnlyCollection<DbColumn> GetColumnSchema();
}
