/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// This class orders blocks in reverse order. This stage is used for testing.
	/// </summary>
	public class ReverseBlockOrderStage : BaseStage, IMethodCompilerStage
	{
		#region Data members

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"ReverseBlockOrderStage"; }
		}

		#endregion // Properties

		#region IMethodCompilerStage Members

		/// <summary>
		/// Adds to pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline.</param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertAfter<IPlatformTransformationStage>(this);
			pipeline.InsertBefore<CodeGenerationStage>(this);
		}

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			for (int i = 1; i <= BasicBlocks.Count / 2; i++) {
				BasicBlock temp = BasicBlocks[i];
				BasicBlocks[i] = BasicBlocks[BasicBlocks.Count - i];
				BasicBlocks[BasicBlocks.Count - i] = temp;
			}
		}

		#endregion // Methods
	}
}
