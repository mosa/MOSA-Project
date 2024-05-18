using System.Runtime.Versioning;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Pipes;

public sealed class NamedPipeClientStream : PipeStream
{
	[SupportedOSPlatform("windows")]
	public int NumberOfServerInstances
	{
		get
		{
			throw null;
		}
	}

	public NamedPipeClientStream(PipeDirection direction, bool isAsync, bool isConnected, SafePipeHandle safePipeHandle)
		: base((PipeDirection)0, 0)
	{
	}

	public NamedPipeClientStream(string pipeName)
		: base((PipeDirection)0, 0)
	{
	}

	public NamedPipeClientStream(string serverName, string pipeName)
		: base((PipeDirection)0, 0)
	{
	}

	public NamedPipeClientStream(string serverName, string pipeName, PipeDirection direction)
		: base((PipeDirection)0, 0)
	{
	}

	public NamedPipeClientStream(string serverName, string pipeName, PipeDirection direction, PipeOptions options)
		: base((PipeDirection)0, 0)
	{
	}

	public NamedPipeClientStream(string serverName, string pipeName, PipeDirection direction, PipeOptions options, TokenImpersonationLevel impersonationLevel)
		: base((PipeDirection)0, 0)
	{
	}

	public NamedPipeClientStream(string serverName, string pipeName, PipeDirection direction, PipeOptions options, TokenImpersonationLevel impersonationLevel, HandleInheritability inheritability)
		: base((PipeDirection)0, 0)
	{
	}

	protected internal override void CheckPipePropertyOperations()
	{
	}

	public void Connect()
	{
	}

	public void Connect(int timeout)
	{
	}

	public void Connect(TimeSpan timeout)
	{
	}

	public Task ConnectAsync()
	{
		throw null;
	}

	public Task ConnectAsync(int timeout)
	{
		throw null;
	}

	public Task ConnectAsync(int timeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ConnectAsync(TimeSpan timeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ConnectAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	~NamedPipeClientStream()
	{
	}
}
