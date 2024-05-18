namespace System.ComponentModel;

public interface ISupportInitializeNotification : ISupportInitialize
{
	bool IsInitialized { get; }

	event EventHandler Initialized;
}
