using System.Diagnostics.CodeAnalysis;

namespace System.Reflection.Emit;

public readonly struct OpCode : IEquatable<OpCode>
{
	private readonly int _dummyPrimitive;

	public FlowControl FlowControl
	{
		get
		{
			throw null;
		}
	}

	public string? Name
	{
		get
		{
			throw null;
		}
	}

	public OpCodeType OpCodeType
	{
		get
		{
			throw null;
		}
	}

	public OperandType OperandType
	{
		get
		{
			throw null;
		}
	}

	public int Size
	{
		get
		{
			throw null;
		}
	}

	public StackBehaviour StackBehaviourPop
	{
		get
		{
			throw null;
		}
	}

	public StackBehaviour StackBehaviourPush
	{
		get
		{
			throw null;
		}
	}

	public short Value
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

	public bool Equals(OpCode obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(OpCode a, OpCode b)
	{
		throw null;
	}

	public static bool operator !=(OpCode a, OpCode b)
	{
		throw null;
	}

	public override string? ToString()
	{
		throw null;
	}
}
