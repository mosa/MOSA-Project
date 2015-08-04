// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.FileSystem.VFS;

namespace Mosa.FileSystem.FAT
{
	/// <summary>
	///
	/// </summary>
	public class VfsFileSystem : IFileSystemService, IFileSystem
	{
		/// <summary>
		///
		/// </summary>
		protected FatFileSystem fat;

		/// <summary>
		/// Gets the FAT.
		/// </summary>
		/// <value>The FAT.</value>
		public FatFileSystem FAT
		{
			get { return fat; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VfsFileSystem"/> class.
		/// </summary>
		/// <param name="fat">The fat.</param>
		public VfsFileSystem(FatFileSystem fat)
		{
			this.fat = fat;
		}

		/// <summary>
		/// Retrieves the type of the filesystem settings class to pass to IFileSystemService.Format
		/// </summary>
		/// <value></value>
		public GenericFileSystemSettings SettingsType { get { return fat.SettingsType; } }

		/// <summary>
		/// Mounts a file system from the specified stream/device.
		/// </summary>
		/// <returns>The mounted filesystem.</returns>
		/// <remarks>
		/// File system implementations should not blindly assume that the block device or file really
		/// contain the expected filesystem. An implementation should run some checks for integrity and
		/// validity before returning an object implementing IFileSystem.
		/// <para/>
		/// Also this method should not throw. In contrast to other operating systems, the user will not
		/// be forced to know the file system on disk. The file system manager will try all file systems
		/// until it finds one, which returns a non-null IFileSystem. So a failure in a mount operation
		/// is not considered an exception, but a normal process.
		/// </remarks>
		public bool Mount()
		{
			return true;
		}

		/// <summary>
		/// Formats the media with the filesystem.
		/// </summary>
		/// <param name="settings">The settings for the filesystem to create.</param>
		/// <returns>The created and mounted filesystem.</returns>
		public bool Format(GenericFileSystemSettings settings)
		{
			return (fat.Format(((FatSettings)settings)));
		}

		/// <summary>
		/// </summary>
		/// <value></value>
		public bool IsReadOnly
		{
			get { return fat.IsReadOnly; }
		}

		/// <summary>
		///
		/// </summary>
		private VfsDirectory root;

		/// <summary>
		/// </summary>
		/// <value></value>
		public IVfsNode Root
		{
			get
			{
				if (root == null)
					root = new VfsDirectory(this, 0);

				return root;
			}
		}
	}
}
