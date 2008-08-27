/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.IO;

namespace Mosa.FileSystem.VFS
{
	public sealed class FileSystemFactory
	{
		/// <summary>
		/// This function iterates all running file system drivers, which have registered themselves 
		/// beneath the /system/filesystems and ask them if they can mount this path.			 
		/// </summary>
		/// <param name="path">The path to the partition</param>
		/// <returns></returns>
		public static IFileSystem CreateFileSystem(string path)
		{
			//string fullPath = path.Substring(1);

			//// since we don't have /system/devices yet, just look for the partition devices in the DeviceManager
			////Device[] partitions = DeviceManager.GetDevicesOf (typeof (IPartitionDevice), fullPath);

			//IPartitionDevice partition = null;

			//foreach (IDevice device in Mosa.Devices.Setup.DeviceManager.GetDevices(new DeviceManager.WithName(fullPath)))
			//    if (device is IPartitionDevice) {
			//        partition = (IPartitionDevice)device;
			//        break;
			//    }

			//if (partition == null)
			//    return null;

			//foreach (IDevice device in Mosa.Devices.Setup.DeviceManager.GetDevices()) {
			//    if (device is IFileSystemService) {
			//        GenericFileSystem fs = (device as IFileSystemDevice).Create(partition);

			//        if (fs.Valid)
			//            return fs.CreateVFSMount();
			//    }
			//}

			return null;
		}
	}
}
