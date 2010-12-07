/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  
 */

namespace Mosa.Test.Runtime.CompilerFramework.CIL
{
	using MbUnit.Framework;

	[TestFixture]
	[Importance(Importance.Critical)]
	[Category(@"Basic types")]
	[Description(@"Tests support for the basic type System.Enum")]
	public class EnumFixture : CodeDomTestRunner
	{
		private static string CreateTestCode()
		{
			return @"
				public enum TestEnum
				{
					ItemA = 5,
					ItemB
				}

				public class TestClass
				{
					public static bool AMustBe5()
					{
						return 5 == (int)TestEnum.ItemA;
					}
				}
			" + Code.AllTestCode;
		}

		[Test]
		public void ItemAMustEqual5()
		{
			CodeSource = CreateTestCode();
			DoNotReferenceMscorlib = true;

			// Due to Code.NoStdLibDefinitions... :(
			UnsafeCode = true;

			Assert.IsTrue(Run<bool>("", "TestClass", "AMustBe5"));
		}
	}
}
