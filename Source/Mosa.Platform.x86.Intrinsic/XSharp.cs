/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System.Runtime.InteropServices;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Provides stub methods for xsharp methods.
	/// </summary>
	public static class XSharp
	{

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.TestMethod, Mosa.Platform.x86")]
		public extern static void TestMethod();


	}
}
