// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.CompilerStages;

public sealed class MultibootStage : Framework.Platform.BaseMultibootStage
{
	protected override void CreateMultibootMethod()
	{
		var basicBlocks = new BasicBlocks();

		var methodCompiler = new MethodCompiler(Compiler, MultibootMethod, basicBlocks, 0);
		methodCompiler.MethodData.DoNotInline = true;

		var transform = new Transform();
		transform.SetCompiler(Compiler);
		transform.SetMethodCompiler(methodCompiler);

		var initializeMethod = TypeSystem.GetMethod("Mosa.Runtime.StartUp", "Initialize");
		var entryPoint = Operand.CreateLabel(initializeMethod, Architecture.Is32BitPlatform);

		var ebp = transform.PhysicalRegisters.Allocate32(CPURegister.EBP);
		var esp = transform.PhysicalRegisters.Allocate32(CPURegister.ESP);

		var stackLocation = Operand.CreateConstant32(MosaSettings.InitialStackLocation);

		var prologueBlock = basicBlocks.CreatePrologueBlock();

		var context = new Context(prologueBlock);

		// Set stack location
		context.AppendInstruction(X86.Mov32, esp, stackLocation);

		// Create stack sentinel
		context.AppendInstruction(X86.Push32, null, Operand.Constant32_0);
		context.AppendInstruction(X86.Push32, null, Operand.Constant32_0);

		// Push registers onto the new stack
		context.AppendInstruction(X86.Mov32, ebp, esp);
		context.AppendInstruction(X86.Pushad);
		context.AppendInstruction(X86.Push32, null, ebp);

		// Call entry point
		context.AppendInstruction(X86.Call, null, entryPoint);
		context.AppendInstruction(X86.Ret);

		Compiler.CompileMethod(transform);
	}
}
