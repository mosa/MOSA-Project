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
	/// Implements the virtual file system service for the kernel.
	/// </summary>
	/// <remarks>
	/// The virtual file system service provides the root '/' naming namespace for
	/// other nodes and services.
	/// </remarks>
	public sealed class VirtualFileSystem : IFileSystem, IFileSystemService
	{

		#region Data members

		/// <summary>
		/// The virtual root directory.
		/// </summary>
		private static DirectoryNode rootDirectory;

		/// <summary>
		/// Root entry of the virtual file system.
		/// </summary>
		private static DirectoryEntry rootNode;

		#endregion // Data members

		#region Construction

		private VirtualFileSystem()
		{
		}

		/// <summary>
		/// Initializes a new instance of the virtual file system.
		/// </summary>
		public static void Setup()
		{
			rootDirectory = new DirectoryNode(null);
			rootNode = DirectoryEntry.AllocateRoot(rootDirectory);

			// FIXME: Add an entry of the virtual file system to /system/filesystems
		}

		#endregion // Construction

		#region Static Properties

		// FIXME: This is not a good idea and should be removed (using by DirectoryEntry.CurrentDirectory, if no one is set) once
		// we have a process structure supporting jails.

		/// <summary>
		/// 
		/// </summary>
		public static DirectoryEntry RootDirectoryEntry
		{
			get
			{
				return rootNode;
			}
		}

		#endregion // Static members

		#region Static Methods

		/// <summary>
		/// Checks if the caller has access to the inode 
		/// </summary>
		/// <param name="path">The resource to check permissions for.</param>
		/// <param name="mode"></param>
		/// <returns>True if the requested access mode combination is available to the immediate caller. If any one requested access mode is not available, the result is false.</returns>
		public static bool Access(string path, AccessMode mode)
		{
			DirectoryEntry entry = PathResolver.Resolve(rootNode, ref path, PathResolutionFlags.DoNotThrowNotFoundException);
			if (null != entry)
			{
				return AccessCheck.Perform(entry, mode, AccessCheckFlags.NoThrow);
			}

			return false;
		}

		/// <summary>
		/// Creates a new node in the (virtual) filesystem.
		/// </summary>
		/// <param name="path">The path to create.</param>
		/// <param name="type">The type of the node to create.</param>
		/// <param name="settings">Settings used to initialize the node.</param>
		/// <param name="access">Requests the specified access modes on the created object.</param>
		/// <param name="share">Requests the specified sharing settings on the object.</param>
		/// <returns>The created filesystem object.</returns>
		/// <remarks>
		/// This function creates new nodes in the virtual filesystem. In contrast to *nix this call
		/// creates all node types, e.g. files, directories, devices and more. Specific types may
		/// require additional settings, which are specified in a settings object passed as the third
		/// parameter.
		/// </remarks>
		public static object Create(string path, VfsNodeType type, object settings, System.IO.FileAccess access, System.IO.FileShare share)
		{
			// Retrieve the parent directory
			DirectoryEntry parent = PathResolver.Resolve(rootNode, ref path, PathResolutionFlags.RetrieveParent);

			// Check if the caller has write access in the directory
			AccessCheck.Perform(parent, AccessMode.Write, AccessCheckFlags.None);

			// Yes, we do have write access. Create the new vfs node
			IVfsNode node = parent.Node.Create(path, type, settings);
			// FIXME: Assert(null != node);
			DirectoryEntry entry = DirectoryEntry.Allocate(parent, path, node);

			// FIXME: Fix the permissions for this call. *nix does this using its bitmasks, Win32 through its huge CreateFile API.
			return node.Open(access, share);
		}

		/// <summary>
		/// Changes the current directory in the thread execution block.
		/// </summary>
		/// <param name="path">The path to change to. This path may be relative or absolute.</param>
		public static void ChangeDirectory(string path)
		{
			DirectoryEntry entry = PathResolver.Resolve(rootNode, ref path);
			// FIXME: Set the current directory in the thread execution block
		}

		/// <summary>
		/// Deletes the named node from the filesystem.
		/// </summary>
		/// <param name="path">The path, which identifies a node.</param>
		public static void Delete(string path)
		{
			DirectoryEntry entry = PathResolver.Resolve(rootNode, ref path, PathResolutionFlags.DoNotThrowNotFoundException);
			if (null != entry)
			{
				AccessCheck.Perform(entry, AccessMode.Delete, AccessCheckFlags.None);
				//entry.Node.Delete();
				entry.Parent.Node.Delete(entry.Node, entry);
				entry.Release();
			}
		}

		/// <summary>
		/// Mounts a new file system.
		/// </summary>
		/// <param name="source">The source of the filesystem. This is usually a device name, but can also be another directory.</param>
		/// <param name="target">The path including the name of the mount point, where to mount the new filesystem.</param>
		public static void Mount(string source, string target)
		{
			// Retrieve the parent directory of the mount
			DirectoryEntry parent = PathResolver.Resolve(rootNode, ref target, PathResolutionFlags.RetrieveParent);

			if (parent == null)
				throw new System.ArgumentException();

			IFileSystem root = FileSystemFactory.CreateFileSystem(source);

			if (root == null)
				throw new System.ArgumentException();

			PathSplitter path = new PathSplitter(target);
			DirectoryEntry.Allocate(parent, path.Last, root.Root);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="access"></param>
		/// <param name="share"></param>
		/// <returns></returns>
		public static object Open(string path, System.IO.FileAccess access, System.IO.FileShare share)
		{
			DirectoryEntry entry = PathResolver.Resolve(rootNode, ref path);

			/* HINT:
			 * 
			 * 1. Do we really need to pass the FileShare flags down to the inode? 
			 * 2. Shouldn't we have some sort of lock deamon governing shared access?
			 *
			 * Ansers:
			 * 1. Yes.
			 * 2. Yes and no. A lock deamon would only work for local filesystems. For imported
			 *    ones we need to notify the server of the sharing lock anyway, so that the IVfsNode
			 *    (acting as a client to the server) is the best place to do it without giving the
			 *    lock deamon knowledge of all file sharing protocols (afp, smb, ftp, name it.)
			 * 3. The inode may reject the file sharing requests. We do want to represent devices
			 *    and sync objects in the VFS, which means *they* need to decide if the flags are
			 *    applicable.
			 *
			 */

			// FIXME: Perform access checks on the DirectoryEntry/IVfsNode.
			AccessMode modeFlags = AccessMode.Exists;
			switch (access)
			{
				case System.IO.FileAccess.Read:
					modeFlags = AccessMode.Read;
					break;
				case System.IO.FileAccess.Write:
					modeFlags = AccessMode.Write;
					break;
				case System.IO.FileAccess.ReadWrite:
					modeFlags = AccessMode.Read | AccessMode.Write;
					break;
			}

			AccessCheck.Perform(entry, modeFlags, AccessCheckFlags.None);

			return entry.Node.Open(access, share);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="old"></param>
		/// <param name="newname"></param>
		public static void Rename(string old, string newname)
		{
			// FIXME: 
			throw new System.NotImplementedException();
		}

		// <summary>
		// Resolves the path evaluating all symbolic links.
		// </summary>
		// <param name="path">The path to resolve.</param>
		// <returns>The absolute path, which is actually represented by the given path.</returns>
		// <exception cref="System.IO.FileNotFoundException">A component of the path or a symbolic link in the path do not name an existing file system entry.</exception>
		// <exception cref="System.IO.PathTooLongException">The path represented by the given path is too long or symbolic links produced infinite recursion.</exception>
		// <exception cref="System.IO.IOException">An I/O exception occurred while resolving the path.</exception>
		// <exception cref="System.Security.SecurityException">A fragment of the path could not be traversed due to a denied search permission.</exception>
		//public static String ResolvePath(String path)
		//{
		//    DirectoryEntry entry = PathResolver.Resolve(_rootNode, ref path);
		//    String pathSep = new String(Path.DirectorySeparatorChar, 1);
		//    String result = String.Concat(pathSep, entry.Name);

		//    while (!Object.ReferenceEquals(entry, entry.Parent))
		//    {
		//        entry = entry.Parent;
		//        result = String.Concat(pathSep, entry.Name, result);
		//    }

		//    return result;
		//}

		/// <summary>
		/// Retrieves a 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static object Stat(string path)
		{
			// FIXME
			// throw new NotImplementedException();
			return null;
		}

		/// <summary>
		/// Unmounts the filesystem rooted at the given path.
		/// </summary>
		/// <param name="path">The path identifying the filesystem to unmount.</param>
		/// <remarks>
		/// In contrast to Posix this does not have to be the root directory of the filesystem. Any path in the filesystem will unmount the 
		/// entire tree.
		/// </remarks>
		/// FIXME: Which exceptions can be thrown.
		/// FIXME: How is the current directory handled? What if it is inside the FS tree being unmounted.
		/// FIXME: We need to check the FS tree for in use status and throw an InvalidOperationException?
		public static void Unmount(string path)
		{
			// FIXME: 
			throw new System.NotImplementedException();
		}

		#endregion // Methods

		#region IFileSystem Members

		bool IFileSystem.IsReadOnly { get { return false; } }

		IVfsNode IFileSystem.Root
		{
			get { return VirtualFileSystem.rootDirectory; }
		}

		#endregion // IFileSystem Members

		#region IFileSystemService Members

		GenericFileSystemSettings IFileSystemService.SettingsType { get { return null; } }

		bool IFileSystemService.Mount()
		{
			// Even though we're a file system, we are not mountable.
			return false;
		}

		bool IFileSystemService.Format(GenericFileSystemSettings settings)
		{
			// We do not support formatting.
			throw new System.NotSupportedException();
		}

		#endregion // IFileSystemService Members
	}
}