/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;
using Mosa.Kernel.x86;

namespace Mosa.HelloWorld.x86.Tests
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
				Console.Color = Colors.Green;
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

			Console.Goto(20, 0);
			Console.Color = Colors.Yellow;
			Console.Write("[");
			Console.Color = Colors.White;
			Console.Write("Tests");
			Console.Color = Colors.Yellow;
			Console.Write("]");

			var stringTest = new StringTest();
			var interfaceTest = new InterfaceTest();
			var genericsTest = new GenericTest();
			var generics2Test = new Generic2Test();
			var isInstanceTest = new IsInstTest();
			var exceptionTest = new ExceptionTest();
			var plugTestTest = new PlugTestTest();
			var compareTest = new ComparisonTest();
			var simpleTest = new SimpleTest();

			stringTest.Test();
			interfaceTest.Test();
			genericsTest.Test();
			generics2Test.Test();
			isInstanceTest.Test();
			exceptionTest.Test();
			plugTestTest.Test();
			compareTest.Test();
			simpleTest.Test();
		}

		public void Test()
		{
			Console.Color = Colors.Gray;
			Console.Write(" ");
			Console.Write(this.testName);
			Console.Write(": ");

			//foreach (TestMethod node in testMethods)
			//{
			//    PrintResult(node());
			//}

			var node = testMethods.First;
			while (node != null)
			{
				PrintResult(node.value());
				node = node.next;
			}
		}
	}
}
