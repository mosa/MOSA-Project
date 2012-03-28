/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System.Collections.Generic;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework
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
		/// <summary>
		/// 
		/// </summary>
		public enum PropagationStage
		{
			/// <summary>
			/// 
			/// </summary>
			PreFolding,
			/// <summary>
			/// 
			/// </summary>
			PostFolding,
		}

		/// <summary>
		/// 
		/// </summary>
		private PropagationStage stage = PropagationStage.PreFolding;
		/// <summary>
		/// 
		/// </summary>
		private HashSet<int> propagated = new HashSet<int>();

		/// <summary>
		/// Initializes a new instance of the <see cref="ConstantPropagationStage"/> class.
		/// </summary>
		public ConstantPropagationStage()
			: this(PropagationStage.PreFolding)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConstantPropagationStage"/> class.
		/// </summary>
		/// <param name="stage">The stage.</param>
		public ConstantPropagationStage(PropagationStage stage)
		{
			this.stage = stage;
		}

		#region IPipelineStage methods

		string IPipelineStage.Name { get { return base.Name + "-" + stage.ToString(); } }

		#endregion // IPipelineStage methods

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			if (AreExceptions)
				return;

			foreach (var block in this.basicBlocks)
				if (block.NextBlocks.Count == 0 && block.PreviousBlocks.Count == 0)
					return;

			bool remove = false;

			foreach (BasicBlock block in basicBlocks)
			{
				if (block == this.FindBlock(-1) || block == this.FindBlock(int.MaxValue))
					continue;

				for (Context ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					//if (this.propagated.Contains(ctx.Index))
					//	continue;

					if (!(ctx.Instruction is IR.MoveInstruction || ctx.Instruction is CIL.StlocInstruction))
						continue;

					if (!(ctx.Operand1 is ConstantOperand))
						continue;

					var sop = ctx.Result as SsaOperand;
					if (sop == null || !(sop.Operand is StackOperand))
						continue;

					if (!this.CheckResultsAreBuiltin(sop))
						continue;

					this.ReplaceUses(sop, ctx.Operand1 as ConstantOperand);
					ctx.SetInstruction(Instruction.NopInstruction);
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
				if (block == this.FindBlock(-1) || block == this.FindBlock(int.MaxValue))
					continue;

				for (Context ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					if (!this.InstructionUsesOperand(ctx, sop))
						continue;

					if (ctx.Result == null)
						continue;

					if (ctx.Instruction is IR.MoveInstruction)
						return false;

					var result = ctx.Result is SsaOperand ? (ctx.Result as SsaOperand).Operand : ctx.Result;

					if (this.CheckOperand(result))
						continue;
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Checks the operand.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		private bool CheckOperand(Operand operand)
		{
			if (operand.Type.GetType() == typeof(SigType))
				return IsBuiltinType(operand.Type);
			if (operand.Type is BuiltInSigType)
				return IsBuiltinType(operand.Type);
			return false;
		}

		/// <summary>
		/// Instructions the uses operand.
		/// </summary>
		/// <param name="ctx">The CTX.</param>
		/// <param name="sop">The sop.</param>
		/// <returns></returns>
		private bool InstructionUsesOperand(Context ctx, SsaOperand sop)
		{
			foreach (var operand in ctx.Operands)
			{
				if (!(operand is SsaOperand))
					continue;
				var ssaOp = operand as SsaOperand;
				return sop.Operand == ssaOp.Operand && sop.SsaVersion == ssaOp.SsaVersion;
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
				if (block == this.FindBlock(-1) || block == this.FindBlock(int.MaxValue))
					continue;

				for (Context ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					for (var i = 0; i < ctx.OperandCount; ++i)
					{
						var op = ctx.GetOperand(i) as SsaOperand;
						if (op == null)
							continue;
						if (op.Operand == sop.Operand && op.SsaVersion == sop.SsaVersion)
						{
							ctx.SetOperand(i, constantOperand);
							this.propagated.Add(ctx.Index);
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
				type.Type == CilElementType.U1 ||
				type.Type == CilElementType.U2 ||
				type.Type == CilElementType.U4;
		}

		#endregion // IMethodCompilerStage Members
	}
}
