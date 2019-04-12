// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Provides indirect access to a block of memory
	/// </summary>
	public class PointerTest
	{
		public static uint PointerTest1()
		{
			var pointer = new Pointer(new IntPtr(1));

			return pointer.Read32(10);
		}

		public static uint PointerTest2(Pointer pointer)
		{
			return pointer.Read32(10);
		}

		public static uint PointerTest3()
		{
			var pointer = new ConstrainedPointer(new IntPtr(1), 100);

			return pointer.Read32(10);
		}


		public static uint PointerTest4(ConstrainedPointer pointer)
		{
			return pointer.Read32(10);
		}
	}
}
