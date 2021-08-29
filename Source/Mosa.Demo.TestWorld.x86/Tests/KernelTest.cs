// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using System.Collections.Generic;

namespace Mosa.Demo.TestWorld.x86.Tests
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

			new StringTest().Test();
			new ComparisonTest().Test();
			new OptimizationTest().Test();
			new Int64Test().Test();
			new DelegateTest().Test();
			new IsInstanceTest().Test();
			new PlugTestTest().Test();
			new GenericTest().Test();
			new Generic2Test().Test();
			new InterfaceTest().Test();
			new OtherTest().Test();
			new ExceptionTest().Test();
			new ArrayTest().Test();
			new ReflectionTest().Test();

			Screen.WriteLine();
			Screen.WriteLine();
		}
	}
}
