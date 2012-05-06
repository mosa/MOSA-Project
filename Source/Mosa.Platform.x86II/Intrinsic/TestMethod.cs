/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Platform.x86II.XSharp;

namespace Mosa.Platform.x86II.Intrinsic
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
