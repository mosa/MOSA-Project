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
	public class VFSFileSystem : IFileSystemService, IFileSystem
	{
        /// <summary>
        /// 
        /// </summary>
		protected FAT fat;

        /// <summary>
        /// 
        /// </summary>
		public FAT FAT 
        { 
            get { return fat; } 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fat"></param>
		public VFSFileSystem(FAT fat)
		{
			this.fat = fat;
		}

        /// <summary>
        /// 
        /// </summary>
		public object SettingsType { get { return fat.SettingsType; } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public bool Mount()
		{
			return true;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
		public bool Format(SettingsBase settings)
		{
			return (fat.Format(((FATSettings)settings)));
		}

        /// <summary>
        /// 
        /// </summary>
		public bool IsReadOnly 
        { 
            get { return fat.IsReadOnly; } 
        }

        /// <summary>
        /// 
        /// </summary>
		private VFSDirectory root;

        /// <summary>
        /// 
        /// </summary>
		public IVfsNode Root
		{
			get
			{
				if (root == null)
					root = new VFSDirectory(this, 0);

				return root;
			}
		}

	}
}
