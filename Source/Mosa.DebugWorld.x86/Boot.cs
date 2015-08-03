// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using System.Runtime.InteropServices;

namespace Mosa.DebugWorld.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Boot
	{
		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static void Main()
		{
			Screen.Color = 0x0;
			Screen.Clear();
			Screen.GotoTop();
			Screen.Color = 0x0E;
			Screen.Write('>');
			SSE.Setup();

			if (Test())
				Screen.Write('X');


			var t = Test2();
			if (t.TestResult1)
				Screen.Write('O');

			if (Test3(t).TestResult2)
				Screen.Write('P');

			while (true) ;
		}

		public static bool Test()
		{
			uint address = 0x0B8050; //it's the display memory, but you can use any other adress, so far it's no critical area
			Mosa.Platform.Internal.x86.Native.Set8(address, 81); //set ascii 'Q'
			var num = Mosa.Platform.Internal.x86.Native.Get8(address); //get the 'Q' back

			#region COMPILER_BUG

			if (num < 32 && num >= 128) //COMPILER_BUG: This conditinal expression will not resolved correctly!
				return false;
			else
				return true;

			#endregion COMPILER_BUG
		}
		
		private static TestStruct Test2()
		{
			return TestStruct.Create();
		}

		private static TestStruct Test3(TestStruct t)
		{
			t.Limit = 0;
			return t;
		}

	}

	[StructLayout(LayoutKind.Explicit)]
	unsafe public struct TestStruct
	{
		[FieldOffset(0)]
		private ushort limitLow;

		[FieldOffset(2)]
		private ushort baseLow;

		[FieldOffset(4)]
		private byte baseMiddle;

		[FieldOffset(5)]
		private byte access;

		[FieldOffset(6)]
		private byte flags;

		public static TestStruct Create()
		{
			return new TestStruct() { limitLow = (ushort)0xFFFF };
		}

		public uint Limit
		{
			set
			{
				limitLow = (ushort)(0xFFFFFFFFU & 0xFFFF);
			}
		}

		public bool TestResult1
		{
			get
			{
				return limitLow == (ushort)(0xFFFF);
			}
		}

		public bool TestResult2
		{
			get
			{
				return limitLow == (ushort)(0xFFFFFFFFU & 0xFFFF);
			}
		}
	}
}
