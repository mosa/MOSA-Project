using System.Diagnostics.CodeAnalysis;

namespace System;

public readonly struct Index(int value, bool fromEnd = false) : IEquatable<Index>
{
	public static Index End { get; } = new(0, true);

	public bool IsFromEnd { get; } = fromEnd;

	public static Index Start { get; } = new(0);

	public int Value { get; } = value;

	public bool Equals(Index other) => Value == other.Value && IsFromEnd == other.IsFromEnd;

	public override bool Equals([NotNullWhen(true)] object? value) => value is Index index && Equals(index);

	public static Index FromEnd(int value) => new(value, true);

	public static Index FromStart(int value) => new(value);

	public override int GetHashCode() => HashCode.Combine(Value, IsFromEnd);

	public int GetOffset(int length) => IsFromEnd ? length - Value : Value;

	public static implicit operator Index(int value) => new(value);

	public override string ToString() => IsFromEnd ? $"^{Value}" : Value.ToString();
}
