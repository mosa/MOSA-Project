// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	public class StaticAllocationResolutionStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			if (MethodCompiler.Method.Name != @".cctor")
				return;

			AttemptToStaticallyAllocateObjects();
		}

		private void AttemptToStaticallyAllocateObjects()
		{
			var newOperators = ScanForNewOperators();

			foreach (var allocation in newOperators)
			{
				var assignment = GetAssignmentOfAllocatedObject(allocation);

				if (assignment != null && CheckAssignmentForCompliance(allocation, assignment))
				{
					PerformStaticAllocationOf(allocation, assignment);
				}
			}
		}

		private List<InstructionNode> ScanForNewOperators()
		{
			List<InstructionNode> list = new List<InstructionNode>();

			foreach (var block in BasicBlocks)
			{
				for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction is NewobjInstruction || node.Instruction is NewarrInstruction)
					{
						list.Add(node);
					}
				}
			}

			return list;
		}

		private void PerformStaticAllocationOf(InstructionNode allocation, InstructionNode assignment)
		{
			var allocatedType = (allocation.InvokeMethod != null) ? allocation.InvokeMethod.DeclaringType : allocation.Result.Type;
			var assignmentField = assignment.MosaField;

			// Get size of type
			int typeSize = TypeLayout.GetTypeSize(allocatedType);

			// If instruction is newarr then get the size of the element, multiply it by array size, and add array header size
			// Also need to align to a 4-byte boundary
			if (allocation.Instruction is NewarrInstruction)
			{
				int elements = GetConstant(allocation.Operand1);
				typeSize = (TypeLayout.GetTypeSize(allocatedType.ElementType) * elements) + (TypeLayout.NativePointerSize * 3);
			}

			// Allocate a linker symbol to refer to this allocation. Use the destination field name as the linker symbol name.
			var symbolName = MethodCompiler.Linker.CreateSymbol(assignmentField.FullName + @"<<$cctor", SectionKind.ROData, Architecture.NativeAlignment, typeSize);

			// Try to get typeDefinitionSymbol if allocatedType isn't a value type
			string typeDefinitionSymbol = GetTypeDefinition(allocatedType);

			if (typeDefinitionSymbol != null)
			{
				MethodCompiler.Linker.Link(LinkType.AbsoluteAddress, PatchType.I4, symbolName, 0, 0, typeDefinitionSymbol, SectionKind.ROData, 0);
			}

			Operand staticAddress = Operand.CreateManagedSymbol(assignmentField.FieldType, symbolName.Name);
			Operand result1 = MethodCompiler.CreateVirtualRegister(assignmentField.FieldType);

			//Operand result2 = MethodCompiler.CreateVirtualRegister(assignmentField.FieldType);

			// Issue a load request before the newobj and before the assignment.
			new Context(allocation).InsertBefore().SetInstruction(CILInstruction.Get(OpCode.Ldc_i4), result1, staticAddress);
			assignment.Operand1 = result1;

			// If the instruction is a newarr
			if (allocation.Instruction is NewarrInstruction)
			{
				allocation.SetInstruction(CILInstruction.Get(OpCode.Ldc_i4), allocation.Result, result1);
				return;
			}

			//new Context(allocation).InsertBefore().SetInstruction(CILInstruction.Get(OpCode.Ldc_i4), result2, staticAddress);

			// Change the newobj to a call and increase the operand count to include the this ptr.
			// If the instruction is a newarr, then just replace with a nop instead
			allocation.Result = null;
			allocation.ResultCount = 0;
			allocation.OperandCount++;

			for (int i = allocation.OperandCount; i > 0; i--)
			{
				var op = allocation.GetOperand(i - 1);
				allocation.SetOperand(i, op);
			}

			allocation.Operand1 = result1;
			allocation.Instruction = CILInstruction.Get(OpCode.Call);
		}

		private string GetTypeDefinition(MosaType allocatedType)
		{
			if (!allocatedType.IsValueType)
			{
				return allocatedType.FullName + Metadata.TypeDefinition;
			}
			return null;
		}

		private static InstructionNode GetAssignmentOfAllocatedObject(InstructionNode allocation)
		{
			foreach (var node in allocation.Result.Uses)
			{
				if (node.Instruction is StsfldInstruction)
				{
					return node;
				}
			}

			return null;
		}

		private static int GetConstant(Operand operand)
		{
			if (operand.Definitions.Count == 1)
			{
				var op = operand.Definitions[0];

				if (op.Instruction is LdcInstruction)
				{
					if (op.Operand1.IsConstant)
					{
						return op.Operand1.ConstantSignedInteger;
					}
				}
			}

			throw new InvalidCompilerException("unable to find constant value");
		}

		private static bool CheckAssignmentForCompliance(InstructionNode allocation, InstructionNode assignment)
		{
			// Only direct assignment without any casts is compliant. We can't perform casts or anything alike here,
			// as that is hard to complete at this point of time.

			var allocationType = (allocation.InvokeMethod != null) ? allocation.InvokeMethod.DeclaringType : allocation.Result.Type.ElementType;
			var storageType = (allocation.Instruction is CIL.NewarrInstruction) ? assignment.Operand1.Type.ElementType : assignment.MosaField.DeclaringType;

			return ReferenceEquals(allocationType, storageType);
		}
	}
}
