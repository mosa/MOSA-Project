/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 *
 */


using System;
using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework
{
	[TestFixture]
	//[Parallelizable]
	public class ExceptionHandlingFixture : CodeDomTestRunner
	{
		private static string CreateTestCode()
		{
			return @"
				using System;

				static class Test
				{
					public static bool CatchException(int value)
					{
						try
						{
							throw new Exception ();
						}
						catch (Exception exception)
						{
							return true;
						}
						return false;
					}
				}"
				+ Code.AllTestCode;
		}

		[Test]
		[Row(1)]
		public void CatchException(int value)
		{
			CodeSource = CreateTestCode();
			DoNotReferenceMscorlib = true;
			UnsafeCode = true;
			Assert.IsTrue(Run<bool>("", "Test", "CatchException", value));
		}
	}
}