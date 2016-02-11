// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Linker;
using System.Diagnostics;

namespace Mosa.TinyCPUSimulator.Adaptor
{
	internal class SimCodeGeneratorStage : CodeGenerationStage
	{
		protected ISimAdapter simAdapter;
		protected LinkerSymbol symbol;

		private SimLinkerFinalizationStage stage;

		public SimCodeGeneratorStage(ISimAdapter simAdapter)
			: base(true)
		{
			this.simAdapter = simAdapter;
		}

		protected override void Setup()
		{
			base.Setup();

			symbol = MethodCompiler.Linker.GetSymbol(MethodCompiler.Method.FullName, SectionKind.Text);

			stage = MethodCompiler.Compiler.PostCompilePipeline.FindFirst<SimLinkerFinalizationStage>() as SimLinkerFinalizationStage;

			Debug.Assert(stage != null);

			stage.ClearSymbolInformation(symbol);
		}

		protected override void EmitInstruction(InstructionNode node, BaseCodeEmitter codeEmitter)
		{
			long start = codeEmitter.CurrentPosition;

			base.EmitInstruction(node, codeEmitter);

			long end = codeEmitter.CurrentPosition;

			var instruction = simAdapter.Convert(node, MethodCompiler.Method, BasicBlocks, (byte)(end - start));

			if (instruction != null)
			{
				stage.AddInstruction(symbol, start, instruction);
			}

			stage.AddSourceInformation(symbol, start, node.Offset.ToString() + "\t0x" + node.Offset.ToString("X") + "\t" + node.Block.ToString() + "\t" + symbol + "\t" + node.ToString());
		}

		protected override void BlockStart(BasicBlock block)
		{
			base.BlockStart(block);

			stage.AddTargetSymbol(symbol, codeEmitter.CurrentPosition, block.ToString() + ":" + MethodCompiler.Method.FullName);
		}
	}
}
