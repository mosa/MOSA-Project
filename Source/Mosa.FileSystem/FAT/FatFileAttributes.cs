// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.FileSystem.FAT
{
	/// <summary>
	/// FAT File Attributes
	/// </summary>
	public static class FatFileAttributes
	{
		public static byte ReadOnly => 0x01;

		public static byte Hidden => 0x02;

		public static byte System => 0x04;

		public static byte VolumeLabel => 0x08;

		public static byte SubDirectory => 0x10;

		public static byte Archive => 0x20;

		public static byte Device => 0x40;

		public static byte Unused => 0x80;

		public static byte LongFileName => 0x0F;
	}
}
