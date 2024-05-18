using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Dynamic;

public sealed class CallInfo
{
	public int ArgumentCount
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<string> ArgumentNames
	{
		get
		{
			throw null;
		}
	}

	public CallInfo(int argCount, IEnumerable<string> argNames)
	{
	}

	public CallInfo(int argCount, params string[] argNames)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
