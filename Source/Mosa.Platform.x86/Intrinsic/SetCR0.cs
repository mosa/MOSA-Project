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
	public sealed class SetCR0 : SetControlRegisterBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SetCR0"/> class.
		/// </summary>
		public SetCR0()
			: base(ControlRegister.CR0)
		{
		}
	}
}