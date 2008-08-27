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
using Mosa.FileSystem.VFS;

namespace Mosa.FileSystem
{
	public abstract class GenericFileSystem
	{
		protected IPartitionDevice partition;
		protected uint blockSize;
		protected bool valid;
		protected string volumeLabel;
		protected byte[] serialNbr;

		public GenericFileSystem(IPartitionDevice partition)
		{
			this.partition = partition;
			this.blockSize = partition.BlockSize;
			this.valid = false;
			this.volumeLabel = string.Empty;
			this.serialNbr = new byte[0];
		}

		public abstract IFileSystem CreateVFSMount();

		public bool Valid { get { return valid; } }
		public string VolumeLabel { get { return volumeLabel; } }
		public byte[] SerialNbr { get { return serialNbr; } }

	}
}
