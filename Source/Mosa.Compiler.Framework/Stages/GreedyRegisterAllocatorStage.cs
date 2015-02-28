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
			var allocator = new GreedyRegisterAllocator(BasicBlocks, MethodCompiler.VirtualRegisters, MethodCompiler.StackLayout, Architecture, this);

			allocator.Start();

			return;
		}
	}
}