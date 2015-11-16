namespace Mosa.Utility.BootImage
{
	public static class Syslinux
	{
		public const uint LDLINUX_MAGIC = 0x3EB202FE;

		public struct PatchAreaOffset
		{
			public const uint Magic = 0;            // LDLINUX_MAGIC
			public const uint Instance = 4;         // Per-version value
			public const uint DataSectors = 8;      // Number of sectors (not including bootsec)
			public const uint AdvSectors = 10;      // Additional sectors for ADVs
			public const uint Dwords = 12;          // Total dwords starting at ldlinux_sys
			public const uint Checksum = 16;        // Checksum starting at ldlinux_sys
			public const uint MaxTransfer = 20;     // Max sectors to transfer
			public const uint EPAOffset = 22;       // Pointer to the extended patch area
		}

		public struct ExtendedPatchAreaOffset
		{
			public const uint AdvPtrOffset = 0;     // ADV pointers
			public const uint DirOffset = 2;        // Current directory field
			public const uint DirLen = 4;           // Length of current directory field
			public const uint SubVolOffset = 6;     // Subvolume field
			public const uint SubVolLen = 8;        // Length of subvolume field
			public const uint SecPtrOffset = 10;    // Sector extent pointers
			public const uint SecPtrCnt = 12;       // Number of sector extent pointers
			public const uint Sect1Ptr0 = 14;       // Boot sector offset of sector 1 ptr LSW
			public const uint Sect1Ptr1 = 16;       // Boot sector offset of sector 1 ptr MSW
			public const uint RaidPatch = 18;       // Boot sector RAID mode patch pointer
		}
	}
}
