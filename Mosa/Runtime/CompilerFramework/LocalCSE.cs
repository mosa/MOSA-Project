/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using System.Collections.Generic;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// A stage to compute local common subexpression elimination
	/// according to Steven S. Muchnick, Advanced Compiler Design 
	/// and Implementation (Morgan Kaufmann, 1997) pp. 378-396
	/// </summary>
	public class LocalCSE : IMethodCompilerStage
	{
		/// <summary>
		/// 
		/// </summary>
		struct AEBinExp
		{
			/// <summary>
			/// 
			/// </summary>
			public readonly int Position;
			/// <summary>
			/// 
			/// </summary>
			public readonly Operand Operand1;
			/// <summary>
			/// 
			/// </summary>
			public readonly Operation Operator;
			/// <summary>
			/// 
			/// </summary>
			public readonly Operand Operand2;
			/// <summary>
			/// 
			/// </summary>
			public readonly Operand Var;

			/// <summary>
			/// Initializes a new instance of the <see cref="AEBinExp"/> struct.
			/// </summary>
			/// <param name="pos">The pos.</param>
			/// <param name="op1">The op1.</param>
			/// <param name="opr">The opr.</param>
			/// <param name="op2">The op2.</param>
			/// <param name="var">The var.</param>
			public AEBinExp(int pos, Operand op1, Operation opr, Operand op2, Operand var)
			{
				Position = pos;
				Operand1 = op1;
				Operator = opr;
				Operand2 = op2;
				Var = var;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		enum Operation
		{
			/// <summary>
			/// 
			/// </summary>
			None,
			/// <summary>
			/// 
			/// </summary>
			Add,
			/// <summary>
			/// 
			/// </summary>
			Mul,
			/// <summary>
			/// 
			/// </summary>
			And,
			/// <summary>
			/// 
			/// </summary>
			Or,
			/// <summary>
			/// 
			/// </summary>
			Xor
		}

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"Local Common Subexpression Elimination Stage"; }
		}

		/// <summary>
		/// </summary>
		/// <param name="pipeline"></param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertAfter<ILConstantFoldingStage>(this);
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(IMethodCompiler compiler)
		{
			// Retrieve the basic block provider
			IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
			if (blockProvider == null)
				throw new InvalidOperationException(@"Instruction stream must have been split to basic Blocks.");

			// Retrieve the instruction provider and the instruction set
			InstructionSet instructionset = (compiler.GetPreviousStage(typeof(IInstructionsProvider)) as IInstructionsProvider).InstructionSet;

			foreach (BasicBlock block in blockProvider.Blocks)
				EliminateCommonSubexpressions(new Context(instructionset, block));
		}

		/// <summary>
		/// Eliminates the common subexpressions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private static void EliminateCommonSubexpressions(Context ctx)
		{
			List<AEBinExp> AEB = new List<AEBinExp>();
			List<AEBinExp> tmp;

			AEBinExp aeb;

			//int i = 0;
			
			while (!ctx.EndOfInstructions)
			{
				IInstruction instruction = ctx.Instruction; // block.Instructions[i];
				RegisterOperand temp = null;
				bool found = false;

				if ((instruction is CIL.ArithmeticInstruction) && (instruction is CIL.BinaryInstruction)) {
					tmp = new List<AEBinExp>(AEB);

					while (tmp.Count > 0) {
						aeb = tmp[0];
						tmp.RemoveAt(0);

						// Match current instruction's expression against those
						// in AEB, including commutativity
						if (IsCommutative(instruction)) {
							//int position = aeb.Position;
							found = true;

							// If no variable in tuple, create a new temporary and
							// insert an instruction evaluating the expression
							// and assigning it to the temporary
							if (aeb.Var == null) {
								// new_tmp()
								AEB.Remove(aeb);
								AEB.Add(new AEBinExp(aeb.Position, aeb.Operand1, aeb.Operator, aeb.Operand2, temp));

								// Insert new assignment to instruction stream in block
								Context insert = ctx.InsertBefore();

								switch (aeb.Operator) {
									case Operation.Add:
										//inst = new IL.AddInstruction(IL.OpCode.Add, temp, aeb.Operand1, aeb.Operand2);
										insert.SetInstruction(CIL.Map.GetInstruction(CIL.OpCode.Add));
										break;
									case Operation.Mul:
										//inst = new IL.MulInstruction(IL.OpCode.Mul, temp, aeb.Operand1, aeb.Operand2);
										insert.SetInstruction(CIL.Map.GetInstruction(CIL.OpCode.Mul));
										break;
									case Operation.Or:
										//inst = new IL.BinaryLogicInstruction(IL.OpCode.Or);
										//inst.SetResult(0, temp);
										//inst.SetOperand(0, aeb.Operand1);
										//inst.SetOperand(1, aeb.Operand2);
										insert.SetInstruction(CIL.Map.GetInstruction(CIL.OpCode.Or));
										break;
									case Operation.Xor:
										//inst = new IL.BinaryLogicInstruction(IL.OpCode.Xor);
										//inst.SetResult(0, temp);
										//inst.SetOperand(0, aeb.Operand1);
										//inst.SetOperand(1, aeb.Operand2);
										insert.SetInstruction(CIL.Map.GetInstruction(CIL.OpCode.Xor));
										break;
									default:
										break;
								}

								insert.Operand1 = aeb.Operand1;
								insert.Operand2 = aeb.Operand2;
								insert.Result = temp; 

								//block.Instructions.Insert(position, inst);

								//++position;
								//++i;

								// Replace current instruction by one that copies
								// the temporary instruction
								// FIXME PG:
								// block.Instructions[position] = new IR.MoveInstruction(block.Instructions[position].Results[0], temp);
								// ctx.SetInstruction(IR.MoveInstruction); // FIXME PG
 								// ctx.Result = block.Instructions[position].Results[0]; // FIXME PG
								ctx.Operand1 = temp; 
							}
							else {
								temp = (RegisterOperand)aeb.Var;
							}

							// FIXME PG
							// block.Instructions[i] = new IR.MoveInstruction(instruction.Results[0], temp);
						}
					}

					if (!found) {
						Operation opr = Operation.None;

						if (instruction is CIL.AddInstruction)
							opr = Operation.Add;
						else if (instruction is CIL.MulInstruction)
							opr = Operation.Mul;
						else if (instruction is IR.LogicalAndInstruction)
							opr = Operation.And;
						// Insert new tuple
						AEB.Add(new AEBinExp(ctx.Index, ctx.Operand1, opr, ctx.Operand2, null));
					}

					// Remove all tuples that use the variable assigned to by
					// the current instruction
					tmp = new List<AEBinExp>(AEB);

					while (tmp.Count > 0) {
						aeb = tmp[0];
						tmp.RemoveAt(0);

						if (ctx.Operand1 == aeb.Operand1 || ctx.Operand2 == aeb.Operand2)
							AEB.Remove(aeb);
					}
				}

				//++i;
				ctx.Forward();
			}
		}

		/// <summary>
		/// Determines whether the specified instruction is commutative.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// 	<c>true</c> if the specified instruction is commutative; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsCommutative(IInstruction instruction)
		{
			return (instruction is CIL.AddInstruction) ||
				   (instruction is CIL.MulInstruction) ||
				   (instruction is IR.LogicalAndInstruction) ||
				   (instruction is IR.LogicalOrInstruction) ||
				   (instruction is IR.LogicalXorInstruction);
		}
	}
}
