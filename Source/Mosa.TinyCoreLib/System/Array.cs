using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace System;

public abstract class Array : ICollection, IEnumerable, IList, IStructuralComparable, IStructuralEquatable, ICloneable
{
	public bool IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	public int Length
	{
		get
		{
			throw null;
		}
	}

	public long LongLength
	{
		get
		{
			throw null;
		}
	}

	public static int MaxLength
	{
		get
		{
			throw null;
		}
	}

	public int Rank
	{
		get
		{
			throw null;
		}
	}

	public object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	int ICollection.Count
	{
		get
		{
			throw null;
		}
	}

	object? IList.this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal Array()
	{
	}

	public static ReadOnlyCollection<T> AsReadOnly<T>(T[] array)
	{
		throw null;
	}

	public static int BinarySearch(Array array, int index, int length, object? value)
	{
		throw null;
	}

	public static int BinarySearch(Array array, int index, int length, object? value, IComparer? comparer)
	{
		throw null;
	}

	public static int BinarySearch(Array array, object? value)
	{
		throw null;
	}

	public static int BinarySearch(Array array, object? value, IComparer? comparer)
	{
		throw null;
	}

	public static int BinarySearch<T>(T[] array, int index, int length, T value)
	{
		throw null;
	}

	public static int BinarySearch<T>(T[] array, int index, int length, T value, IComparer<T>? comparer)
	{
		throw null;
	}

	public static int BinarySearch<T>(T[] array, T value)
	{
		throw null;
	}

	public static int BinarySearch<T>(T[] array, T value, IComparer<T>? comparer)
	{
		throw null;
	}

	public static void Clear(Array array)
	{
	}

	public static void Clear(Array array, int index, int length)
	{
	}

	public object Clone()
	{
		throw null;
	}

	public static void ConstrainedCopy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
	{
	}

	public static TOutput[] ConvertAll<TInput, TOutput>(TInput[] array, Converter<TInput, TOutput> converter)
	{
		throw null;
	}

	public static void Copy(Array sourceArray, Array destinationArray, int length)
	{
	}

	public static void Copy(Array sourceArray, Array destinationArray, long length)
	{
	}

	public static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
	{
	}

	public static void Copy(Array sourceArray, long sourceIndex, Array destinationArray, long destinationIndex, long length)
	{
	}

	public void CopyTo(Array array, int index)
	{
	}

	public void CopyTo(Array array, long index)
	{
	}

	[RequiresDynamicCode("The code for an array of the specified type might not be available.")]
	public static Array CreateInstance(Type elementType, int length)
	{
		throw null;
	}

	public static Array CreateInstance(Type elementType, int length1, int length2)
	{
		throw null;
	}

	public static Array CreateInstance(Type elementType, int length1, int length2, int length3)
	{
		throw null;
	}

	[RequiresDynamicCode("The code for an array of the specified type might not be available.")]
	public static Array CreateInstance(Type elementType, params int[] lengths)
	{
		throw null;
	}

	[RequiresDynamicCode("The code for an array of the specified type might not be available.")]
	public static Array CreateInstance(Type elementType, int[] lengths, int[] lowerBounds)
	{
		throw null;
	}

	[RequiresDynamicCode("The code for an array of the specified type might not be available.")]
	public static Array CreateInstance(Type elementType, params long[] lengths)
	{
		throw null;
	}

	public static T[] Empty<T>()
	{
		throw null;
	}

	public static bool Exists<T>(T[] array, Predicate<T> match)
	{
		throw null;
	}

	public static void Fill<T>(T[] array, T value)
	{
	}

	public static void Fill<T>(T[] array, T value, int startIndex, int count)
	{
	}

	public static T[] FindAll<T>(T[] array, Predicate<T> match)
	{
		throw null;
	}

	public static int FindIndex<T>(T[] array, int startIndex, int count, Predicate<T> match)
	{
		throw null;
	}

	public static int FindIndex<T>(T[] array, int startIndex, Predicate<T> match)
	{
		throw null;
	}

	public static int FindIndex<T>(T[] array, Predicate<T> match)
	{
		throw null;
	}

	public static int FindLastIndex<T>(T[] array, int startIndex, int count, Predicate<T> match)
	{
		throw null;
	}

	public static int FindLastIndex<T>(T[] array, int startIndex, Predicate<T> match)
	{
		throw null;
	}

	public static int FindLastIndex<T>(T[] array, Predicate<T> match)
	{
		throw null;
	}

	public static T? FindLast<T>(T[] array, Predicate<T> match)
	{
		throw null;
	}

	public static T? Find<T>(T[] array, Predicate<T> match)
	{
		throw null;
	}

	public static void ForEach<T>(T[] array, Action<T> action)
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public int GetLength(int dimension)
	{
		throw null;
	}

	public long GetLongLength(int dimension)
	{
		throw null;
	}

