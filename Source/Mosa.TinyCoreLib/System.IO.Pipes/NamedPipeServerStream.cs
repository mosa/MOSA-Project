using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Pipes;

public sealed class NamedPipeServerStream : PipeStream
{
	public const int MaxAllowedServerInstances = -1;

	public NamedPipeServerStream(PipeDirection direction, bool isAsync, bool isConnected, SafePipeHandle safePipeHandle)
		: base((PipeDirection)0, 0)
	{
	}

	public NamedPipeServerStream(string pipeName)
		: base((PipeDirection)0, 0)
	{
	}

	public NamedPipeServerStream(string pipeName, PipeDirection direction)
		: base((PipeDirection)0, 0)
	{
	}

	public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances)
		: base((PipeDirection)0, 0)
	{
	}

	public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode)
		: base((PipeDirection)0, 0)
	{
	}

	public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options)
		: base((PipeDirection)0, 0)
	{
	}

	public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options, int inBufferSize, int outBufferSize)
		: base((PipeDirection)0, 0)
	{
	}

	public IAsyncResult BeginWaitForConnection(AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public void Disconnect()
	{
	}

	public void EndWaitForConnection(IAsyncResult asyncResult)
	{
	}

	~NamedPipeServerStream()
	{
	}

	public string GetImpersonationUserName()
	{
		throw null;
	}

	public void RunAsClient(PipeStreamImpersonationWorker impersonationWorker)
	{
	}

	public void WaitForConnection()
	{
	}

	public Task WaitForConnectionAsync()
	{
		throw null;
	}

	public Task WaitForConnectionAsync(CancellationToken cancellationToken)
	{
		throw null;
	}
}
