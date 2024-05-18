namespace System.ComponentModel;

public interface IDataErrorInfo
{
	string Error { get; }

	string this[string columnName] { get; }
}
