/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.EmulatedKernel;

namespace Mosa.EmulatedDevices.Emulated
{

	/// <summary>
	/// Emulates the RAM Chip 
	/// </summary>
	public class RAMChip : IHardwareDevice
	{

		/// <summary>
		/// 
		/// </summary>
		protected uint addressBase = 0xB0000;
		/// <summary>
		/// 
		/// </summary>
		protected byte[] memory;

		/// <summary>
		/// Initializes a new instance of the <see cref="RAMChip"/> class.
		/// </summary>
		public RAMChip(uint addressBase, uint size)
		{
			this.addressBase = addressBase;
			memory = new byte[size];
			MemoryDispatch.RegisterMemory(addressBase, size, 1, Read8, Write8);
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		/// <returns></returns>
		public bool Initialize()
		{
			return true;
		}

		/// <summary>
		/// Resets this instance.
		/// </summary>
		public void Reset()
		{
		}

		/// <summary>
		/// Reads at the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		protected virtual byte Read8(uint address)
		{
			return memory[address - addressBase];
		}

		/// <summary>
		/// Writes at the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="value">The value.</param>
		protected virtual void Write8(uint address, byte value)
		{
			if ((value != 0) && (memory[address - addressBase] == value))
				return;

			memory[address - addressBase] = value;
		}
	}
}
