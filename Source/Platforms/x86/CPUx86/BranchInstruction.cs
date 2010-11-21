/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Runtime.CompilerFramework;

using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platform.X86.CPUx86
{
	/// <summary>
	/// Representation a x86 branch instruction.
	/// </summary>
	public sealed class BranchInstruction : BaseInstruction
	{

		#region Data Members

		private static readonly byte[] JAE = new byte[] { 0x0F, 0x83 };
		private static readonly byte[] JNE = new byte[] { 0x0F, 0x85 };
		private static readonly byte[] JL = new byte[] { 0x0F, 0x8C };
		private static readonly byte[] JLE = new byte[] { 0x0F, 0x8E };
		private static readonly byte[] JG = new byte[] { 0x0F, 0x8F };
		private static readonly byte[] JGE = new byte[] { 0x0F, 0x8D };
		private static readonly byte[] JE = new byte[] { 0x0F, 0x84 };
		private static readonly byte[] JBE = new byte[] { 0x0F, 0x86 };
		private static readonly byte[] JA = new byte[] { 0x0F, 0x87 };
		private static readonly byte[] JB = new byte[] { 0x0F, 0x82 };
		private static readonly byte[] JNS = new byte[] { 0x0F, 0x89 };
		private static readonly byte[] JS = new byte[] { 0x0F, 0x88 };

		#endregion

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context ctx, MachineCodeEmitter emitter)
		{

			switch (ctx.ConditionCode)
			{
				case IR.ConditionCode.Equal:
					emitter.EmitBranch(JE, ctx.Branch.Targets[0]);
					break;

				case IR.ConditionCode.GreaterOrEqual:
					emitter.EmitBranch(JGE, ctx.Branch.Targets[0]);
					break;

				case IR.ConditionCode.GreaterThan:
					emitter.EmitBranch(JG, ctx.Branch.Targets[0]);
					break;

				case IR.ConditionCode.LessOrEqual:
					emitter.EmitBranch(JLE, ctx.Branch.Targets[0]);
					break;

				case IR.ConditionCode.LessThan:
					emitter.EmitBranch(JL, ctx.Branch.Targets[0]);
					break;

				case IR.ConditionCode.NotEqual:
					emitter.EmitBranch(JNE, ctx.Branch.Targets[0]);
					break;

				case IR.ConditionCode.UnsignedGreaterOrEqual:
					emitter.EmitBranch(JAE, ctx.Branch.Targets[0]);
					break;

				case IR.ConditionCode.UnsignedGreaterThan:
					emitter.EmitBranch(JA, ctx.Branch.Targets[0]);
					break;

				case IR.ConditionCode.UnsignedLessOrEqual:
					emitter.EmitBranch(JBE, ctx.Branch.Targets[0]);
					break;

				case IR.ConditionCode.UnsignedLessThan:
					emitter.EmitBranch(JB, ctx.Branch.Targets[0]);
					break;

				case IR.ConditionCode.NotSigned:
					emitter.EmitBranch(JNS, ctx.Branch.Targets[0]);
					break;

				case IR.ConditionCode.Signed:
					emitter.EmitBranch(JS, ctx.Branch.Targets[0]);
					break;

				default:
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Branch(context);
		}

		#endregion // Methods

	}
}
