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
			if (options.Emulator == EmulatorType.Qemu && options.ImageFormat == ImageFormat.VDI)
			{
				return "QEMU does not support the VDI image format";
			}

			if (options.Emulator == EmulatorType.Bochs && options.ImageFormat == ImageFormat.VDI)
			{
				return "Boches does not support the VDI image format";
			}

			if (options.Emulator == EmulatorType.Bochs && options.ImageFormat == ImageFormat.VMDK)
			{
				return "Boches does not support the VMDK image format";
			}

			if (options.Emulator == EmulatorType.VMware && options.ImageFormat == ImageFormat.IMG)
			{
				return "VMware does not support the IMG image format";
			}

			if (options.Emulator == EmulatorType.VMware && options.ImageFormat == ImageFormat.VDI)
			{
				return "VMware does not support the VHD image format";
			}

			return null;
		}

	}
}
