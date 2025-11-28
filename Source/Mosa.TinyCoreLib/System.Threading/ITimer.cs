namespace System.Threading;

public interface ITimer : IDisposable, IAsyncDisposable
{
	bool Change(TimeSpan dueTime, TimeSpan period);
}
