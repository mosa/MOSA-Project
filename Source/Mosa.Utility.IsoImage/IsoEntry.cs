/*
 * Copyright (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Author(s):
 *  Royce Mitchell III (royce3) <royce3 [at] gmail [dot] com>
 */


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

}
