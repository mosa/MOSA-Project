using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System;

public readonly struct SequencePosition : IEquatable<SequencePosition>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public SequencePosition(object? @object, int integer)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(SequencePosition other)
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int GetInteger()
	{
		throw null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public object? GetObject()
	{
		throw null;
	}
}
