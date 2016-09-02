// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
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
			//Debug.Assert(context.Operand1.IsConstant); // only constants are supported
			var operand1 = context.Operand1;

			// HACK --- for when optimizations are turned off
			if (!operand1.IsConstant)
			{
				if (operand1.Definitions.Count == 1)
				{
					var node = operand1.Definitions[0];

					if ((node.Instruction == X86.Mov || node.Instruction == IRInstruction.MoveInteger) && node.Operand1.IsConstant)
						operand1 = node.Operand1;
				}
			}

			Debug.Assert(operand1.IsConstant); // only constants are supported

			var constantx10 = Operand.CreateConstant(methodCompiler.TypeSystem, 0x10);

			Operand ax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, GeneralPurposeRegister.EAX);
			Operand ds = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.DS);
			Operand es = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.ES);
			Operand fs = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.FS);
			Operand gs = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.GS);
			Operand ss = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I2, SegmentRegister.SS);

			context.SetInstruction(X86.Lgdt, null, operand1);
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
