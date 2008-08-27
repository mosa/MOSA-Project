/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.FileSystem.FATFileSystem
{
	public class FATSettings : SettingsBase
	{
		public FATType FatType;
		public string VolumeLabel;
		public byte[] SerialID;

		public FATSettings()
		{
			this.FatType = FATType.FAT16;	// default
			this.VolumeLabel = string.Empty;
			this.SerialID = new byte[0];
		}
	}
}