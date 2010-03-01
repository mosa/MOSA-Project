/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.QuickTest
{

	/// <summary>
	/// 
	/// </summary>
	public static class App
	{
		public delegate void Test();

		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			Execute(MyTest);
		}

		public static void MyTest()
		{
			// DO NOTHING
		}

		public static void Execute(Test a)
		{
			a();
		}

	}
}
