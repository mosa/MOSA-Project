// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x64.CompilerStages
{
	public sealed class MultibootV1Stage : Intel.CompilerStages.MultibootV1Stage
	{
		protected override void CreateMultibootMethod()
		{
			var startUpType = TypeSystem.GetTypeByName("Mosa.Runtime", "StartUp");
			var initializeMethod = startUpType.FindMethodByName("Initialize");
			var entryPoint = Operand.CreateSymbolFromMethod(initializeMethod, TypeSystem);

			var eax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, GeneralPurposeRegister.EAX);
			var ebx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, GeneralPurposeRegister.EBX);
			var ebp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, GeneralPurposeRegister.EBP);
			var esp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, GeneralPurposeRegister.ESP);

			var multibootEAX = Operand.CreateUnmanagedSymbolPointer(MultibootEAX, TypeSystem);
			var multibootEBX = Operand.CreateUnmanagedSymbolPointer(MultibootEBX, TypeSystem);

			var stackTop = CreateConstant(STACK_ADDRESS);
			var zero = CreateConstant(0);
			var offset = CreateConstant(8);

			var basicBlocks = new BasicBlocks();
			var block = basicBlocks.CreateBlock(BasicBlock.PrologueLabel);
			basicBlocks.AddHeadBlock(block);
			var ctx = new Context(block);

			// Setup the stack and place the sentinel on the stack to indicate the start of the stack
			ctx.AppendInstruction(X64.Mov64, esp, stackTop);
			ctx.AppendInstruction(X64.Mov64, ebp, stackTop);
			ctx.AppendInstruction(X64.MovStore64, null, esp, zero, zero);
			ctx.AppendInstruction(X64.MovStore64, null, esp, offset, zero);

			// Place the multiboot address into a static field
			ctx.AppendInstruction(X64.MovStore64, null, multibootEAX, zero, eax);
			ctx.AppendInstruction(X64.MovStore64, null, multibootEBX, zero, ebx);

			ctx.AppendInstruction(X64.Call, null, entryPoint);
			ctx.AppendInstruction(X64.Ret);

			Compiler.CompileMethod(multibootMethod, basicBlocks);
		}
	}
}
