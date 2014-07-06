/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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