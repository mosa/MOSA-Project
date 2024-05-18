using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.NetworkInformation;

public class Ping : Component
{
	public event PingCompletedEventHandler? PingCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	protected override void Dispose(bool disposing)
	{
	}

	protected void OnPingCompleted(PingCompletedEventArgs e)
	{
	}

	public PingReply Send(IPAddress address)
	{
		throw null;
	}

	public PingReply Send(IPAddress address, int timeout)
	{
		throw null;
	}

	public PingReply Send(IPAddress address, int timeout, byte[] buffer)
	{
		throw null;
	}

	public PingReply Send(IPAddress address, int timeout, byte[] buffer, PingOptions? options)
	{
		throw null;
	}

	public PingReply Send(string hostNameOrAddress)
	{
		throw null;
	}

	public PingReply Send(string hostNameOrAddress, int timeout)
	{
		throw null;
	}

	public PingReply Send(string hostNameOrAddress, int timeout, byte[] buffer)
	{
		throw null;
	}

	public PingReply Send(string hostNameOrAddress, int timeout, byte[] buffer, PingOptions? options)
	{
		throw null;
	}

	public PingReply Send(IPAddress address, TimeSpan timeout, byte[]? buffer, PingOptions? options)
	{
		throw null;
	}

	public PingReply Send(string hostNameOrAddress, TimeSpan timeout, byte[]? buffer, PingOptions? options)
	{
		throw null;
	}

	public void SendAsync(IPAddress address, int timeout, byte[] buffer, PingOptions? options, object? userToken)
	{
	}

	public void SendAsync(IPAddress address, int timeout, byte[] buffer, object? userToken)
	{
	}

	public void SendAsync(IPAddress address, int timeout, object? userToken)
	{
	}

	public void SendAsync(IPAddress address, object? userToken)
	{
	}

	public void SendAsync(string hostNameOrAddress, int timeout, byte[] buffer, PingOptions? options, object? userToken)
	{
	}

	public void SendAsync(string hostNameOrAddress, int timeout, byte[] buffer, object? userToken)
	{
	}

	public void SendAsync(string hostNameOrAddress, int timeout, object? userToken)
	{
	}

	public void SendAsync(string hostNameOrAddress, object? userToken)
	{
	}

	public void SendAsyncCancel()
	{
	}

	public Task<PingReply> SendPingAsync(IPAddress address)
	{
		throw null;
	}

	public Task<PingReply> SendPingAsync(IPAddress address, int timeout)
	{
		throw null;
	}

	public Task<PingReply> SendPingAsync(IPAddress address, int timeout, byte[] buffer)
	{
		throw null;
	}

	public Task<PingReply> SendPingAsync(IPAddress address, int timeout, byte[] buffer, PingOptions? options)
	{
		throw null;
	}

	public Task<PingReply> SendPingAsync(IPAddress address, TimeSpan timeout, byte[]? buffer = null, PingOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public Task<PingReply> SendPingAsync(string hostNameOrAddress)
	{
		throw null;
	}

	public Task<PingReply> SendPingAsync(string hostNameOrAddress, int timeout)
	{
		throw null;
	}

	public Task<PingReply> SendPingAsync(string hostNameOrAddress, int timeout, byte[] buffer)
	{
		throw null;
	}

	public Task<PingReply> SendPingAsync(string hostNameOrAddress, int timeout, byte[] buffer, PingOptions? options)
	{
		throw null;
	}

	public Task<PingReply> SendPingAsync(string hostNameOrAddress, TimeSpan timeout, byte[]? buffer = null, PingOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
