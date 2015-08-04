// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.FileSystem.VFS;

namespace Mosa.FileSystem.FAT
{
	/// <summary>
	///
	/// </summary>
	public class VfsFile : NodeBase
	{
		/// <summary>
		///
		/// </summary>
		protected uint fileCluster;

		/// <summary>
		///
		/// </summary>
		protected uint directorySector;

		/// <summary>
		///
		/// </summary>
		protected uint directoryIndex;

		/// <summary>
		/// Initializes a new instance of the <see cref="VfsFile"/> class.
		/// </summary>
		/// <param name="fs">The fs.</param>
		/// <param name="fileCluster">The file cluster.</param>
		/// <param name="directorySector">The directory sector.</param>
		/// <param name="directoryIndex">Index of the directory.</param>
		public VfsFile(IFileSystem fs, uint fileCluster, uint directorySector, uint directoryIndex)
			: base(fs, VfsNodeType.File)
		{
			this.fileCluster = fileCluster;
			this.directorySector = directorySector;
			this.directoryIndex = directoryIndex;
		}

		/// <summary>
		/// Creates a new filesystem entry of the given name and type.
		/// </summary>
		/// <param name="name">The name of the filesystem entry to create.</param>
		/// <param name="type">The type of the filesystem entry. See remarks.</param>
		/// <param name="settings">Potential settings for the file systeme entry.</param>
		/// <returns>
		/// The created file system node for the requested object.
		/// </returns>
		/// <exception cref="System.NotSupportedException">The specified nodetype is not supported in the filesystem owning the node. See remarks about this.</exception>
		/// <remarks>
		/// In theory every filesystem should support any VfsNodeType. Standard objects, such as directories and files are obvious. For other objects however, the
		/// file system is encouraged to store the passed settings in a specially marked file and treat these files as the appropriate node type. Instances of these
		/// objects can be retrieved using VfsObjectFactory.Create(settings).
		/// <para/>
		/// Access rights do not need to be checked by the node implementation. They have been already been checked by the VirtualFileSystem itself.
		/// </remarks>
		public override IVfsNode Create(string name, VfsNodeType type, object settings)
		{
			// TODO
			throw new System.NotSupportedException("file write not supported");
		}

		/// <summary>
		/// Requests the IVfsNode to perform a lookup on its children.
		/// </summary>
		/// <param name="name">The name of the item to find.</param>
		/// <returns>
		/// The vfs node, which represents the item. If there's no node with the specified name, the return value is null.
		/// </returns>
		public override IVfsNode Lookup(string name)
		{
			return null;    // Lookup() method doesn't make sense here
		}

		/// <summary>
		/// Opens the specified access.
		/// </summary>
		/// <param name="access">The access.</param>
		/// <param name="sharing">The sharing.</param>
		/// <returns></returns>
		public override object Open(System.IO.FileAccess access, System.IO.FileShare sharing)
		{
			if (access == System.IO.FileAccess.Read)
				return new FatFileStream((FileSystem as VfsFileSystem).FAT, fileCluster, directorySector, directoryIndex);

			// TODO
			throw new System.NotSupportedException("file write not supported");
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
			throw new System.ArgumentException(); // Delete() method doesn't make sense here
		}
	}
}
