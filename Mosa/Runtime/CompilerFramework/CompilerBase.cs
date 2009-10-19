/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Base class providing methods common to all compiler classes.
    /// </summary>
    public abstract class CompilerBase<TPipelineType> where TPipelineType: class
    {
        #region Data members

        /// <summary>
        /// Holds the pipeline of the compiler.
        /// </summary>
        protected CompilerPipeline<TPipelineType> _pipeline;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of.
        /// </summary>
        protected CompilerBase()
        {
            _pipeline = new CompilerPipeline<TPipelineType>();
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Provides access to the pipeline of this compiler.
        /// </summary>
        public CompilerPipeline<TPipelineType> Pipeline
        {
            get { return _pipeline; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Gets the previous stage.
        /// </summary>
        /// <typeparam name="T">The type of the previous stage. Usually a public interface.</typeparam>
        /// <returns>The previous compilation stage supporting the requested type or null.</returns>
        public T GetPreviousStage<T>()
        {
            return (T)GetPreviousStage(typeof(T));
        }

        /// <summary>
        /// Finds a stage, which ran before the current one and supports the specified type.
        /// </summary>
        /// <param name="stageType">The (interface) type to look for.</param>
        /// <returns>The previous compilation stage supporting the requested type.</returns>
        /// <remarks>
        /// This method is used by stages to access the results of a previous compilation stage.
        /// </remarks>
        public object GetPreviousStage(Type stageType)
        {
            TPipelineType result = null;

            for (int stage = _pipeline.CurrentStage - 1; -1 != stage; stage--)
            {
                TPipelineType temp = _pipeline[stage];
                if (stageType.IsInstanceOfType(temp))
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
