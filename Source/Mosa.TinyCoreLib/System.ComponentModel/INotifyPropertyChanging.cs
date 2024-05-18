namespace System.ComponentModel;

public interface INotifyPropertyChanging
{
	event PropertyChangingEventHandler? PropertyChanging;
}
