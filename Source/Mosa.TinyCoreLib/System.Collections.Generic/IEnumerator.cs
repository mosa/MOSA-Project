namespace System.Collections.Generic;

public interface IEnumerator<out T> : IEnumerator, IDisposable
{
	new T Current { get; }
}
