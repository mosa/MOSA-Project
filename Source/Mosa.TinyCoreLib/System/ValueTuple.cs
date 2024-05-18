using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ValueTuple : IStructuralComparable, IStructuralEquatable, IComparable, IComparable<ValueTuple>, IEquatable<ValueTuple>, ITuple
{
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

	public int CompareTo(ValueTuple other)
	{
		throw null;
	}

	public static ValueTuple Create()
	{
		throw null;
	}

	public static ValueTuple<T1> Create<T1>(T1 item1)
	{
		throw null;
	}

	public static (T1, T2) Create<T1, T2>(T1 item1, T2 item2)
	{
		throw null;
	}

	public static (T1, T2, T3) Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
	{
		throw null;
	}

	public static (T1, T2, T3, T4) Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
	{
		throw null;
	}

	public static (T1, T2, T3, T4, T5) Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
	{
		throw null;
	}

	public static (T1, T2, T3, T4, T5, T6) Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
	{
		throw null;
	}

	public static (T1, T2, T3, T4, T5, T6, T7) Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
	{
		throw null;
	}

	public static (T1, T2, T3, T4, T5, T6, T7, T8) Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(ValueTuple other)
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

	bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? other)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public struct ValueTuple<T1> : IStructuralComparable, IStructuralEquatable, IComparable, IComparable<ValueTuple<T1>>, IEquatable<ValueTuple<T1>>, ITuple
{
	public T1 Item1;

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

	public ValueTuple(T1 item1)
	{
		throw null;
	}

	public int CompareTo(ValueTuple<T1> other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(ValueTuple<T1> other)
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

	bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? other)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public struct ValueTuple<T1, T2> : IStructuralComparable, IStructuralEquatable, IComparable, IComparable<(T1, T2)>, IEquatable<(T1, T2)>, ITuple
{
	public T1 Item1;

	public T2 Item2;

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

	public ValueTuple(T1 item1, T2 item2)
	{
		throw null;
	}

	public int CompareTo((T1, T2) other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals((T1, T2) other)
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

	bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? other)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public struct ValueTuple<T1, T2, T3> : IStructuralComparable, IStructuralEquatable, IComparable, IComparable<(T1, T2, T3)>, IEquatable<(T1, T2, T3)>, ITuple
{
	public T1 Item1;

	public T2 Item2;

	public T3 Item3;

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

	public ValueTuple(T1 item1, T2 item2, T3 item3)
	{
		throw null;
	}

	public int CompareTo((T1, T2, T3) other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals((T1, T2, T3) other)
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

	bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? other)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public struct ValueTuple<T1, T2, T3, T4> : IStructuralComparable, IStructuralEquatable, IComparable, IComparable<(T1, T2, T3, T4)>, IEquatable<(T1, T2, T3, T4)>, ITuple
{
	public T1 Item1;

	public T2 Item2;

	public T3 Item3;

	public T4 Item4;

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

	public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4)
	{
		throw null;
	}

	public int CompareTo((T1, T2, T3, T4) other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals((T1, T2, T3, T4) other)
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

	bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? other)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public struct ValueTuple<T1, T2, T3, T4, T5> : IStructuralComparable, IStructuralEquatable, IComparable, IComparable<(T1, T2, T3, T4, T5)>, IEquatable<(T1, T2, T3, T4, T5)>, ITuple
{
	public T1 Item1;

	public T2 Item2;

	public T3 Item3;

	public T4 Item4;

	public T5 Item5;

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

	public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
	{
		throw null;
	}

	public int CompareTo((T1, T2, T3, T4, T5) other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals((T1, T2, T3, T4, T5) other)
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

	bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? other)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public struct ValueTuple<T1, T2, T3, T4, T5, T6> : IStructuralComparable, IStructuralEquatable, IComparable, IComparable<(T1, T2, T3, T4, T5, T6)>, IEquatable<(T1, T2, T3, T4, T5, T6)>, ITuple
{
	public T1 Item1;

	public T2 Item2;

	public T3 Item3;

	public T4 Item4;

	public T5 Item5;

	public T6 Item6;

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

	public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
	{
		throw null;
	}

	public int CompareTo((T1, T2, T3, T4, T5, T6) other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals((T1, T2, T3, T4, T5, T6) other)
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

	bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? other)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7> : IStructuralComparable, IStructuralEquatable, IComparable, IComparable<(T1, T2, T3, T4, T5, T6, T7)>, IEquatable<(T1, T2, T3, T4, T5, T6, T7)>, ITuple
{
	public T1 Item1;

	public T2 Item2;

	public T3 Item3;

	public T4 Item4;

	public T5 Item5;

	public T6 Item6;

	public T7 Item7;

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

	public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
	{
		throw null;
	}

	public int CompareTo((T1, T2, T3, T4, T5, T6, T7) other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals((T1, T2, T3, T4, T5, T6, T7) other)
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

	bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? other)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> : IStructuralComparable, IStructuralEquatable, IComparable, IComparable<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>>, IEquatable<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>>, ITuple where TRest : struct
{
	public T1 Item1;

	public T2 Item2;

	public T3 Item3;

	public T4 Item4;

	public T5 Item5;

	public T6 Item6;

	public T7 Item7;

	public TRest Rest;

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

	public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
	{
		throw null;
	}

	public int CompareTo(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> other)
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

	bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
	{
		throw null;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		throw null;
	}

	int IComparable.CompareTo(object? other)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
