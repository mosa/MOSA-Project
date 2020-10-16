// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.RegisterAllocator;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Greedy Register Allocator Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	public sealed class GreedyRegisterAllocatorStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			var allocator = new GreedyRegisterAllocator(BasicBlocks, MethodCompiler.VirtualRegisters, Architecture, MethodCompiler.AddStackLocal, StackFrame, CreateTraceLog);

			allocator.Start();
		}
	}
}
