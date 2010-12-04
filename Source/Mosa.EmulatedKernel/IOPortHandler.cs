/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;
using Mosa.EmulatedKernel;

namespace Mosa.EmulatedKernel
{
	/// <summary>
	/// 
	/// </summary>
	public class IOPortHandler
	{
		/// <summary>
		/// 
		/// </summary>
		private static IIOPortDevice[] ports = new IIOPortDevice[ushort.MaxValue];

		/// <summary>
		/// Initializes a new instance of the <see cref="IOPortHandler"/> class.
		/// </summary>
		public IOPortHandler()
		{
			for (ushort i = 0; i < ushort.MaxValue; i++)
				ports[i] = null;
		}

		/// <summary>
		/// Registers the specified device.
		/// </summary>
		/// <param name="device">The device.</param>
		public void Register(IIOPortDevice device)
		{
			foreach (ushort port in device.GetPortsRequested())
				ports[port] = device;
		}

		/// <summary>
		/// Unregisters the specified device.
		/// </summary>
		/// <param name="device">The device.</param>
		public void Unregister(IIOPortDevice device)
		{
			foreach (ushort port in device.GetPortsRequested())
				ports[port] = null;
		}

		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <returns></returns>
		public byte Read8(ushort port)
		{
			if (ports[port] == null) return 0xFF;
			return ports[port].Read8(port);
		}

		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <returns></returns>
		public ushort Read16(ushort port)
		{
			if (ports[port] == null) return 0xFFFF;
			return ports[port].Read16(port);
		}

		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <returns></returns>
		public uint Read32(ushort port)
		{
			if (ports[port] == null) return 0xFFFFFFFF;
			return ports[port].Read32(port);
		}

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="data">The data.</param>
		public void Write8(ushort port, byte data)
		{
			if (ports[port] == null) return;
			ports[port].Write8(port, data);
		}

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="data">The data.</param>
		public void Write16(ushort port, ushort data)
		{
			if (ports[port] == null) return;
			ports[port].Write16(port, data);
		}

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="data">The data.</param>
		public void Write32(ushort port, uint data)
		{
			if (ports[port] == null) return;
			ports[port].Write32(port, data);
		}
	}

}
