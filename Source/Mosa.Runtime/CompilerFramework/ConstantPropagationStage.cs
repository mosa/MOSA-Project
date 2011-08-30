/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// This method compiler stage performs constant propagation, e.g. it removes
	/// local variables in favor of constants.
	/// </summary>
	/// <remarks>
	/// Constant propagation has a couple of advantages: First of all it removes
	/// a local variable from the stack and secondly it reduces the register pressure
	/// on systems with only a small number of registers (x86).
	/// <para/>
	/// It is only safe to use this stage on an instruction stream in SSA form.
	/// </remarks>
	public sealed class ConstantPropagationStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"ConstantPropagationStage"; } }

		#endregion // IPipelineStage Members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
			if (this.methodCompiler.Method.Name == "PrintBrand")
				System.Console.WriteLine();
			bool remove = false;

			foreach (BasicBlock block in basicBlocks)
			{
				for (Context ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					if (ctx.Instruction is IR.MoveInstruction || ctx.Instruction is CIL.StlocInstruction)
					{
						if (ctx.Operand1 is ConstantOperand)
						{
							var sop = ctx.Result as SsaOperand;

							if (sop != null && sop.Operand is StackOperand)
							{
								if (!this.CheckResultsAreBuiltin(sop))
								{
									this.ReplaceUses(sop, ctx.Operand1 as ConstantOperand);
									remove = true;
								}
							}
						}
					}

					// Shall we remove this instruction?
					if (remove)
					{
						ctx.Remove();
						remove = false;
					}

				}
			}
		}

		/// <summary>
		/// Checks for by ref.
		/// </summary>
		/// <param name="sop">The sop.</param>
		private bool CheckResultsAreBuiltin(SsaOperand sop)
		{
			foreach (BasicBlock block in basicBlocks)
			{
				for (Context ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					if (ctx.Result == null)
						continue;

					var ssaOp = ctx.Result as SsaOperand;
					if (ssaOp != null)
					{
						if (!(ssaOp.Operand.Type is BuiltInSigType))
							return true;
					}
					else if (!(ctx.Result.Type is BuiltInSigType))
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Replaces the uses.
		/// </summary>
		/// <param name="sop">The sop.</param>
		/// <param name="constantOperand">The constant operand.</param>
		private void ReplaceUses(SsaOperand sop, ConstantOperand constantOperand)
		{
			foreach (BasicBlock block in basicBlocks)
			{
				for (Context ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					if (ctx.Instruction is IR.AddressOfInstruction)
						continue;
					for (var i = 0; i < ctx.OperandCount; ++i)
					{
						var op = ctx.GetOperand(i) as SsaOperand;
						if (op == null)
							continue;
						if (op.Operand == sop.Operand && op.SsaVersion == sop.SsaVersion)
						{
							ctx.SetOperand(i, constantOperand);
						}
					}
				}
			}
		}

		/// <summary>
		/// Determines whether [is builtin type] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		///   <c>true</c> if [is builtin type] [the specified type]; otherwise, <c>false</c>.
		/// </returns>
		private bool IsBuiltinType(SigType type)
		{
			return type.Type == CilElementType.Boolean ||
				type.Type == CilElementType.Char ||
				type.Type == CilElementType.I ||
				type.Type == CilElementType.I1 ||
				type.Type == CilElementType.I2 ||
				type.Type == CilElementType.I4 ||
				type.Type == CilElementType.I8 ||
				type.Type == CilElementType.U1 ||
				type.Type == CilElementType.U2 ||
				type.Type == CilElementType.U4 ||
				type.Type == CilElementType.U8;
		}

		#endregion // IMethodCompilerStage Members
	}
}
