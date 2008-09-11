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
    /// <summary>
    /// 
    /// </summary>
	public class VFSFile : NodeBase
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
        /// 
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="fileCluster"></param>
        /// <param name="directorySector"></param>
        /// <param name="directoryIndex"></param>
		public VFSFile(IFileSystem fs, uint fileCluster, uint directorySector, uint directoryIndex)
			: base(fs, VfsNodeType.File)
		{
			this.fileCluster = fileCluster;
			this.directorySector = directorySector;
			this.directoryIndex = directoryIndex;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
		public override IVfsNode Create(string name, VfsNodeType type, object settings)
		{
			// TODO
			throw new System.NotSupportedException("file write not supported");
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
		public override IVfsNode Lookup(string name)
		{
			return null;	// Lookup() method doesn't make sense here
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="access"></param>
        /// <param name="sharing"></param>
        /// <returns></returns>
		public override object Open(System.IO.FileAccess access, System.IO.FileShare sharing)
		{
			if (access == System.IO.FileAccess.Read)
				return new FATFileStream((FileSystem as VFSFileSystem).FAT, fileCluster, directorySector, directoryIndex);

			// TODO
			throw new System.NotSupportedException("file write not supported");
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="child"></param>
        /// <param name="dentry"></param>
		public override void Delete(IVfsNode child, DirectoryEntry dentry)
		{
			throw new System.ArgumentException(); // Delete() method doesn't make sense here
		}
	}
}