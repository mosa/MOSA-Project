namespace System.Data;

public interface IColumnMapping
{
	string DataSetColumn { get; set; }

	string SourceColumn { get; set; }
}
