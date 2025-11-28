namespace System.Buffers;

public abstract class ReadOnlySequenceSegment<T>
{
	public ReadOnlyMemory<T> Memory
	{
		get
		{
			throw null;
		}
		protected set
		{
		}
	}

	public ReadOnlySequenceSegment<T>? Next
	{
		get
		{
			throw null;
		}
		protected set
		{
		}
	}

	public long RunningIndex
	{
		get
		{
			throw null;
		}
		protected set
		{
		}
	}
}
