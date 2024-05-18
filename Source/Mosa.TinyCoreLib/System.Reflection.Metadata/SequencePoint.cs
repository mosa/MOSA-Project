using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata;

public readonly struct SequencePoint : IEquatable<SequencePoint>
{
	private readonly int _dummyPrimitive;

	public const int HiddenLine = 16707566;

	public DocumentHandle Document
	{
		get
		{
			throw null;
		}
	}

	public int EndColumn
	{
		get
		{
			throw null;
		}
	}

	public int EndLine
	{
		get
		{
			throw null;
		}
	}

	public bool IsHidden
	{
		get
		{
			throw null;
		}
	}

	public int Offset
	{
		get
		{
			throw null;
		}
	}

	public int StartColumn
	{
		get
		{
			throw null;
		}
	}

	public int StartLine
	{
		get
		{
			throw null;
		}
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(SequencePoint other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
