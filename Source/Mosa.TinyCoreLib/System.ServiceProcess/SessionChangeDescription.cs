using System.Diagnostics.CodeAnalysis;

namespace System.ServiceProcess;

public readonly struct SessionChangeDescription : IEquatable<SessionChangeDescription>
{
	private readonly int _dummyPrimitive;

	public SessionChangeReason Reason
	{
		get
		{
			throw null;
		}
	}

	public int SessionId
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

	public bool Equals(SessionChangeDescription changeDescription)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(SessionChangeDescription a, SessionChangeDescription b)
	{
		throw null;
	}

	public static bool operator !=(SessionChangeDescription a, SessionChangeDescription b)
	{
		throw null;
	}
}
