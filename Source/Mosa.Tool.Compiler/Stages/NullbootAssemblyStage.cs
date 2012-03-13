/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Pascal Delprat (pdelprat) <pascal.delprat@online.fr>
 */

using System;
using System.IO;
using System.Text;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Tool.Compiler.Stages
{
    public sealed class NullbootAssemblyStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage, IPipelineStage
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="NullbootAssemblyStage"/> class.
        /// </summary>
        public NullbootAssemblyStage()
        {

        }

        #endregion // Construction

        #region IAssemblyCompilerStage Members

        void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
        {
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        void IAssemblyCompilerStage.Run()
        {
            // Nothing todo
        }

        #endregion // IAssemblyCompilerStage Members
    }
}
