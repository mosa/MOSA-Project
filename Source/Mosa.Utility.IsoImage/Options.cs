/*
* (c) 2012 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System.Collections.Generic;

namespace Mosa.Utility.IsoImage
{
	

	/// <summary>
	/// 
	/// </summary>
	public class Options
	{
		public short BootLoadSize = 4;
		public bool BootInfoTable = false;
		public string VolumeLabel = string.Empty;
		public bool Pedantic = false;
		public string IsoFileName = null;
		public string BootFileName = null;

		public List<string> IncludeFiles = new List<string>();

		public Options()
		{
		}


	}
}
