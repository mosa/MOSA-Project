/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Utility.Launcher
{
	public static class CheckOptions
	{
		public static string Verify(Options options)
		{
			if (options.Emulator == EmulatorType.Boches && options.ImageFormat == ImageFormat.VMDK)
			{
				return "Boches does not support the VMDK image format";
			}

			//TODO: Add more checks

			return null;
		}

	}
}
