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

namespace Mosa.Test.Cases.OLD.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	[Category(@"Memory Model")]
	public class PinnedFixture : TestCompilerAdapter
	{

		static readonly string TestCode = @"
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
		";

		[Test]
		public void MustCompileCodePinningVariables()
		{
			settings.CodeSource = TestCode;
			Assert.DoesNotThrow(() => CompileTestCode());
		}
	}
}
