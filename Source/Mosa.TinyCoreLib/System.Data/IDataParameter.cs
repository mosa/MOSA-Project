using System.Diagnostics.CodeAnalysis;

namespace System.Data;

public interface IDataParameter
{
	DbType DbType { get; set; }

	ParameterDirection Direction { get; set; }

	bool IsNullable { get; }

	string ParameterName
	{
		get; [param: AllowNull]
		set;
	}

	string SourceColumn
	{
		get; [param: AllowNull]
		set;
	}

	DataRowVersion SourceVersion { get; set; }

	object? Value { get; set; }
}
