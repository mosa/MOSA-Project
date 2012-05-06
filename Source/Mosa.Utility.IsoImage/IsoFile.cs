/*
 * Copyright (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Author(s):
 *  Royce Mitchell III (royce3) <royce3 [at] gmail [dot] com>
 */

using System;
using System.IO;

namespace Mosa.Utility.IsoImage
{

	internal class IsoFile : IsoEntry
	{
		[FlagsAttribute]
		public enum Flags
		{
			BootFile = 1 << 0,
			BootInfoTable = 1 << 1
		}

		public FileInfo fileInfo;
		public Flags flags;

		public IsoFile(FileInfo fi)
		{
			fileInfo = fi;
			flags = 0;
		}

		public bool BootFile
		{
			get
			{
				return (flags & Flags.BootFile) != 0;
			}
			set
			{
				if (value)
					flags |= Flags.BootFile;
				else
					flags &= ~Flags.BootFile;
			}
		}

		public bool BootInfoTable
		{
			get
			{
				return (flags & Flags.BootInfoTable) != 0;
			}
			set
			{
				if (value)
					flags |= Flags.BootInfoTable;
				else
					flags &= ~Flags.BootInfoTable;
			}
		}

		public override bool IsFile { get { return true; } }
	}

}
