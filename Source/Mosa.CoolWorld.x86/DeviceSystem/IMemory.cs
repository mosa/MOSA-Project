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
	/// Interface to a region of memory
	/// </summary>
	public interface IMemory
	{
		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
		uint Address { get; }

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		uint Size { get; }

		/// <summary>
		/// Gets or sets the <see cref="System.Byte"/> at the specified index.
		/// </summary>
		/// <value></value>
		byte this[uint index] { get; set; }

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		byte Read8(uint index);

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		void Write8(uint index, byte value);

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		ushort Read16(uint index);

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		void Write16(uint index, ushort value);

		/// <summary>
		/// Reads the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		uint Read32(uint index);

		/// <summary>
		/// Writes the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		void Write32(uint index, uint value);

	}

}
