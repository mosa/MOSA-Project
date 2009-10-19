/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Collections.Generic;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// MethodCompilationStageComposite composes several MethodCompilerStages into 
    /// one stage and forwards calls to the stage to multiple _stages.
    /// </summary>
    public class MethodCompilationStageComposite : IMethodCompilerStage
    {
        /// <summary>
        /// List of _stages
        /// </summary>
        private List<IMethodCompilerStage> _stages;

        /// <summary>
        /// List-Accessor
        /// </summary>
        /// <value>The _stages.</value>
        public List<IMethodCompilerStage> Stages
        {
            get { return _stages; }
            set { _stages = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public MethodCompilationStageComposite()
        {
        }

        /// <summary>
        /// Takes the enumeration and copies all _stages into the list
        /// </summary>
        /// <param name="stages"></param>
        public MethodCompilationStageComposite(IEnumerable<IMethodCompilerStage> stages)
        {
            // Walk through enumeration and copy _stages
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
        public void Run(IMethodCompiler compiler)
        {
            // Call Run on every stage
            foreach (IMethodCompilerStage stage in Stages)
                stage.Run(compiler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            foreach (IMethodCompilerStage stage in Stages)
                stage.AddToPipeline(pipeline);
        }
    }
}
