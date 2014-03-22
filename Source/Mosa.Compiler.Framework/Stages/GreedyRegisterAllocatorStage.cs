/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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
		void IMethodCompilerStage.Execute()
		{
			var trace = CreateTrace();

			var allocator = new GreedyRegisterAllocator(BasicBlocks, MethodCompiler.VirtualRegisters, InstructionSet, MethodCompiler.StackLayout, Architecture, trace);

			return;
		}

		#endregion IMethodCompilerStage Members
	}
}