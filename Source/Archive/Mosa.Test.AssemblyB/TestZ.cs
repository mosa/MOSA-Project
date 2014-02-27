using Mosa.Test.AssemblyA;

namespace Mosa.Test.AssemblyB
{
	public class TestZ
	{
		public virtual void Test1<T>(T value)
		{

		}

		public void Test1(int value)
		{

		}

	}

	public class TestZZ : TestZ
	{
		public override void Test1<T>(T value)
		{

		}
	}
}