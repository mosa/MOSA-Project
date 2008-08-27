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
	public class VFSFileSystem : IFileSystemService, IFileSystem
	{
		protected FAT fat;

		public FAT FAT { get { return fat; } }

		public VFSFileSystem(FAT fat)
		{
			this.fat = fat;
		}

		public object SettingsType { get { return fat.SettingsType; } }

		public bool Mount()
		{
			return true;
		}

		public bool Format(SettingsBase settings)
		{
			return (fat.Format(((FATSettings)settings)));
		}

		public bool IsReadOnly { get { return fat.IsReadOnly; } }

		private VFSDirectory root;

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
