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

namespace Mosa.VFS
{
	/// <summary>
	/// Interface implemented by all VFS nodes.
	/// </summary>
	public interface IVfsNode
	{

		#region Properties

		/// <summary>
		/// Retrieves the filesystem, where the node belongs to.
		/// </summary>
		//IFileSystem FileSystem { get; }

		/// <summary>
		/// Returns the type of the node.
		/// </summary>
		VfsNodeType NodeType { get; }

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Creates a new filesystem entry of the given name and type.
		/// </summary>
		/// <param name="name">The name of the filesystem entry to create.</param>
		/// <param name="type">The type of the filesystem entry. See remarks.</param>
		/// <param name="settings">Potential settings for the file systeme entry.</param>
		/// <returns>The created file system node for the requested object.</returns>
		/// <exception cref="System.NotSupportedException">The specified nodetype is not supported in the filesystem owning the node. See remarks about this.</exception>
		/// <exception cref=""></exception>
		/// <remarks>
		/// In theory every filesystem should support any VfsNodeType. Standard objects, such as directories and files are obvious. For other objects however, the
		/// file system is encouraged to store the passed settings in a specially marked file and treat these files as the appropriate node type. Instances of these
		/// objects can be retrieved using VfsObjectFactory.Create(settings).
		/// <para />
		/// Access rights do not need to be checked by the node implementation. They have been already been checked by the VirtualFileSystem itself.
		/// </remarks>
		IVfsNode Create(string name, VfsNodeType type, object settings);

		/// <summary>
		/// Requests the IVfsNode to perform a lookup on its children.
		/// </summary>
		/// <param name="name">The name of the item to find.</param>
		/// <returns>The vfs node, which represents the item. If there's no node with the specified name, the return value is null.</returns>
		IVfsNode Lookup(string name);

		/// <summary>
		/// Opens the IVfsNode and returns an object capable of doing something smart with the IVfsNode.
		/// </summary>
		/// <returns>An object instance, which represents the node.</returns>
		/// <remarks>
		/// This method is central to the entire VFS. It allows for interaction with filesystem entries in a way not possible
		/// with classical operating systems. The result of this function is heavily dependant on the item represented by the node, e.g.
		/// for a classic file (stream of bytes) the result of this method call would be a System.IO.Stream. For a device the result would
		/// be the driver object, for a directory it would be a System.IO.DirectoryInfo object, for kernel objects the respective object such
		/// as System.Threading.EventWaitHandle, System.Threading.Mutex, System.Threading.Thread, System.Diagnostics.Process etc. Note: The object
		/// retrieved can be closed by the respective methods on the returned object. There's no close functionality on the IVfsNode itself.
		/// </remarks>
		object Open(FileAccess access, FileShare share);

		/// <summary>
		/// Called to delete a child from a directory.
		/// </summary>
		/// <remarks>
		/// This function deletes a child IVfsNode from a directory. If child is a directory, it will be empty
		/// before this call is executed. It is recommended to include a debug sanity check though. If the file
		/// system needs to know the name of the child to delete, it can retrieve it from <see cref="DirectoryEntry.Name"/>.
		/// </remarks>
		/// <param name="child">The IVfsNode interface of the child.</param>
		/// <param name="dentry">The DirectoryEntry of the child.</param>
		/// <exception cref="System.NotSupportedException">The object does not support removal this way. There's most likely an object specific API to remove this IVfsNode.</exception>
		void Delete(IVfsNode child, DirectoryEntry dentry);

		#endregion // Methods
	}
}
