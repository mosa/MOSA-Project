using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata;

public readonly struct Handle : IEquatable<Handle>
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

	public bool Equals(Handle other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(Handle left, Handle right)
	{
		throw null;
	}

	public static bool operator !=(Handle left, Handle right)
	{
		throw null;
	}
}
