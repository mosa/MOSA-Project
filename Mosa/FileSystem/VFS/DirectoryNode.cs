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
using System.Collections;
using System.IO;


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
		private ArrayList nodes;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the DirectoryNode object.
		/// </summary>
		/// <param name="fs">The filesystem, which owns the node.</param>
		public DirectoryNode(IFileSystem fs)
			: base(fs, VfsNodeType.Directory)
		{
			nodes = new ArrayList();
		}

		#endregion // Construction

		#region IVfsNode members

		public override IVfsNode Create(string name, VfsNodeType nodeType, object settings)
		{
			// FIXME: throw new NotImplementedException();
			return null;
		}

		public override IVfsNode Lookup(string name)
		{
			// FIXME: Lookup the node in the members
			return null;
		}

		public override object Open(FileAccess access, FileShare sharing)
		{
			// FIXME: return something like: new System.IO.DirectoryInfo(VirtualFileSystem.GetPath(this));
			//throw new NotImplementedException();
			return null;
		}

		public override void Delete(IVfsNode child, DirectoryEntry dentry)
		{
			// FIXME: throw new NotImplementedException();
		}

		#endregion // IVfsNode members
	}
}