using Mosa.Platform.x86;
using Mosa.Kernel;
using Mosa.Kernel.x86;

namespace Mosa.HelloWorld.Tests
{
	public class KernelTest
	{
		public static void PrintResult(bool flag)
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

		public static void RunTests()
		{
			Screen.Goto(23, 0);
			Screen.Color = Colors.Yellow;
			Screen.Write("[");
			Screen.Color = Colors.White;
			Screen.Write("Tests");
			Screen.Color = Colors.Yellow;
			Screen.Write("]");

			StringTest.Test();
			InterfaceTest.Test();
			GenericTest.Test();
			Generic2Test.Test();
			IsInstTest.Test();
		}
	}
}
