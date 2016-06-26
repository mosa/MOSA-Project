// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 Lgdt instruction.
	/// </summary>
	internal sealed class Lgdt : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Debug.Assert(context.Operand1.IsConstant); // only constants are supported

			var zero = Operand.CreateConstant(methodCompiler.TypeSystem, 0);
			var constantx10 = Operand.CreateConstant(methodCompiler.TypeSystem, 0x10);

			Operand ax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, GeneralPurposeRegister.EAX);
			Operand ds = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.DS);
			Operand es = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.ES);
			Operand fs = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.FS);
			Operand gs = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.GS);
			Operand ss = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.SS);

			context.SetInstruction(X86.Lgdt, null, context.Operand1);
			context.AppendInstruction(X86.Mov, ax, constantx10);
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
