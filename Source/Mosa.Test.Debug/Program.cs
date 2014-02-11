﻿using Mosa.Compiler.MosaTypeSystem;
using Mosa.Test.Collection.MbUnit;
using System.IO;

namespace Mosa.Test.Debug
{
	internal class Program
	{
		public static void Main()
		{
			Test0();
			//Test4();
			//Test3();
			//Test2();
			//Test1();
			return;
		}

		private static void Test0()
		{
			var assemblyLoader = new MosaAssemblyLoader();

			assemblyLoader.AddPrivatePath(Directory.GetCurrentDirectory());

			assemblyLoader.LoadModule("Mosa.Test.AssemblyB.dll");
			assemblyLoader.LoadModule("Mosa.Test.AssemblyC.dll");
			assemblyLoader.LoadModule("Mosa.ClassLib.dll");

			var typeSystem = new TypeSystem();

			typeSystem.Load(assemblyLoader);

			return;
		}

		private static void Test4()
		{
			var fixture = new UInt32Fixture();

			fixture.AddU4U4(1, 2);
		}

		private static void Test3()
		{
			var fixture = new UInt64Fixture();

			fixture.DivU8U8(18446744073709551615, 4294967294);
		}

		private static void Test2()
		{
			var fixture = new EnumFixture();

			fixture.ItemAMustEqual5();
		}

		private static void Test1()
		{
			var fixture = new BoxingFixture();

			fixture.BoxU8(100);
		}
	}
}