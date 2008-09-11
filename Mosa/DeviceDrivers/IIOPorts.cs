/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceDrivers
{
    /// <summary>
    /// 
    /// </summary>
	public interface IReadWriteIOPort : IReadOnlyIOPort, IWriteOnlyIOPort
	{
        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>The address.</value>
		new ushort Address { get; }

        /// <summary>
        /// Read8s this instance.
        /// </summary>
        /// <returns></returns>
		new byte Read8();
        /// <summary>
        /// Write8s the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        new void Write8(byte data);
        /// <summary>
        /// Read16s this instance.
        /// </summary>
        /// <returns></returns>
        new ushort Read16();
        /// <summary>
        /// Write16s the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        new void Write16(ushort data);
        /// <summary>
        /// Read32s this instance.
        /// </summary>
        /// <returns></returns>
        new uint Read32();
        /// <summary>
        /// Write32s the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        new void Write32(uint data);
	}

    /// <summary>
    /// 
    /// </summary>
	public interface IReadOnlyIOPort
	{
        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>The address.</value>
        ushort Address { get; }
        /// <summary>
        /// Read8s this instance.
        /// </summary>
        /// <returns></returns>
        byte Read8();
        /// <summary>
        /// Read16s this instance.
        /// </summary>
        /// <returns></returns>
        ushort Read16();
        /// <summary>
        /// Read32s this instance.
        /// </summary>
        /// <returns></returns>
        uint Read32();
	}

    /// <summary>
    /// 
    /// </summary>
	public interface IWriteOnlyIOPort
	{
        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>The address.</value>
        ushort Address { get; }
        /// <summary>
        /// Write8s the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        void Write8(byte data);
        /// <summary>
        /// Write16s the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        void Write16(ushort data);
        /// <summary>
        /// Write32s the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        void Write32(uint data);
	}


}
