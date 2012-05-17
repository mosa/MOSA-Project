/*
 * Copyright (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Author(s):
 *  Royce Mitchell III (royce3) <royce3 [at] gmail [dot] com>
 */

using System.Collections.Generic;

namespace Mosa.Utility.IsoImage
{
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

}
