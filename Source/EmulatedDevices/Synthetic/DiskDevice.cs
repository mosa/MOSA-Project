/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;
using Mosa.ClassLib;
using Mosa.DeviceSystem;

namespace Mosa.EmulatedDevices.Synthetic
{
	/// <summary>
	/// Emulates a disk device
	/// </summary>
	public class DiskDevice : Device, IDiskDevice
	{
		/// <summary>
		/// 
		/// </summary>
		protected FileStream diskFile;

		/// <summary>
		/// 
		/// </summary>
		public uint BlockOffset = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="DiskDevice"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public DiskDevice(string filename)
		{
			base.name = "DiskDevice_" + Path.GetFileName(filename);
			base.parent = null;
			base.deviceStatus = DeviceStatus.Online;

			diskFile = new FileStream(filename, FileMode.OpenOrCreate);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		public void Dispose()
		{
			diskFile.Close();
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
		public uint TotalBlocks { get { return (uint)(diskFile.Length / 512); } }

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
			byte[] data = new byte[count  * 512];
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
			diskFile.Seek((block + BlockOffset) * 512, SeekOrigin.Begin);
			diskFile.Read(data, 0, (int)(count*512));
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
			diskFile.Seek((block + BlockOffset) * 512, SeekOrigin.Begin);
			diskFile.Write(data, 0, (int)(count*512));
			return true;
		}
	}
}
