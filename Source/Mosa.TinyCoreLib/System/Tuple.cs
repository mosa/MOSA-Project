using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System;

public static class Tuple
{
	public static Tuple<T1> Create<T1>(T1 item1)
	{
		throw null;
	}

	public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
	{
		throw null;
	}

	public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
	{
		throw null;
	}

	public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
	{
		throw null;
	}

	public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
	{
		throw null;
	}

	public static Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
	{
		throw null;
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
	{
		throw null;
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
	{
		throw null;
	}
}
public class Tuple<T1> : IStructuralComparable, IStructuralEquatable, IComparable, ITuple
{
	public T1 Item1
	{
		get
		{
			throw null;
		}
	}

	object? ITuple.this[int index]
	{
		get
		{
			throw null;
		}
	}

	int ITuple.Length
	{
		get
		{
			throw null;
		}
	}

	public Tuple(T1 item1)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	int IStructuralComparable.CompareTo(object? other, IComparer comparer)
	{
		throw null;
	}

	bool IStructuralEquatable.Equals([NotNullWhen(true)] object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? obj)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public class Tuple<T1, T2> : IStructuralComparable, IStructuralEquatable, IComparable, ITuple
{
	public T1 Item1
	{
		get
		{
			throw null;
		}
	}

	public T2 Item2
	{
		get
		{
			throw null;
		}
	}

	object? ITuple.this[int index]
	{
		get
		{
			throw null;
		}
	}

	int ITuple.Length
	{
		get
		{
			throw null;
		}
	}

	public Tuple(T1 item1, T2 item2)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	int IStructuralComparable.CompareTo(object? other, IComparer comparer)
	{
		throw null;
	}

	bool IStructuralEquatable.Equals([NotNullWhen(true)] object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? obj)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public class Tuple<T1, T2, T3> : IStructuralComparable, IStructuralEquatable, IComparable, ITuple
{
	public T1 Item1
	{
		get
		{
			throw null;
		}
	}

	public T2 Item2
	{
		get
		{
			throw null;
		}
	}

	public T3 Item3
	{
		get
		{
			throw null;
		}
	}

	object? ITuple.this[int index]
	{
		get
		{
			throw null;
		}
	}

	int ITuple.Length
	{
		get
		{
			throw null;
		}
	}

	public Tuple(T1 item1, T2 item2, T3 item3)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	int IStructuralComparable.CompareTo(object? other, IComparer comparer)
	{
		throw null;
	}

	bool IStructuralEquatable.Equals([NotNullWhen(true)] object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? obj)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public class Tuple<T1, T2, T3, T4> : IStructuralComparable, IStructuralEquatable, IComparable, ITuple
{
	public T1 Item1
	{
		get
		{
			throw null;
		}
	}

	public T2 Item2
	{
		get
		{
			throw null;
		}
	}

	public T3 Item3
	{
		get
		{
			throw null;
		}
	}

	public T4 Item4
	{
		get
		{
			throw null;
		}
	}

	object? ITuple.this[int index]
	{
		get
		{
			throw null;
		}
	}

	int ITuple.Length
	{
		get
		{
			throw null;
		}
	}

	public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	int IStructuralComparable.CompareTo(object? other, IComparer comparer)
	{
		throw null;
	}

	bool IStructuralEquatable.Equals([NotNullWhen(true)] object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? obj)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public class Tuple<T1, T2, T3, T4, T5> : IStructuralComparable, IStructuralEquatable, IComparable, ITuple
{
	public T1 Item1
	{
		get
		{
			throw null;
		}
	}

	public T2 Item2
	{
		get
		{
			throw null;
		}
	}

	public T3 Item3
	{
		get
		{
			throw null;
		}
	}

	public T4 Item4
	{
		get
		{
			throw null;
		}
	}

	public T5 Item5
	{
		get
		{
			throw null;
		}
	}

	object? ITuple.this[int index]
	{
		get
		{
			throw null;
		}
	}

	int ITuple.Length
	{
		get
		{
			throw null;
		}
	}

	public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	int IStructuralComparable.CompareTo(object? other, IComparer comparer)
	{
		throw null;
	}

	bool IStructuralEquatable.Equals([NotNullWhen(true)] object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? obj)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public class Tuple<T1, T2, T3, T4, T5, T6> : IStructuralComparable, IStructuralEquatable, IComparable, ITuple
{
	public T1 Item1
	{
		get
		{
			throw null;
		}
	}

	public T2 Item2
	{
		get
		{
			throw null;
		}
	}

	public T3 Item3
	{
		get
		{
			throw null;
		}
	}

	public T4 Item4
	{
		get
		{
			throw null;
		}
	}

	public T5 Item5
	{
		get
		{
			throw null;
		}
	}

	public T6 Item6
	{
		get
		{
			throw null;
		}
	}

	object? ITuple.this[int index]
	{
		get
		{
			throw null;
		}
	}

	int ITuple.Length
	{
		get
		{
			throw null;
		}
	}

	public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	int IStructuralComparable.CompareTo(object? other, IComparer comparer)
	{
		throw null;
	}

	bool IStructuralEquatable.Equals([NotNullWhen(true)] object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? obj)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public class Tuple<T1, T2, T3, T4, T5, T6, T7> : IStructuralComparable, IStructuralEquatable, IComparable, ITuple
{
	public T1 Item1
	{
		get
		{
			throw null;
		}
	}

	public T2 Item2
	{
		get
		{
			throw null;
		}
	}

	public T3 Item3
	{
		get
		{
			throw null;
		}
	}

	public T4 Item4
	{
		get
		{
			throw null;
		}
	}

	public T5 Item5
	{
		get
		{
			throw null;
		}
	}

	public T6 Item6
	{
		get
		{
			throw null;
		}
	}

	public T7 Item7
	{
		get
		{
			throw null;
		}
	}

	object? ITuple.this[int index]
	{
		get
		{
			throw null;
		}
	}

	int ITuple.Length
	{
		get
		{
			throw null;
		}
	}

	public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	int IStructuralComparable.CompareTo(object? other, IComparer comparer)
	{
		throw null;
	}

	bool IStructuralEquatable.Equals([NotNullWhen(true)] object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? obj)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> : IStructuralComparable, IStructuralEquatable, IComparable, ITuple where TRest : notnull
{
	public T1 Item1
	{
		get
		{
			throw null;
		}
	}

	public T2 Item2
	{
		get
		{
			throw null;
		}
	}

	public T3 Item3
	{
		get
		{
			throw null;
		}
	}

	public T4 Item4
	{
		get
		{
			throw null;
		}
	}

	public T5 Item5
	{
		get
		{
			throw null;
		}
	}

	public T6 Item6
	{
		get
		{
			throw null;
		}
	}

	public T7 Item7
	{
		get
		{
			throw null;
		}
	}

	public TRest Rest
	{
		get
		{
			throw null;
		}
	}

	object? ITuple.this[int index]
	{
		get
		{
			throw null;
		}
	}

	int ITuple.Length
	{
		get
		{
			throw null;
		}
	}

	public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	int IStructuralComparable.CompareTo(object? other, IComparer comparer)
	{
		throw null;
	}

	bool IStructuralEquatable.Equals([NotNullWhen(true)] object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? obj)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
