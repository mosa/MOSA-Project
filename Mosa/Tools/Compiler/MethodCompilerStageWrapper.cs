/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai P. Reisert <kpreisert@googlemail.com>
 */

using System;

using Mosa.Runtime.CompilerFramework;

using NDesk.Options;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Abstract base class for method compiler stage wrappers.
    /// </summary>
    /// <typeparam name="WrappedType">The type of the wrapped type.</typeparam>
    public abstract class MethodCompilerStageWrapper<WrappedType> : IMethodCompilerStage, IHasOptions
        where WrappedType: IMethodCompilerStage, new()
    {
        
        /// <summary>
        /// Holds the selected compiler stage.
        /// </summary>
        private readonly WrappedType wrapped;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodCompilerStageWrapper&lt;WrappedType&gt;"/> class.
        /// </summary>
        protected MethodCompilerStageWrapper()
        {
            this.wrapped = new WrappedType();
        }
        
        /// <summary>
        /// Set whether this stage will be executed or not.
        /// </summary>
        protected bool Enabled = true;
        
        /// <summary>
        /// Gets the wrapped method compiler stage.
        /// </summary>
        /// <value>The wrapped method compiler stage.</value>
        public WrappedType Wrapped
        {
            get { return this.wrapped; }
        }
        
        /// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get
            {
                return wrapped.Name + (Enabled ? String.Empty : " (Disabled)");
            }
        }
        
        /// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(IMethodCompiler compiler)
        {
            if (Enabled)
            {
                wrapped.Run(compiler);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            if (Enabled)
            {
                wrapped.AddToPipeline(pipeline);
            }
        }
        
        /// <summary>
        /// Adds the additional options for the parsing process to the given OptionSet.
        /// </summary>
        /// <param name="optionSet">A given OptionSet to add the options to.</param>
        public abstract void AddOptions(OptionSet optionSet);
    }
}
