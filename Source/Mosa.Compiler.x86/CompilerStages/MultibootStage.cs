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
		var basicBlocks = new BasicBlocks();

		var methodCompiler = new MethodCompiler(Compiler, multibootMethod, basicBlocks, 0);
		methodCompiler.MethodData.DoNotInline = true;

		var transform = new Transform();
		transform.SetCompiler(Compiler);
		transform.SetMethodCompiler(methodCompiler);

		var startUpType = TypeSystem.GetTypeByName("Mosa.Runtime.StartUp");
		var initializeMethod = startUpType.FindMethodByName("Initialize");

		var entryPoint = Operand.CreateLabel(initializeMethod, Architecture.Is32BitPlatform);

		var eax = transform.PhysicalRegisters.Allocate32(CPURegister.EAX);
		var ebx = transform.PhysicalRegisters.Allocate32(CPURegister.EBX);
		var ebp = transform.PhysicalRegisters.Allocate32(CPURegister.EBP);
		var esp = transform.PhysicalRegisters.Allocate32(CPURegister.ESP);

		var multibootEAX = Operand.CreateLabel(MultibootEAX, Architecture.Is32BitPlatform);
		var multibootEBX = Operand.CreateLabel(MultibootEBX, Architecture.Is32BitPlatform);
		var stackBottom = Operand.CreateLabel(MultibootInitialStack, Architecture.Is32BitPlatform);

		var stackTopOffset = CreateConstant(StackSize - 8);

		var prologueBlock = basicBlocks.CreatePrologueBlock();

		var context = new Context(prologueBlock);

		// Setup the stack and place the sentinel on the stack to indicate the start of the stack
		context.AppendInstruction(X86.Mov32, esp, stackBottom);
		context.AppendInstruction(X86.Add32, esp, esp, stackTopOffset);
		context.AppendInstruction(X86.Mov32, ebp, stackBottom);
		context.AppendInstruction(X86.Add32, ebp, ebp, stackTopOffset);
		context.AppendInstruction(X86.MovStore32, null, esp, Operand.Constant32_0, Operand.Constant32_0);
		context.AppendInstruction(X86.MovStore32, null, esp, Operand.Constant32_8, Operand.Constant32_0);

		// Place the multiboot address into a static field
		context.AppendInstruction(X86.MovStore32, null, multibootEAX, Operand.Constant32_0, eax);
		context.AppendInstruction(X86.MovStore32, null, multibootEBX, Operand.Constant32_0, ebx);

		context.AppendInstruction(X86.Call, null, entryPoint);
		context.AppendInstruction(X86.Ret);

		Compiler.CompileMethod(transform);
	}
}
