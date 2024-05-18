using Microsoft.Win32.SafeHandles;

namespace System.IO.Pipes;

public sealed class AnonymousPipeClientStream : PipeStream
{
	public override PipeTransmissionMode ReadMode
	{
		set
		{
		}
	}

	public override PipeTransmissionMode TransmissionMode
	{
		get
		{
			throw null;
		}
	}

	public AnonymousPipeClientStream(PipeDirection direction, SafePipeHandle safePipeHandle)
		: base((PipeDirection)0, 0)
	{
	}

	public AnonymousPipeClientStream(PipeDirection direction, string pipeHandleAsString)
		: base((PipeDirection)0, 0)
	{
	}

	public AnonymousPipeClientStream(string pipeHandleAsString)
		: base((PipeDirection)0, 0)
	{
	}

	~AnonymousPipeClientStream()
	{
	}
}
