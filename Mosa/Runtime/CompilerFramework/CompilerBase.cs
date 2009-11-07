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
	public abstract class CompilerBase
	{
		#region Data members

		/// <summary>
		/// Holds the pipeline of the compiler.
		/// </summary>
		protected CompilerPipeline _pipeline;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of.
		/// </summary>
		protected CompilerBase()
		{
			_pipeline = new CompilerPipeline();
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Provides access to the pipeline of this compiler.
		/// </summary>
		public CompilerPipeline Pipeline
		{
			get { return _pipeline; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Gets the previous stage.
		/// </summary>
		/// <param name="stage">The stage.</param>
		/// <returns>
		/// The previous compilation stage supporting the requested type or null.
		/// </returns>
		public IPipelineStage GetPreviousStage(IPipelineStage stage)
		{
			return GetPreviousStage(typeof(IPipelineStage));
		}

		/// <summary>
		/// Finds a stage, which ran before the current one and supports the specified type.
		/// </summary>
		/// <param name="stageType">The (interface) type to look for.</param>
		/// <returns>The previous compilation stage supporting the requested type.</returns>
		/// <remarks>
		/// This method is used by stages to access the results of a previous compilation stage.
		/// </remarks>
		public IPipelineStage GetPreviousStage(Type stageType)
		{
			IPipelineStage result = null;

			for (int stage = _pipeline.CurrentStage - 1; -1 != stage; stage--) {
				IPipelineStage temp = _pipeline[stage];
				if (stageType.IsInstanceOfType(temp)) {
					result = temp;
					break;
				}
			}

			return result;
		}

		#endregion // Methods
	}
}
