using System;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 CPUID instruction.
	/// </summary>
	public sealed class BochsDebug : BaseInstruction, IIntrinsicInstruction
	{
		#region Data Members

		private static readonly OpCode code = new OpCode(new byte[] { 0x66, 0x87 });

		#endregion

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			return code;
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
			ctx.Result = new RegisterOperand(new SigType(Runtime.Metadata.CilElementType.I2), GeneralPurposeRegister.EBX);
			ctx.Operand1 = ctx.Result;
			base.Emit(ctx, emitter);
		}

		/// <summary>
		/// Returns a string representation of the instruction.
		/// </summary>
		/// <returns>
		/// A string representation of the instruction in intermediate form.
		/// </returns>
		public override string ToString()
		{
			return base.ToString() + " [BochsDebug]";
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.BochsDebug(context);
		}

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		public void ReplaceIntrinsicCall(Context context)
		{
			context.SetInstruction(CPUx86.Instruction.DebugInstruction);
		}

		#endregion // Methods
	}
}
