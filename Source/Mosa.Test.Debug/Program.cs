using Mosa.Compiler.MosaTypeSystem;
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
			var moduleLoader = new MosaModuleLoader();

			moduleLoader.AddPrivatePath(Directory.GetCurrentDirectory());

			moduleLoader.LoadModuleFromFile("Mosa.Test.AssemblyB.dll");
			moduleLoader.LoadModuleFromFile("Mosa.Test.AssemblyC.dll");
			moduleLoader.LoadModuleFromFile("Mosa.ClassLib.dll");

			var typeSystem = TypeSystem.Load(moduleLoader);

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