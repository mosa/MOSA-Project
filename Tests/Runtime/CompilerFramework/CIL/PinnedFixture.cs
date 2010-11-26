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

using Test.Mosa.Runtime.CompilerFramework;

namespace Test.Mosa.Runtime.CompilerFramework.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	[Category(@"Memory Model")]
	[Description(@"Tests support for pinning variables in memory.")]
	public class PinnedFixture : TestFixtureBase
	{
		private static string CreateTestCode()
		{
			return @"
				public class PinsAMemberClass
				{
					private int length;

					public unsafe void PinsAVariable()
					{
						fixed (int *pLength = &this.length)
						{
							char* pChars = (char*)(pLength + 1);
							*(pChars) = (char)0;
						}
					}
				}
			"
			+ Code.AllTestCode;
		}

		[Test]
		public void MustCompileCodePinningVariables()
		{
			CodeSource = CreateTestCode();

			Assert.DoesNotThrow(() => this.CompileTestCode());
		}
	}
}
