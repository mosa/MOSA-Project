/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai P. Reisert <kpreisert@googlemail.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

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
			if (MethodCompiler.Method.Name == @".cctor")
			{
				AttemptToStaticallyAllocateObjects();
			}
		}

		private void AttemptToStaticallyAllocateObjects()
		{
			foreach (Context allocation in ScanForOperatorNew())
			{
				Context assignment = SeekAssignmentOfAllocatedObject(allocation);

				if (assignment != null && CheckAssignmentForCompliance(allocation, assignment))
				{
					PerformStaticAllocationOf(allocation, assignment);
				}
			}
		}

		private void PerformStaticAllocationOf(Context allocation, Context assignment)
		{
			MosaType allocatedType = (allocation.MosaMethod != null) ? allocation.MosaMethod.DeclaringType : allocation.Result.Type;

			// Get size of type
			int typeSize = TypeLayout.GetTypeSize(allocatedType);

			// If instruction is newarr then get the size of the element, multiply it by array size, and add array header size
			// Also need to align to a 4-byte boundry
			if (allocation.Instruction is NewarrInstruction)
				typeSize = (TypeLayout.GetTypeSize(allocatedType.ElementType) * (int)allocation.Previous.Operand1.ConstantSignedInteger) + (TypeLayout.NativePointerSize * 3);

			// Allocate a linker symbol to refer to this allocation. Use the destination field name as the linker symbol name.
			var symbolName = MethodCompiler.Linker.CreateSymbol(assignment.MosaField.FullName + @"<<$cctor", SectionKind.ROData, Architecture.NativeAlignment, typeSize);

			// Try to get typeDefinitionSymbol if allocatedType isn't a value type
			string typeDefinitionSymbol = GetTypeDefinition(allocatedType);

			if (typeDefinitionSymbol != null)
				MethodCompiler.Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, symbolName, 0, 0, typeDefinitionSymbol, SectionKind.ROData, 0);

			// Issue a load request before the newobj and before the assignment.
			Operand symbol1 = InsertLoadBeforeInstruction(assignment, symbolName.Name, assignment.MosaField.FieldType);
			assignment.Operand1 = symbol1;

			// Change the newobj to a call and increase the operand count to include the this ptr.
			// If the instruction is a newarr, then just replace with a nop instead
			allocation.ResultCount = 0;
			if (allocation.Instruction is NewarrInstruction)
			{
				allocation.OperandCount = 0;
				allocation.ReplaceInstructionOnly(CILInstruction.Get(OpCode.Nop));
			}
			else
			{
				Operand symbol2 = InsertLoadBeforeInstruction(allocation, symbolName.Name, assignment.MosaField.FieldType);
				IEnumerable<Operand> ops = allocation.Operands;
				allocation.OperandCount++;
				allocation.Operand1 = symbol2;
				int i = 0;
				foreach (Operand op in ops)
				{
					i++;
					allocation.SetOperand(i, op);
				}
				allocation.ReplaceInstructionOnly(CILInstruction.Get(OpCode.Call));
			}
		}

		private string GetTypeDefinition(MosaType allocatedType)
		{
			if (!allocatedType.IsValueType)
				return allocatedType.FullName + Metadata.TypeDefinition;
			return null;
		}

		private Operand InsertLoadBeforeInstruction(Context context, string symbolName, MosaType type)
		{
			Context before = context.InsertBefore();
			Operand result = MethodCompiler.CreateVirtualRegister(type);
			Operand op = Operand.CreateManagedSymbol(TypeSystem, type, symbolName);

			before.SetInstruction(CILInstruction.Get(OpCode.Ldc_i4), result, op);

			return result;
		}

		private IEnumerable<Context> ScanForOperatorNew()
		{
			foreach (BasicBlock block in BasicBlocks)
			{
				for (Context context = new Context(InstructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (!context.IsEmpty && (context.Instruction is NewobjInstruction || context.Instruction is NewarrInstruction))
					{
						yield return context.Clone();
					}
				}
			}
		}

		private Context SeekAssignmentOfAllocatedObject(Context allocation)
		{
			Context next = allocation.Next;

			while (next.IsEmpty)
			{
				next.GotoNext();
			}

			if (next.IsBlockEndInstruction || !(next.Instruction is StsfldInstruction))
			{
				return null;
			}
			else
			{
				return next;
			}
		}

		private bool CheckAssignmentForCompliance(Context allocation, Context assignment)
		{
			// Only direct assignment without any casts is compliant. We can't perform casts or anything alike here,
			// as that is hard to complete at this point of time.

			MosaType allocationType = (allocation.MosaMethod != null) ? allocation.MosaMethod.DeclaringType : allocation.Result.Type.ElementType;
			MosaType storageType = assignment.MosaField.DeclaringType;

			return ReferenceEquals(allocationType, storageType);
		}

		private void ExpandAllocationToStaticInitialization()
		{
		}
	}
}