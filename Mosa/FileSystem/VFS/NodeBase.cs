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

		#endregion // Data members

		#region Construction
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="type"></param>
		protected NodeBase(IFileSystem fs, VfsNodeType type)
		{
			this.fs = fs;
			this.type = type;
		}

		#endregion // Construction

		#region IVfsNode members
        /// <summary>
        /// 
        /// </summary>
		public IFileSystem FileSystem { get { return fs; } }

        /// <summary>
        /// 
        /// </summary>
		public VfsNodeType NodeType { get { return type; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
		public abstract IVfsNode Create(string name, VfsNodeType type, object settings);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
		public virtual IVfsNode Lookup(string name)
		{
			return null;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="access"></param>
        /// <param name="sharing"></param>
        /// <returns></returns>
		public abstract object Open(FileAccess access, FileShare sharing);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="child"></param>
        /// <param name="entry"></param>
		public abstract void Delete(IVfsNode child, DirectoryEntry entry);

		#endregion // IVfsNode members
	}
}
