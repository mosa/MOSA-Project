namespace System.ComponentModel;

public interface INotifyPropertyChanged
{
	event PropertyChangedEventHandler? PropertyChanged;
}
