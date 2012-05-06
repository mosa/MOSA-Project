/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


using Mosa.Platform.x86.XSharp;

namespace Mosa.Platform.x86.Intrinsic
{
	class TestMethod : XSharpMethod
	{

		public override void Assemble()
		{
			EDX.Push();
			EDX.Pop();
			Return();
		}
	}
}
