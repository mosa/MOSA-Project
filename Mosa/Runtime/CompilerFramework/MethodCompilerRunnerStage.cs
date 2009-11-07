using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class MethodCompilerRunnerStage : IAssemblyCompilerStage, IPipelineStage
	{

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value></value>
		string IPipelineStage.Name { get { return @"Method Compiler Runner"; } }

		/// <summary>
		/// Gets the pipeline stage order.
		/// </summary>
		/// <value>The pipeline stage order.</value>
		PipelineStageOrder[] IPipelineStage.PipelineStageOrder { get { return null; } }

		#endregion // IPipelineStage Members

		#region IAssemblyCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		void IAssemblyCompilerStage.Run(AssemblyCompiler compiler)
		{
			IMethodCompilerBuilder mcb = compiler.Pipeline.Find<IMethodCompilerBuilder>();
			Debug.Assert(null != mcb, @"Failed to find a method compiler builder stage.");
			foreach (MethodCompilerBase mc in mcb.Scheduled) {
				try {
					mc.Compile();
				}
				finally {
					mc.Dispose();
				}
			}
		}

		#endregion // IAssemblyCompilerStage Members
	}
}
