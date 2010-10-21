/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  
 */

namespace Test.Mosa.Runtime.CompilerFramework.CIL
{
	using MbUnit.Framework;

	[TestFixture]
	[Importance(Importance.Critical)]
	[Category(@"Basic types")]
	[Description(@"Tests support for the basic type System.String")]
	public class StringFixture : CodeDomTestRunner
	{
		private static string CreateTestCode(string value)
		{
			return @"
				public class TestClass
				{
					public static string valueA = @""" + value + @""";
					public static string valueB = @""" + value + @""";

					public static bool LengthMustMatch()
					{
						return " + value.Length + @" == valueA.Length;
					}

					public static bool FirstCharacterMustMatch()
					{
						return '" + value[0] + @"' == valueA[0];
					}

					public static bool LastCharacterMustMatch()
					{
						char ch = '\0';
						for (int index = 0; index < valueA.Length; index++)
						{
							ch = valueA[index];
						}

						return '" + value[value.Length - 1] + @"' == ch;
					}
				}

			"
			+ Code.ObjectClassDefinition
			+ Code.NoStdLibDefinitions;
		}

		private delegate bool B_V();

		[Test]
		public void MustProperlyCompileLdstrAndLengthMustMatch()
		{
			CodeSource = CreateTestCode(@"Foo");
			this.DoNotReferenceMsCorlib = true;
			UnsafeCode = true;

			Assert.IsTrue((bool)Run<B_V>("", "TestClass", "LengthMustMatch"));
		}

		[Test]
		public void FirstCharacterMustMatchInStrings()
		{
			CodeSource = CreateTestCode(@"Foo");
			this.DoNotReferenceMsCorlib = true;
			UnsafeCode = true;

			Assert.IsTrue((bool)Run<B_V>("", "TestClass", "FirstCharacterMustMatch"));
		}

		[Test]
		public void LastCharacterMustMatchInStrings()
		{
			CodeSource = CreateTestCode(@"Foo");
			this.DoNotReferenceMsCorlib = true;
			UnsafeCode = true;

			Assert.IsTrue((bool)Run<B_V>("", "TestClass", "LastCharacterMustMatch"));
		}
	}
}
