/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using Mosa.Kernel.x86;

namespace Mosa.TestWorld.x86.Tests
{
	public class KernelTest
	{
		private static ConsoleSession Console;

		/// <summary>
		///
		/// </summary>
		private string testName = string.Empty;

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		protected delegate bool TestMethod();

		/// <summary>
		///
		/// </summary>
		protected LinkedList<TestMethod> testMethods = new LinkedList<TestMethod>();

		/// <summary>
		///
		/// </summary>
		/// <param name="testName"></param>
		protected KernelTest(string testName)
		{
			this.testName = testName;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="flag"></param>
		public void PrintResult(bool flag)
		{
			byte color = Console.Color;

			if (flag)
			{
				Console.Color = Colors.White;
				Console.Write("+");
			}
			else
			{
				Console.Color = Colors.Red;
				Console.Write("X");
			}

			Console.Color = color;
		}

		/// <summary>
		///
		/// </summary>
		public static void RunTests()
		{
			Console = Boot.Console;

			Console.Goto(4, 0);
			Console.Color = Colors.Yellow;
			Console.Write("[");
			Console.Color = Colors.White;
			Console.Write("Tests");
			Console.Color = Colors.Yellow;
			Console.Write("]");
			Console.WriteLine();
			Console.WriteLine();
			Console.Color = Colors.Yellow;

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
			//var int64Test = new Int64Test();
			var otherTest = new OtherTest();

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
			otherTest.Test();
			reflectionTest.Test();
			//int64Test.Test();
		}

		public void Test()
		{
			Console.Color = Colors.Yellow;
			Console.Write(testName);

			int len = 15 - testName.Length;

			while (len > 0)
			{
				Console.Write(' ');
				len--;
			}

			Console.Write(": ");

			//foreach (TestMethod node in testMethods)
			//{
			//    PrintResult(node());
			//}

			var node = testMethods.First;
			while (node != null)
			{
				PrintResult(node.Value());
				node = node.Next;
			}

			Console.WriteLine();
		}
	}
}