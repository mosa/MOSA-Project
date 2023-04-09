// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.CompilerStages;

public sealed class MultibootV1Stage : Mosa.Compiler.Framework.Platform.BaseMultibootV1Stage
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

		var entryPoint = Operand.CreateSymbolFromMethod(initializeMethod, Architecture.Is32BitPlatform);

		var rax = Operand.CreateCPURegister64(CPURegister.RAX);
		var rbx = Operand.CreateCPURegister64(CPURegister.RBX);
		var rbp = Operand.CreateCPURegister64(CPURegister.RBP);
		var rsp = Operand.CreateCPURegister64(CPURegister.RSP);

		var multibootEAX = Operand.CreateUnmanagedSymbolPointer(MultibootEAX, TypeSystem);
		var multibootEBX = Operand.CreateUnmanagedSymbolPointer(MultibootEBX, TypeSystem);

		var stackTop = CreateConstant(InitialStackAddress);
		var zero = CreateConstant(0);
		var offset = CreateConstant(8);

		var basicBlocks = new BasicBlocks();

		var prologueBlock = basicBlocks.CreatePrologueBlock();

		var context = new Context(prologueBlock);

		// Setup the stack and place the sentinel on the stack to indicate the start of the stack
		context.AppendInstruction(X64.Mov64, rsp, stackTop);
		context.AppendInstruction(X64.Mov64, rbp, stackTop);
		context.AppendInstruction(X64.MovStore64, null, rsp, zero, zero);
		context.AppendInstruction(X64.MovStore64, null, rsp, offset, zero);

		// Place the multiboot address into a static field
		context.AppendInstruction(X64.MovStore64, null, multibootEAX, zero, rax);
		context.AppendInstruction(X64.MovStore64, null, multibootEBX, zero, rbx);

		context.AppendInstruction(X64.Call, null, entryPoint);
		context.AppendInstruction(X64.Ret);

		Compiler.CompileMethod(multibootMethod, basicBlocks);
	}
}
