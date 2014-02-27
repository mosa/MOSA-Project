/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai P. Reisert <kpreisert@googlemail.com>
 */

using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	public class StaticAllocationResolutionStage : BaseMethodCompilerStage, IMethodCompilerStage
	{
		void IMethodCompilerStage.Run()
		{
			if (methodCompiler.Method.Name == @".cctor")
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
			MosaType allocatedType = allocation.MosaMethod.DeclaringType;

			// Allocate a linker symbol to refer to this allocation. Use the destination field name as the linker symbol name.
			string symbolName = assignment.MosaField.ToString() + @"<<$cctor";

			using (var stream = methodCompiler.Linker.Allocate(symbolName, SectionKind.BSS, typeLayout.GetTypeSize(allocatedType), 4))
			{
				// FIXME: Do we have to initialize this?
				string methodTableSymbol = GetMethodTableForType(allocatedType);

				if (methodTableSymbol != null)
					methodCompiler.Linker.Link(LinkType.AbsoluteAddress | LinkType.I4, BuiltInPatch.I4, symbolName, 0, 0, methodTableSymbol, 0);
			}

			// Issue a load request before the newobj and before the assignment.
			Operand symbol1 = InsertLoadBeforeInstruction(allocation, symbolName, assignment.MosaField.FieldType);
			allocation.Operand1 = symbol1;

			Operand symbol2 = InsertLoadBeforeInstruction(assignment, symbolName, assignment.MosaField.FieldType);
			assignment.Operand1 = symbol2;

			// Change the newobj to a call and increase the operand count to include the this ptr.
			allocation.OperandCount++;
			allocation.ResultCount = 0;
			allocation.ReplaceInstructionOnly(CILInstruction.Get(OpCode.Call));
		}

		private string GetMethodTableForType(MosaType allocatedType)
		{
			if (!allocatedType.IsValueType)
			{
				return allocatedType.FullName + @"$mtable";
			}

			return null;
		}

		private Operand InsertLoadBeforeInstruction(Context context, string symbolName, MosaType type)
		{
			Context before = context.InsertBefore();
			Operand result = methodCompiler.CreateVirtualRegister(type);
			Operand op = Operand.CreateManagedSymbol(typeSystem, type, symbolName);

			before.SetInstruction(CILInstruction.Get(OpCode.Ldc_i4), result, op);

			return result;
		}

		private IEnumerable<Context> ScanForOperatorNew()
		{
			foreach (BasicBlock block in basicBlocks)
			{
				for (Context context = new Context(instructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (!context.IsEmpty && (context.Instruction is NewobjInstruction || context.Instruction is NewarrInstruction))
					{
						//Debug.WriteLine(@"StaticAllocationResolutionStage: Found a newobj or newarr instruction.");
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

			MosaType allocationType = allocation.MosaMethod.DeclaringType;
			MosaType storageType = assignment.MosaField.DeclaringType;

			return ReferenceEquals(allocationType, storageType);
		}

		private void ExpandAllocationToStaticInitialization()
		{
		}
	}
}