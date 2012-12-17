/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.Framework.RegisterAllocator;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class GreedyRegisterAllocatorStage : BaseMethodCompilerStage, IMethodCompilerStage
	{

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			var allocator = new GreedyRegisterAllocator(basicBlocks, methodCompiler.VirtualRegisters, instructionSet, architecture);

			return;
		}

		#endregion // IMethodCompilerStage Members

	}
}


