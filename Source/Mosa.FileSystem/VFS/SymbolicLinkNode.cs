// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.FileSystem.VFS
{
	/// <summary>
	///
	/// </summary>
	public sealed class SymbolicLinkNode : NodeBase
	{
		#region Data Members

		/// <summary>
		/// The target of the symbolic link.
		/// </summary>
		private readonly string target;

		#endregion Data Members

		#region Construction

		/// <summary>
		///
		/// </summary>
		/// <param name="fs"></param>
		/// <param name="target"></param>
		public SymbolicLinkNode(IFileSystem fs, string target)
			: base(fs, VfsNodeType.SymbolicLink)
		{
			this.target = target;
		}

		#endregion Construction

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

		#endregion Properties

		#region IVfsNode Members

		/// <summary>
		///
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="settings"></param>
		/// <returns></returns>
		public override IVfsNode Create(string name, VfsNodeType type, object settings)
		{
			// Pass this request to the link target node.
			// FIXME: throw new NotImplementedException();
			return null;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="access"></param>
		/// <param name="sharing"></param>
		/// <returns></returns>
		public override object Open(System.IO.FileAccess access, System.IO.FileShare sharing)
		{
			// FIXME:
			// - Pass this request to the link target node?
			// - Do we really want this?
			// FIXME: throw new NotImplementedException();
			return null;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="child"></param>
		/// <param name="dentry"></param>
		public override void Delete(IVfsNode child, DirectoryEntry dentry)
		{
			// FIXME: Delete the symbolic link from the file system, after all names have been dropped.
			throw new System.NotSupportedException();
		}

		#endregion IVfsNode Members
	}
}
