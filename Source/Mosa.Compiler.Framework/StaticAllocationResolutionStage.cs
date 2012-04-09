/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai P. Reisert <kpreisert@googlemail.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{
	public class StaticAllocationResolutionStage : BaseMethodCompilerStage, IMethodCompilerStage
	{

		void IMethodCompilerStage.Run()
		{

			if (this.methodCompiler.Method.Name == @".cctor")
			{
				this.AttemptToStaticallyAllocateObjects();
			}
		}

		private void AttemptToStaticallyAllocateObjects()
		{
			foreach (Context allocation in this.ScanForOperatorNew())
			{
				Context assignment = this.SeekAssignmentOfAllocatedObject(allocation);

				if (assignment != null && this.CheckAssignmentForCompliance(allocation, assignment))
				{
					Debug.WriteLine(@"StaticAllocationResolutionStage: Static allocation of object possible.");
					this.PerformStaticAllocationOf(allocation, assignment);
				}
				else
				{
					Debug.WriteLine(@"StaticAllocationResolutionStage: Can't statically allocate object.");
				}
			}
		}

		private void PerformStaticAllocationOf(Context allocation, Context assignment)
		{
			RuntimeType allocatedType = allocation.InvokeTarget.DeclaringType;

			// Allocate a linker symbol to refer to for this allocation. Use the destination field name as the linker symbol name.
			string symbolName = assignment.RuntimeField.ToString() + @"<<$cctor";
			using (var stream = methodCompiler.Linker.Allocate(symbolName, SectionKind.BSS, typeLayout.GetTypeSize(allocatedType), 4))
			{
				// FIXME: Do we have to initialize this?
				string methodTableSymbol = GetMethodTableForType(allocatedType);
				
				if (methodTableSymbol != null)
					methodCompiler.Linker.Link(LinkType.AbsoluteAddress | LinkType.NativeI4, symbolName, 0, 0, methodTableSymbol, IntPtr.Zero);
			}

			// Issue a load request before the newobj and before the assignment.
			Operand symbol1 = this.InsertLoadBeforeInstruction(allocation, symbolName, assignment.RuntimeField.SignatureType);
			allocation.Operand1 = symbol1;

			Operand symbol2 = this.InsertLoadBeforeInstruction(assignment, symbolName, assignment.RuntimeField.SignatureType);
			assignment.Operand1 = symbol2;

			// Change the newobj to a call and increase the operand count to include the this ptr.
			allocation.OperandCount++;
			allocation.ResultCount = 0;
			allocation.ReplaceInstructionOnly(Instruction.Get(OpCode.Call));
		}

		private string GetMethodTableForType(RuntimeType allocatedType)
		{
			if (!allocatedType.IsValueType)
			{
				return allocatedType.FullName + @"$mtable";
			}

			return null;
		}

		private Operand InsertLoadBeforeInstruction(Context context, string symbolName, SigType type)
		{
			Context before = context.InsertBefore();
			Operand result = methodCompiler.CreateTemporary(type);
			Operand op = new SymbolOperand(type, symbolName);

			before.SetInstruction(Instruction.Get(OpCode.Ldc_i4), result, op);

			return result;
		}

		private IEnumerable<Context> ScanForOperatorNew()
		{
			foreach (BasicBlock block in this.basicBlocks)
			{
				Context context = new Context(instructionSet, block);
				while (!context.EndOfInstruction)
				{
					if (context.Instruction is NewobjInstruction || context.Instruction is NewarrInstruction)
					{
						Debug.WriteLine(@"StaticAllocationResolutionStage: Found a newobj or newarr instruction.");
						yield return context.Clone();
					}

					context.GotoNext();
				}
			}
		}

		private Context SeekAssignmentOfAllocatedObject(Context allocation)
		{
			Context next = allocation.Next;
			if (next.EndOfInstruction || !(next.Instruction is StsfldInstruction))
			{
				next = null;
			}

			return next;
		}

		private bool CheckAssignmentForCompliance(Context allocation, Context assignment)
		{
			// Only direct assignment without any casts is compliant. We can't perform casts or anything alike here,
			// as that is hard to complete at this point of time.

			RuntimeType allocationType = allocation.InvokeTarget.DeclaringType;
			RuntimeType storageType = assignment.RuntimeField.DeclaringType;

			return ReferenceEquals(allocationType, storageType);
		}

		private void ExpandAllocationToStaticInitialization()
		{
		}
	}
}
