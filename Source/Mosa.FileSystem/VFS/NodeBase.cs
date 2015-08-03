// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.FileSystem.VFS
{
	/// <summary>
	/// Provides a default implementation for INodes. A filesystem implementation
	/// may choose to derive from BasicNode to receive a default implementation of
	/// the interface.
	/// </summary>
	public abstract class NodeBase : IVfsNode
	{
		#region Data members

		/// <summary>
		/// Holds the filesystem of the node.
		/// </summary>
		private IFileSystem fs;

		/// <summary>
		/// Holds the type of the IVfsNode represented by this instance.
		/// </summary>
		private VfsNodeType type;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NodeBase"/> class.
		/// </summary>
		/// <param name="fs">The fs.</param>
		/// <param name="type">The type.</param>
		protected NodeBase(IFileSystem fs, VfsNodeType type)
		{
			this.fs = fs;
			this.type = type;
		}

		#endregion Construction

		#region IVfsNode members

		/// <summary>
		/// Gets the file system.
		/// </summary>
		/// <value>The file system.</value>
		public IFileSystem FileSystem { get { return fs; } }

		/// <summary>
		/// Returns the type of the node.
		/// </summary>
		/// <value></value>
		public VfsNodeType NodeType { get { return type; } }

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
		public abstract object Open(System.IO.FileAccess access, System.IO.FileShare sharing);

		/// <summary>
		/// Deletes the specified child.
		/// </summary>
		/// <param name="child">The child.</param>
		/// <param name="entry">The entry.</param>
		public abstract void Delete(IVfsNode child, DirectoryEntry entry);

		#endregion IVfsNode members
	}
}