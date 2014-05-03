/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.TinyCPUSimulator.TestSystem;
using Mosa.TinyCPUSimulator.x86.Adaptor;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class X86TestFixture : TestFixture
	{
		protected override BaseTestPlatform BasePlatform { get { return new TestPlatform(); } }
	}
}