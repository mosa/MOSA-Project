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
			internal const uint DiskGeometryCylinders = 56; // 2
			internal const uint DiskGeometryHeads = 58; // 1
			internal const uint DiskGeometrySectors = 59; // 1			
			internal const uint DiskType = 60; // 4
			internal const uint Checksum = 64; // 4
			internal const uint UniqueId = 68; // 16
			internal const uint SavedState = 84; // 1
			internal const uint Reserved = 85; // 427
		}

		#endregion

		public struct CHS
		{
			public ushort Cylinders;
			public byte Heads;
			public byte Sectors;
		}

		static CHS ComputeCHS(ulong totalSectors)
		{
			uint cylinderTimesHeads;

			CHS chs = new CHS();

			if (totalSectors > 65535 * 16 * 255)
				totalSectors = 65535 * 16 * 255;

			if (totalSectors >= 65535 * 16 * 63) {
				chs.Sectors = 255;
				chs.Heads = 16;
				cylinderTimesHeads = (uint) (totalSectors / chs.Sectors);
			}
			else {
				chs.Sectors = 17;
				cylinderTimesHeads = (uint)(totalSectors / chs.Sectors);

				chs.Heads = (byte)((cylinderTimesHeads + 1023) / 1024);

				if (chs.Heads < 4)
					chs.Heads = 4;

				if (cylinderTimesHeads >= (chs.Heads * 1024) || chs.Heads > 16) {
					chs.Sectors = 31;
					chs.Heads = 16;
					cylinderTimesHeads = (uint)(totalSectors / chs.Sectors);
				}

				if (cylinderTimesHeads >= (chs.Heads * 1024)) {
					chs.Sectors = 63;
					chs.Heads = 16;
					cylinderTimesHeads = (uint)(totalSectors / chs.Sectors);
				}
			}

			chs.Cylinders = (ushort)(cylinderTimesHeads / chs.Heads);

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
			binaryFooter.SetUIntReversed(VHDFooterOffset.Features, 0x00000002);
			binaryFooter.SetUIntReversed(VHDFooterOffset.FileFormatVersion, 0x00010000);
			binaryFooter.SetULong(VHDFooterOffset.DataOffset, ~(ulong)0);
			binaryFooter.SetUIntReversed(VHDFooterOffset.TimeStamp, (uint)(DateTime.Now.Second - (new DateTime(2000, 1, 1, 0, 0, 0)).Second));
			binaryFooter.SetString(VHDFooterOffset.CreatorApplication, "MOSA");
			binaryFooter.SetUIntReversed(VHDFooterOffset.CreatorVersion, 0x00010000);
			binaryFooter.SetUIntReversed(VHDFooterOffset.CreatorHostOS, 0x5769326B); // Windows
			binaryFooter.SetULongReversed(VHDFooterOffset.OriginalSize, alignedSize);
			binaryFooter.SetULongReversed(VHDFooterOffset.CurrentSize, alignedSize);

			CHS chs = VHD.ComputeCHS(alignedSize);
			binaryFooter.SetUShortReversed(VHDFooterOffset.DiskGeometryCylinders, chs.Cylinders);
			binaryFooter.SetByte(VHDFooterOffset.DiskGeometryHeads, chs.Heads);
			binaryFooter.SetByte(VHDFooterOffset.DiskGeometrySectors, chs.Sectors);

			binaryFooter.SetUIntReversed(VHDFooterOffset.DiskType, 0x02); // Fixed disk
			binaryFooter.SetUIntReversed(VHDFooterOffset.Checksum, 0x00);
			binaryFooter.SetBytes(VHDFooterOffset.UniqueId, (new Guid()).ToByteArray(), 0, 16);
			binaryFooter.SetByte(VHDFooterOffset.SavedState, 0x00); // No saved state

			uint checksum = 0;
			for (uint index = 0; index < 512; index++)
				checksum += footer[index];

			binaryFooter.SetUIntReversed(VHDFooterOffset.Checksum, ~checksum);

			return footer;
		}

	}
}
