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

namespace Mosa.Compiler.Framework.Stages
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
		private PropagationStage stage;

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
			foreach (BasicBlock block in basicBlocks)
			{
				for (Context ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					if (!(ctx.Instruction is IR.Move))
						continue;

					if (!(ctx.Operand1 is ConstantOperand))
						continue;

					var sop = ctx.Result as SsaOperand;
					if (sop == null || !(sop.Operand is StackOperand))
						continue;

					if (!AreResultsBuiltIn(sop))
						continue;

					// Replace all operand uses with constant
					sop.Replace(ctx.Operand1, instructionSet);

					ctx.Remove();
				}
			}
		}

		/// <summary>
		/// Checks the results are built in.
		/// </summary>
		/// <param name="sop">The sop.</param>
		/// <returns></returns>
		private bool AreResultsBuiltIn(SsaOperand sop)
		{
			foreach (int index in sop.Uses)
			{
				Context ctx = new Context(instructionSet, index);

				if (ctx.Result == null)
					continue;

				if (ctx.Instruction is IR.Move)
					return false;
				
				var result = ctx.Result is SsaOperand ? (ctx.Result as SsaOperand).Operand : ctx.Result;

				if (!(result.Type is BuiltInSigType))
					continue;

				if (!IsBuiltinType(result.Type))
					continue;

				return false;
			}

			return false;
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
