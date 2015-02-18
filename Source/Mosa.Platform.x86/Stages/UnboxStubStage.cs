/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// This stage replace the entries in value type method table with a unbox stub.
	/// </summary>
	public class UnboxStubStage : BaseCompilerStage
	{
		/// <summary>
		/// Executes this stage.
		/// </summary>
		protected override void Run()
		{
			foreach (var type in TypeSystem.AllTypes)
			{
				if (type.IsModule || type.HasOpenGenericParams || type.IsInterface)
					continue;

				if (!type.IsValueType)
					continue;

				CreateUnboxStub(type);
			}
		}

		private void CreateUnboxStub(MosaType type)
		{
			var methodTable = Compiler.TypeLayout.GetMethodTable(type);

			for (int i = 0; i < methodTable.Count; i++)
			{
				var method = methodTable[i];
				// If the method is not defined in the type (i.e. in Object),
				// Leave it in boxed form.
				if (method.DeclaringType != type)
					continue;

				var unboxStub = Compiler.CreateLinkerMethod(type.FullName + "$unbox$" + method.ID);

				var eax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
				var esp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ESP);

				var basicBlocks = new BasicBlocks();
				var block = basicBlocks.CreateBlock();
				basicBlocks.AddHeaderBlock(block);
				var ctx = new Context(block);

				// Stack frame hasn't been pushed yet, so subtract a pointer.
				var thisOffset = Architecture.CallingConvention.OffsetOfFirstParameter - Architecture.NativePointerSize;
				// Actual value in boxed form is after type definition and a sync block, so 2 pointers.
				var valueOffset = Architecture.NativePointerSize * 2;

				var thisArg = Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, esp, thisOffset);
				ctx.AppendInstruction(X86.Mov, eax, thisArg);
				ctx.AppendInstruction(X86.Add, eax, eax, Operand.CreateConstant(TypeSystem.BuiltIn.I4, valueOffset));
				ctx.AppendInstruction(X86.Mov, thisArg, eax);
				ctx.AppendInstruction(X86.Jmp, null, Operand.CreateSymbolFromMethod(TypeSystem, method));

				Compiler.CompileMethod(unboxStub, basicBlocks, 0);
				methodTable[i] = unboxStub;
			}
		}
	}
}
