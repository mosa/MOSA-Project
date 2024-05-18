using System.Diagnostics.CodeAnalysis;

namespace System.Threading.Tasks.Dataflow;

public readonly struct DataflowMessageHeader : IEquatable<DataflowMessageHeader>
{
	private readonly int _dummyPrimitive;

	public long Id
	{
		get
		{
			throw null;
		}
	}

	public bool IsValid
	{
		get
		{
			throw null;
		}
	}

	public DataflowMessageHeader(long id)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(DataflowMessageHeader other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(DataflowMessageHeader left, DataflowMessageHeader right)
	{
		throw null;
	}

	public static bool operator !=(DataflowMessageHeader left, DataflowMessageHeader right)
	{
		throw null;
	}
}
