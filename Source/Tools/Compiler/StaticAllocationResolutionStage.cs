
using System;
using System.Collections.Generic;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.CIL;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Tools.Compiler
{
	public class StaticAllocationResolutionStage : BaseStage, IMethodCompilerStage
	{
        private IAssemblyLinker linker;

		public StaticAllocationResolutionStage()
		{
		}
		
		public string Name 
		{
			get 
			{
				return @"StaticAllocationResolutionStage";
			}
		}
		
		public void Run()
		{
            this.linker = this.MethodCompiler.Linker;

			if (this.MethodCompiler.Method.Name == @".cctor")
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
            string symbolName = this.linker.CreateSymbolName(assignment.RuntimeField) + @"<<$cctor";
            using (var stream = this.linker.Allocate(symbolName, SectionKind.BSS, allocatedType.Size, 4))
            {
                // FIXME: Do we have to initialize this?
                string methodTableSymbol = this.GetMethodTableForType(allocatedType);
                if (methodTableSymbol != null)
                {
                    this.linker.Link(LinkType.AbsoluteAddress | LinkType.I4, symbolName, 0, 0, methodTableSymbol, IntPtr.Zero);
                }
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
            string result = null;
            if (allocatedType.IsValueType == false)
            {
                result = allocatedType.FullName + @"$mtable";
            }

            return result;
        }

        private Operand InsertLoadBeforeInstruction(Context context, string symbolName, SigType type)
        {
            Context before = context.InsertBefore();
            Operand result = this.MethodCompiler.CreateTemporary(type);
            Operand op = new SymbolOperand(type, symbolName);

            before.SetInstruction(Instruction.Get(OpCode.Ldc_i4), result, op);

            return result;
        }
		
		private IEnumerable<Context> ScanForOperatorNew()
		{
			foreach (BasicBlock block in this.BasicBlocks)
			{
				Context context = new Context(InstructionSet, block);
				while (context.EndOfInstruction == false)
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
            if (next.EndOfInstruction == true || !(next.Instruction is StsfldInstruction))
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
            RuntimeType storageType = assignment.RuntimeField.Type;
            
			return ReferenceEquals(allocationType, storageType);
		}
		
		private void ExpandAllocationToStaticInitialization()
		{
		}
	}
}
