using System;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Intermediate representation of the x86 CPUID instruction.
	/// </summary>
	public sealed class BochsDebug : BaseInstruction
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
		public override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
			ctx.Result = new RegisterOperand(new SigType(Runtime.Metadata.CilElementType.I2), GeneralPurposeRegister.EBX);
			ctx.Operand1 = new RegisterOperand(new SigType(Runtime.Metadata.CilElementType.I2), GeneralPurposeRegister.EBX);
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
			return String.Format(@"X86.Xchg bx, bx ; [BochsDebug]");
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

		#endregion // Methods
	}
}
