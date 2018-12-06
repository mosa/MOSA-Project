// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Platform.Intel;
using System.Diagnostics;

namespace Mosa.Platform.x64.Stages
{
	/// <summary>
	/// Completes the stack handling after register allocation
	/// </summary>
	/// <seealso cref="Mosa.Platform.Intel.Stages.BuildStackStage" />
	public sealed class BuildStackStage : Intel.Stages.BuildStackStage
	{
		/// <summary>
		/// Adds the prologue instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		protected override void AddPrologueInstructions(Context context)
		{
			var ebp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, GeneralPurposeRegister.EBP);
			var esp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, GeneralPurposeRegister.ESP);

			context.SetInstruction(X64.Push64, null, ebp);
			context.AppendInstruction(X64.Mov64, ebp, esp);

			if (MethodCompiler.StackSize != 0)
			{
				context.AppendInstruction(X64.Sub64, esp, esp, CreateConstant(-MethodCompiler.StackSize));
			}
		}

		/// <summary>
		/// Adds the epilogue instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		protected override void AddEpilogueInstructions(Context context)
		{
			var ebp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, GeneralPurposeRegister.EBP);
			var esp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, GeneralPurposeRegister.ESP);

			context.Empty();

			if (MethodCompiler.StackSize != 0)
			{
				context.AppendInstruction(X64.Add64, esp, esp, CreateConstant(-MethodCompiler.StackSize));
			}

			context.AppendInstruction(X64.Pop64, ebp);
			context.AppendInstruction(X64.Ret);
		}
	}
}
