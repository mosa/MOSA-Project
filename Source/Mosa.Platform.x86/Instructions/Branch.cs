/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using IR = Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representation a x86 branch instruction.
	/// </summary>
	public sealed class Branch : X86Instruction
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
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{

			switch (context.ConditionCode)
			{
				case IR.ConditionCode.Equal:
					emitter.EmitBranch(JE, context.Branch.Targets[0]);
					break;

				case IR.ConditionCode.GreaterOrEqual:
					emitter.EmitBranch(JGE, context.Branch.Targets[0]);
					break;

				case IR.ConditionCode.GreaterThan:
					emitter.EmitBranch(JG, context.Branch.Targets[0]);
					break;

				case IR.ConditionCode.LessOrEqual:
					emitter.EmitBranch(JLE, context.Branch.Targets[0]);
					break;

				case IR.ConditionCode.LessThan:
					emitter.EmitBranch(JL, context.Branch.Targets[0]);
					break;

				case IR.ConditionCode.NotEqual:
					emitter.EmitBranch(JNE, context.Branch.Targets[0]);
					break;

				case IR.ConditionCode.UnsignedGreaterOrEqual:
					emitter.EmitBranch(JAE, context.Branch.Targets[0]);
					break;

				case IR.ConditionCode.UnsignedGreaterThan:
					emitter.EmitBranch(JA, context.Branch.Targets[0]);
					break;

				case IR.ConditionCode.UnsignedLessOrEqual:
					emitter.EmitBranch(JBE, context.Branch.Targets[0]);
					break;

				case IR.ConditionCode.UnsignedLessThan:
					emitter.EmitBranch(JB, context.Branch.Targets[0]);
					break;

				case IR.ConditionCode.NotSigned:
					emitter.EmitBranch(JNS, context.Branch.Targets[0]);
					break;

				case IR.ConditionCode.Signed:
					emitter.EmitBranch(JS, context.Branch.Targets[0]);
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
