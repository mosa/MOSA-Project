using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Metadata.Ecma335;

public readonly struct EditAndContinueLogEntry : IEquatable<EditAndContinueLogEntry>
{
	private readonly int _dummyPrimitive;

	public EntityHandle Handle
	{
		get
		{
			throw null;
		}
	}

	public EditAndContinueOperation Operation
	{
		get
		{
			throw null;
		}
	}

	public EditAndContinueLogEntry(EntityHandle handle, EditAndContinueOperation operation)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(EditAndContinueLogEntry other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
