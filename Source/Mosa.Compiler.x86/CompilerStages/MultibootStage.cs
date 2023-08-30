// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.CompilerStages;

public sealed class MultibootStage : Mosa.Compiler.Framework.Platform.BaseMultibootStage
{
	protected override void Finalization()
	{
		CreateMultibootMethod();

		WriteMultibootHeader(Linker.EntryPoint);
	}

	private void CreateMultibootMethod()
	{
		var startUpType = TypeSystem.GetTypeByName("Mosa.Runtime.StartUp");
		var initializeMethod = startUpType.FindMethodByName("Initialize");

		Compiler.GetMethodData(initializeMethod).DoNotInline = true;

		var entryPoint = Operand.CreateLabel(initializeMethod, Architecture.Is32BitPlatform);

		var eax = Operand.CreateCPURegister32(CPURegister.EAX);
		var ebx = Operand.CreateCPURegister32(CPURegister.EBX);
		var ebp = Operand.CreateCPURegister32(CPURegister.EBP);
		var esp = Operand.CreateCPURegister32(CPURegister.ESP);

		var multibootEAX = Operand.CreateLabel(MultibootEAX, Architecture.Is32BitPlatform);
		var multibootEBX = Operand.CreateLabel(MultibootEBX, Architecture.Is32BitPlatform);
		var stackBottom = Operand.CreateLabel(MultibootInitialStack, Architecture.Is32BitPlatform);

		var stackTopOffset = CreateConstant(StackSize - 8);
		var zero = CreateConstant(0);
		var offset = CreateConstant(4);

		var basicBlocks = new BasicBlocks();

		var prologueBlock = basicBlocks.CreatePrologueBlock();

		var context = new Context(prologueBlock);

		// Setup the stack and place the sentinel on the stack to indicate the start of the stack
		context.AppendInstruction(X86.Mov32, esp, stackBottom);
		context.AppendInstruction(X86.Add32, esp, esp, stackTopOffset);
		context.AppendInstruction(X86.Mov32, ebp, stackBottom);
		context.AppendInstruction(X86.Add32, ebp, ebp, stackTopOffset);
		context.AppendInstruction(X86.MovStore32, null, esp, zero, zero);
		context.AppendInstruction(X86.MovStore32, null, esp, offset, zero);

		// Place the multiboot address into a static field
		context.AppendInstruction(X86.MovStore32, null, multibootEAX, zero, eax);
		context.AppendInstruction(X86.MovStore32, null, multibootEBX, zero, ebx);

		context.AppendInstruction(X86.Call, null, entryPoint);  // FUTURE: Remove line (SetupStage)
		context.AppendInstruction(X86.Ret);

		Compiler.CompileMethod(multibootMethod, basicBlocks);
	}
}
