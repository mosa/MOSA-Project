// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.TinyCPUSimulator.TestSystem;
using Mosa.TinyCPUSimulator.x86.Adaptor;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class X86TestFixture : TestFixture
	{
		private static BaseTestPlatform testPlatform = new TestPlatform();

		protected override BaseTestPlatform BasePlatform { get { return testPlatform; } }
	}
}
