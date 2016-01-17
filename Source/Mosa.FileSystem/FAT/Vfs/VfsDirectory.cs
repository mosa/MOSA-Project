// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;
using Mosa.FileSystem.VFS;
using Mosa.FileSystem.FAT.Vfs;

namespace Mosa.FileSystem.FAT
{
	/// <summary>
	///
	/// </summary>
	public class VfsDirectory : DirectoryNode
	{
		/// <summary>
		///
		/// </summary>
		protected uint directoryCluster;

		/// <summary>
		/// Initializes a new instance of the <see cref="VfsDirectory"/> class.
		/// </summary>
		/// <param name="fs">The fs.</param>
		/// <param name="directoryCluster">The directory cluster.</param>
		public VfsDirectory(IFileSystem fs, uint directoryCluster)
			: base(fs)
		{
			this.directoryCluster = directoryCluster;
		}

		/// <summary>
		/// Creates a new file system entry of the given name and type.
		/// </summary>
		/// <param name="name">The name of the file system entry to create.</param>
		/// <param name="type">The type of the file system entry. See remarks.</param>
		/// <param name="settings">Potential settings for the file system entry.</param>
		/// <returns>
		/// The created file system node for the requested object.
		/// </returns>
		/// <exception cref="System.NotSupportedException">The specified node type is not supported in the file system owning the node. See remarks about this.</exception>
		/// <remarks>
		/// In theory every file system should support any VfsNodeType. Standard objects, such as directories and files are obvious. For other objects however, the
		/// file system is encouraged to store the passed settings in a specially marked file and treat these files as the appropriate node type. Instances of these
		/// objects can be retrieved using VfsObjectFactory.Create(settings).
		/// <para/>
		/// Access rights do not need to be checked by the node implementation. They have been already been checked by the VirtualFileSystem itself.
		/// </remarks>
		public override IVfsNode Create(string name, VfsNodeType type, object settings)
		{
			return null;
		}

		/// <summary>
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public override IVfsNode Lookup(string name)
		{
			var location = (FileSystem as VfsFileSystem).Fat.FindEntry(new Find.WithName(name), directoryCluster);

			if (!location.IsValid)
				return null;

			if (location.IsDirectory)
				return new VfsDirectory(FileSystem, location.FirstCluster);
			else
				return new VfsFile(FileSystem, location.FirstCluster, location.DirectorySector, location.DirectorySectorIndex);
		}

		/// <summary>
		/// Opens the IVfsNode and returns an object capable of doing something smart with the IVfsNode.
		/// </summary>
		/// <param name="access"></param>
		/// <param name="share"></param>
		/// <returns>
		/// An object instance, which represents the node.
		/// </returns>
		/// <remarks>
		/// This method is central to the entire VFS. It allows for interaction with file system entries in a way not possible
		/// with classical operating systems. The result of this function is heavily dependent on the item represented by the node, e.g.
		/// for a classic file (stream of bytes) the result of this method call would be a System.IO.Stream. For a device the result would
		/// be the driver object, for a directory it would be a System.IO.DirectoryInfo object, for kernel objects the respective object such
		/// as System.Threading.EventWaitHandle, System.Threading.Mutex, System.Threading.Thread, System.Diagnostics.Process etc. Note: The object
		/// retrieved can be closed by the respective methods on the returned object. There's no close functionality on the IVfsNode itself.
		/// </remarks>
		public override object Open(FileAccess access, FileShare share)
		{
			return null;
		}

		/// <summary>
		/// Called to delete a child from a directory.
		/// </summary>
		/// <param name="child">The IVfsNode interface of the child.</param>
		/// <param name="dentry">The DirectoryEntry of the child.</param>
		/// <remarks>
		/// This function deletes a child IVfsNode from a directory. If child is a directory, it will be empty
		/// before this call is executed. It is recommended to include a debug sanity check though. If the file
		/// system needs to know the name of the child to delete, it can retrieve it from <see cref="DirectoryEntry.Name"/>.
		/// </remarks>
		/// <exception cref="System.NotSupportedException">The object does not support removal this way. There's most likely an object specific API to remove this IVfsNode.</exception>
		public override void Delete(IVfsNode child, DirectoryEntry dentry)
		{
			var fs = FileSystem as FatFileSystem;

			uint targetCluster = (child as VfsDirectory).directoryCluster;

			var location = fs.FindEntry(new Find.ByCluster(targetCluster), directoryCluster);

			if (!location.IsValid)
				throw new System.ArgumentException(); //throw new IOException ("Unable to delete directory because it is not empty");

			fs.Delete(targetCluster, location.DirectorySector, location.DirectorySectorIndex);
		}
	}
}
