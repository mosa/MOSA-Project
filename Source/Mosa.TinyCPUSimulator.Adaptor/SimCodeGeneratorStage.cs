/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
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
		protected long sectionAddress;
		protected long startPosition;

		public SimCodeGeneratorStage(ISimAdapter simAdapter)
			: base(true)
		{
			this.simAdapter = simAdapter;
		}

		/// <summary>
		/// Begins the generate.
		/// </summary>
		protected override void BeginGenerate()
		{
			base.BeginGenerate();

			//TODO!
			//var section = MethodCompiler.Linker.GetSection(SectionKind.Text) as SimLinkerSection;

			//sectionAddress = section.VirtualAddress;

			//startPosition = (codeStream as LinkerStream).BaseStream.Position;

			return;
		}

		protected override void EmitInstruction(Context context, BaseCodeEmitter codeEmitter)
		{
			long start = codeEmitter.CurrentPosition;

			base.EmitInstruction(context, codeEmitter);

			long end = codeEmitter.CurrentPosition;
			byte opcodeSize = (byte)(end - start);

			var instruction = simAdapter.Convert(context, MethodCompiler.Method, BasicBlocks, opcodeSize);
			instruction.Source = context.ToString(); // context.Instruction.ToString(context);

			simAdapter.SimCPU.AddInstruction((ulong)(sectionAddress + startPosition + start), instruction);

			return;
		}

		protected override void BlockStart(BasicBlock block)
		{
			long current = codeEmitter.CurrentPosition;

			base.BlockStart(block);

			simAdapter.SimCPU.SetSymbol(block.ToString() + ":" + MethodCompiler.Method.FullName, (ulong)(sectionAddress + startPosition + current), 0);
		}
	}
}