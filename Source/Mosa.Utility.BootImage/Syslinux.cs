using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			public const uint EpaOffset = 22;
		}
	}
}
