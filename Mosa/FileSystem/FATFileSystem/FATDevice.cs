/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.DeviceDrivers;
using Mosa.FileSystem;
using Mosa.FileSystem.VFS;

namespace Mosa.FileSystem.FATFileSystem
{
    /// <summary>
    /// 
    /// </summary>
	public class FATDevice : Device, IDevice, IFileSystemDevice
	{
        /// <summary>
        /// 
        /// </summary>
		public FATDevice()
		{
			base.name = "FAT";
			base.parent = null;
			base.deviceStatus = DeviceStatus.Available;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partition"></param>
        /// <returns></returns>
		public GenericFileSystem Create(IPartitionDevice partition)
		{
			return new FAT(partition);
		}

	}
}
