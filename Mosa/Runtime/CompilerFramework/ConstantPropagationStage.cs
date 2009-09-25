/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// This method compiler stage performs constant propagation, e.g. it removes
	/// local variables in favor of constants.
	/// </summary>
	/// <remarks>
	/// Constant propagation has a couple of advantages: First of all it removes
	/// a local variable From the stack and secondly it reduces the register pressure
	/// on systems with only a small number of registers (x86).
	/// <para/>
	/// It is only safe to use this stage on an instruction stream in SSA form.
	/// </remarks>
	public sealed class ConstantPropagationStage : IMethodCompilerStage
	{
		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"Constant Propagation"; }
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(IMethodCompiler compiler)
		{
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");

			IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));

			if (blockProvider == null)
				throw new InvalidOperationException(@"Instruction stream must be split to basic Blocks.");

			// Retrieve the instruction provider and the instruction set
			InstructionSet instructionset = (compiler.GetPreviousStage(typeof(IInstructionsProvider)) as IInstructionsProvider).InstructionSet;

			bool remove = false;

			foreach (BasicBlock block in blockProvider.Blocks) {
				Context ctx = new Context(instructionset, block);

				while (!ctx.EndOfInstruction) {

					if (ctx.Instruction is IR.MoveInstruction || ctx.Instruction is CIL.StlocInstruction) {
						if ( ctx.Operand1 is ConstantOperand) {
							// HACK: We can't track a constant through a register, so we keep those moves
							if (ctx.Result is StackOperand) {
								Debug.Assert(1 == res.Definitions.Count, @"Operand defined multiple times. Instruction stream not in SSA form!");
								ctx.Result.Replace( ctx.Operand1);
								remove = true;
							}
						}
					}
					else if (ctx.Instruction is IR.PhiInstruction) {
						IR.PhiInstruction phi = (IR.PhiInstruction)ctx.Instruction;
						ConstantOperand co = ctx.Instruction.Operand2 as ConstantOperand;
						if (co != null && ctx.Blocks.Count = 1) {
							// We can remove the phi, as it is only defined once
							// HACK: We can't track a constant through a register, so we keep those moves
							if (false == ctx.Result.IsRegister) {
								Debug.Assert(1 == ctx.Result.Definitions.Count, @"Operand defined multiple times. Instruction stream not in SSA form!");
								ctx.Result.Replace(co);
								remove = true;
							}
						}
					}

					// Shall we remove this instruction?
					if (remove) {
						ctx.Remove();
						remove = false;
					}

					ctx.Forward();
				}

			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pipeline"></param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertAfter<EnterSSA>(this);
		}

		#endregion // IMethodCompilerStage Members
	}
}
