// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.CompilerStages;

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

		var rax = transform.PhysicalRegisters.Allocate64(CPURegister.RAX);
		var rbx = transform.PhysicalRegisters.Allocate64(CPURegister.RBX);
		var rbp = transform.PhysicalRegisters.Allocate64(CPURegister.RBP);
		var rsp = transform.PhysicalRegisters.Allocate64(CPURegister.RSP);

		var multibootEAX = Operand.CreateLabel(MultibootEAX, Architecture.Is32BitPlatform);
		var multibootEBX = Operand.CreateLabel(MultibootEBX, Architecture.Is32BitPlatform);
		var stackBottom = Operand.CreateLabel(MultibootInitialStack, Architecture.Is32BitPlatform);

		var stackTopOffset = CreateConstant(StackSize - 16);

		var prologueBlock = basicBlocks.CreatePrologueBlock();

		var context = new Context(prologueBlock);

		// Setup the stack and place the sentinel on the stack to indicate the start of the stack
		context.AppendInstruction(X64.Mov64, rsp, stackBottom);
		context.AppendInstruction(X64.Add64, rsp, rsp, stackTopOffset);
		context.AppendInstruction(X64.Mov64, rbp, stackBottom);
		context.AppendInstruction(X64.Add64, rbp, rbp, stackTopOffset);
		context.AppendInstruction(X64.MovStore64, null, rsp, Operand.Constant64_0, Operand.Constant64_0);
		context.AppendInstruction(X64.MovStore64, null, rsp, Operand.Constant64_16, Operand.Constant64_0);

		// Place the multiboot address into a static field
		context.AppendInstruction(X64.MovStore64, null, multibootEAX, Operand.Constant64_0, rax);
		context.AppendInstruction(X64.MovStore64, null, multibootEBX, Operand.Constant64_0, rbx);

		context.AppendInstruction(X64.Call, null, entryPoint);
		context.AppendInstruction(X64.Ret);

		Compiler.CompileMethod(multibootMethod, basicBlocks);
	}
}
