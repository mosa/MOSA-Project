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
	public class FATDevice : Device, IDevice, IFileSystemDevice
	{
		public FATDevice()
		{
			base.name = "FAT";
			base.parent = null;
			base.deviceStatus = DeviceStatus.Available;
		}

		public GenericFileSystem Create(IPartitionDevice partition)
		{
			return new FAT(partition);
		}

	}
}
