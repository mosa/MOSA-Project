using System.Net;

namespace Mosa.DeviceSystem.Network
{
	public static class Network
	{
		public static byte[] MAC, IP, Broadcast, Gateway;

		public static void Initialise(IPAddress IPAddress, IPAddress GatewayAddress)
		{
			Broadcast = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
			Gateway = GatewayAddress.Address;
			IP = IPAddress.Address;

			ARP.Initialise();
		}
	}
}
