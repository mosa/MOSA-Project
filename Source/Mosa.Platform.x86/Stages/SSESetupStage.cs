// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Sets up SSE before any code that relies on SSE runs.
	/// </summary>
	public sealed class SSESetupStage : BaseCompilerStage
	{
		#region Constants

		#endregion Constants

		#region Data members

		/// <summary>
		/// The SSE setup method
		/// </summary>
		private MosaMethod setupMethod;

		#endregion Data members

		protected override void Run()
		{
			Debug.Assert(setupMethod == null, "SSE setup method already generated!");

			setupMethod = Compiler.CreateLinkerMethod("SSEInit");

			var eax = Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EAX);
			var cr0 = Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, ControlRegister.CR0);
			var cr4 = Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, ControlRegister.CR4);

			var basicBlocks = new BasicBlocks();
			var block = basicBlocks.CreateBlock();
			basicBlocks.AddHeaderBlock(block);
			var ctx = new Context(block);

			/*
			   ;enable SSE and the like
				mov eax, cr0
				and ax, 0xFFFB		;clear coprocessor emulation CR0.EM
				or ax, 0x2			;set coprocessor monitoring  CR0.MP
				mov cr0, eax
				mov eax, cr4
				or ax, 3 << 9		;set CR4.OSFXSR and CR4.OSXMMEXCPT at the same time
				mov cr4, eax
				ret
			*/

			ctx.AppendInstruction(X86.MovCR, eax, cr0);
			ctx.AppendInstruction(X86.And, eax, eax, Operand.CreateConstant(TypeSystem.BuiltIn.U4, 0xFFFB));
			ctx.AppendInstruction(X86.Or, eax, eax, Operand.CreateConstant(TypeSystem.BuiltIn.U4, 0x2));
			ctx.AppendInstruction(X86.MovCR, cr0, eax);

			ctx.AppendInstruction(X86.MovCR, eax, cr4);
			ctx.AppendInstruction(X86.Or, eax, eax, Operand.CreateConstant(TypeSystem.BuiltIn.U4, 0x600));
			ctx.AppendInstruction(X86.MovCR, cr4, eax);

			ctx.AppendInstruction(X86.Ret);

			Compiler.CompileMethod(setupMethod, basicBlocks, 0);

			var typeInitializerSchedulerStage = Compiler.PostCompilePipeline.FindFirst<TypeInitializerSchedulerStage>();
			typeInitializerSchedulerStage.Schedule(setupMethod);
		}

		#region Internals

		#endregion Internals
	}
}