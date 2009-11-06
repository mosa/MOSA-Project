using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// 
    /// </summary>
	public interface IPipelineStage
    {
		/// <summary>
		/// Sets the position of the stage within the pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline for which this stage is a member of.</param>
		void SetPipelinePosition(CompilerPipeline<IPipelineStage> pipeline);
    }
}
