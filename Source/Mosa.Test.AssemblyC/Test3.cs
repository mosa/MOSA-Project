namespace Mosa.Test.AssemblyC
{
	public class Test3<T1, T2>
	{
		public object DoWork<M>(T1 t1, M m1, M m2, T2 t2)
		{
			return t1;
		}

		public static void test()
		{
			var t = new Test3<int, int>();

			t.DoWork<string>(1, string.Empty, string.Empty, 2);
		}
	}
}