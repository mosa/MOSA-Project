using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net;

public static class Dns
{
	public static IAsyncResult BeginGetHostAddresses(string hostNameOrAddress, AsyncCallback? requestCallback, object? state)
	{
		throw null;
	}

	[Obsolete("BeginGetHostByName has been deprecated. Use BeginGetHostEntry instead.")]
	public static IAsyncResult BeginGetHostByName(string hostName, AsyncCallback? requestCallback, object? stateObject)
	{
		throw null;
	}

	public static IAsyncResult BeginGetHostEntry(IPAddress address, AsyncCallback? requestCallback, object? stateObject)
	{
		throw null;
	}

	public static IAsyncResult BeginGetHostEntry(string hostNameOrAddress, AsyncCallback? requestCallback, object? stateObject)
	{
		throw null;
	}

	[Obsolete("BeginResolve has been deprecated. Use BeginGetHostEntry instead.")]
	public static IAsyncResult BeginResolve(string hostName, AsyncCallback? requestCallback, object? stateObject)
	{
		throw null;
	}

	public static IPAddress[] EndGetHostAddresses(IAsyncResult asyncResult)
	{
		throw null;
	}

	[Obsolete("EndGetHostByName has been deprecated. Use EndGetHostEntry instead.")]
	public static IPHostEntry EndGetHostByName(IAsyncResult asyncResult)
	{
		throw null;
	}

	public static IPHostEntry EndGetHostEntry(IAsyncResult asyncResult)
	{
		throw null;
	}

	[Obsolete("EndResolve has been deprecated. Use EndGetHostEntry instead.")]
	public static IPHostEntry EndResolve(IAsyncResult asyncResult)
	{
		throw null;
	}

	public static IPAddress[] GetHostAddresses(string hostNameOrAddress)
	{
		throw null;
	}

	public static IPAddress[] GetHostAddresses(string hostNameOrAddress, AddressFamily family)
	{
		throw null;
	}

	public static Task<IPAddress[]> GetHostAddressesAsync(string hostNameOrAddress)
	{
		throw null;
	}

	public static Task<IPAddress[]> GetHostAddressesAsync(string hostNameOrAddress, AddressFamily family, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<IPAddress[]> GetHostAddressesAsync(string hostNameOrAddress, CancellationToken cancellationToken)
	{
		throw null;
	}

	[Obsolete("GetHostByAddress has been deprecated. Use GetHostEntry instead.")]
	public static IPHostEntry GetHostByAddress(IPAddress address)
	{
		throw null;
	}

	[Obsolete("GetHostByAddress has been deprecated. Use GetHostEntry instead.")]
	public static IPHostEntry GetHostByAddress(string address)
	{
		throw null;
	}

	[Obsolete("GetHostByName has been deprecated. Use GetHostEntry instead.")]
	public static IPHostEntry GetHostByName(string hostName)
	{
		throw null;
	}

	public static IPHostEntry GetHostEntry(IPAddress address)
	{
		throw null;
	}

	public static IPHostEntry GetHostEntry(string hostNameOrAddress)
	{
		throw null;
	}

	public static IPHostEntry GetHostEntry(string hostNameOrAddress, AddressFamily family)
	{
		throw null;
	}

	public static Task<IPHostEntry> GetHostEntryAsync(IPAddress address)
	{
		throw null;
	}

	public static Task<IPHostEntry> GetHostEntryAsync(string hostNameOrAddress)
	{
		throw null;
	}

	public static Task<IPHostEntry> GetHostEntryAsync(string hostNameOrAddress, AddressFamily family, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static Task<IPHostEntry> GetHostEntryAsync(string hostNameOrAddress, CancellationToken cancellationToken)
	{
		throw null;
	}

	public static string GetHostName()
	{
		throw null;
	}

	[Obsolete("Resolve has been deprecated. Use GetHostEntry instead.")]
	public static IPHostEntry Resolve(string hostName)
	{
		throw null;
	}
}
