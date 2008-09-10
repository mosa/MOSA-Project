/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;

namespace Mosa.FileSystem.VFS
{
	/// <summary>
	/// Implements path resolution functionality for the <see cref="Mosa.VFS.VirtualFileSystem"/>.
	/// </summary>
	class PathResolver
	{
		#region Constants

		// FIXME: Make this configurable - do we have some sort of configuration provider?
		/// <summary>
		/// Controls the maximum number of symbolic links to follow while resolving a directory path.
		/// </summary>
		private static readonly int MAX_SYMLINKS_TO_FOLLOW = 8;

		/// <summary>
		/// Array to split paths properly for the local system.
		/// </summary>
		//private static readonly char[] splitChars = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

		#endregion // Constants

		#region Data members

		/// <summary>
		/// Reference to the root directory of the system.
		/// </summary>
		private DirectoryEntry rootDirectory;

		/// <summary>
		/// Reference to the current directory of the system.
		/// </summary>
		private DirectoryEntry currentDirectory;

		/// <summary>
		/// Remaining lookup depth for symbolic links.
		/// </summary>
		private int depth;

		#endregion // Data members

		#region Construction

		private PathResolver(DirectoryEntry rootDirectory, DirectoryEntry currentDirectory)
		{
			this.rootDirectory = rootDirectory;
			this.currentDirectory = currentDirectory;
			depth = PathResolver.MAX_SYMLINKS_TO_FOLLOW;
		}

		#endregion // Construction

		#region Static methods

		/// <summary>
		/// Performs a standard path lookup.
		/// </summary>
		/// <param name="rootDirectory">The root directory.</param>
		/// <param name="path">The path to resolve.</param>
		/// <returns>The directory entry of the resolved path.</returns>
		/// <exception cref="System.Security.SecurityException">The caller does not have access to the path or a component. For example the caller does not have the right to traverse the path.</exception>
		/// <exception cref="System.IO.PathTooLongException">The path is too long to traverse. This can be the result of circular symbolic links in the path.</exception>
		/// <exception cref="System.IO.FileNotFoundException">The file or folder path not found.</exception>
		/// <exception cref="System.IO.DirectoryNotFoundException">A path component was not found.</exception>
		/// <remarks>
		/// This call my result in other exceptions not specified in the above list. Other exceptions can be thrown by IVfsNode implementations, which are visited during the traversal
		/// process. For example a network file system node may throw an exception, if the server is unreachable.
		/// </remarks>
		public static DirectoryEntry Resolve(DirectoryEntry rootDirectory, ref string path)
		{
			// FIXME: Remove the root argument. The filesystem root should be unique for a process as part of a security model similar to jails, e.g. give apps from
			// untrusted sources their private filesystem regions.

			// FIXME: Get the root from the thread execution block
			DirectoryEntry current = rootDirectory;
			PathResolver resolver = new PathResolver(rootDirectory, current);
			return resolver.Resolve(ref path, PathResolutionFlags.None);
		}

		/// <summary>
		/// Performs a path lookup obeying to the passed flags.
		/// </summary>
		/// <param name="rootDirectory">The root directory.</param>
		/// <param name="path">The path to resolve.</param>
		/// <param name="flags">Controls aspects of the path lookup process.</param>
		/// <returns>The directory entry of the resolved path.</returns>
		/// <exception cref="System.Security.SecurityException">The caller does not have access to the path or a component. For example the caller does not have the right to traverse the path.</exception>
		/// <exception cref="System.IO.PathTooLongException">The path is too long to traverse. This can be the result of circular symbolic links in the path.</exception>
		/// <exception cref="System.IO.FileNotFoundException">The file or folder path was not found. This exception can be prevented by specifying PathResolutionFlags.DoNotThrowNotFoundException.</exception>
		/// <exception cref="System.IO.DirectoryNotFoundException">A path component was not found. This exception can be prevented by specifying PathResolutionFlags.DoNotThrowNotFoundException.</exception>
		/// <remarks>
		/// This call my result in other exceptions not specified in the above list. Other exceptions can be thrown by IVfsNode implementations, which are visited during the traversal
		/// process. For example a network file system node may throw an exception, if the server is unreachable.
		/// </remarks>
		public static DirectoryEntry Resolve(DirectoryEntry rootDirectory, ref string path, PathResolutionFlags flags)
		{
			// FIXME: Get the root from the thread execution block
			DirectoryEntry current = rootDirectory;
			PathResolver resolver = new PathResolver(rootDirectory, current);
			return resolver.Resolve(ref path, flags);
		}

		#endregion // Static methods

		#region Methods

