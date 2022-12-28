// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.CompilerStages
{
	public sealed class MultibootV1Stage : Intel.CompilerStages.MultibootV1Stage
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

			var entryPoint = Operand.CreateSymbolFromMethod(initializeMethod, TypeSystem);

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, CPURegister.RAX);
			var rbx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, CPURegister.RBX);
			var rbp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, CPURegister.RBP);
			var rsp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, CPURegister.RSP);

			var multibootEAX = Operand.CreateUnmanagedSymbolPointer(MultibootEAX, TypeSystem);
			var multibootEBX = Operand.CreateUnmanagedSymbolPointer(MultibootEBX, TypeSystem);

			var stackTop = CreateConstant(InitialStackAddress);
			var zero = CreateConstant(0);
			var offset = CreateConstant(8);

			var basicBlocks = new BasicBlocks();
			var block = basicBlocks.CreateBlock(BasicBlock.PrologueLabel);
			basicBlocks.AddHeadBlock(block);
			var ctx = new Context(block);

			// Setup the stack and place the sentinel on the stack to indicate the start of the stack
			ctx.AppendInstruction(X64.Mov64, rsp, stackTop);
			ctx.AppendInstruction(X64.Mov64, rbp, stackTop);
			ctx.AppendInstruction(X64.MovStore64, null, rsp, zero, zero);
			ctx.AppendInstruction(X64.MovStore64, null, rsp, offset, zero);

			// Place the multiboot address into a static field
			ctx.AppendInstruction(X64.MovStore64, null, multibootEAX, zero, rax);
			ctx.AppendInstruction(X64.MovStore64, null, multibootEBX, zero, rbx);

			ctx.AppendInstruction(X64.Call, null, entryPoint);
			ctx.AppendInstruction(X64.Ret);

			Compiler.CompileMethod(multibootMethod, basicBlocks);
		}
	}
}
