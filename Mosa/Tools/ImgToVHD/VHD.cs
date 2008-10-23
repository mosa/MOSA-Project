using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.ImgToVHD
{
	static class VHD
	{
		#region Constants

		internal struct VHDFooterOffset
		{
			internal const uint Cookie = 0; // 8
			internal const uint Features = 8; // 4
			internal const uint FileFormatVersion = 12; // 4
			internal const uint DataOffset = 16; // 8
			internal const uint TimeStamp = 24; // 4
			internal const uint CreatorApplication = 28; // 4
			internal const uint CreatorVersion = 32; // 4
			internal const uint CreatorHostOS = 36; // 4
			internal const uint OriginalSize = 40; // 8
			internal const uint CurrentSize = 48; // 8
			internal const uint DiskGeometry = 56; // 4
			internal const uint DiskType = 60; // 4
			internal const uint Checksum = 64; // 4
			internal const uint UniqueId = 68; // 16
			internal const uint SavedState = 84; // 1
			internal const uint Reserved = 85; // 427
		}

		#endregion

		public struct CHS
		{
			public uint Cylinders;
			public uint Heads;
			public uint Sectors;
		}

		static CHS ComputeCHS(ulong totalSectors)
		{
			ulong cylinderTimesHeads;

			CHS chs = new CHS();

			if (totalSectors > 65535 * 16 * 255)
				totalSectors = 65535 * 16 * 255;


			if (totalSectors >= 65535 * 16 * 63) {
				chs.Sectors = 255;
				chs.Heads = 16;
				cylinderTimesHeads = totalSectors / chs.Sectors;
			}
			else {
				chs.Sectors = 17;
				cylinderTimesHeads = totalSectors / chs.Sectors;

				chs.Heads = (uint)(cylinderTimesHeads + 1023) / 1024;

				if (chs.Heads < 4)
					chs.Heads = 4;

				if (cylinderTimesHeads >= (chs.Heads * 1024) || chs.Heads > 16) {
					chs.Sectors = 31;
					chs.Heads = 16;
					cylinderTimesHeads = totalSectors / chs.Sectors;
				}

				if (cylinderTimesHeads >= (chs.Heads * 1024)) {
					chs.Sectors = 63;
					chs.Heads = 16;
					cylinderTimesHeads = totalSectors / chs.Sectors;
				}
			}

			chs.Cylinders = (uint)cylinderTimesHeads / chs.Heads;

			return chs;
		}

		static private ulong GetAlignedSize(ulong size)
		{
			return (size + 511) & ~((ulong)511);
		}

		static public uint GetAlignmentPadding(ulong size)
		{
			return (uint)(GetAlignedSize(size) - size);
		}

		static public byte[] CreateFooter(ulong size)
		{
			byte[] footer = new byte[512];
			ulong alignedSize = GetAlignedSize(size);

			Mosa.ClassLib.BinaryFormat binaryFooter = new Mosa.ClassLib.BinaryFormat(footer);

			binaryFooter.SetString(VHDFooterOffset.Cookie, "conectix");
			binaryFooter.SetUInt(VHDFooterOffset.Features, 0x00000002);
			binaryFooter.SetUInt(VHDFooterOffset.FileFormatVersion, 0x00010000);
			binaryFooter.SetULong(VHDFooterOffset.DataOffset, ~(ulong)0);
			binaryFooter.SetUInt(VHDFooterOffset.TimeStamp, (uint)(DateTime.Now.Second - (new DateTime(2000, 1, 1, 0, 0, 0)).Second));
			binaryFooter.SetString(VHDFooterOffset.CreatorApplication, "MOSA");
			binaryFooter.SetUInt(VHDFooterOffset.CreatorVersion, 0x00050000);
			binaryFooter.SetUInt(VHDFooterOffset.CreatorHostOS, 0x5769326B); // 0x5769326B = Windows
			binaryFooter.SetULong(VHDFooterOffset.OriginalSize, alignedSize);
			binaryFooter.SetULong(VHDFooterOffset.CurrentSize, alignedSize);
			binaryFooter.SetUInt(VHDFooterOffset.DiskType, 0x02); // Fixed disk
			binaryFooter.SetUInt(VHDFooterOffset.SavedState, 0x00); // No saved state
			binaryFooter.SetUInt(VHDFooterOffset.Checksum, 0x00);
			binaryFooter.SetBytes(VHDFooterOffset.UniqueId, (new Guid()).ToByteArray(), 0, 16);

			uint checksum = 0;
			for (uint index = 0; index < 512; index++)
				checksum += footer[index];

			binaryFooter.SetUInt(VHDFooterOffset.Checksum, ~checksum);

			return footer;
		}

	}
}
