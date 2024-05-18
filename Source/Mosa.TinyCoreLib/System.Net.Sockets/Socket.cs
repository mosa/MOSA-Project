using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Sockets;

public class Socket : IDisposable
{
	public AddressFamily AddressFamily
	{
		get
		{
			throw null;
		}
	}

	public int Available
	{
		get
		{
			throw null;
		}
	}

	public bool Blocking
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Connected
	{
		get
		{
			throw null;
		}
	}

	public bool DontFragment
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool DualMode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool EnableBroadcast
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ExclusiveAddressUse
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IntPtr Handle
	{
		get
		{
			throw null;
		}
	}

	public bool IsBound
	{
		get
		{
			throw null;
		}
	}

	public LingerOption? LingerState
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	public EndPoint? LocalEndPoint
	{
		get
		{
			throw null;
		}
	}

	public bool MulticastLoopback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool NoDelay
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static bool OSSupportsIPv4
	{
		get
		{
			throw null;
		}
	}

	public static bool OSSupportsIPv6
	{
		get
		{
			throw null;
		}
	}

	public static bool OSSupportsUnixDomainSockets
	{
		get
		{
			throw null;
		}
	}

	public ProtocolType ProtocolType
	{
		get
		{
			throw null;
		}
	}

	public int ReceiveBufferSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ReceiveTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EndPoint? RemoteEndPoint
	{
		get
		{
			throw null;
		}
	}

	public SafeSocketHandle SafeHandle
	{
		get
		{
			throw null;
		}
	}

