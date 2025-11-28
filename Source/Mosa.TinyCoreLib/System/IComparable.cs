namespace System;

public interface IComparable
{
	int CompareTo(object? obj);
}
public interface IComparable<in T>
{
	int CompareTo(T? other);
}