		/// <summary>
		/// Performs an iterative lookup of the given path starting from the root and obeying to the specified flags.
		/// </summary>
		/// <param name="path">The path to lookup. This can be a relative or absolute path. Path.DirectorySeparatorChar or Path.AltDirectorySeparatorChar are valid delimiters.</param>
		/// <param name="flags">The lookup flags, which control the lookup process.</param>
		/// <returns>The directory entry of the resolved path.</returns>
		/// <exception cref="System.Security.SecurityException">The caller does not have access to the path or a component. For example the caller does not have the right to traverse the path.</exception>
		/// <exception cref="System.IO.PathTooLongException">The path is too long to traverse. This can be the result of circular symbolic links in the path.</exception>
		/// <exception cref="System.IO.FileNotFoundException">The path or a component was not found. This exception can be prevented by specifying PathResolutionFlags.DoNotThrowNotFoundException.</exception>
		/// <exception cref="System.IO.DirectoryNotFoundException">A path component was not found. This exception can be prevented by specifying PathResolutionFlags.DoNotThrowNotFoundException.</exception>
		/// <remarks>
		/// This call may result in other exceptions not specified in the above list. Other exceptions can be thrown by IVfsNode implementations, which are visited during the traversal
		/// process. For example a network file system node may throw an exception, if the server is unreachable.
		/// </remarks>
		private DirectoryEntry Resolve(ref string path, PathResolutionFlags flags)
		{
			// DirectoryNode entry found by stepping through the path
			DirectoryEntry entry = null;

			// Split the given path to its components
			PathSplitter dirs = new PathSplitter(path);

			// Determine the number of path components
			int max = dirs.Length;

			// Current path component
			string item;
			// Loop index
			int index = 0;
			// Perform an access check on the root directory
			AccessCheck.Perform(currentDirectory, AccessMode.Traverse, AccessCheckFlags.None);

			// Do not resolve the last name, if we want the parent directory.
			if (PathResolutionFlags.RetrieveParent == (flags & PathResolutionFlags.RetrieveParent)) {
				path = dirs[dirs.Length - 1];
				max--;
			}

			// Check if this is an absolute path?
			if (dirs[0].Length == 0) {
				// Yes, replace the current directory
				currentDirectory = rootDirectory;
				index++;
			}

			// Iterate over the remaining path components
			while ((currentDirectory != null) && (index < max)) {
				item = dirs[index];
				entry = null;
				if (currentDirectory.Node.NodeType == VfsNodeType.SymbolicLink) {
					SymbolicLinkNode link = (SymbolicLinkNode)currentDirectory.Node;
					if (0 != depth--) {
						// The symlink stores a relative path, use it for a current relative lookup.
						string target = link.Target;

						// Build a new flags set for symlink lookups, as we do not want all of them.
						PathResolutionFlags symflags = (flags & PathResolutionFlags.SymLinkLookupSafe);
						entry = Resolve(ref target, symflags);
					}
					else {
						if (PathResolutionFlags.DoNotThrowNotFoundException != (PathResolutionFlags.DoNotThrowNotFoundException & flags)) {
							// FIXME: Provide a MUI resource string for the exception
#if VFS_EXCEPTIONS
							throw new PathTooLongException();
#endif // #if !VFS_EXCEPTIONS
						}
					}
				}
				else {
					// Pass the lookup to the DirectoryEntry (and ultimately to the inode itself.)
					entry = currentDirectory.Lookup(item);

					// If lookup in the directory entry failed, ask the real INode to perform the lookup.
					if (entry == null) {
						IVfsNode node = currentDirectory.Node.Lookup(item);
						if (node != null) {
							entry = DirectoryEntry.Allocate(currentDirectory, item, node);
						}
					}
				}

				// Increment the path component index
				index++;

				// Check if we have a new path component?
				if ((entry == null) && (PathResolutionFlags.DoNotThrowNotFoundException != (PathResolutionFlags.DoNotThrowNotFoundException & flags))) {
					// FIXME: Move exception messages to MUI resources
#if VFS_EXCEPTIONS
					if (index == max)
						throw new FileNotFoundException(@"Failed to resolve the path.", path);
					else
						throw new DirectoryNotFoundException(@"Failed to resolve the path.");
#endif // #if VFS_EXCEPTIONS
				}

				// Set the current resolution directory
				currentDirectory = entry;

				// Check if the caller has traverse access to the directory
				AccessCheck.Perform(currentDirectory, AccessMode.Traverse, AccessCheckFlags.None);
			}

			return currentDirectory;
		}

		#endregion // Methods
	}
}
