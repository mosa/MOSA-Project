/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	public sealed class ExceptionHeaderPreprocessingStage : BaseMethodCompilerStage, IMethodCompilerStage, IPlatformStage, IPipelineStage
	{
		/// <summary>
		/// Runs the specified method compiler.
		/// </summary>
		public void Run()
		{
			// Iterate all blocks and preprocess them
			foreach (BasicBlock block in BasicBlocks)
				ProcessBlock(block);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="block"></param>
		private void ProcessBlock(BasicBlock block)
		{
			Context context = CreateContext(block);
		}
	}
}
