using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows.Input;

[TypeConverter("System.Windows.Input.CommandConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
[ValueSerializer("System.Windows.Input.CommandValueSerializer, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
public interface ICommand
{
	event EventHandler? CanExecuteChanged;

	bool CanExecute(object? parameter);

	void Execute(object? parameter);
}
