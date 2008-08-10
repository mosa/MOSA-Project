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
    /// Base class providing methods common to all compiler classes.
    /// </summary>
    public abstract class CompilerBase<PipelineType> where PipelineType: class
    {
        #region Data members

        /// <summary>
        /// Holds the pipeline of the compiler.
        /// </summary>
        protected CompilerPipeline<PipelineType> _pipeline;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of.
        /// </summary>
        protected CompilerBase()
        {
            _pipeline = new CompilerPipeline<PipelineType>();
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Provides access to the pipeline of this compiler.
        /// </summary>
        public CompilerPipeline<PipelineType> Pipeline
        {
            get { return _pipeline; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Finds a stage, which ran before the current one and supports the specified type.
        /// </summary>
        /// <param name="stageType">The (interface) type to look for.</param>
        /// <returns>The previous compilation stage supporting the requested type.</returns>
        /// <remarks>
        /// This method is used by stages to access the results of a previous compilation stage.
        /// </remarks>
        public PipelineType GetPreviousStage(Type stageType)
        {
            PipelineType result = null;

            for (int stage = _pipeline.CurrentStage - 1; -1 != stage; stage--)
            {
                PipelineType temp = _pipeline[stage];
                if (true == stageType.IsInstanceOfType(temp))
                {
                    result = temp;
                    break;
                }
            }

            return result;
        }

        #endregion // Methods
    }
}
