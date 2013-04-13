/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Bruce Markham (<mailto:illuminus86@gmail.com>)
 *
 */

using System;
using System.Runtime.InteropServices;
using MbUnit.Framework;

using Mosa.Test.System;

namespace Mosa.Test.Cases.OLD
{
	[TestFixture]
	public unsafe class Stind : TestCompilerAdapter
	{
		#region DereffedVoidPtrAssign

		private static string CreateDereferencedVoidPointerAssignmentTestCode(string pointerType, string methodNameSuffix)
		{
			return @"
				static class Test {
				public unsafe static bool DereffedVoidPtrAssign_" + methodNameSuffix + "(" + pointerType + @" value, void* address)
				{
					*(" + pointerType + @"*)address = value;
					return true;
				}
				}";
		}

		#region I1

		[Row(sbyte.MinValue)]
		[Row(-1)]
		[Row(0)]
		[Row(1)]
		[Row(sbyte.MaxValue)]
		[Test]
		public unsafe void DereffedVoidPtrAssign_I1(sbyte a)
		{
			settings.CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("sbyte", "I1");

			void* address = (void*)Marshal.AllocHGlobal(sizeof(sbyte));
			Run<bool>(string.Empty, "Test", "DereffedVoidPtrAssign_I1", a, (IntPtr)address);
			bool success = (*(sbyte*)address == a);
			Marshal.FreeHGlobal((IntPtr)address);

			Assert.IsTrue(success, "DereffedVoidPtrAssign_I1");
		}

		#endregion I1

		#region I2

		[Row(short.MinValue)]
		[Row(-1)]
		[Row(0)]
		[Row(1)]
		[Row(short.MaxValue)]
		[Test]
		public unsafe void DereffedVoidPtrAssign_I2(short a)
		{
			settings.CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("short", "I2");

			void* address = (void*)Marshal.AllocHGlobal(sizeof(short));

			Run<bool>(string.Empty, "Test", "DereffedVoidPtrAssign_I2", a, (IntPtr)address);
			bool success = (*(short*)address == a);
			Marshal.FreeHGlobal((IntPtr)address);

			Assert.IsTrue(success, "DereffedVoidPtrAssign_I2");
		}

		#endregion I2

		#region I4

		[Row(int.MinValue)]
		[Row(-1)]
		[Row(0)]
		[Row(1)]
		[Row(int.MaxValue)]
		[Test]
		public unsafe void DereffedVoidPtrAssign_I4(int a)
		{
			settings.CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("int", "I4");

			void* address = (void*)Marshal.AllocHGlobal(sizeof(int));
			Run<bool>(string.Empty, "Test", "DereffedVoidPtrAssign_I4", a, (IntPtr)address);
			bool success = (*(int*)address == a);
			Marshal.FreeHGlobal((IntPtr)address);

			Assert.IsTrue(success, "DereffedVoidPtrAssign_I4");
		}

		#endregion I4

		#region I8

		[Row(long.MinValue)]
		[Row(-1L)]
		[Row(0L)]
		[Row(1L)]
		[Row(long.MaxValue)]
		[Test]
		public unsafe void DereffedVoidPtrAssign_I8(long a)
		{
			settings.CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("long", "I8");

			void* address = (void*)Marshal.AllocHGlobal(sizeof(long));
			Run<bool>(string.Empty, "Test", "DereffedVoidPtrAssign_I8", a, (IntPtr)address);
			bool success = (*(long*)address == a);
			Marshal.FreeHGlobal((IntPtr)address);

			Assert.IsTrue(success, "DereffedVoidPtrAssign_I8");
		}

		#endregion I8

		#region U1

		[Row(byte.MinValue)]
		[Row(125)]
		[Row(127)]
		[Row(byte.MaxValue)]
		[Test]
		public unsafe void DereffedVoidPtrAssign_U1(byte a)
		{
			settings.CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("byte", "U1");

			void* address = (void*)Marshal.AllocHGlobal(sizeof(byte));
			Run<bool>(string.Empty, "Test", "DereffedVoidPtrAssign_U1", a, (IntPtr)address);
			bool success = (*(byte*)address == a);
			Marshal.FreeHGlobal((IntPtr)address);

			Assert.IsTrue(success, "DereffedVoidPtrAssign_U1");
		}

		#endregion U1

		#region U2

		[Row(ushort.MinValue)]
		[Row((ushort.MaxValue / 2) - 1)]
		[Row((ushort.MaxValue / 2) + 1)]
		[Row(1)]
		[Row(ushort.MaxValue)]
		[Test]
		public unsafe void DereffedVoidPtrAssign_U2(ushort a)
		{
			settings.CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("ushort", "U2");

			void* address = (void*)Marshal.AllocHGlobal(sizeof(ushort));
			Run<bool>(string.Empty, "Test", "DereffedVoidPtrAssign_U2", a, (IntPtr)address);
			bool success = (*(ushort*)address == a);
			Marshal.FreeHGlobal((IntPtr)address);

			Assert.IsTrue(success, "DereffedVoidPtrAssign_U2");
		}

		#endregion U2

		#region U4

		[Row(uint.MinValue)]
		[Row(uint.MaxValue / 2)]
		[Row((uint.MaxValue / 2) + 1)]
		[Row(uint.MaxValue)]
		[Test]
		public void DereffedVoidPtrAssign_U4(uint a)
		{
			settings.CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("uint", "U4");

			void* address = (void*)Marshal.AllocHGlobal(sizeof(uint));
			Run<bool>(string.Empty, "Test", "DereffedVoidPtrAssign_U4", a, (IntPtr)address);
			bool success = (*(uint*)address == a);
			Marshal.FreeHGlobal((IntPtr)address);

			Assert.IsTrue(success, "DereffedVoidPtrAssign_U4");
		}

		#endregion U4

		#region U8

		[Row(ulong.MinValue)]
		[Row((ulong.MaxValue / 2) + 1)]
		[Row(ulong.MaxValue)]
		[Test]
		public unsafe void DereffedVoidPtrAssign_U8(ulong a)
		{
			settings.CodeSource = CreateDereferencedVoidPointerAssignmentTestCode("ulong", "U8");

			void* address = (void*)Marshal.AllocHGlobal(sizeof(ulong));
			Run<bool>(string.Empty, "Test", "DereffedVoidPtrAssign_U8", a, (IntPtr)address);
			bool success = (*(ulong*)address == a);
			Marshal.FreeHGlobal((IntPtr)address);

			Assert.IsTrue(success, "DereffedVoidPtrAssign_U8");
		}

		#endregion U8

		#endregion DereffedVoidPtrAssign
	}
}