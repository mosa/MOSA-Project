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

namespace Mosa.Tools.MakeFAT
{
	class Program
	{
		static int Main(string[] args)
		{
			if (args.Length < 3) {
				Console.WriteLine("ERROR: Missing arguments");
				Console.WriteLine("MakeFAT -s <size in blocks> [-mbr <master boot record>] <destination img file>");
				return -1;
			}

			try {
				// TODO: 
			}
			catch (Exception e) {
				Console.WriteLine("Error: " + e.ToString());
				return -1;
			}

			return 0;
		}
	}
}
