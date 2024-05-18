using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata;

public readonly struct EntityHandle : IEquatable<EntityHandle>
{
	private readonly int _dummyPrimitive;

	public static readonly AssemblyDefinitionHandle AssemblyDefinition;

	public static readonly ModuleDefinitionHandle ModuleDefinition;

	public bool IsNil
	{
		get
		{
			throw null;
		}
	}

	public HandleKind Kind
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

	public bool Equals(EntityHandle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(EntityHandle left, EntityHandle right)
	{
		throw null;
	}

	public static explicit operator EntityHandle(Handle handle)
	{
		throw null;
	}

	public static implicit operator Handle(EntityHandle handle)
	{
		throw null;
	}

	public static bool operator !=(EntityHandle left, EntityHandle right)
	{
		throw null;
	}
}
