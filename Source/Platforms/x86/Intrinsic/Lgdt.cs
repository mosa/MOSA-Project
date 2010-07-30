/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Vm;

namespace Mosa.Platforms.x86.Intrinsic
{
    /// <summary>
    /// Representations the x86 Lgdt instruction.
    /// </summary>
	public sealed class Lgdt : IIntrinsicMethod
    {
		
		#region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem)
		{
			MemoryOperand operand = new MemoryOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.Ptr), GeneralPurposeRegister.EAX, new System.IntPtr(0));
			context.SetInstruction(CPUx86.Instruction.MovInstruction, new RegisterOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.Ptr), GeneralPurposeRegister.EAX), context.Operand1);
			context.AppendInstruction(CPUx86.Instruction.LgdtInstruction, null, operand);

			RegisterOperand ax = new RegisterOperand (new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I2), GeneralPurposeRegister.EAX);
			RegisterOperand ds = new RegisterOperand (new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I2), SegmentRegister.DS);
			RegisterOperand es = new RegisterOperand (new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I2), SegmentRegister.ES);
			RegisterOperand fs = new RegisterOperand (new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I2), SegmentRegister.FS);
			RegisterOperand gs = new RegisterOperand (new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I2), SegmentRegister.GS);
			RegisterOperand ss = new RegisterOperand (new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I2), SegmentRegister.SS);

			context.AppendInstruction(CPUx86.Instruction.MovInstruction, ax, new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), (int)0x00000010));
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, ds, ax);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, es, ax);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, fs, ax);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, gs, ax);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, ss, ax);
			context.AppendInstruction(CPUx86.Instruction.FarJmpInstruction);
			context.AppendInstruction(CPUx86.Instruction.NopInstruction);
			context.Previous.SetBranch(context.Offset);
		}

		#endregion // Methods

    }
}
