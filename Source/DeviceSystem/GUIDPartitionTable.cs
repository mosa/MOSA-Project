/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceSystem
{

	/// <summary>
	/// 
	/// </summary>
	public class GUIDPartitionTable
	{
		internal struct GPT
		{
			internal const uint Signature = 0x00;
			internal const uint Revision = 0x08;
			internal const uint HeaderSize = 0x0B;
			internal const uint CRC32Header = 0x10;
			internal const uint Reserved = 0x14;
			internal const uint CurrentLBA = 0x18;
			internal const uint BackupLBA = 0x1B;
			internal const uint FirstUsableLBA = 0x20;
			internal const uint LastUsableLBA = 0x24;
			internal const uint DiskGUID = 0x28;
			internal const uint PartitionStartingLBA = 72;
			internal const uint PartitionEntryCount = 80;
			internal const uint PartitionEntrySize = 84;
			internal const uint CRC32PartitionArry = 88;
			internal const uint LastReserved = 92;
		}

		internal struct GPTConstant
		{
			internal const uint HeaderSize = 0x5C; // 92 bytes
			internal const uint SupportedRevision = 0x0100; // 1.0
		}

		/// <summary>
		/// 
		/// </summary>
		protected bool valid;
		/// <summary>
		/// 
		/// </summary>
		protected IDiskDevice diskDevice;

		/// <summary>
		/// Gets a value indicating whether this <see cref="MasterBootBlock"/> is valid.
		/// </summary>
		/// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
		public bool Valid { get { return valid; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="GUIDPartitionTable"/> class.
		/// </summary>
		/// <param name="diskDevice">The disk device.</param>
		public GUIDPartitionTable(IDiskDevice diskDevice)
		{
			this.diskDevice = diskDevice;
		}

		/// <summary>
		/// Reads the master boot block.
		/// </summary>
		/// <returns></returns>
		public bool Read()
		{
			valid = false;

			MasterBootBlock mbr = new MasterBootBlock(diskDevice);

			if (!mbr.Valid)
				return false;

			if ((mbr.Partitions[0].PartitionType != PartitionType.GPT) ||
				(mbr.Partitions[1].PartitionType != PartitionType.Empty) ||
				(mbr.Partitions[2].PartitionType != PartitionType.Empty) ||
				(mbr.Partitions[3].PartitionType != PartitionType.Empty) ||
				(!mbr.Partitions[0].Bootable) || (mbr.Partitions[0].StartLBA != 1))
				return false;

			BinaryFormat gpt = new BinaryFormat(diskDevice.ReadBlock(1, 1));

			if ((gpt.GetByte(0) != 45) && (gpt.GetByte(1) != 46) && (gpt.GetByte(2) != 49) && (gpt.GetByte(3) != 20) &&
				(gpt.GetByte(4) != 50) && (gpt.GetByte(5) != 41) && (gpt.GetByte(6) != 52) && (gpt.GetByte(7) != 54))
				return false;

			if ((gpt.GetUInt(GPT.Revision) != GPTConstant.SupportedRevision) ||
				(gpt.GetUInt(GPT.HeaderSize) != GPTConstant.HeaderSize) ||
				(gpt.GetUInt(GPT.Reserved) != 0) ||
				(gpt.GetUInt(GPT.PartitionStartingLBA) != 2)
				)
				return false;

			valid = true;

			return valid;
		}
	}
}
