using Microsoft.Win32.SafeHandles;

namespace System.IO.Pipes;

public sealed class AnonymousPipeServerStream : PipeStream
{
	public SafePipeHandle ClientSafePipeHandle
	{
		get
		{
			throw null;
		}
	}

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

	public AnonymousPipeServerStream()
		: base((PipeDirection)0, 0)
	{
	}

	public AnonymousPipeServerStream(PipeDirection direction)
		: base((PipeDirection)0, 0)
	{
	}

	public AnonymousPipeServerStream(PipeDirection direction, SafePipeHandle serverSafePipeHandle, SafePipeHandle clientSafePipeHandle)
		: base((PipeDirection)0, 0)
	{
	}

	public AnonymousPipeServerStream(PipeDirection direction, HandleInheritability inheritability)
		: base((PipeDirection)0, 0)
	{
	}

	public AnonymousPipeServerStream(PipeDirection direction, HandleInheritability inheritability, int bufferSize)
		: base((PipeDirection)0, 0)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public void DisposeLocalCopyOfClientHandle()
	{
	}

	~AnonymousPipeServerStream()
	{
	}

	public string GetClientHandleAsString()
	{
		throw null;
	}
}
