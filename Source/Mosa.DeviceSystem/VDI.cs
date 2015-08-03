// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public static class VDI
	{
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

		internal const string HeaderText = "<<< Sun xVM VirtualBox Disk Image >>>";

		#endregion Constants

		/// <summary>
		/// Creates the header.
		/// </summary>
		/// <param name="blocks">The blocks.</param>
		/// <param name="guid">The GUID.</param>
		/// <param name="lastSnapGuid">The last snap GUID.</param>
		/// <param name="diskGeometry">The disk geometry.</param>
		/// <returns></returns>
		static public byte[] CreateHeader(uint blocks, byte[] guid, byte[] lastSnapGuid, DiskGeometry diskGeometry)
		{
			Mosa.ClassLib.BinaryFormat binaryHeader = new Mosa.ClassLib.BinaryFormat(512);

			binaryHeader.SetString(VHIHeaderOffset.HeaderText, HeaderText, (uint)HeaderText.Length);
			binaryHeader.SetByte(VHIHeaderOffset.HeaderText + 0x25, 0x0A);
			binaryHeader.SetUInt(VHIHeaderOffset.ImageSignature, 0xBEda107F);
			binaryHeader.SetUInt(VHIHeaderOffset.Version, 0x00010001);
			binaryHeader.SetUInt(VHIHeaderOffset.HeaderSize, 0x180);
			binaryHeader.SetUInt(VHIHeaderOffset.ImageType, 0x02);
			binaryHeader.SetUInt(VHIHeaderOffset.ImageFlags, 0x00);
			binaryHeader.SetUInt(VHIHeaderOffset.OffsetBlocks, 0x200);
			binaryHeader.SetUInt(VHIHeaderOffset.OffsetData, 0x400);
			binaryHeader.SetUInt(VHIHeaderOffset.DiskGeometryCylinders, 0); // diskGeometry.Cylinders);
			binaryHeader.SetUInt(VHIHeaderOffset.DiskGeometryHeads, 0); // diskGeometry.Heads);
			binaryHeader.SetUInt(VHIHeaderOffset.DiskGeometrySectors, 0); // diskGeometry.SectorsPerTrack);
			binaryHeader.SetUInt(VHIHeaderOffset.SectorSize, 512);
			binaryHeader.SetULong(VHIHeaderOffset.DiskSize, blocks * 512);
			binaryHeader.SetUInt(VHIHeaderOffset.BlockSize, 0x100000);
			binaryHeader.SetUInt(VHIHeaderOffset.BlockExtraData, 0);
			binaryHeader.SetUInt(VHIHeaderOffset.BlocksInHDD, (uint)((blocks * 512) / 0x100000));
			binaryHeader.SetUInt(VHIHeaderOffset.BlocksAllocated, (uint)((blocks * 512) / 0x100000));
			binaryHeader.SetBytes(VHIHeaderOffset.UUID, guid, 0, 16);
			binaryHeader.SetBytes(VHIHeaderOffset.UUIDLastSnap, lastSnapGuid, 0, 16);

			return binaryHeader.Data;
		}

		/// <summary>
		/// Creates the image map.
		/// </summary>
		/// <param name="blocks">The blocks.</param>
		/// <returns></returns>
		static public byte[] CreateImageMap(uint blocks)
		{
			uint size = ((blocks * 512) / 0x100000) * 4;

			uint imageBlocks = (uint)(GetAlignedSize(size) / 512);

			Mosa.ClassLib.BinaryFormat binaryMap = new Mosa.ClassLib.BinaryFormat(imageBlocks * 512);

			for (uint i = 0; i < ((blocks * 512) / 0x100000); i++)
				binaryMap.SetUInt(i * 4, i);

			return binaryMap.Data;
		}

		/// <summary>
		/// Gets the size of the aligned.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		static private ulong GetAlignedSize(ulong size)
		{
			return (size + 511) & ~((ulong)511);
		}
	}
}