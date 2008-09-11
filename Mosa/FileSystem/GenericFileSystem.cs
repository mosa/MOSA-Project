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
    /// <summary>
    /// 
    /// </summary>
	public abstract class GenericFileSystem
	{
        /// <summary>
        /// 
        /// </summary>
		protected IPartitionDevice partition;

        /// <summary>
        /// 
        /// </summary>
		protected uint blockSize;

        /// <summary>
        /// 
        /// </summary>
		protected bool valid;

        /// <summary>
        /// 
        /// </summary>
		protected string volumeLabel;

        /// <summary>
        /// 
        /// </summary>
		protected byte[] serialNbr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partition"></param>
		public GenericFileSystem(IPartitionDevice partition)
		{
			this.partition = partition;
			this.blockSize = partition.BlockSize;
			this.valid = false;
			this.volumeLabel = string.Empty;
			this.serialNbr = new byte[0];
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public abstract IFileSystem CreateVFSMount();

        /// <summary>
        /// 
        /// </summary>
		public bool Valid 
        { 
            get { return valid; } 
        }

        /// <summary>
        /// 
        /// </summary>
		public string VolumeLabel 
        { 
            get { return volumeLabel; } 
        }

        /// <summary>
        /// 
        /// </summary>
		public byte[] SerialNbr 
        { 
            get { return serialNbr; } 
        }

	}
}
