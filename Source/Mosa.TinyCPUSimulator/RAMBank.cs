/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator
{
	public class RAMBank
	{
		public ulong Address { get; private set; }

		public ulong Size { get { return (ulong)memory.Length; } }

		public uint Type { get; private set; }

		protected byte[] memory;

		public RAMBank(ulong address, ulong size, uint type)
		{
			Address = address;
			memory = new byte[size];
			Type = type;
		}

		public RAMBank(byte[] memory, ulong address, ulong size, uint type)
		{
			Address = address;
			this.memory = memory;
			Type = type;
		}

		public bool Contains(ulong address)
		{
			return address >= Address && address < (Address + (ulong)memory.Length);
		}

		public byte Read8(ulong address)
		{
			return memory[address - Address];
		}

		public void Write8(ulong address, byte value)
		{
			if ((value != 0) && (memory[address - Address] == value))
				return;

			memory[address - Address] = value;
		}

		public override string ToString()
		{
			return Address.ToString("X") + " - " + (Address + (ulong)memory.Length).ToString("X");
		}
	}
}