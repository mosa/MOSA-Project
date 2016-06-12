/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.EmulatedDevices.Synthetic
{
	/// <summary>
	/// Emulates a ram disk device
	/// </summary>
	public class RamDiskDevice : Device, IDiskDevice
	{
		/// <summary>
		///
		/// </summary>
		protected uint totalBlocks;

		/// <summary>
		///
		/// </summary>
		protected byte[] mem;

		/// <summary>
		/// Initializes a new instance of the <see cref="RamDiskDevice"/> class.
		/// </summary>
		/// <param name="blocks">The blocks.</param>
		public RamDiskDevice(uint blocks)
		{
			base.name = "RamDiskDevice_" + ((blocks * 512) / (1024 * 1024)).ToString() + "Mb";
			base.parent = null;
			base.deviceStatus = DeviceStatus.Online;
			this.totalBlocks = blocks;
			this.mem = new byte[blocks * 512];
		}

		/// <summary>
		/// Gets a value indicating whether this instance can write.
		/// </summary>
		/// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
		public bool CanWrite { get { return true; } }

		/// <summary>
		/// Gets the total blocks.
		/// </summary>
		/// <value>The total blocks.</value>
		public uint TotalBlocks { get { return totalBlocks; } }

		/// <summary>
		/// Gets the size of the block.
		/// </summary>
		/// <value>The size of the block.</value>
		public uint BlockSize { get { return 512; } }

		/// <summary>
		/// Reads the block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public byte[] ReadBlock(uint block, uint count)
		{
			byte[] data = new byte[512];
			ReadBlock(block, count, data);
			return data;
		}

		/// <summary>
		/// Reads the block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="count">The count.</param>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public bool ReadBlock(uint block, uint count, byte[] data)
		{
			for (int i = 0; i < 512; i++)
				data[i] = mem[(block * 512) + i];
			return true;
		}

		/// <summary>
		/// Writes the block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="count">The count.</param>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public bool WriteBlock(uint block, uint count, byte[] data)
		{
			for (int i = 0; i < 512; i++)
				mem[(block * 512) + i] = data[i];
			return true;
		}
	}
}