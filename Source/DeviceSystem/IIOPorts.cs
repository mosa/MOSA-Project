/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{

	/// <summary>
	/// Interface to IOPort without any read/write permissions
	/// </summary>
	public interface IBaseIOPort
	{
		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
		ushort Address { get; }
	}

	/// <summary>
	/// Interface to IOPort with read only permission
	/// </summary>
	public interface IReadOnlyIOPort : IBaseIOPort
	{
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
	/// Interface to IOPort with write only permission
	/// </summary>
	public interface IWriteOnlyIOPort : IBaseIOPort
	{
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

	/// <summary>
	/// Interface to IOPort with full read/write permissions
	/// </summary>
	public interface IReadWriteIOPort : IBaseIOPort, IReadOnlyIOPort, IWriteOnlyIOPort
	{
	}
}
