// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.FileSystem
{
	/// <summary>
	/// Provides a default implementation for INodes. A file system implementation
	/// may choose to derive from BasicNode to receive a default implementation of
	/// the interface.
	/// </summary>
	public abstract class NodeBase : IVfsNode
	{
		#region Data Members

		/// <summary>
		/// Holds the file system of the node.
		/// </summary>

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NodeBase"/> class.
		/// </summary>
		/// <param name="fs">The fs.</param>
		/// <param name="type">The type.</param>
		protected NodeBase(IFileSystem fs, VfsNodeType type)
		{
			this.FileSystem = fs;
			this.NodeType = type;
		}

		#endregion Construction

		#region IVfsNode members

		/// <summary>
		/// Gets the file system.
		/// </summary>
		/// <value>The file system.</value>

		/* Unmerged change from project 'Mosa.Utility.FileSystem'
		Before:
				public IFileSystem FileSystem { get { return fs; } }
		After:
				public IFileSystem FileSystem { get; } }
		*/
		public IFileSystem FileSystem { get; private set; }

		/// <summary>
		/// Returns the type of the node.
		/// </summary>
		/// <value></value>

		/* Unmerged change from project 'Mosa.Utility.FileSystem'
		Before:
				public VfsNodeType NodeType { get { return type; } }
		After:
				public VfsNodeType NodeType { get; } }
		*/
		public VfsNodeType NodeType { get; private set; }

		/// <summary>
		/// Creates a new file system entry of the given name and type.
		/// </summary>
		/// <param name="name">The name of the file system entry to create.</param>
		/// <param name="type">The type of the file system entry. See remarks.</param>
		/// <param name="settings">Potential settings for the file systeme entry.</param>
		/// <returns>
		/// The created file system node for the requested object.
		/// </returns>
		/// <exception cref="System.NotSupportedException">The specified nodetype is not supported in the file system owning the node. See remarks about this.</exception>
		/// <remarks>
		/// In theory every file system should support any VfsNodeType. Standard objects, such as directories and files are obvious. For other objects however, the
		/// file system is encouraged to store the passed settings in a specially marked file and treat these files as the appropriate node type. Instances of these
		/// objects can be retrieved using VfsObjectFactory.Create(settings).
		/// <para/>
		/// Access rights do not need to be checked by the node implementation. They have been already been checked by the VirtualFileSystem itself.
		/// </remarks>
		public abstract IVfsNode Create(string name, VfsNodeType type, object settings);

		/// <summary>
		/// Requests the IVfsNode to perform a lookup on its children.
		/// </summary>
		/// <param name="name">The name of the item to find.</param>
		/// <returns>
		/// The vfs node, which represents the item. If there's no node with the specified name, the return value is null.
		/// </returns>
		public virtual IVfsNode Lookup(string name)
		{
			return null;
		}

		/// <summary>
		/// Opens the specified access.
		/// </summary>
		/// <param name="access">The access.</param>
		/// <param name="sharing">The sharing.</param>
		/// <returns></returns>
		public abstract object Open(FileAccess access, FileShare sharing);

		/// <summary>
		/// Deletes the specified child.
		/// </summary>
		/// <param name="child">The child.</param>
		/// <param name="entry">The entry.</param>
		public abstract void Delete(IVfsNode child, DirectoryEntry entry);

		#endregion IVfsNode members
	}
}
