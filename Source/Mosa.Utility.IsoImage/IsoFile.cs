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

#pragma warning disable 169, 414, 219

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

		//public IncludeFile IncludeFile;

		public FileInfo fileInfo;
		public Flags flags;

		public long Length { get { return fileInfo.Length; } }

		public bool Hidden { get { return (fileInfo.Attributes & FileAttributes.Hidden) != 0; } }

		public byte[] Content
		{
			get
			{
				var b = new byte[Length];

				var datastream = fileInfo.OpenRead();

				datastream.Read(b, 0, (int)Length);

				return b;
			}
		}

		public IsoFile(FileInfo fi, string filename)
		{
			fileInfo = fi;
			flags = 0;
			Name = filename;
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