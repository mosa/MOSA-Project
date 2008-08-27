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
	public sealed class SymbolicLinkNode : NodeBase
	{
		#region Data members

		/// <summary>
		/// The target of the symbolic link.
		/// </summary>
		private string target;

		#endregion // Data members

		#region Construction

		public SymbolicLinkNode(IFileSystem fs, string target)
			: base(fs, VfsNodeType.SymbolicLink)
		{
			this.target = target;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Retrieves the target path of the symbolic link.
		/// </summary>
		public string Target
		{
			get
			{
				return target;
			}
		}

		#endregion // Properties

		#region IVfsNode Members

		public override IVfsNode Create(string name, VfsNodeType type, object settings)
		{
			// Pass this request to the link target node.
			// FIXME: throw new NotImplementedException();
			return null;
		}

		public override object Open(FileAccess access, FileShare sharing)
		{
			// FIXME:
			// - Pass this request to the link target node?
			// - Do we really want this?
			// FIXME: throw new NotImplementedException();
			return null;
		}

		public override void Delete(IVfsNode child, DirectoryEntry dentry)
		{
			// FIXME: Delete the symbolic link from the filesystem, after all names have been dropped.
			throw new NotSupportedException();
		}

		#endregion // IVfsNode Members
	}
}