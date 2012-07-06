/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System.Collections.Generic;

// FIXME: This stage has not been updated and does not work as-is.

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// A stage to compute local common subexpression elimination
	/// according to Steven S. Muchnick, Advanced Compiler Design 
	/// and Implementation (Morgan Kaufmann, 1997) pp. 378-396
	/// </summary>
	public class LocalCSE : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
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
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			foreach (BasicBlock block in basicBlocks)
				EliminateCommonSubexpressions(new Context(instructionSet, block));
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

			for (; !ctx.EndOfInstruction; ctx.GotoNext())
			{
				BaseInstruction instruction = ctx.Instruction; 
				Operand temp = null;
				bool found = false;

				if ((instruction is CIL.ArithmeticInstruction) && (instruction is CIL.BinaryInstruction))
				{
					tmp = new List<AEBinExp>(AEB);

					while (tmp.Count > 0)
					{
						aeb = tmp[0];
						tmp.RemoveAt(0);

						// Match current instruction's expression against those
						// in AEB, including commutativity
						if (IsCommutative(instruction))
						{
							//int position = aeb.Position;
							found = true;

							// If no variable in tuple, create a new temporary and
							// insert an instruction evaluating the expression
							// and assigning it to the temporary
							if (aeb.Var == null)
							{
								// new_tmp()
								AEB.Remove(aeb);
								AEB.Add(new AEBinExp(aeb.Position, aeb.Operand1, aeb.Operator, aeb.Operand2, temp));

								// Insert new assignment to instruction stream in block
								Context inserted = ctx.InsertBefore();

								switch (aeb.Operator)
								{
									case Operation.Add:
										inserted.SetInstruction(CIL.CILInstruction.Get(CIL.OpCode.Add), temp, aeb.Operand1, aeb.Operand2);
										break;
									case Operation.Mul:
										inserted.SetInstruction(CIL.CILInstruction.Get(CIL.OpCode.Mul), temp, aeb.Operand1, aeb.Operand2);
										break;
									case Operation.Or:
										inserted.SetInstruction(CIL.CILInstruction.Get(CIL.OpCode.Or), temp, aeb.Operand1, aeb.Operand2);
										break;
									case Operation.Xor:
										inserted.SetInstruction(CIL.CILInstruction.Get(CIL.OpCode.Xor), temp, aeb.Operand1, aeb.Operand2);
										break;
									default:
										break;
								}

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
							else
							{
								temp = (Operand)aeb.Var;
							}

							// FIXME PG
							// block.Instructions[i] = new IR.MoveInstruction(instruction.Results[0], temp);
						}
					}

					if (!found)
					{
						Operation opr = Operation.None;

						if (instruction is CIL.AddInstruction)
							opr = Operation.Add;
						else if (instruction is CIL.MulInstruction)
							opr = Operation.Mul;
						else if (instruction is IR.LogicalAnd)
							opr = Operation.And;
						// Insert new tuple
						AEB.Add(new AEBinExp(ctx.Index, ctx.Operand1, opr, ctx.Operand2, null));
					}

					// Remove all tuples that use the variable assigned to by
					// the current instruction
					tmp = new List<AEBinExp>(AEB);

					while (tmp.Count > 0)
					{
						aeb = tmp[0];
						tmp.RemoveAt(0);

						if (ctx.Operand1 == aeb.Operand1 || ctx.Operand2 == aeb.Operand2)
							AEB.Remove(aeb);
					}
				}
			}
		}

		/// <summary>
		/// Determines whether the specified instruction is commutative.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// 	<c>true</c> if the specified instruction is commutative; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsCommutative(BaseInstruction instruction)
		{
			return (instruction is CIL.AddInstruction) ||
				   (instruction is CIL.MulInstruction) ||
				   (instruction is IR.LogicalAnd) ||
				   (instruction is IR.LogicalOr) ||
				   (instruction is IR.LogicalXor);
		}
	}
}
