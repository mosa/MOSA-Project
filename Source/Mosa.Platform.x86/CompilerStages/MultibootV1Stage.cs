// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.CompilerStages
{
	public sealed class MultibootV1Stage : Intel.CompilerStages.MultibootV1Stage
	{
		protected override void CreateMultibootMethod()
		{
			var startUpType = TypeSystem.GetTypeByName("Mosa.Runtime", "StartUp");
			var initializeMethod = startUpType.FindMethodByName("Initialize");
			var entryPoint = Operand.CreateSymbolFromMethod(initializeMethod, TypeSystem);

			var eax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			var ebx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EBX);
			var ebp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EBP);
			var esp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);

			var multibootEAX = Operand.CreateUnmanagedSymbolPointer(MultibootEAX, TypeSystem);
			var multibootEBX = Operand.CreateUnmanagedSymbolPointer(MultibootEBX, TypeSystem);

			var stackTop = CreateConstant(STACK_ADDRESS);
			var zero = CreateConstant(0);
			var offset = CreateConstant(4);

			var basicBlocks = new BasicBlocks();
			var block = basicBlocks.CreateBlock(BasicBlock.PrologueLabel);
			basicBlocks.AddHeadBlock(block);
			var ctx = new Context(block);

			// Setup the stack and place the sentinel on the stack to indicate the start of the stack
			ctx.AppendInstruction(X86.Mov32, esp, stackTop);
			ctx.AppendInstruction(X86.Mov32, ebp, stackTop);
			ctx.AppendInstruction(X86.MovStore32, null, esp, zero, zero);
			ctx.AppendInstruction(X86.MovStore32, null, esp, offset, zero);

			// Place the multiboot address into a static field
			ctx.AppendInstruction(X86.MovStore32, null, multibootEAX, zero, eax);
			ctx.AppendInstruction(X86.MovStore32, null, multibootEBX, zero, ebx);

			ctx.AppendInstruction(X86.Call, null, entryPoint);
			ctx.AppendInstruction(X86.Ret);

			Compiler.CompileMethod(multibootMethod, basicBlocks);
		}
	}
}
