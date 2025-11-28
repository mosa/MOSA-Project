using System.Collections;

namespace System.ComponentModel;

public interface INotifyDataErrorInfo
{
	bool HasErrors { get; }

	event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

	IEnumerable GetErrors(string? propertyName);
}
