// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to a network device
	/// </summary>
	public interface INetworkDevice
	{
		/// <summary>
		/// Sends the packet to the device
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		void SendPacket(Pointer data, uint length);
	}
}
