using Mosa.Platform.X86;
using Mosa.Kernel;
using Mosa.Kernel.X86;

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
			StringTest.Test();
			InterfaceTest.Test();
		}
	}
}
