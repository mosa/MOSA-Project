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
	#region PartitionTypes

	public struct PartitionTypes
	{
		public const byte GPT = 0xEE;
		public const byte Empty = 0x00;
		public const byte ExtendedPartition = 0x0F;
		public const byte OldExtendedPartition = 0x05; // limited to disks under 8.4Gb
		public const byte Fat12 = 0x01;
		public const byte Fat16 = 0x04;
		public const byte Fat32 = 0x0B;
	}

	#endregion

	public class GenericPartition
	{
		private bool bootable;
		private uint startLBA;
		private uint totalBlocks;
		private byte type;
		private uint index;
		private uint[] guid;	// for Guid Partition Table (GPT)

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericPartition"/> class.
		/// </summary>
		/// <param name="index">The index.</param>
		public GenericPartition(uint index) { this.index = index; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="GenericPartition"/> is bootable.
		/// </summary>
		/// <value><c>true</c> if bootable; otherwise, <c>false</c>.</value>
		public bool Bootable
		{
			get { return bootable; }
			set { bootable = value; }
		}

		/// <summary>
		/// Gets the partition index.
		/// </summary>
		/// <value>The partition index.</value>
		public uint Index
		{
			get { return index; }
		}

		/// <summary>
		/// Gets or sets the start LBA.
		/// </summary>
		/// <value>The start LBA.</value>
		public uint StartLBA
		{
			get { return startLBA; }
			set { startLBA = value; }
		}

		/// <summary>
		/// Gets the end LBA.
		/// </summary>
		/// <value>The end LBA.</value>
		public uint EndLBA
		{
			get { return startLBA + totalBlocks; }
		}

		/// <summary>
		/// Gets or sets the total blocks.
		/// </summary>
		/// <value>The total blocks.</value>
		public uint TotalBlocks
		{
			get { return totalBlocks; }
			set { totalBlocks = value; }
		}

		/// <summary>
		/// Gets or sets the type of the partition.
		/// </summary>
		/// <value>The type of the partition.</value>
		public byte PartitionType
		{
			get { return type; }
			set { type = value; }
		}

		/// <summary>
		/// Gets or sets the GUID.
		/// </summary>
		/// <value>The GUID.</value>
		public uint[] GUID
		{
			get { return guid; }
			set { guid = value; }
		}
	}
}
