// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.ClassLib;

namespace Mosa.DeviceSystem
{
	#region Definitions

	internal struct MBR
	{
		internal const uint CodeArea = 0x00;
		internal const uint DiskSignature = 0x01B8;
		internal const uint FirstPartition = 0x01BE;
		internal const uint MBRSignature = 0x01FE;
	}

	internal struct MBRConstant
	{
		internal const ushort MBRSignature = 0xAA55;
		internal const byte Bootable = 0x80;
		internal const ushort CodeAreaSize = 446;
		internal const byte PartitionSize = 16;
	}

	internal struct PartitionRecord
	{
		internal const uint Status = 0x00; // 1
		internal const uint FirstCRS = 0x01; // 3
		internal const uint PartitionType = 0x04;   // 1
		internal const uint LastCRS = 0x05; // 3
		internal const uint LBA = 0x08; // 4
		internal const uint Sectors = 0x0C; // 4
	}

	#endregion Definitions

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
		protected IDiskDevice diskDevice;

		/// <summary>
		///
		/// </summary>
		public GenericPartition[] Partitions;

		/// <summary>
		///
		/// </summary>
		protected uint diskSignature;

		/// <summary>
		///
		/// </summary>
		protected bool valid;

		/// <summary>
		///
		/// </summary>
		protected byte[] code;

		/// <summary>
		/// Gets a value indicating whether this <see cref="MasterBootBlock"/> is valid.
		/// </summary>
		/// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
		public bool Valid { get { return valid; } }

		/// <summary>
		/// Gets the disk signature.
		/// </summary>
		/// <value>The disk signature.</value>
		public uint DiskSignature { get { return diskSignature; } set { diskSignature = value; } }

		/// <summary>
		/// Gets or sets the code.
		/// </summary>
		/// <value>The code.</value>
		public byte[] Code
		{
			get
			{
				if (code == null) return null;

				byte[] copy = new byte[code.Length];

				for (int i = 0; i < code.Length; i++)
					copy[i] = code[i];

				return copy;
			}
			set
			{
				if (value == null)
				{
					code = null;
					return;
				}

				code = new byte[value.Length];

				for (int i = 0; i < value.Length; i++)
					code[i] = value[i];
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MasterBootBlock"/> class.
		/// </summary>
		/// <param name="diskDevice">The disk device.</param>
		public MasterBootBlock(IDiskDevice diskDevice)
		{
			this.diskDevice = diskDevice;
			Partitions = new GenericPartition[MaxMBRPartitions];
			for (uint i = 0; i < MaxMBRPartitions; i++)
				Partitions[i] = new GenericPartition(i);
			Read();
		}

		/// <summary>
		/// Reads the master boot block.
		/// </summary>
		/// <returns></returns>
		public bool Read()
		{
			valid = false;

			if (diskDevice.BlockSize != 512) return false;  // only going to work with 512 sector sizes
			if (diskDevice.TotalBlocks < 3) return false;

			BinaryFormat masterboot = new BinaryFormat(diskDevice.ReadBlock(0, 1));

			if (masterboot.GetUShort(MBR.MBRSignature) != MBRConstant.MBRSignature)
				return false;

			valid = true;

			diskSignature = masterboot.GetUInt(MBR.DiskSignature);

			for (uint index = 0; index < MaxMBRPartitions; index++)
			{
				uint offset = MBR.FirstPartition + (index * MBRConstant.PartitionSize);

				GenericPartition partition = new GenericPartition(index);

				partition.Bootable = masterboot.GetByte(offset + PartitionRecord.Status) == MBRConstant.Bootable;
				partition.PartitionType = masterboot.GetByte(offset + PartitionRecord.PartitionType);
				partition.StartLBA = masterboot.GetUInt(offset + PartitionRecord.LBA);
				partition.TotalBlocks = masterboot.GetUInt(offset + PartitionRecord.Sectors);

				Partitions[index] = partition;
			}

			//TODO: Extended Partitions

			code = new byte[MBRConstant.CodeAreaSize];
			for (uint index = 0; index < MBRConstant.CodeAreaSize; index++)
				code[index] = masterboot.GetByte(index);

			return valid;
		}

		/// <summary>
		/// Writes the master boot block.
		/// </summary>
		/// <returns></returns>
		public bool Write()
		{
			if (!diskDevice.CanWrite) { return false; }

			BinaryFormat masterboot = new BinaryFormat(new byte[512]);

			masterboot.SetUInt(MBR.DiskSignature, diskSignature);
			masterboot.SetUShort(MBR.MBRSignature, MBRConstant.MBRSignature);

			if (code != null)
				for (uint index = 0; ((index < MBRConstant.CodeAreaSize) && (index < code.Length)); index++)
					masterboot.SetByte(index, code[index]);

			for (uint index = 0; index < MaxMBRPartitions; index++)
				if (Partitions[index].TotalBlocks != 0)
				{
					uint offset = MBR.FirstPartition + (index * 16);
					masterboot.SetByte(offset + PartitionRecord.Status, (byte)(Partitions[index].Bootable ? 0x80 : 0x00));
					masterboot.SetByte(offset + PartitionRecord.PartitionType, Partitions[index].PartitionType);
					masterboot.SetUInt(offset + PartitionRecord.LBA, Partitions[index].StartLBA);
					masterboot.SetUInt(offset + PartitionRecord.Sectors, Partitions[index].TotalBlocks);

					DiskGeometry diskGeometry = new DiskGeometry();
					diskGeometry.GuessGeometry(diskDevice.TotalBlocks);

					CHS chsStart = new CHS();
					CHS chsEnd = new CHS();

					chsStart.SetCHS(diskGeometry, Partitions[index].StartLBA);
					chsEnd.SetCHS(diskGeometry, Partitions[index].StartLBA + Partitions[index].TotalBlocks - 1);

					masterboot.SetByte(offset + PartitionRecord.FirstCRS, chsStart.Head);
					masterboot.SetByte(offset + PartitionRecord.FirstCRS + 1, (byte)((chsStart.Sector & 0x3F) | ((chsStart.Cylinder >> 8) & 0x03)));
					masterboot.SetByte(offset + PartitionRecord.FirstCRS + 2, (byte)(chsStart.Cylinder & 0xFF));
					masterboot.SetByte(offset + PartitionRecord.LastCRS, chsEnd.Head);
					masterboot.SetByte(offset + PartitionRecord.LastCRS + 1, (byte)((chsEnd.Sector & 0x3F) | ((chsEnd.Cylinder >> 8) & 0x03)));
					masterboot.SetByte(offset + PartitionRecord.LastCRS + 2, (byte)(chsEnd.Cylinder & 0xFF));
				}

			diskDevice.WriteBlock(0, 1, masterboot.Data);

			return true;
		}
	}
}
