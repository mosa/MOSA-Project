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
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.Loader;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// MethodCompilationStageComposite composes several MethodCompilerStages into 
    /// one sage and forwards calls to the stage to multiple stages.
    /// </summary>
    public class MethodCompilationStageComposite : IMethodCompilerStage
    {
        /// <summary>
        /// List of stages
        /// </summary>
        private List<IMethodCompilerStage> stages;

        /// <summary>
        /// List-Accessor
        /// </summary>
        /// <value>The stages.</value>
        public List<IMethodCompilerStage> Stages
        {
            get { return stages; }
            set { stages = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public MethodCompilationStageComposite()
        {
        }

        /// <summary>
        /// Takes the enumeration and copies all stages into the list
        /// </summary>
        /// <param name="stages"></param>
        public MethodCompilationStageComposite(IEnumerable<IMethodCompilerStage> stages)
        {
            // Walk through enumeration and copy stages
            foreach (IMethodCompilerStage stage in stages)
            {
                Stages.Add(stage);
            }
        }

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get
            {
                string result = "[";
                foreach (IMethodCompilerStage stage in Stages)
                    result += stage.Name + ",";

                if (result.Length > 1)
                    result = result.Substring(0, result.Length - 1);

                result += "]";

                return result;
            }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(MethodCompilerBase compiler)
        {
            // Call Run on every stage
            foreach (IMethodCompilerStage stage in Stages)
                stage.Run(compiler);
        }
    }
}
