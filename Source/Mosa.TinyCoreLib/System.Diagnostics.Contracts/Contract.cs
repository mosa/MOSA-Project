using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics.Contracts;

public static class Contract
{
	public static event EventHandler<ContractFailedEventArgs>? ContractFailed
	{
		add
		{
		}
		remove
		{
		}
	}

	[Conditional("CONTRACTS_FULL")]
	[Conditional("DEBUG")]
	public static void Assert([DoesNotReturnIf(false)] bool condition)
	{
	}

	[Conditional("CONTRACTS_FULL")]
	[Conditional("DEBUG")]
	public static void Assert([DoesNotReturnIf(false)] bool condition, string? userMessage)
	{
	}

	[Conditional("CONTRACTS_FULL")]
	[Conditional("DEBUG")]
	public static void Assume([DoesNotReturnIf(false)] bool condition)
	{
	}

	[Conditional("CONTRACTS_FULL")]
	[Conditional("DEBUG")]
	public static void Assume([DoesNotReturnIf(false)] bool condition, string? userMessage)
	{
	}

	[Conditional("CONTRACTS_FULL")]
	public static void EndContractBlock()
	{
	}

	[Conditional("CONTRACTS_FULL")]
	public static void Ensures(bool condition)
	{
	}

	[Conditional("CONTRACTS_FULL")]
	public static void Ensures(bool condition, string? userMessage)
	{
	}

	[Conditional("CONTRACTS_FULL")]
	public static void EnsuresOnThrow<TException>(bool condition) where TException : Exception
	{
	}

	[Conditional("CONTRACTS_FULL")]
	public static void EnsuresOnThrow<TException>(bool condition, string? userMessage) where TException : Exception
	{
	}

	public static bool Exists(int fromInclusive, int toExclusive, Predicate<int> predicate)
	{
		throw null;
	}

	public static bool Exists<T>(IEnumerable<T> collection, Predicate<T> predicate)
	{
		throw null;
	}

	public static bool ForAll(int fromInclusive, int toExclusive, Predicate<int> predicate)
	{
		throw null;
	}

	public static bool ForAll<T>(IEnumerable<T> collection, Predicate<T> predicate)
	{
		throw null;
	}

	[Conditional("CONTRACTS_FULL")]
	public static void Invariant(bool condition)
	{
	}

	[Conditional("CONTRACTS_FULL")]
	public static void Invariant(bool condition, string? userMessage)
	{
	}

	public static T OldValue<T>(T value)
	{
		throw null;
	}

	[Conditional("CONTRACTS_FULL")]
	public static void Requires(bool condition)
	{
	}

	[Conditional("CONTRACTS_FULL")]
	public static void Requires(bool condition, string? userMessage)
	{
	}

	public static void Requires<TException>(bool condition) where TException : Exception
	{
	}

	public static void Requires<TException>(bool condition, string? userMessage) where TException : Exception
	{
	}

	public static T Result<T>()
	{
		throw null;
	}

	public static T ValueAtReturn<T>(out T value)
	{
		throw null;
	}
}
