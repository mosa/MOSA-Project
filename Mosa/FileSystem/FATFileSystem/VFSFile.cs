/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.FileSystem.VFS;

namespace Mosa.FileSystem.FATFileSystem
{
	public class VFSFile : NodeBase
	{
		protected uint fileCluster;
		protected uint directorySector;
		protected uint directoryIndex;

		public VFSFile(IFileSystem fs, uint fileCluster, uint directorySector, uint directoryIndex)
			: base(fs, VfsNodeType.File)
		{
			this.fileCluster = fileCluster;
			this.directorySector = directorySector;
			this.directoryIndex = directoryIndex;
		}

		public override IVfsNode Create(string name, VfsNodeType type, object settings)
		{
			// TODO
			throw new System.NotSupportedException("file write not supported");
		}

		public override IVfsNode Lookup(string name)
		{
			return null;	// Lookup() method doesn't make sense here
		}

		public override object Open(System.IO.FileAccess access, System.IO.FileShare sharing)
		{
			if (access == System.IO.FileAccess.Read)
				return new FATFileStream((FileSystem as VFSFileSystem).FAT, fileCluster, directorySector, directoryIndex);

			// TODO
			throw new System.NotSupportedException("file write not supported");
		}

		public override void Delete(IVfsNode child, DirectoryEntry dentry)
		{
			throw new System.ArgumentException(); // Delete() method doesn't make sense here
		}
	}
}