// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Schedules type initializers and creates a hidden mosacl_main method,
	/// which runs all type initializers in sequence.
	/// </summary>
	/// <remarks>
	/// Dependencies are not resolved, it is hoped that dependencies are resolved
	/// by the high-level language compiler by placing cctors in some order in
	/// metadata.
	/// </remarks>
	public sealed class TypeInitializerSchedulerStage : BaseCompilerStage
	{
		public readonly static string TypeInitializerName = "AssemblyInit";

		#region Data Members

		/// <summary>
		/// Hold the current context
		/// </summary>
		private Context start;

		/// <summary>
		/// The basic blocks
		/// </summary>
		private BasicBlocks basicBlocks;

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeInitializerSchedulerStage"/> class.
		/// </summary>
		public TypeInitializerSchedulerStage()
		{
			basicBlocks = new BasicBlocks();

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

			// create start instructions
			start = new Context(startBlock);
			start.AppendInstruction(IRInstruction.Jmp, epilogueBlock);
			start.GotoPrevious();
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the initializer method.
		/// </summary>
		/// <value>The method.</value>
		public MosaMethod TypeInitializerMethod { get; private set; }

		#endregion Properties

		protected override void Run()
		{
			if (TypeSystem.EntryPoint != null)
			{
				Schedule(TypeSystem.EntryPoint);
			}

			TypeInitializerMethod = Compiler.CreateLinkerMethod(TypeInitializerName);

			Compiler.CompileMethod(TypeInitializerMethod, basicBlocks, 0);
		}

		#region Methods

		/// <summary>
		/// Schedules the specified method for invocation in the main.
		/// </summary>
		/// <param name="method">The method.</param>
		public void Schedule(MosaMethod method)
		{
			var symbolOperand = Operand.CreateSymbolFromMethod(TypeSystem, method);
			start.AppendInstruction(IRInstruction.Call, null, symbolOperand);
			start.InvokeMethod = method;
		}

		#endregion Methods
	}
}
