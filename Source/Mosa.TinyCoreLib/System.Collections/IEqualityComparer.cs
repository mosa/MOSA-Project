namespace System.Collections;

public interface IEqualityComparer
{
	new bool Equals(object? x, object? y);

	int GetHashCode(object obj);
}
