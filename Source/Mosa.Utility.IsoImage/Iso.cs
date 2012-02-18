/*
 * Copyright (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Author(s):
 *  Royce Mitchell III (royce3) <royce3 [at] gmail [dot] com>
 */

#define ROCKRIDGE

using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Utility.IsoImage
{
	internal class IsoEntry
	{
		public string Name;
		public int DataBlock;
		public int DataLength;
		public byte RrFlags;

		public IsoEntry()
		{
			DataBlock = 0;
			DataLength = 0;
			this.RrFlags = 0;
		}

		public virtual bool IsFile { get { return false; } }
		public virtual bool IsFolder { get { return false; } }
	}

	internal class IsoFolder : IsoEntry
	{
		//public DirectoryInfo dirInfo;
		public Dictionary<string, IsoEntry> entries;
		public short PathTableEntry;

		public IsoFolder()
		{
			entries = new Dictionary<string, IsoEntry>();
		}

		public void AddEntry(string path, IsoEntry e)
		{
			var tmp = path.IndexOf('/');
			if (tmp >= 0)
			{
				string subpath = path.Substring(tmp + 1).Trim();
				path = path.Substring(0, tmp).Trim();
			}
		}

		public override bool IsFolder { get { return true; } }
	}

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
