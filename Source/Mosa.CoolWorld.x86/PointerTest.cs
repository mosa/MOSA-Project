// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using System;

namespace Mosa.CoolWorld.x86
{
	/// <summary>
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
