// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Test.Collection
{
	public class NewObjectTests
	{
		public int Test()
		{
			return 5;
		}

		public static bool Create()
		{
			NewObjectTests d = new NewObjectTests();
			return d != null;
		}

		public static bool CreateAndCallMethod()
		{
			NewObjectTests d = new NewObjectTests();
			return d.Test() == 5;
		}
	}
}
