// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System;

[NonVersionable]
public readonly ref struct Span<T>
{
	internal readonly ByReference<T> _pointer;
	private readonly int _length;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe Span(void* pointer, int length)
	{
		if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			throw new ArgumentException(nameof(pointer));
		}
		if (length < 0)
		{
			throw new ArgumentException(nameof(length));
		}

		_pointer = new ByReference<T>(ref Unsafe.As<byte, T>(ref *(byte*)pointer));
		_length = length;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal Span(ref T ptr, int length)
	{
		Debug.Assert(length >= 0);

		_pointer = new ByReference<T>(ref ptr);
		_length = length;
	}

	public ref T this[int index]
	{
		[Intrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[NonVersionable]
		get
		{
			if ((uint)index >= (uint)_length)
			{
				throw new IndexOutOfRangeException();
			}

			return ref Unsafe.Add(ref _pointer.Value, (nint)(uint)index);
		}
	}

	public int Length
	{
		[NonVersionable]
		get => _length;
	}

	public bool IsEmpty
	{
		[NonVersionable]
		get => 0 >= (uint)_length;
	}

	public static bool operator !=(Span<T> left, Span<T> right) => !(left == right);

	public static bool operator ==(Span<T> left, Span<T> right) =>
		left._length == right._length &&
		Unsafe.AreSame<T>(ref left._pointer.Value, ref right._pointer.Value);

	public override bool Equals(object obj) =>
		throw new NotSupportedException();

	public override int GetHashCode() =>
		throw new NotSupportedException();

	public static Span<T> Empty => default;

	public Enumerator GetEnumerator() => new Enumerator(this);

	public ref struct Enumerator
	{
		private readonly Span<T> _span;
		private int _index;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal Enumerator(Span<T> span)
		{
			_span = span;
			_index = -1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool MoveNext()
		{
			int index = _index + 1;
			if (index < _span.Length)
			{
				_index = index;
				return true;
			}

			return false;
		}

		public ref T Current
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => ref _span[_index];
		}
	}

	public static implicit operator ReadOnlySpan<T>(Span<T> span) =>
		new ReadOnlySpan<T>(ref span._pointer.Value, span._length);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Span<T> Slice(int start)
	{
		if ((uint)start > (uint)_length)
		{
			throw new ArgumentOutOfRangeException();
		}

		return new Span<T>(ref Unsafe.Add(ref _pointer.Value, (nint)(uint)start), _length - start);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Span<T> Slice(int start, int length)
	{
		if ((ulong)(uint)start + (ulong)(uint)length > (ulong)(uint)_length)
		{
			throw new ArgumentOutOfRangeException();
		}

		return new Span<T>(ref Unsafe.Add(ref _pointer.Value, (nint)(uint)start), length);
	}

	public override string ToString()
	{
		if (typeof(T) == typeof(char))
		{
			return new string(new ReadOnlySpan<char>(ref Unsafe.As<T, char>(ref this._pointer.Value), this._length));
		}

		return $"{nameof(System)}.{nameof(Span<T>)}";
	}
}
