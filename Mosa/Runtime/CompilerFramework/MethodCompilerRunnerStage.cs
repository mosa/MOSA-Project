using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
    public sealed class MethodCompilerRunnerStage : IAssemblyCompilerStage
    {
        #region IAssemblyCompilerStage Members

        string IAssemblyCompilerStage.Name
        {
            get { return @"Method Compiler Runner"; }
        }

        void IAssemblyCompilerStage.Run(AssemblyCompiler compiler)
        {
            IMethodCompilerBuilder mcb = compiler.Pipeline.Find<IMethodCompilerBuilder>();
            Debug.Assert(null != mcb, @"Failed to find a method compiler builder stage.");
            foreach (MethodCompilerBase mc in mcb.Scheduled)
            {
                mc.Compile();
            }
        }

        #endregion // IAssemblyCompilerStage Members
    }
}
