using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.Design.Serialization;

public readonly struct MemberRelationship : IEquatable<MemberRelationship>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public static readonly MemberRelationship Empty;

	public bool IsEmpty
	{
		get
		{
			throw null;
		}
	}

	public MemberDescriptor Member
	{
		get
		{
			throw null;
		}
	}

	public object? Owner
	{
		get
		{
			throw null;
		}
	}

	public MemberRelationship(object owner, MemberDescriptor member)
	{
		throw null;
	}

	public bool Equals(MemberRelationship other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(MemberRelationship left, MemberRelationship right)
	{
		throw null;
	}

	public static bool operator !=(MemberRelationship left, MemberRelationship right)
	{
		throw null;
	}
}
