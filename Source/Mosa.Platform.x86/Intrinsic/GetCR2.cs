/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal sealed class GetCR2 : GetControlRegisterBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GetCR2"/> class.
		/// </summary>
		public GetCR2()
			: base(ControlRegister.CR2)
		{
		}
	}
}