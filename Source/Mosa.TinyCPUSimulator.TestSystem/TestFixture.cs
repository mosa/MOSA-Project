/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator.TestSystem
{
	public class TestFixture
	{
		private static TestCompiler testCompiler;

		protected TestCompiler TestCompiler
		{
			get
			{
				if (testCompiler == null)
				{
					testCompiler = new TestCompiler(new X86Platform());
					testCompiler.EnableSSA = true;
					testCompiler.EnableSSAOptimizations = true;
				}

				return testCompiler;
			}
		}
	}
}