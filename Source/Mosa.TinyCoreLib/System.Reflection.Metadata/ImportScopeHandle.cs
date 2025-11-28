using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata;

public readonly struct ImportScopeHandle : IEquatable<ImportScopeHandle>
{
	private readonly int _dummyPrimitive;

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

	public bool Equals(ImportScopeHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(ImportScopeHandle left, ImportScopeHandle right)
	{
		throw null;
	}

	public static explicit operator ImportScopeHandle(EntityHandle handle)
	{
		throw null;
	}

	public static explicit operator ImportScopeHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator EntityHandle(ImportScopeHandle handle)
	{
		throw null;
	}

	public static implicit operator Handle(ImportScopeHandle handle)
	{
		throw null;
	}

	public static bool operator !=(ImportScopeHandle left, ImportScopeHandle right)
	{
		throw null;
	}
}
