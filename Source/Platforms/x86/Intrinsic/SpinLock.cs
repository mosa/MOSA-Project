/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR = Mosa.Runtime.CompilerFramework.IR;
using System.Diagnostics;

namespace Mosa.Platforms.x86.Intrinsic
{
	/// <summary>
	/// Representations a spin lock
	/// </summary>
	public sealed class SpinLock : IIntrinsicMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		public void ReplaceIntrinsicCall(Context context)
		{
			// TODO
			context.Remove();
		}

		#endregion // Methods
	}
}
