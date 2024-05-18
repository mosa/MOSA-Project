using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata.Ecma335;

public readonly struct LabelHandle : IEquatable<LabelHandle>
{
	private readonly int _dummyPrimitive;

	public int Id
	{
		get
		{
			throw null;
		}
	}

	public bool IsNil
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

	public bool Equals(LabelHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(LabelHandle left, LabelHandle right)
	{
		throw null;
	}

	public static bool operator !=(LabelHandle left, LabelHandle right)
	{
		throw null;
	}
}
