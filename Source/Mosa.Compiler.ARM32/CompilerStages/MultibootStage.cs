// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.CompilerStages;

public sealed class MultibootStage : Framework.Platform.BaseMultibootStage
{
	protected override void Finalization()
	{
		CreateMultibootMethod();

		WriteMultibootHeader(Linker.EntryPoint);
	}

	private void CreateMultibootMethod()
	{
		var basicBlocks = new BasicBlocks();

		var methodCompiler = new MethodCompiler(Compiler, multibootMethod, basicBlocks, 0);
		methodCompiler.MethodData.DoNotInline = true;

		var transform = new Transform();
		transform.SetCompiler(Compiler);
		transform.SetMethodCompiler(methodCompiler);

		var initializeMethod = TypeSystem.GetMethod("Mosa.Runtime.StartUp", "Initialize");
		var entryPoint = Operand.CreateLabel(initializeMethod, Architecture.Is32BitPlatform);

		var lr = transform.PhysicalRegisters.Allocate64(CPURegister.LR);
		var sp = transform.PhysicalRegisters.Allocate64(CPURegister.SP);

		var d10 = transform.PhysicalRegisters.Allocate64(CPURegister.d10);
		var d11 = transform.PhysicalRegisters.Allocate64(CPURegister.d11);

		var stackBottom = Operand.CreateLabel(MultibootInitialStack, Architecture.Is32BitPlatform);

		var prologueBlock = basicBlocks.CreatePrologueBlock();

		var context = new Context(prologueBlock);

		// Set stack location
		context.AppendInstruction(ARM32.Movw, sp, stackBottom);
		context.AppendInstruction(ARM32.Movt, sp, sp, stackBottom);

		// Create stack sentinel
		context.AppendInstruction(ARM32.Movw, lr, Operand.Constant32_0);
		context.AppendInstruction(ARM32.Stm, null, sp, Operand.Constant32_0, lr);
		context.AppendInstruction(ARM32.Stm, null, sp, Operand.Constant32_0, lr);

		//// Push registers onto the new stack
		context.AppendInstruction(ARM32.Mov, lr, sp);
		context.AppendInstruction(ARM32.Stm, null, sp, Operand.Constant32_0, Operand.Constant32_FFFF);
		context.AppendInstruction(ARM32.Stm, null, sp, Operand.Constant32_0, sp);

		// Call entry point
		context.AppendInstruction(ARM32.Bl, null, entryPoint);
		context.AppendInstruction(ARM32.Pop, null, Operand.Constant32_0);

		Compiler.CompileMethod(multibootMethod);
	}
}
