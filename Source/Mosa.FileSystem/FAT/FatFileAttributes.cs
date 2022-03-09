// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.FileSystem.FAT
{
	/// <summary>
	/// FAT File Attributes
	/// </summary>
	public static class FatFileAttributes
	{
		public const byte ReadOnly = 0x01;

		public const byte Hidden = 0x02;

		public const byte System = 0x04;

		public const byte VolumeLabel = 0x08;

		public const byte SubDirectory = 0x10;

		public const byte Archive = 0x20;

		public const byte Device = 0x40;

		public const byte Unused = 0x80;

		public const byte LongFileName = 0x0F;
	}
}
