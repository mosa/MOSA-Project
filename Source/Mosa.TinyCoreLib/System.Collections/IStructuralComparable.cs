namespace System.Collections;

public interface IStructuralComparable
{
	int CompareTo(object? other, IComparer comparer);
}
