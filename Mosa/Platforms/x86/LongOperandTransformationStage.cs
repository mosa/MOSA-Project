/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Transforms 64-bit arithmetic to 32-bit operations.
    /// </summary>
    /// <remarks>
    /// This stage translates all 64-bit operations to appropriate 32-bit operations on
    /// architectures without appropriate 64-bit integral operations.
    /// </remarks>
    public sealed class LongOperandTransformationStage : CodeTransformationStage
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LongOperandTransformationStage"/> class.
        /// </summary>
        public LongOperandTransformationStage()
        {
        }

        #endregion // Construction

        #region IMethodCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public sealed override string Name
        {
            get { return @"LongArithmeticTransformationStage"; }
        }

        #endregion // IMethodCompilerStage Members
    }
}
