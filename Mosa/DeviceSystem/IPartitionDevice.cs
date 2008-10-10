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
	public interface IPartitionDevice
	{
        /// <summary>
        /// 
        /// </summary>
		uint BlockCount { get; }

        /// <summary>
        /// 
        /// </summary>
		uint BlockSize { get; }

        /// <summary>
        /// 
        /// </summary>
		bool CanWrite { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <param name="count"></param>
        /// <returns></returns>
		byte[] ReadBlock(uint block, uint count);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
		bool ReadBlock(uint block, uint count, byte[] data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
		bool WriteBlock(uint block, uint count, byte[] data);
	}
}
