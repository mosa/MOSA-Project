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

namespace Mosa.Test.Runtime.CompilerFramework
{
	[TestFixture]
	//[Parallelizable]
	public class ExceptionHandlingFixture : TestCompilerAdapter
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
				}" + Code.AllTestCode;
		}

		[Test]
		[Row(1)]
		public void CatchException(int value)
		{
			compiler.CodeSource = CreateTestCode();
			compiler.DoNotReferenceMscorlib = true;
			compiler.UnsafeCode = true;
			Assert.IsTrue(compiler.Run<bool>(string.Empty, "Test", "CatchException", value));
		}
	}
}