/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 Lgdt instruction.
	/// </summary>
	public sealed class Lgdt : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			context.SetInstruction(X86.Lgdt, null, Operand.CreateMemoryAddress(BuiltInSigType.Ptr, context.Operand1, 0));

			Operand ax = Operand.CreateCPURegister(BuiltInSigType.Int16, GeneralPurposeRegister.EAX);
			Operand ds = Operand.CreateCPURegister(BuiltInSigType.Int16, SegmentRegister.DS);
			Operand es = Operand.CreateCPURegister(BuiltInSigType.Int16, SegmentRegister.ES);
			Operand fs = Operand.CreateCPURegister(BuiltInSigType.Int16, SegmentRegister.FS);
			Operand gs = Operand.CreateCPURegister(BuiltInSigType.Int16, SegmentRegister.GS);
			Operand ss = Operand.CreateCPURegister(BuiltInSigType.Int16, SegmentRegister.SS);

			context.AppendInstruction(X86.Mov, ax, Operand.CreateConstantSignedInt(0x10));
			context.AppendInstruction(X86.Mov, ds, ax);
			context.AppendInstruction(X86.Mov, es, ax);
			context.AppendInstruction(X86.Mov, fs, ax);
			context.AppendInstruction(X86.Mov, gs, ax);
			context.AppendInstruction(X86.Mov, ss, ax);
			context.AppendInstruction(X86.FarJmp);
		}

		#endregion Methods
	}
}