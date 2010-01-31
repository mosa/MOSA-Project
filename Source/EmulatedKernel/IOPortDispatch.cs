/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.EmulatedKernel
{    
    /// <summary>
    /// 
    /// </summary>
	public static class IOPortDispatch
	{
		/// <summary>
		/// 
		/// </summary>
		static private IOPortHandler portHandler = new IOPortHandler();

		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <returns></returns>
		public static byte Read8(ushort port) { return portHandler.Read8(port); }

		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <returns></returns>
		public static ushort Read16(ushort port) { return portHandler.Read16(port); }

		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <returns></returns>
		public static uint Read32(ushort port) { return portHandler.Read32(port); }

		/// <summary>
		/// Writes the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="data">The data.</param>
		public static void Write8(ushort port, byte data) { portHandler.Write8(port, data); }

		/// <summary>
		/// Writes the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="data">The data.</param>
		public static void Write16(ushort port, ushort data) { portHandler.Write16(port, data); }

		/// <summary>
		/// Writes the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="data">The data.</param>
		public static void Write32(ushort port, uint data) { portHandler.Write32(port, data); }

		/// <summary>
		/// Registers the IO port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <returns></returns>
		public static IReadWriteIOPort RegisterIOPort(ushort port)
		{
			return new IOPort(port);
		}

		/// <summary>
		/// Registers the device.
		/// </summary>
		/// <param name="device">The device.</param>
		public static void RegisterDevice(IIOPortDevice device)
		{
			portHandler.Register(device);
		}
	}
}
