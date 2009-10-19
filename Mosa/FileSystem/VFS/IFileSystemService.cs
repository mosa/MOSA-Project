/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.FileSystem;

namespace Mosa.FileSystem.VFS
{

	/// <summary>
	/// Interface, which a filesystem driver must implement.
	/// </summary>
	/// <remarks>
	/// A filesystem driver, which implements this interface should register itself beneath /proc/filesystems
	/// to make the filesystem available for mounting and other operations.
	/// </remarks>
	interface IFileSystemService
	{
		#region Properties

		/// <summary>
		/// Retrieves the type of the filesystem settings class to pass to IFileSystemService.Format
		/// </summary>
		GenericFileSystemSettings SettingsType { get; }

		#endregion // Properties

		#region Methods

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
		bool Mount();

		/// <summary>
		/// Formats the media with the filesystem.
		/// </summary>
		/// <param name="settings">The settings for the filesystem to create.</param>
		/// <returns>The created and mounted filesystem.</returns>
		bool Format(GenericFileSystemSettings settings);

		#endregion // Methods
	}
}
