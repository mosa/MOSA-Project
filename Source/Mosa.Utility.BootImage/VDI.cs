// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.Utility.BootImage
{
	public static class VDI
	{
		// FIXME: Header creation (offsets and values)

		#region Constants

		internal struct VHIHeaderOffset
		{
			internal const uint HeaderText = 0x0; // 64
			internal const uint ImageSignature = 0x40; // 4
			internal const uint Version = 0x44; // 4
			internal const uint HeaderSize = 0x48; // 4
			internal const uint ImageType = 0x4C; // 4
			internal const uint ImageFlags = 0x50; // 4
			internal const uint ImageDescription = 0x54; // 16
			internal const uint OffsetBlocks = 0x154; // 4
			internal const uint OffsetData = 0x158; // 4
			internal const uint DiskGeometryCylinders = 0x15C; //4
			internal const uint DiskGeometryHeads = 0x160; // 4
			internal const uint DiskGeometrySectors = 0x164; // 4
			internal const uint SectorSize = 0x168; // 4
			internal const uint DiskSize = 0x170; // 8
			internal const uint BlockSize = 0x178; // 4
			internal const uint BlockExtraData = 0x17C; // 4
			internal const uint BlocksInHDD = 0x180; // 4
			internal const uint BlocksAllocated = 0x184; // 4
			internal const uint UUID = 0x188; // 16
			internal const uint UUIDLastSnap = 0x198; // 16
			internal const uint UUIDLink = 0x1A8; // 16
			internal const uint UUIDParent = 0x1B8; // 16
		}

		internal const string HeaderText = "<<< QEMU VM Virtual Disk Image >>>";

		#endregion Constants

		/// <summary>
		/// Creates the header.
		/// </summary>
		/// <param name="blocks">The blocks.</param>
		/// <param name="guid">The GUID.</param>
		/// <param name="lastSnapGuid">The last snap GUID.</param>
		/// <param name="diskGeometry">The disk geometry.</param>
		/// <returns></returns>
		public static byte[] CreateHeader(uint blocks, byte[] guid, byte[] lastSnapGuid, DiskGeometry diskGeometry)
		{
			var binaryHeader = new DataBlock(512);
			var textLength = (uint)HeaderText.Length;

			//https://forums.virtualbox.org/viewtopic.php?t=8046
			// Also some RE :)
			binaryHeader.SetString(VHIHeaderOffset.HeaderText, HeaderText, textLength);
			binaryHeader.SetByte(VHIHeaderOffset.HeaderText + textLength, 0x0A);
			binaryHeader.SetUInt32(VHIHeaderOffset.ImageSignature, 0xBEDA107F);
			binaryHeader.SetUInt32(VHIHeaderOffset.Version, 0x00010001); // 1.1
			binaryHeader.SetUInt32(VHIHeaderOffset.HeaderSize, 0x190);
			binaryHeader.SetUInt32(VHIHeaderOffset.ImageType, 0x01); // Dynamic VDI
			binaryHeader.SetUInt32(VHIHeaderOffset.ImageFlags, 0x00);
			binaryHeader.SetUInt32(VHIHeaderOffset.OffsetBlocks, 0x200);
			binaryHeader.SetUInt32(VHIHeaderOffset.OffsetData, 0x400);
			binaryHeader.SetUInt32(VHIHeaderOffset.SectorSize, 512);
			binaryHeader.SetULong(VHIHeaderOffset.DiskSize, blocks * 512);
			binaryHeader.SetUInt32(VHIHeaderOffset.BlockSize, 0x100000);
			binaryHeader.SetUInt32(VHIHeaderOffset.BlocksInHDD, blocks * 512 / 0x100000);
			binaryHeader.SetUInt32(VHIHeaderOffset.BlocksAllocated, blocks * 512 / 0x100000);
			binaryHeader.SetBytes(VHIHeaderOffset.UUID, guid, 0, 16);
			binaryHeader.SetBytes(VHIHeaderOffset.UUIDLastSnap, lastSnapGuid, 0, 16);

			return binaryHeader.Data;
		}

		/// <summary>
		/// Creates the image map.
		/// </summary>
		/// <param name="blocks">The blocks.</param>
		/// <returns></returns>
		public static byte[] CreateImageMap(uint blocks)
		{
			var size = blocks * 512 / 0x100000 * 4;
			var imageBlocks = (uint)(GetAlignedSize(size) / 512);
			var binaryMap = new DataBlock(imageBlocks * 512);

			for (uint i = 0; i < size / 4; i++)
				binaryMap.SetUInt32(i * 4, i);

			return binaryMap.Data;
		}

		private static ulong GetAlignedSize(ulong size)
		{
			return (size + 511) & ~(ulong)511;
		}
	}
}
