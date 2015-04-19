/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using Mosa.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Mosa.Platform.Internal.x86
{
	public unsafe static class GC
	{
		private static bool isSetup = false;

		// This method will be plugged by "Mosa.Kernel.x86.KernelMemory.AllocateMemory"
		private static uint AllocateMemory(uint size)
		{
			return 0;
		}

		public static void* AllocateObject(uint size)
		{
			Debug.Assert(isSetup, "GC not setup yet!");
			// TODO: GC
			return (void*)AllocateMemory(size);
		}

		public static void Setup()
		{
			// TODO: GC
			isSetup = true;
		}
	}
}