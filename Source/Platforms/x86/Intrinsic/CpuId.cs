/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Scott Balmos <sbalmos@fastmail.fm>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.Intrinsic
{
    /// <summary>
    /// Representations the x86 CPUID instruction.
    /// </summary>
	public sealed class CpuId : IIntrinsicMethod
    {
 
        #region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		public void ReplaceIntrinsicCall(Context context, RuntimeBase runtime)
		{
			context.SetInstruction(CPUx86.Instruction.CpuIdInstruction, context.Result, context.Operand1);
		}

        #endregion // Methods

    }
}
