// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Linker;

namespace Mosa.TinyCPUSimulator.Adaptor
{
	internal class SimCodeGeneratorStage : CodeGenerationStage
	{
		protected ISimAdapter simAdapter;
		protected LinkerSymbol symbol;
		protected SimLinker simLinker;

		public SimCodeGeneratorStage(ISimAdapter simAdapter)
			: base(true)
		{
			this.simAdapter = simAdapter;
		}

		protected override void Setup()
		{
			base.Setup();

			symbol = MethodCompiler.Linker.GetSymbol(MethodCompiler.Method.FullName, SectionKind.Text);
			simLinker = MethodCompiler.Linker as SimLinker;

			simLinker.ClearSymbolInformation(symbol);
		}

		protected override void EmitInstruction(InstructionNode node, BaseCodeEmitter codeEmitter)
		{
			long start = codeEmitter.CurrentPosition;

			base.EmitInstruction(node, codeEmitter);

			long end = codeEmitter.CurrentPosition;

			var instruction = simAdapter.Convert(node, MethodCompiler.Method, BasicBlocks, (byte)(end - start));

			if (instruction != null)
			{
				simLinker.AddInstruction(symbol, start, instruction);
			}

			simLinker.AddSourceInformation(symbol, start, node.Offset.ToString() + "\t0x" + node.Offset.ToString("X") + "\t" + node.Block.ToString() + "\t" + symbol + "\t" + node.ToString());
		}

		protected override void BlockStart(BasicBlock block)
		{
			base.BlockStart(block);

			simLinker.AddTargetSymbol(symbol, codeEmitter.CurrentPosition, block.ToString() + ":" + MethodCompiler.Method.FullName);
		}
	}
}
