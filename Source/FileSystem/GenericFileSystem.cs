/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;
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
		/// Initializes a new instance of the <see cref="GenericFileSystem"/> class.
		/// </summary>
		/// <param name="partition">The partition.</param>
		public GenericFileSystem(IPartitionDevice partition)
		{
			this.partition = partition;
			this.blockSize = partition.BlockSize;
			this.valid = false;
			this.volumeLabel = string.Empty;
			this.serialNbr = new byte[0];
		}

		/// <summary>
		/// Creates the VFS mount.
		/// </summary>
		/// <returns></returns>
		public abstract IFileSystem CreateVFSMount();

		/// <summary>
		/// Gets a value indicating whether this <see cref="GenericFileSystem"/> is valid.
		/// </summary>
		/// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
		public bool IsValid { get { return valid; } }

		/// <summary>
		/// Gets the volume label.
		/// </summary>
		/// <value>The volume label.</value>
		public string VolumeLabel { get { return volumeLabel; } }

		/// <summary>
		/// Gets the serial number.
		/// </summary>
		/// <value>The serial number.</value>
		public byte[] SerialNumber { get { return serialNbr; } }

	}
}
