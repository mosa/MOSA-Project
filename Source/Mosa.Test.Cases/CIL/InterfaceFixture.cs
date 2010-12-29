/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *
 */

using MbUnit.Framework;

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Cases.CIL
{

	[TestFixture]
	[Importance(Importance.Critical)]
	[Category(@"Compiler")]
	[Description(@"Tests support for interfaces.")]
	public class InterfaceFixture : TestCompilerAdapter
	{
		private static string CreateTestCode()
		{
			return @"
				public interface InterfaceA
				{
					int A();
				}

				public interface InterfaceB
				{
					int A();
					int B();
				}

				public class TestClass : InterfaceA, InterfaceB
				{
					public int A()
					{
						return 1;
					}

					int InterfaceB.A()
					{
						return 2;
					}

					public int B()
					{
						return 3;
					}

					public static bool MustCompileWithInterfaces()
					{
						return true;
					}

					public static bool MustReturn3FromB()
					{
						TestClass tc = new TestClass();
						bool result = tc.B() == 3;
						InterfaceB b = tc;
						result = result & (b.B() == 3);
						return result;
					}
				}
			";
		}

		[Test]
		public void MustCompileInterfaces()
		{
			settings.CodeSource = CreateTestCode();
			Assert.IsTrue(Run<bool>(string.Empty, "TestClass", "MustCompileWithInterfaces"));
		}

		[Test]
		public void MustReturn3FromB()
		{
			settings.CodeSource = CreateTestCode();
			Assert.IsTrue(Run<bool>(string.Empty, "TestClass", "MustReturn3FromB"));
		}
	}
}
