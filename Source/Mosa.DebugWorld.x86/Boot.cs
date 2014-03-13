/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Kernel.x86;
using Mosa.Platform.Internal.x86;

namespace Mosa.DebugWorld.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Boot
	{

		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static void Main()
		{
			Screen.Color = 0x0;
			Screen.Clear();
			Screen.GotoTop();
			Screen.Color = 0x0E;
			Screen.Write('>');
			SSE.Setup();

			Test();
			Screen.Write('X');

			while (true) ;
		}

		public static void Test()
		{
			Mosa.Test.Collection.DoubleTests.IsNaN(double.NaN);
		}
	}
}