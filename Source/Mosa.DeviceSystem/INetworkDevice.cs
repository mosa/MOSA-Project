// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to a network device
	/// </summary>
	public interface INetworkDevice
	{
		/// <summary>
		/// Gets the MAC address.
		/// </summary>
		/// <value>The MAC address.</value>
		MACAddress MACAddress { get; }

		/// <summary>
		/// Sends the packet to the device
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		bool SendPacket(byte[] data);

		/// <summary>
		/// Assigns the packet buffer to the device
		/// </summary>
		/// <param name="packetBuffer">The packet buffer.</param>
		void AssignPacketBuffer(NetworkDevicePacketBuffer packetBuffer);
	}
}