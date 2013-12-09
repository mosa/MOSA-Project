using Mosa.Compiler.Metadata.Loader;
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
			var resolver = new MosaTypeResolver();
			var loader = new TypeLoader(resolver);

			var mscorlib = new PortableExecutableImage(new FileStream(@"mscorlib.dll", FileMode.Open, FileAccess.Read));
			var assemblyA = new PortableExecutableImage(new FileStream(@"Mosa.Test.AssemblyA.dll", FileMode.Open, FileAccess.Read));
			var assemblyB = new PortableExecutableImage(new FileStream(@"Mosa.Test.AssemblyB.dll", FileMode.Open, FileAccess.Read));
			var assemblyC = new PortableExecutableImage(new FileStream(@"Mosa.Test.AssemblyC.dll", FileMode.Open, FileAccess.Read));
			var classLib = new PortableExecutableImage(new FileStream(@"Mosa.ClassLib.dll", FileMode.Open, FileAccess.Read));
			var testCollection = new PortableExecutableImage(new FileStream(@"Mosa.Test.Collection.dll", FileMode.Open, FileAccess.Read));

			loader.Load(mscorlib);
			loader.Load(assemblyA);
			loader.Load(assemblyB);
			loader.Load(assemblyC);
			loader.Load(classLib);
			loader.Load(testCollection);

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