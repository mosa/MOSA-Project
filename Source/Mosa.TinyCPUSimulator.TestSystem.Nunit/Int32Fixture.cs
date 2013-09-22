/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Test.Collection;
using NUnit.Framework;

namespace Mosa.TinyCPUSimulator.TestSystem.Nunit
{
	[TestFixture]
	internal class Int32Fixture
	{
		private TestCompiler testCompiler;

		[SetUp]
		public void Setup()
		{
			if (testCompiler != null)
				return;

			testCompiler = new TestCompiler(new X86Platform());
			testCompiler.EnableSSA = true;
			testCompiler.EnableSSAOptimizations = true;
		}

		[Test]
		[TestCase((int)0, (int)0)]
		[TestCase((int)1, (int)1)]
		public void AddI4I4(int a, int b)
		{
			Assert.AreEqual(Int32Tests.AddI4I4(a, b), testCompiler.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", a, b));
		}
	}
}