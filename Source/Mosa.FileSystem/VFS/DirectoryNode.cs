/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.FileSystem.VFS
{
	/// <summary>
	/// Represents a basic directory in the VFS namespace.
	/// </summary>
	/// <remarks>
	/// This class can be inherited if necessary to provide specialized
	/// directory handling if required.
	/// </remarks>
	public class DirectoryNode : NodeBase
	{
		#region Data members

		/// <summary>
		/// Holds all nodes added to the root vfs node.
		/// </summary>
		private System.Collections.ArrayList nodes;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the DirectoryNode object.
		/// </summary>
		/// <param name="fs">The filesystem, which owns the node.</param>
		public DirectoryNode(IFileSystem fs)
			: base(fs, VfsNodeType.Directory)
		{
			nodes = new System.Collections.ArrayList();
		}

		#endregion Construction

		#region IVfsNode members

		/// <summary>
		/// Creates the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="nodeType">Type of the node.</param>
		/// <param name="settings">The settings.</param>
		/// <returns></returns>
		public override IVfsNode Create(string name, VfsNodeType nodeType, object settings)
		{
			// FIXME: throw new NotImplementedException();
			return null;
		}

		/// <summary>
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public override IVfsNode Lookup(string name)
		{
			// FIXME: Lookup the node in the members
			return null;
		}

		/// <summary>
		/// Opens the specified access.
		/// </summary>
		/// <param name="access">The access.</param>
		/// <param name="sharing">The sharing.</param>
		/// <returns></returns>
		public override object Open(System.IO.FileAccess access, System.IO.FileShare sharing)
		{
			// FIXME: return something like: new System.IO.DirectoryInfo(VirtualFileSystem.GetPath(this));
			//throw new NotImplementedException();
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
			// FIXME: throw new NotImplementedException();
		}

		#endregion IVfsNode members
	}
}