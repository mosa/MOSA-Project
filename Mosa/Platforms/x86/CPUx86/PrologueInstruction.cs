/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR2 = Mosa.Runtime.CompilerFramework.IR2;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation of the x86 specific prologue instruction.
    /// </summary>
	public sealed class PrologueInstruction : BaseInstruction
    {
        #region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="PrologueInstruction"/>.
		/// </summary>
        public PrologueInstruction() :
            base()
        {
        }

        #endregion // Construction
    }
}
