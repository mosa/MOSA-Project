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
	public class ReverseBlockOrderStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage, IBlockOrderStage
	{

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name
		{
			get { return @"ReverseBlockOrderStage"; }
		}

		#endregion // IPipelineStage Methods

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		public void Run()
		{
			for (int i = 1; i <= BasicBlocks.Count / 2; i++)
			{
				BasicBlock temp = BasicBlocks[i];
				BasicBlocks[i] = BasicBlocks[BasicBlocks.Count - i];
				BasicBlocks[BasicBlocks.Count - i] = temp;
			}
		}
	}
}
