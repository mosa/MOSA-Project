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
			if (!MethodCompiler.IsCILDecodeRequired)
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
						PerformStaticAllocation(node);
					}
				}
			}
		}

		private void PerformStaticAllocation(InstructionNode node)
		{
			var allocatedType = node.MosaType; // node.Result.Type;

			//Debug.WriteLine($"Method: {Method} : {node}");
			//Debug.WriteLine($"  --> {allocatedType}");

			MethodScanner.TypeAllocated(allocatedType, Method);

			int allocationSize;

			if (node.Instruction == IRInstruction.NewObject)
			{
				allocationSize = TypeLayout.GetTypeSize(allocatedType);
			}
			else
			{
				var elements = (int)GetConstant(node.Operand3);
				allocationSize = (TypeLayout.GetTypeSize(allocatedType.ElementType) * elements) + (TypeLayout.NativePointerSize * 3);
			}

			var symbolName = Linker.DefineSymbol(StaticSymbolPrefix + allocatedType.FullName, SectionKind.BSS, Architecture.NativeAlignment, allocationSize);

			string typeDefinitionSymbol = Metadata.TypeDefinition + allocatedType.FullName;

			Linker.Link(LinkType.AbsoluteAddress, Is32BitPlatform ? PatchType.I32 : PatchType.I64, symbolName, 0, typeDefinitionSymbol, 0);

			var staticAddress = Operand.CreateSymbol(allocatedType, symbolName.Name);

			var move = Is32BitPlatform ? (BaseInstruction)IRInstruction.Move32 : IRInstruction.Move64;

			node.SetInstruction(move, node.Result, staticAddress);
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
