using System.Diagnostics.CodeAnalysis;

namespace System.Transactions;

public struct TransactionOptions : IEquatable<TransactionOptions>
{
	private readonly int _dummyPrimitive;

	public IsolationLevel IsolationLevel
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan Timeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(TransactionOptions other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(TransactionOptions x, TransactionOptions y)
	{
		throw null;
	}

	public static bool operator !=(TransactionOptions x, TransactionOptions y)
	{
		throw null;
	}
}
