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

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Performs memory layout of a type for compilation.
    /// </summary>
    public sealed class TypeLayoutStage : IAssemblyCompilerStage
    {
        #region IAssemblyCompilerStage members

        string IAssemblyCompilerStage.Name
        {
            get { return @"Type Layout"; }
        }

        void IAssemblyCompilerStage.Run(AssemblyCompiler compiler)
        {

        }

        #endregion // IAssemblyCompilerStage members
    }
}
