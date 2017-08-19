// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Platform.x86.MethodStages;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Frame Call Ret R8
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicPlatformMethod" />
	internal class FrameCallRetR8 : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			Operand result = context.Result;
			Operand methodAddress = context.Operand1;
			Operand newESP = context.Operand2;

			Operand eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			Operand edx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);
			Operand mmx1 = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, SSE2Register.XMM0);

			LongOperandTransformationStage.SplitLongOperand(methodCompiler, result, out Operand op0L, out Operand op0H);

			context.SetInstruction(X86.Call, null, methodAddress);
			context.AppendInstruction(IRInstruction.Gen, mmx1);

			context.AppendInstruction(X86.Movd, eax, mmx1);
			context.AppendInstruction(X86.Pextrd, edx, mmx1, Operand.CreateConstant(methodCompiler.TypeSystem.BuiltIn.U1, 1));

			context.AppendInstruction(X86.Mov, op0L, eax);
			context.AppendInstruction(X86.Mov, op0H, edx);
		}

		#endregion Methods
	}
}
