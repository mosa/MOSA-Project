using Mosa.Platform.x86;
using Mosa.Kernel;
using Mosa.Kernel.x86;
using Mosa.ClassLib;

namespace Mosa.HelloWorld.Tests
{
	public class KernelTest
	{
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
			byte color = Screen.Color;
			if (flag)
			{
				Screen.Color = Colors.Green;
				Screen.Write("+");
			}
			else
			{
				Screen.Color = Colors.Red;
				Screen.Write("X");
			}
			Screen.Color = color;
		}

		/// <summary>
		/// 
		/// </summary>
		public static void RunTests()
		{
			Screen.Goto(23, 0);
			Screen.Color = Colors.Yellow;
			Screen.Write("[");
			Screen.Color = Colors.White;
			Screen.Write("Tests");
			Screen.Color = Colors.Yellow;
			Screen.Write("]");

			var stringTest = new StringTest();
			var interfaceTest = new InterfaceTest();
			var genericsTest = new GenericTest();
			var generics2Test = new Generic2Test();
			var isInstanceTest = new IsInstTest();
			var exceptionTest = new ExceptionTest();

			stringTest.Test();
			interfaceTest.Test();
			genericsTest.Test();
			generics2Test.Test();
			isInstanceTest.Test();
			exceptionTest.Test();
		}

		public void Test()
		{
			Screen.Color = Colors.Gray;
			Screen.Write(" ");
			Screen.Write(this.testName);
			Screen.Write(": ");

			var node = testMethods.First;
			while (node != null)
			{
				PrintResult(node.value());
				node = node.next;
			}
		}
	}
}
