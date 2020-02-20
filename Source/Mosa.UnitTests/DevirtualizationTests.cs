// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Test1 is copied from: https://github.com/dotnet/coreclr/pull/9230#issuecomment-277876789
// MOSA correctly devirtualizes it, inlines it, and then determines it returns a constant value.

namespace Mosa.UnitTests
{
	internal class Base
	{
		public virtual int Foo()
		{
			return 33;
		}

		private static BaseSealed s_Default = new BaseSealed();
		public static Base Default => s_Default;
	}

	internal sealed class BaseSealed : Base
	{ }

	public class DevirtualizationTests
	{
		[MosaUnitTest]
		public static int Test1()
		{
			Base b = Base.Default;
			int x = b.Foo();
			return (x == 33 ? 100 : -1);
		}
	}
}
