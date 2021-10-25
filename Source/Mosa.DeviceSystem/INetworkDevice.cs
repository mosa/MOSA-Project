// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to a network device
	/// </summary>
	public unsafe interface INetworkDevice
	{
		/// <summary>
		/// Gets the MAC address.
		/// </summary>
		/// <value>The MAC address.</value>
		MACAddress MACAddress { get; set; }

		/// <summary>
		/// Sends the packet to the device
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		void SendPacket(byte* data, ushort length);
	}
}
