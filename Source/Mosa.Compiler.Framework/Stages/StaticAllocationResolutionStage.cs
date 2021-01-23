// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.Linker;

namespace Mosa.Compiler.Framework.Stages
{
	public class StaticAllocationResolutionStage : BaseMethodCompilerStage
	{
		public const string StaticSymbolPrefix = "$cctor$";

		protected override void Run()
		{
			if (!MethodCompiler.IsCILStream)
				return;

			if (!Method.IsTypeConstructor)
				return;

			foreach (var block in BasicBlocks)
			{
				for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmptyOrNop)
						continue;

					if (node.Instruction == IRInstruction.NewObject || node.Instruction == IRInstruction.NewArray)
					{
						PerformStaticAllocation(new Context(node));
					}
				}
			}
		}

		private void PerformStaticAllocation(Context context)
		{
			var allocatedType = context.MosaType; // node.Result.Type;
			var handle = context.Operand1;

			bool newObject = context.Instruction == IRInstruction.NewObject;
			int elements = 0;

			//Debug.WriteLine($"Method: {Method} : {node}");
			//Debug.WriteLine($"  --> {allocatedType}");

			MethodScanner.TypeAllocated(allocatedType, Method);

			int allocationSize;

			if (newObject)
			{
				allocationSize = TypeLayout.GetTypeSize(allocatedType);
			}
			else
			{
				elements = (int)GetConstant(context.Operand3);
				allocationSize = (TypeLayout.GetTypeSize(allocatedType.ElementType) * elements) + (TypeLayout.NativePointerSize * 3);
			}

			var symbolName = Linker.DefineSymbol(StaticSymbolPrefix + allocatedType.FullName, SectionKind.BSS, Architecture.NativeAlignment, allocationSize);

			string typeDefinitionSymbol = Metadata.TypeDefinition + allocatedType.FullName;

			Linker.Link(LinkType.AbsoluteAddress, Is32BitPlatform ? PatchType.I32 : PatchType.I64, symbolName, 0, typeDefinitionSymbol, 0);

			var staticAddress = Operand.CreateSymbol(allocatedType, symbolName.Name);

			var move = Is32BitPlatform ? (BaseInstruction)IRInstruction.Move32 : IRInstruction.Move64;
			var store = Is32BitPlatform ? (BaseInstruction)IRInstruction.Store32 : IRInstruction.Store64;

			context.SetInstruction(move, context.Result, staticAddress);
			context.AppendInstruction(store, null, staticAddress, ConstantZero, handle);

			if (!newObject)
			{
				context.AppendInstruction(store, null, staticAddress, CreateConstant32(2 * (Is32BitPlatform ? 4 : 8)), CreateConstant32(elements));
			}
		}

		private static long GetConstant(Operand operand)
		{
			while (true)
			{
				if (operand.IsConstant)
					return operand.ConstantSigned64;

				if (operand.Definitions.Count != 1)
					break;

				var node = operand.Definitions[0];

				if ((node.Instruction == IRInstruction.Move32 || node.Instruction == IRInstruction.Move64) && node.Operand1.IsConstant)
				{
					operand = node.Operand1;
					continue;
				}

				break;
			}

			throw new CompilerException("unable to find constant value");
		}
	}
}
