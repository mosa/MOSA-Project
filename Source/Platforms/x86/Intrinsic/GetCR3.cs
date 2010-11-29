/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platform.X86.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class GetCR3 : GetControlRegisterBase
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="GetCR3"/> class.
		/// </summary>
		public GetCR3()
			: base(ControlRegister.CR3)
		{
		}

	}
}
