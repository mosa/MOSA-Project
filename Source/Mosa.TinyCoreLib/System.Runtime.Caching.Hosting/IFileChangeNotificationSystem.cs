namespace System.Runtime.Caching.Hosting;

public interface IFileChangeNotificationSystem
{
	void StartMonitoring(string filePath, OnChangedCallback onChangedCallback, out object state, out DateTimeOffset lastWriteTime, out long fileSize);

	void StopMonitoring(string filePath, object state);
}
