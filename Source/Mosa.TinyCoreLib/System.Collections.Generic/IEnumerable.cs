namespace System.Collections.Generic;

public interface IEnumerable<out T> : IEnumerable
{
	new IEnumerator<T> GetEnumerator();
}
