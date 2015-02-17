/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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

			simLinker.AddSourceInformation(symbol, start, node.ToString());
		}

		protected override void BlockStart(BasicBlock block)
		{
			base.BlockStart(block);

			simLinker.AddTargetSymbol(symbol, (int)codeEmitter.CurrentPosition, block.ToString() + ":" + MethodCompiler.Method.FullName);
		}
	}
}
