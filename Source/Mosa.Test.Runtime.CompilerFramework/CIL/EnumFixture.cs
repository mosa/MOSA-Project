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
	public class EnumFixture : TestCompilerAdapter
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
			";
		}

		[Test]
		public void ItemAMustEqual5()
		{
			settings.CodeSource = CreateTestCode();
			Assert.IsTrue(Run<bool>(string.Empty, "TestClass", "AMustBe5"));
		}
	}
}
