// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.MultibootSpecification;
using System;

namespace Mosa.Kernel.BareMetal
{
	public static class Multiboot
	{
		/// <summary>
		/// Location of the Multiboot Structure
		/// </summary>
		public static MultibootV1 MultibootV1 { get; private set; }

		public static void Setup(IntPtr location, uint magic)
		{
			MultibootV1 = new MultibootV1(magic == MultibootV1.MultibootMagic ? location : IntPtr.Zero);
		}

		/// <summary>
		/// Gets a value indicating whether multiboot is available.
		/// </summary>
		public static bool IsAvailable => MultibootV1.IsAvailable;
	}
}
