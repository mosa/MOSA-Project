// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
		public uint BlockSize { get; protected set; }

		/// <summary>
		///
		/// </summary>
		public bool IsValid { get; protected set; }

		/// <summary>
		/// Gets the volume label.
		/// </summary>
		/// <value>The volume label.</value>
		public string VolumeLabel { get; protected set; }

		/// <summary>
		/// Gets the serial number.
		/// </summary>
		/// <value>The serial number.</value>
		public byte[] SerialNumber { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericFileSystem"/> class.
		/// </summary>
		/// <param name="partition">The partition.</param>
		public GenericFileSystem(IPartitionDevice partition)
		{
			this.partition = partition;
			BlockSize = partition.BlockSize;
			IsValid = false;
			VolumeLabel = string.Empty;
			SerialNumber = new byte[0];
		}

		/// <summary>
		/// Creates the VFS mount.
		/// </summary>
		/// <returns></returns>
		public abstract IFileSystem CreateVFSMount();
	}
}
