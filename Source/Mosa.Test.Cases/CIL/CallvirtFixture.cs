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
	[Category(@"Method calls")]
	[Description(@"Tests proper method table building and virtual call support.")]
	public class CallvirtFixture : TestCompilerAdapter
	{
		public static readonly string TestCode = @"
			public class Base
			{
				public virtual int Test()
				{
					return 5;
				}
			}

			public class Derived : Base
			{
				public static readonly Derived Instance = new Derived();

				public static int STest()
				{
					return Instance.Test();
				}

				public static int STestBaseCall()
				{
					return Instance.TestBaseCall();
				}

				public override int Test()
				{
					return 7;
				}

				public int TestBaseCall()
				{
					return this.Test() + base.Test();
				}
			}
		";

		[Test]
		public void TestVirtualCall()
		{
			settings.CodeSource = TestCode;
			int result = Run<int>(string.Empty, @"Derived", @"STest");
			Assert.AreEqual(7, result);
		}

		[Test]
		public void TestBaseCall()
		{
			settings.CodeSource = TestCode;
			int result = Run<int>(string.Empty, @"Derived", @"STestBaseCall");
			Assert.AreEqual(12, result);
		}

	}
}
