// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Platform.Intel;

namespace Mosa.Platform.x64.CompilerStages
{
	public sealed class MultibootV1Stage : Intel.CompilerStages.MultibootV1Stage
	{
		protected override void Finalization()
		{
			var multibootEntry = CreateMultibootEntry();

			CreateMultibootMethod();

			WriteMultibootHeader(multibootEntry);
		}

		private void CreateMultibootMethod()
		{
			var startUpType = TypeSystem.GetTypeByName("Mosa.Runtime", "StartUp");
			var initializeMethod = startUpType.FindMethodByName("Initialize");

			Compiler.GetMethodData(initializeMethod).DoNotInline = true;

			var entryPoint = Operand.CreateSymbolFromMethod(initializeMethod, TypeSystem);

			var eax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, GeneralPurposeRegister.EAX);
			var ebx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, GeneralPurposeRegister.EBX);
			var ebp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, GeneralPurposeRegister.EBP);
			var esp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, GeneralPurposeRegister.ESP);

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

		private LinkerSymbol CreateMultibootEntry()
		{
			var symbol = Linker.GetSymbol(MultibootEntry);

			var data = new byte[]
			{
				0x68, 0x00, 0x00,  0x00, 0x00, // ba 00 00 00 00          0: push  { entry point }
				0x68, 0x00, 0x00,  0x00, 0x00, // ba 00 00 00 00          5: push  0x0 - allows for 64-bit return later
				0xe9, 0x00, 0x00,  0x00, 0x00, // e9 00 00 00 00		  10: jmp	{ enter long mode code }
			};

			symbol.SetData(data);

			var type = TypeSystem.GetTypeByName("Mosa.Runtime.Boot.LongMode", "LongMode");
			var method = type.FindMethodByName("EnterLongMode");

			Linker.Link(LinkType.AbsoluteAddress, PatchType.I32, symbol, 1, Linker.EntryPoint, 0);
			Linker.Link(LinkType.RelativeOffset, PatchType.I32, symbol, 11, method.FullName, 0);

			return symbol;
		}
	}
}
