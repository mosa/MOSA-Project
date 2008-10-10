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
	#region Definitions

	internal struct MasterBootRecord
	{
		internal const uint CodeArea = 0x00;
		internal const uint DiskSignature = 0x01B8;
		internal const uint PrimaryPartitions = 0x01BE;
		internal const uint MBRSignature = 0x01FE;
	}

	internal struct MasterBootConstants
	{
		internal const ushort MBRSignature = 0xAA55;
		internal const byte BootableIndicator = 0x00;
		internal const ushort CodeAreaSize = 446;
	}

	internal struct PartitionRecord
	{
		internal const uint Status = 0x00; // 1
		internal const uint FirstCRS = 0x01; // 3
		internal const uint PartitionType = 0x04;	// 1
		internal const uint LastCRS = 0x05; // 3
		internal const uint LBA = 0x08; // 4
		internal const uint Sectors = 0x0C; // 4
	}

	#endregion

    /// <summary>
    /// 
    /// </summary>
	public class MasterBootBlock
	{
        /// <summary>
        /// 
        /// </summary>
		public const uint MaxMBRPartitions = 4;

        /// <summary>
        /// 
        /// </summary>
		private IDiskDevice diskDevice;
        /// <summary>
        /// 
        /// </summary>
		private GenericPartition[] partitions;
        /// <summary>
        /// 
        /// </summary>
		private uint diskSignature;
        /// <summary>
        /// 
        /// </summary>
		private bool valid;
        /// <summary>
        /// 
        /// </summary>
		private byte[] code;

        /// <summary>
        /// Gets a value indicating whether this <see cref="MasterBootBlock"/> is valid.
        /// </summary>
        /// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
		public bool Valid
		{
			get { return valid; }
		}

        /// <summary>
        /// Gets the disk signature.
        /// </summary>
        /// <value>The disk signature.</value>
		public uint DiskSignature
		{
			get { return diskSignature; }
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterBootBlock"/> class.
        /// </summary>
        /// <param name="diskDevice">The disk device.</param>
		public MasterBootBlock(IDiskDevice diskDevice)
		{
			this.diskDevice = diskDevice;
			this.valid = false;	// needs to be read first
			partitions = new GenericPartition[MaxMBRPartitions];
			code = null;
			ReadMasterBootBlock();
		}

        /// <summary>
        /// Reads the master boot block.
        /// </summary>
        /// <returns></returns>
		private bool ReadMasterBootBlock()
		{
			valid = false;

			if (diskDevice.BlockSize != 512) { return false; } // only going to work with 512 sector sizes

			BinaryFormat masterboot = new BinaryFormat(diskDevice.ReadBlock(0, 1));

			ushort mbrsignature = masterboot.GetUShort(MasterBootRecord.MBRSignature);
			diskSignature = masterboot.GetUInt(MasterBootRecord.DiskSignature);

			valid = (mbrsignature == MasterBootConstants.MBRSignature);

			if (valid) {
				for (uint index = 0; index < MaxMBRPartitions; index++) {
					uint offset = MasterBootRecord.PrimaryPartitions + (index * 16);

					GenericPartition partition = new GenericPartition(index);

					partition.Bootable = masterboot.GetByte(offset + PartitionRecord.Status) == 0x80;
					partition.PartitionType = masterboot.GetByte(offset + PartitionRecord.PartitionType);
					partition.StartLBA = masterboot.GetUInt(offset + PartitionRecord.LBA);
					partition.TotalBlocks = masterboot.GetUInt(offset + PartitionRecord.Sectors);

					partitions[index] = partition;
				}

				//TODO: Extended Partitions
			}

			code = new byte[MasterBootConstants.CodeAreaSize];
			for (uint index = 0; index < MasterBootConstants.CodeAreaSize; index++) { code[index] = masterboot.GetByte(index); }

			return valid;
		}

        /// <summary>
        /// Gets the <see cref="Mosa.DeviceSystem.GenericPartition"/> with the specified partition NBR.
        /// </summary>
        /// <value></value>
		public GenericPartition this[uint partitionNbr]
		{
			get
			{
				if (partitionNbr >= MaxMBRPartitions) { return null; } // TODO: or throw exception?

				return partitions[partitionNbr];
			}
		}

        /// <summary>
        /// Formats this instance.
        /// </summary>
        /// <returns></returns>
		public bool Format()
		{
			if (!diskDevice.CanWrite) { return false; }

			BinaryFormat masterboot = new BinaryFormat(new byte[512]);

			masterboot.SetUInt(MasterBootRecord.DiskSignature, diskSignature);
			masterboot.SetUShort(MasterBootRecord.MBRSignature, MasterBootConstants.MBRSignature);

			if (code != null) {
				for (uint index = 0; index < MasterBootConstants.CodeAreaSize; index++) {
					masterboot.SetByte(index, code[index]);
				}
			}

			//TODO: write partitions

			diskDevice.WriteBlock(0, 1, masterboot.Data);

			return true;
		}

		//public void Dump ()
		//{
		//    for (uint index = 0; index < MaxMBRPartitions; index++) {

		//        GenericPartition partition = partitions[index];

		//        if (partition.PartitionType != PartitionTypes.Empty) {

		//            TextMode.Write ("partition #", (int)index);
		//            TextMode.Write (" found, lba=", (int)partition.StartLBA);
		//            TextMode.Write (", count=", (int)partition.TotalBlocks);
		//            TextMode.Write (", type=", (int)partition.PartitionType);

		//            // for testing
		//            //if (FAT.FileSystem.IsFat (partition.BlockDevice))
		//            //    TextMode.Write (" (fat)");
		//            //else
		//            TextMode.Write (" (unknown)");
		//            TextMode.WriteLine ();
		//        }
		//    }
		//}
	}
}
