using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace System;

[NativeMarshalling(typeof(SpanMarshaller<,>))]
public readonly ref struct Span<T>
{
	public ref struct Enumerator
	{
		public ref T Current => ref current;

		private ref T current;
		private int index;
		private readonly Span<T> span;

		internal Enumerator(Span<T> instance) => span = instance;

		public bool MoveNext()
		{
			if (index < span.backingLength)
			{
				current = ref span[index++];
				return true;
			}

			return false;
		}
	}

	public static Span<T> Empty => default;

	public bool IsEmpty => backingLength == 0;

	internal readonly ref T backingPointer;

	private readonly int backingLength;

	public ref T this[int index]
	{
		get
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, backingLength, nameof(index));

			return ref Unsafe.Add(ref backingPointer, index);
		}
	}

	public int Length => backingLength;

	[CLSCompliant(false)]
	public unsafe Span(void* pointer, int length)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

		if (Internal.Impl.RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			Internal.Exceptions.Span.ThrowReferenceException();

		backingPointer = ref Unsafe.AsRef<T>(pointer);
		backingLength = length;
	}

	public Span(T[]? array)
	{
		// FIXME: We aren't following what the spec says here, and we throw an exception if "array" is null.
		// Instead, we should return a null Span<T>... but how?
		ArgumentNullException.ThrowIfNull(array, nameof(array));

		if (Internal.Impl.RuntimeHelpers.IsReferenceOrContainsReferences<T>() && array.GetType().GetElementType() is not T)
			Internal.Exceptions.Span.ThrowTypeMismatchException();

		backingPointer = ref array[0];
		backingLength = array.Length;
	}

	public Span(T[]? array, int start, int length)
	{
		// FIXME: We aren't following what the spec says here, and we throw an exception if "array" is null.
		// Instead, we should return a default Span<T>... but how?
		ArgumentNullException.ThrowIfNull(array, nameof(array));

		if (array is null)
		{
			ArgumentOutOfRangeException.ThrowIfNotEqual(start, 0, nameof(start));
			ArgumentOutOfRangeException.ThrowIfNotEqual(length, 0, nameof(length));
		}

		ArgumentOutOfRangeException.ThrowIfNegative(start, nameof(start));
		ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(start, array.Length, nameof(start));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(length, array.Length, nameof(length));

		if (Internal.Impl.RuntimeHelpers.IsReferenceOrContainsReferences<T>() && array.GetType().GetElementType() is not T)
			Internal.Exceptions.Span.ThrowTypeMismatchException();

		backingPointer = ref array[start];
		backingLength = length;
	}

	public Span(ref T reference)
	{
		backingPointer = ref reference;
		backingLength = 1;
	}

	internal Span(ref T reference, int length)
	{
		backingPointer = ref reference;
		backingLength = length;
	}

	public void Clear()
	{
		for (var i = 0; i < backingLength; i++)
			Unsafe.Add(ref backingPointer, i) = default;
	}

	public void CopyTo(Span<T> destination)
	{
		if (destination.backingLength < backingLength)
			Internal.Exceptions.Span.ThrowDestinationShorterException();

		for (var i = 0; i < backingLength; i++)
			Unsafe.Add(ref destination.backingPointer, i) = Unsafe.Add(ref backingPointer, i);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Equals() on Span will always throw an exception. Use the equality operator instead.")]
	public override bool Equals(object? obj)
	{
		Internal.Exceptions.Generic.NotSupported();
		return false;
	}

	public void Fill(T value)
	{
		for (var i = 0; i < backingLength; i++)
			Unsafe.Add(ref backingPointer, i) = value;
	}

	public Enumerator GetEnumerator() => new(this);

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("GetHashCode() on Span will always throw an exception.")]
	public override int GetHashCode()
	{
		Internal.Exceptions.Generic.NotSupported();
		return 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public ref T GetPinnableReference()
	{
		if (!IsEmpty)
			return ref backingPointer;

		return ref Unsafe.NullRef<T>();
	}

	public static bool operator ==(Span<T> left, Span<T> right) => left.backingLength == right.backingLength &&
		Unsafe.AreSame(ref left.backingPointer, ref right.backingPointer);

	// TODO
	public static implicit operator Span<T>(ArraySegment<T> segment) => throw new NotImplementedException();

	public static implicit operator ReadOnlySpan<T>(Span<T> span) => new(ref span.backingPointer, span.backingLength);

	public static implicit operator Span<T>(T[]? array) => new(array);

	public static bool operator !=(Span<T> left, Span<T> right) => !(left == right);

	public Span<T> Slice(int start)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(start, nameof(start));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(start, backingLength, nameof(start));

		return new Span<T>(ref Unsafe.Add(ref backingPointer, start), backingLength - start);
	}

	public Span<T> Slice(int start, int length)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(start, nameof(start));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(start, backingLength, nameof(start));
		// TODO: Correct parameter name
		ArgumentOutOfRangeException.ThrowIfNegative(start + length, nameof(length));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(start + length, backingLength, nameof(length));

		return new Span<T>(ref Unsafe.Add(ref backingPointer, start), length);
	}

	public T[] ToArray()
	{
		var array = new T[backingLength];

		for (var i = 0; i < backingLength; i++)
			array[i] = Unsafe.Add(ref backingPointer, i);

		return array;
	}

	public override string ToString()
	{
		if (typeof(T) == typeof(char))
		{
			var span = new ReadOnlySpan<char>(ref Unsafe.As<T, char>(ref backingPointer), backingLength);
			return new string(new ReadOnlySpan<char>(span.ToArray()));
		}

		return $"{nameof(System)}.{nameof(Span<T>)}[{backingLength}]";
	}

	public bool TryCopyTo(Span<T> destination)
	{
		if (destination.backingLength < backingLength)
			return false;

		CopyTo(destination);
		return true;
	}
}
