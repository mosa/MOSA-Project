// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.CompilerStages
{
	/// <summary>
	/// Setup Stage - experimental - not fully implemented!!!!
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public sealed class SetupStage : BaseCompilerStage
	{
		public const string SetupStagerName = "SetupStage";

		#region Data Members

		private MosaMethod setupMethod;

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SetupStage"/> class.
		/// </summary>
		public SetupStage()
		{
		}

		#endregion Construction

		#region Overrides

		protected override void Setup()
		{
			setupMethod = Compiler.CreateLinkerMethod(SetupStagerName);

			Compiler.CompilerData.GetMethodData(setupMethod).DoNotInline = true;
			MethodScanner.MethodInvoked(setupMethod, setupMethod);

			Linker.EntryPoint = Linker.GetSymbol(setupMethod.FullName);
		}

		protected override void Finalization()
		{
			var basicBlocks = new BasicBlocks();

			// Create the blocks
			var prologueBlock = basicBlocks.CreateBlock(BasicBlock.PrologueLabel);
			var startBlock = basicBlocks.CreateBlock(BasicBlock.StartLabel);
			var epilogueBlock = basicBlocks.CreateBlock(BasicBlock.EpilogueLabel);

			// Create the prologue instructions
			basicBlocks.AddHeadBlock(prologueBlock);
			var prologue = new Context(prologueBlock);
			prologue.AppendInstruction(IRInstruction.Prologue);
			prologue.Label = -1;
			prologue.AppendInstruction(IRInstruction.Jmp, startBlock);

			// Create the epilogue instruction
			var epilogue = new Context(epilogueBlock);
			epilogue.AppendInstruction(IRInstruction.Epilogue);

			var context = new Context(startBlock);

			var entrySymbol = Operand.CreateSymbolFromMethod(TypeSystem.EntryPoint, TypeSystem);
			context.AppendInstruction(IRInstruction.CallStatic, null, entrySymbol);

			var methods = new List<MosaMethod>();

			// TODO!

			foreach (var method in methods)
			{
				var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

				context.AppendInstruction(IRInstruction.CallStatic, null, symbol);
			}

			context.AppendInstruction(IRInstruction.Jmp, epilogueBlock);

			Compiler.CompileMethod(setupMethod, basicBlocks);
		}

		#endregion Overrides
	}
}
