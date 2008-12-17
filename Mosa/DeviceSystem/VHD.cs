/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// 
	/// </summary>
	public static class VHD
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

		/// <summary>
		/// Gets the size of the aligned.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		static private ulong GetAlignedSize(ulong size)
		{
			return (size + 511) & ~((ulong)511);
		}

		/// <summary>
		/// Gets the alignment padding.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		static public uint GetAlignmentPadding(ulong size)
		{
			return (uint)(GetAlignedSize(size) - size);
		}

		/// <summary>
		/// Creates the footer.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="timeStamp">The time stamp.</param>
		/// <param name="guid">The GUID.</param>
		/// <param name="diskGeometry">The disk geometry.</param>
		/// <returns></returns>
		static public byte[] CreateFooter(ulong size, uint timeStamp, byte[] guid, DiskGeometry diskGeometry)
		{
			byte[] footer = new byte[512];
			ulong alignedSize = GetAlignedSize(size);

			Mosa.ClassLib.BinaryFormat binaryFooter = new Mosa.ClassLib.BinaryFormat(footer);

			binaryFooter.SetString(VHDFooterOffset.Cookie, "conectix");
			binaryFooter.SetUIntReversed(VHDFooterOffset.Features, 0x00000002);
			binaryFooter.SetUIntReversed(VHDFooterOffset.FileFormatVersion, 0x00010000);
			binaryFooter.SetULong(VHDFooterOffset.DataOffset, ~(ulong)0);
			binaryFooter.SetUIntReversed(VHDFooterOffset.TimeStamp, timeStamp);
			binaryFooter.SetString(VHDFooterOffset.CreatorApplication, "MOSA");
			binaryFooter.SetUIntReversed(VHDFooterOffset.CreatorVersion, 0x00050000);
			binaryFooter.SetUIntReversed(VHDFooterOffset.CreatorHostOS, 0x5769326B); // Windows
			binaryFooter.SetULongReversed(VHDFooterOffset.OriginalSize, alignedSize);
			binaryFooter.SetULongReversed(VHDFooterOffset.CurrentSize, alignedSize);

			//Mosa.DeviceSystem.CHS chs = new Mosa.DeviceSystem.CHS();
			//chs.ComputeCHSv2(alignedSize/512);

			binaryFooter.SetUShortReversed(VHDFooterOffset.DiskGeometryCylinders, diskGeometry.Cylinders);
			binaryFooter.SetByte(VHDFooterOffset.DiskGeometryHeads, diskGeometry.Heads);
			binaryFooter.SetByte(VHDFooterOffset.DiskGeometrySectors, (byte)diskGeometry.SectorsPerTrack);

			binaryFooter.SetUIntReversed(VHDFooterOffset.DiskType, 0x02); // Fixed disk
			binaryFooter.SetUIntReversed(VHDFooterOffset.Checksum, 0x00);
			binaryFooter.SetBytes(VHDFooterOffset.UniqueId, guid, 0, 16);
			binaryFooter.SetByte(VHDFooterOffset.SavedState, 0x00); // No saved state

			uint checksum = 0;
			for (uint index = 0; index < 512; index++)
				checksum += footer[index];

			binaryFooter.SetUIntReversed(VHDFooterOffset.Checksum, ~checksum);

			return footer;
		}

	}
}