	public int GetLowerBound(int dimension)
	{
		throw null;
	}

	public int GetUpperBound(int dimension)
	{
		throw null;
	}

	public object? GetValue(int index)
	{
		throw null;
	}

	public object? GetValue(int index1, int index2)
	{
		throw null;
	}

	public object? GetValue(int index1, int index2, int index3)
	{
		throw null;
	}

	public object? GetValue(params int[] indices)
	{
		throw null;
	}

	public object? GetValue(long index)
	{
		throw null;
	}

	public object? GetValue(long index1, long index2)
	{
		throw null;
	}

	public object? GetValue(long index1, long index2, long index3)
	{
		throw null;
	}

	public object? GetValue(params long[] indices)
	{
		throw null;
	}

	public static int IndexOf(Array array, object? value)
	{
		throw null;
	}

	public static int IndexOf(Array array, object? value, int startIndex)
	{
		throw null;
	}

	public static int IndexOf(Array array, object? value, int startIndex, int count)
	{
		throw null;
	}

	public static int IndexOf<T>(T[] array, T value)
	{
		throw null;
	}

	public static int IndexOf<T>(T[] array, T value, int startIndex)
	{
		throw null;
	}

	public static int IndexOf<T>(T[] array, T value, int startIndex, int count)
	{
		throw null;
	}

	public void Initialize()
	{
	}

	public static int LastIndexOf(Array array, object? value)
	{
		throw null;
	}

	public static int LastIndexOf(Array array, object? value, int startIndex)
	{
		throw null;
	}

	public static int LastIndexOf(Array array, object? value, int startIndex, int count)
	{
		throw null;
	}

	public static int LastIndexOf<T>(T[] array, T value)
	{
		throw null;
	}

	public static int LastIndexOf<T>(T[] array, T value, int startIndex)
	{
		throw null;
	}

	public static int LastIndexOf<T>(T[] array, T value, int startIndex, int count)
	{
		throw null;
	}

	public static void Resize<T>([NotNull] ref T[]? array, int newSize)
	{
		throw null;
	}

	public static void Reverse(Array array)
	{
	}

	public static void Reverse(Array array, int index, int length)
	{
	}

	public static void Reverse<T>(T[] array)
	{
	}

	public static void Reverse<T>(T[] array, int index, int length)
	{
	}

	public void SetValue(object? value, int index)
	{
	}

	public void SetValue(object? value, int index1, int index2)
	{
	}

	public void SetValue(object? value, int index1, int index2, int index3)
	{
	}

	public void SetValue(object? value, params int[] indices)
	{
	}

	public void SetValue(object? value, long index)
	{
	}

	public void SetValue(object? value, long index1, long index2)
	{
	}

	public void SetValue(object? value, long index1, long index2, long index3)
	{
	}

	public void SetValue(object? value, params long[] indices)
	{
	}

	public static void Sort(Array array)
	{
	}

	public static void Sort(Array keys, Array? items)
	{
	}

	public static void Sort(Array keys, Array? items, IComparer? comparer)
	{
	}

	public static void Sort(Array keys, Array? items, int index, int length)
	{
	}

	public static void Sort(Array keys, Array? items, int index, int length, IComparer? comparer)
	{
	}

	public static void Sort(Array array, IComparer? comparer)
	{
	}

	public static void Sort(Array array, int index, int length)
	{
	}

	public static void Sort(Array array, int index, int length, IComparer? comparer)
	{
	}

	public static void Sort<T>(T[] array)
	{
	}

	public static void Sort<T>(T[] array, IComparer<T>? comparer)
	{
	}

	public static void Sort<T>(T[] array, Comparison<T> comparison)
	{
	}

	public static void Sort<T>(T[] array, int index, int length)
	{
	}

	public static void Sort<T>(T[] array, int index, int length, IComparer<T>? comparer)
	{
	}

	public static void Sort<TKey, TValue>(TKey[] keys, TValue[]? items)
	{
	}

	public static void Sort<TKey, TValue>(TKey[] keys, TValue[]? items, IComparer<TKey>? comparer)
	{
	}

	public static void Sort<TKey, TValue>(TKey[] keys, TValue[]? items, int index, int length)
	{
	}

	public static void Sort<TKey, TValue>(TKey[] keys, TValue[]? items, int index, int length, IComparer<TKey>? comparer)
	{
	}

	int IList.Add(object? value)
	{
		throw null;
	}

	void IList.Clear()
	{
	}

	bool IList.Contains(object? value)
	{
		throw null;
	}

	int IList.IndexOf(object? value)
	{
		throw null;
	}

	void IList.Insert(int index, object? value)
	{
	}

	void IList.Remove(object? value)
	{
	}

	void IList.RemoveAt(int index)
	{
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

	public static bool TrueForAll<T>(T[] array, Predicate<T> match)
	{
		throw null;
	}
}
