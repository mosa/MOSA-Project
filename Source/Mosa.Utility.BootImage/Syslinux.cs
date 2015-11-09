namespace Mosa.Utility.BootImage
{
	public static class Syslinux
	{
		public struct PatchAreaOffset
		{
			public const uint Magic = 0;
			public const uint Instance = 4;
			public const uint DataSectors = 8;
			public const uint AdvSectors = 10;
			public const uint Dwords = 12;
			public const uint Checksum = 16;
			public const uint MaxTransfer = 20;
			public const uint EPAOffset = 22;
		}

		public struct ExtendedPatchAreaOffset
		{
			public const uint AdvPtrOffset = 0;  // ADV pointers
			public const uint DirOffset = 2;     // Current directory field
			public const uint DirLen = 4;        // Length of current directory field
			public const uint SubVolOffset = 6;  // Subvolume field
			public const uint SubVolLen = 7;     // Length of subvolume field
			public const uint SecPtrOffset = 10;  // Sector extent pointers
			public const uint SecPtrCnt = 12;     // Number of sector extent pointers
			public const uint Sect1Ptr0 = 14;     // Boot sector offset of sector 1 ptr LSW
			public const uint Sect1Ptr1 = 16;     // Boot sector offset of sector 1 ptr MSW
			public const uint RaidPatch = 18;     // Boot sector RAID mode patch pointer
		}
	}
}
