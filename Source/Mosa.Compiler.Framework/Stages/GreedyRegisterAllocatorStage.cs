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
	public sealed class GreedyRegisterAllocatorStage : BaseMethodCompilerStage
	{

		protected override void Run()
		{
			var trace = CreateTrace();

			var allocator = new GreedyRegisterAllocator(BasicBlocks, MethodCompiler.VirtualRegisters, InstructionSet, MethodCompiler.StackLayout, Architecture, trace);

			return;
		}

	}
}