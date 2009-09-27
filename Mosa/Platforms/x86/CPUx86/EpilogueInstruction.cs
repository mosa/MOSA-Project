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
	/// x86 specific intermediate representation of the <see cref="EpilogueInstruction"/>.
    /// </summary>
	public sealed class EpilogueInstruction : BaseInstruction
    {
        #region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="EpilogueInstruction"/>.
		/// </summary>
        public EpilogueInstruction() :
            base()
        {
        }

        #endregion // Construction
    }
}
