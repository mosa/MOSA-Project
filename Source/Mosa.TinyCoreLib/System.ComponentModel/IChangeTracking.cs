namespace System.ComponentModel;

public interface IChangeTracking
{
	bool IsChanged { get; }

	void AcceptChanges();
}
