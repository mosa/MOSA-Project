using System.Buffers;

namespace System.IO.Pipelines;

public readonly struct ReadResult
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public ReadOnlySequence<byte> Buffer
	{
		get
		{
			throw null;
		}
	}

	public bool IsCanceled
	{
		get
		{
			throw null;
		}
	}

	public bool IsCompleted
	{
		get
		{
			throw null;
		}
	}

	public ReadResult(ReadOnlySequence<byte> buffer, bool isCanceled, bool isCompleted)
	{
		throw null;
	}
}
