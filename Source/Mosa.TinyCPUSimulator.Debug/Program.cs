
namespace Mosa.TinyCPUSimulator.Debug
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			TestCPUx86 test = new TestCPUx86();

			test.RunTest();

			return;
		}
	}
}