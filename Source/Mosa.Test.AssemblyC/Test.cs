using Mosa.Test.AssemblyA;

namespace Mosa.Test.AssemblyC
{
	public static class Test
	{
		public static void Test1()
		{
			Holder<int> HolderOfInt = new Holder<int>(10);
		}

		public static void Test2()
		{
			Holder<uint> HolderOfInt = new Holder<uint>(10);

			IHolder<uint> iholder = HolderOfInt;
			iholder.SetValue(12);
		}
	}
}