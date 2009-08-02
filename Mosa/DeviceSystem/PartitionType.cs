/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{	
    /// <summary>
    /// 
    /// </summary>
	public struct PartitionType
	{
        /// <summary>
        /// 
        /// </summary>
		public const byte Empty = 0x00;
        /// <summary>
        /// 
        /// </summary>
		public const byte GPT = 0xEE;
        /// <summary>
        /// 
        /// </summary>
		public const byte ExtendedPartition = 0x0F;
        /// <summary>
        /// 
        /// </summary>
		public const byte OldExtendedPartition = 0x05; // limited to disks under 8.4Gb
        /// <summary>
        /// 
        /// </summary>
		public const byte FAT12 = 0x01;
        /// <summary>
        /// 
        /// </summary>
		public const byte FAT16 = 0x04;
        /// <summary>
        /// 
        /// </summary>
		public const byte FAT32 = 0x0B;
	}

}
