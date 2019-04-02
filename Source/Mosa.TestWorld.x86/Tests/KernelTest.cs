// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using System.Collections.Generic;

namespace Mosa.TestWorld.x86.Tests
{
	public class KernelTest
	{
		private readonly string testName = string.Empty;

		protected delegate bool TestMethod();

		protected List<TestMethod> testMethods = new List<TestMethod>();

		protected KernelTest(string testName)
		{
			this.testName = testName;
		}

		public void PrintResult(bool flag)
		{
			var color = Screen.Color;

			if (flag)
			{
				Screen.Color = ScreenColor.White;
				Screen.Write("+");
			}
			else
			{
				Screen.Color = ScreenColor.Red;
				Screen.Write("X");
			}

			Screen.Color = color;
		}

		/// <summary>
		/// Runs the tests.
		/// </summary>
		public static void RunTests()
		{
			Screen.Color = ScreenColor.Yellow;
			Screen.Write('[');
			Screen.Color = ScreenColor.White;
			Screen.Write("Tests");
			Screen.Color = ScreenColor.Yellow;
			Screen.Write(']');
			Screen.WriteLine();
			Screen.WriteLine();
			Screen.Color = ScreenColor.Yellow;

			var delegateTest = new DelegateTest();
			var stringTest = new StringTest();
			var interfaceTest = new InterfaceTest();
			var genericsTest = new GenericTest();
			var generics2Test = new Generic2Test();
			var isInstanceTest = new IsInstanceTest();
			var exceptionTest = new ExceptionTest();
			var plugTestTest = new PlugTestTest();
			var compareTest = new ComparisonTest();
			var simpleTest = new OptimizationTest();
			var reflectionTest = new ReflectionTest();
			var arrayTest = new ArrayTest();
			var int64Test = new Int64Test();
			var otherTest = new OtherTest();

			otherTest.Test();
			int64Test.Test();
			delegateTest.Test();
			stringTest.Test();
			interfaceTest.Test();
			genericsTest.Test();
			generics2Test.Test();
			isInstanceTest.Test();
			exceptionTest.Test();
			plugTestTest.Test();
			compareTest.Test();
			simpleTest.Test();
			arrayTest.Test();
			reflectionTest.Test();

			Screen.WriteLine();
			Screen.WriteLine();
		}

		public void Test()
		{
			Screen.Color = ScreenColor.Yellow;

			Screen.Write(testName);
			Screen.Write(':');

			foreach (var node in testMethods)
			{
				PrintResult(node());
			}

			Screen.Write(' ');
		}
	}
}
