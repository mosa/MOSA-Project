/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;

namespace Mosa.EmulatedKernel
{
    /// <summary>
    /// 
    /// </summary>
	public class EmulatedIOPort : IReadWriteIOPort
	{
        /// <summary>
        /// 
        /// </summary>
		protected ushort port;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
		public EmulatedIOPort(ushort port) { this.port = port; }

        /// <summary>
        /// 
        /// </summary>
		public ushort Address { get { return port; } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public byte Read8() { return IOPortDispatch.Read8(port); }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public ushort Read16() { return IOPortDispatch.Read16(port); }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public uint Read32() { return IOPortDispatch.Read32(port); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
		public void Write8(byte data) { IOPortDispatch.Write8(port, data); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
		public void Write16(ushort data) { IOPortDispatch.Write16(port, data); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
		public void Write32(uint data) { IOPortDispatch.Write32(port, data); }
	}
}
