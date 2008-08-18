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

using Mosa.Runtime.CompilerFramework.Ir;
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

        public void Run(MethodCompilerBase compiler)
        {
            // Call Run on every stage
            foreach (IMethodCompilerStage stage in Stages)
                stage.Run(compiler);
        }
    }
}