	public int SendBufferSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int SendTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SocketType SocketType
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("SupportsIPv4 has been deprecated. Use OSSupportsIPv4 instead.")]
	public static bool SupportsIPv4
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("SupportsIPv6 has been deprecated. Use OSSupportsIPv6 instead.")]
	public static bool SupportsIPv6
	{
		get
		{
			throw null;
		}
	}

	public short Ttl
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("UseOnlyOverlappedIO has been deprecated and is not supported.")]
	public bool UseOnlyOverlappedIO
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Socket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
	{
	}

	public Socket(SafeSocketHandle handle)
	{
	}

	[SupportedOSPlatform("windows")]
	public Socket(SocketInformation socketInformation)
	{
	}

	public Socket(SocketType socketType, ProtocolType protocolType)
	{
	}

	public Socket Accept()
	{
		throw null;
	}

	public Task<Socket> AcceptAsync()
	{
		throw null;
	}

	public Task<Socket> AcceptAsync(Socket? acceptSocket)
	{
		throw null;
	}

	public ValueTask<Socket> AcceptAsync(Socket? acceptSocket, CancellationToken cancellationToken)
	{
		throw null;
	}

	public bool AcceptAsync(SocketAsyncEventArgs e)
	{
		throw null;
	}

	public ValueTask<Socket> AcceptAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public IAsyncResult BeginAccept(AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginAccept(int receiveSize, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginAccept(Socket? acceptSocket, int receiveSize, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback? requestCallback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback? requestCallback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginConnect(string host, int port, AsyncCallback? requestCallback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult? BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult? BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult? BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult? BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginSendFile(string? fileName, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginSendFile(string? fileName, byte[]? preBuffer, byte[]? postBuffer, TransmitFileOptions flags, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public IAsyncResult BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP, AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public void Bind(EndPoint localEP)
	{
	}

	public static void CancelConnectAsync(SocketAsyncEventArgs e)
	{
	}

	public void Close()
	{
	}

	public void Close(int timeout)
	{
	}

	public void Connect(EndPoint remoteEP)
	{
	}

	public void Connect(IPAddress address, int port)
	{
	}

	public void Connect(IPAddress[] addresses, int port)
	{
	}

	public void Connect(string host, int port)
	{
	}

	public Task ConnectAsync(EndPoint remoteEP)
	{
		throw null;
	}

	public ValueTask ConnectAsync(EndPoint remoteEP, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ConnectAsync(IPAddress address, int port)
	{
		throw null;
	}

	public ValueTask ConnectAsync(IPAddress address, int port, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task ConnectAsync(IPAddress[] addresses, int port)
	{
		throw null;
	}

	public ValueTask ConnectAsync(IPAddress[] addresses, int port, CancellationToken cancellationToken)
	{
		throw null;
	}

	public bool ConnectAsync(SocketAsyncEventArgs e)
	{
		throw null;
	}

	public static bool ConnectAsync(SocketType socketType, ProtocolType protocolType, SocketAsyncEventArgs e)
	{
		throw null;
	}

	public Task ConnectAsync(string host, int port)
	{
		throw null;
	}

	public ValueTask ConnectAsync(string host, int port, CancellationToken cancellationToken)
	{
		throw null;
	}

	public void Disconnect(bool reuseSocket)
	{
	}

	public ValueTask DisconnectAsync(bool reuseSocket, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public bool DisconnectAsync(SocketAsyncEventArgs e)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	[SupportedOSPlatform("windows")]
	public SocketInformation DuplicateAndClose(int targetProcessId)
	{
		throw null;
	}

	public Socket EndAccept(out byte[] buffer, IAsyncResult asyncResult)
	{
		throw null;
	}

	public Socket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult)
	{
		throw null;
	}

	public Socket EndAccept(IAsyncResult asyncResult)
	{
		throw null;
	}

	public void EndConnect(IAsyncResult asyncResult)
	{
	}

	public void EndDisconnect(IAsyncResult asyncResult)
	{
	}

	public int EndReceive(IAsyncResult asyncResult)
	{
		throw null;
	}

	public int EndReceive(IAsyncResult asyncResult, out SocketError errorCode)
	{
		throw null;
	}

	public int EndReceiveFrom(IAsyncResult asyncResult, ref EndPoint endPoint)
	{
		throw null;
	}

	public int EndReceiveMessageFrom(IAsyncResult asyncResult, ref SocketFlags socketFlags, ref EndPoint endPoint, out IPPacketInformation ipPacketInformation)
	{
		throw null;
	}

	public int EndSend(IAsyncResult asyncResult)
	{
		throw null;
	}

	public int EndSend(IAsyncResult asyncResult, out SocketError errorCode)
	{
		throw null;
	}

	public void EndSendFile(IAsyncResult asyncResult)
	{
	}

	public int EndSendTo(IAsyncResult asyncResult)
	{
		throw null;
	}

	~Socket()
	{
	}

	public int GetRawSocketOption(int optionLevel, int optionName, Span<byte> optionValue)
	{
		throw null;
	}

	public object? GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
	{
		throw null;
	}

	public void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
	{
	}

	public byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength)
	{
		throw null;
	}

	public int IOControl(int ioControlCode, byte[]? optionInValue, byte[]? optionOutValue)
	{
		throw null;
	}

	public int IOControl(IOControlCode ioControlCode, byte[]? optionInValue, byte[]? optionOutValue)
	{
		throw null;
	}

	public void Listen()
	{
	}

	public void Listen(int backlog)
	{
	}

	public bool Poll(int microSeconds, SelectMode mode)
	{
		throw null;
	}

	public bool Poll(TimeSpan timeout, SelectMode mode)
	{
		throw null;
	}

	public int Receive(byte[] buffer)
	{
		throw null;
	}

	public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
	{
		throw null;
	}

	public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
	{
		throw null;
	}

	public int Receive(byte[] buffer, int size, SocketFlags socketFlags)
	{
		throw null;
	}

	public int Receive(byte[] buffer, SocketFlags socketFlags)
	{
		throw null;
	}

	public int Receive(IList<ArraySegment<byte>> buffers)
	{
		throw null;
	}

	public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
	{
		throw null;
	}

	public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
	{
		throw null;
	}

	public int Receive(Span<byte> buffer)
	{
		throw null;
	}

	public int Receive(Span<byte> buffer, SocketFlags socketFlags)
	{
		throw null;
	}

	public int Receive(Span<byte> buffer, SocketFlags socketFlags, out SocketError errorCode)
	{
		throw null;
	}

	public Task<int> ReceiveAsync(ArraySegment<byte> buffer)
	{
		throw null;
	}

	public Task<int> ReceiveAsync(ArraySegment<byte> buffer, SocketFlags socketFlags)
	{
		throw null;
	}

	public Task<int> ReceiveAsync(IList<ArraySegment<byte>> buffers)
	{
		throw null;
	}

	public Task<int> ReceiveAsync(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
	{
		throw null;
	}

	public ValueTask<int> ReceiveAsync(Memory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public bool ReceiveAsync(SocketAsyncEventArgs e)
	{
		throw null;
	}

	public int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP)
	{
		throw null;
	}

	public int ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref EndPoint remoteEP)
	{
		throw null;
	}

	public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP)
	{
		throw null;
	}

	public int ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref EndPoint remoteEP)
	{
		throw null;
	}

	public int ReceiveFrom(Span<byte> buffer, ref EndPoint remoteEP)
	{
		throw null;
	}

	public int ReceiveFrom(Span<byte> buffer, SocketFlags socketFlags, ref EndPoint remoteEP)
	{
		throw null;
	}

	public int ReceiveFrom(Span<byte> buffer, SocketFlags socketFlags, SocketAddress receivedAddress)
	{
		throw null;
	}

	public Task<SocketReceiveFromResult> ReceiveFromAsync(ArraySegment<byte> buffer, EndPoint remoteEndPoint)
	{
		throw null;
	}

	public Task<SocketReceiveFromResult> ReceiveFromAsync(ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint)
	{
		throw null;
	}

	public ValueTask<SocketReceiveFromResult> ReceiveFromAsync(Memory<byte> buffer, EndPoint remoteEndPoint, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask<SocketReceiveFromResult> ReceiveFromAsync(Memory<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask<int> ReceiveFromAsync(Memory<byte> buffer, SocketFlags socketFlags, SocketAddress receivedAddress, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public bool ReceiveFromAsync(SocketAsyncEventArgs e)
	{
		throw null;
	}

	public int ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref EndPoint remoteEP, out IPPacketInformation ipPacketInformation)
	{
		throw null;
	}

	public int ReceiveMessageFrom(Span<byte> buffer, ref SocketFlags socketFlags, ref EndPoint remoteEP, out IPPacketInformation ipPacketInformation)
	{
		throw null;
	}

	public Task<SocketReceiveMessageFromResult> ReceiveMessageFromAsync(ArraySegment<byte> buffer, EndPoint remoteEndPoint)
	{
		throw null;
	}

	public Task<SocketReceiveMessageFromResult> ReceiveMessageFromAsync(ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint)
	{
		throw null;
	}

	public ValueTask<SocketReceiveMessageFromResult> ReceiveMessageFromAsync(Memory<byte> buffer, EndPoint remoteEndPoint, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask<SocketReceiveMessageFromResult> ReceiveMessageFromAsync(Memory<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public bool ReceiveMessageFromAsync(SocketAsyncEventArgs e)
	{
		throw null;
	}

	public static void Select(IList? checkRead, IList? checkWrite, IList? checkError, int microSeconds)
	{
	}

	public static void Select(IList? checkRead, IList? checkWrite, IList? checkError, TimeSpan timeout)
	{
	}

	public int Send(byte[] buffer)
	{
		throw null;
	}

	public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
	{
		throw null;
	}

	public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
	{
		throw null;
	}

	public int Send(byte[] buffer, int size, SocketFlags socketFlags)
	{
		throw null;
	}

	public int Send(byte[] buffer, SocketFlags socketFlags)
	{
		throw null;
	}

	public int Send(IList<ArraySegment<byte>> buffers)
	{
		throw null;
	}

	public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
	{
		throw null;
	}

	public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
	{
		throw null;
	}

	public int Send(ReadOnlySpan<byte> buffer)
	{
		throw null;
	}

	public int Send(ReadOnlySpan<byte> buffer, SocketFlags socketFlags)
	{
		throw null;
	}

	public int Send(ReadOnlySpan<byte> buffer, SocketFlags socketFlags, out SocketError errorCode)
	{
		throw null;
	}

	public Task<int> SendAsync(ArraySegment<byte> buffer)
	{
		throw null;
	}

	public Task<int> SendAsync(ArraySegment<byte> buffer, SocketFlags socketFlags)
	{
		throw null;
	}

	public Task<int> SendAsync(IList<ArraySegment<byte>> buffers)
	{
		throw null;
	}

	public Task<int> SendAsync(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
	{
		throw null;
	}

	public bool SendAsync(SocketAsyncEventArgs e)
	{
		throw null;
	}

	public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public void SendFile(string? fileName)
	{
	}

	public void SendFile(string? fileName, byte[]? preBuffer, byte[]? postBuffer, TransmitFileOptions flags)
	{
	}

	public void SendFile(string? fileName, ReadOnlySpan<byte> preBuffer, ReadOnlySpan<byte> postBuffer, TransmitFileOptions flags)
	{
	}

	public ValueTask SendFileAsync(string? fileName, ReadOnlyMemory<byte> preBuffer, ReadOnlyMemory<byte> postBuffer, TransmitFileOptions flags, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask SendFileAsync(string? fileName, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public bool SendPacketsAsync(SocketAsyncEventArgs e)
	{
		throw null;
	}

	public int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP)
	{
		throw null;
	}

	public int SendTo(byte[] buffer, int size, SocketFlags socketFlags, EndPoint remoteEP)
	{
		throw null;
	}

	public int SendTo(byte[] buffer, EndPoint remoteEP)
	{
		throw null;
	}

	public int SendTo(byte[] buffer, SocketFlags socketFlags, EndPoint remoteEP)
	{
		throw null;
	}

	public int SendTo(ReadOnlySpan<byte> buffer, EndPoint remoteEP)
	{
		throw null;
	}

	public int SendTo(ReadOnlySpan<byte> buffer, SocketFlags socketFlags, EndPoint remoteEP)
	{
		throw null;
	}

	public int SendTo(ReadOnlySpan<byte> buffer, SocketFlags socketFlags, SocketAddress socketAddress)
	{
		throw null;
	}

	public Task<int> SendToAsync(ArraySegment<byte> buffer, EndPoint remoteEP)
	{
		throw null;
	}

	public Task<int> SendToAsync(ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEP)
	{
		throw null;
	}

	public bool SendToAsync(SocketAsyncEventArgs e)
	{
		throw null;
	}

	public ValueTask<int> SendToAsync(ReadOnlyMemory<byte> buffer, EndPoint remoteEP, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask<int> SendToAsync(ReadOnlyMemory<byte> buffer, SocketFlags socketFlags, EndPoint remoteEP, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public ValueTask<int> SendToAsync(ReadOnlyMemory<byte> buffer, SocketFlags socketFlags, SocketAddress socketAddress, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public void SetIPProtectionLevel(IPProtectionLevel level)
	{
	}

	public void SetRawSocketOption(int optionLevel, int optionName, ReadOnlySpan<byte> optionValue)
	{
	}

	public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue)
	{
	}

	public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
	{
	}

	public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue)
	{
	}

	public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
	{
	}

	public void Shutdown(SocketShutdown how)
	{
	}
}
