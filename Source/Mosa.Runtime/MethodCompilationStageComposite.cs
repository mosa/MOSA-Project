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
	/// one stage and forwards calls to the stage to multiple stages.
	/// </summary>
	public class MethodCompilationStageComposite : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		/// <summary>
		/// List of stages
		/// </summary>
		private List<IMethodCompilerStage> stages;

		/// <summary>
		/// List of stages
		/// </summary>
		/// <value>The stages.</value>
		public List<IMethodCompilerStage> Stages
		{
			get { return stages; }
			set { stages = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodCompilationStageComposite"/> class.
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
		string IPipelineStage.Name
		{
			get
			{
				string result = "[";
				foreach (IPipelineStage stage in Stages)
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
		public void Run()
		{
			// Call Run on every stage
			foreach (IMethodCompilerStage stage in Stages)
			{
				stage.Setup(methodCompiler);
				stage.Run();
			}

		}

	}
}
