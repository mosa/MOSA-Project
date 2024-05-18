using System.ComponentModel;
using System.Runtime.Versioning;

namespace System.Net.NetworkInformation;

public class NetworkChange
{
	[UnsupportedOSPlatform("illumos")]
	[UnsupportedOSPlatform("solaris")]
	public static event NetworkAddressChangedEventHandler? NetworkAddressChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	[UnsupportedOSPlatform("illumos")]
	[UnsupportedOSPlatform("solaris")]
	public static event NetworkAvailabilityChangedEventHandler? NetworkAvailabilityChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
	public NetworkChange()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
	public static void RegisterNetworkChange(NetworkChange nc)
	{
	}
}
