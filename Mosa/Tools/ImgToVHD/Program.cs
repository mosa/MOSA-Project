/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.IO;

namespace Mosa.Tools.ImgToVHD
{
	class Program
	{
		static int Main(string[] args)
		{
			if (args.Length < 2) {
				Console.WriteLine("ERROR: Missing arguments");
				Console.WriteLine("ImgToVHD <source img file> <destination vhd file>");
				return -1;
			}

			try {
				// Copy file - the first part of an IMG file is the same within the Fixed VHD file
				File.Copy(args[0], args[1], true);
				
				// Added footer

			}
			catch (Exception e) {
				Console.WriteLine("Error: " + e.ToString());
				return -1;
			}

			return 0;
		}
	}
}
