using System;
using System.Collections.Generic;
using System.Text;
using IR = Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Vm;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// A minimalistic register assignment allocator.
	/// </summary>
	/// <remarks>
	/// This is not a real register allocator. It ensures that operations with register
	/// constraints are executed properly, but does not assign variables to fixed registers.
	/// </remarks>
	public class LinearRegisterAllocator : BaseStage, IMethodCompilerStage, IPipelineStage
	{
		#region Types

		/// <summary>
		/// 
		/// </summary>
		private class LiveRange
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="LiveRange"/> class.
			/// </summary>
			/// <param name="block">The block.</param>
			/// <param name="op">The op.</param>
			/// <param name="start">The start.</param>
			/// <param name="end">The end.</param>
			public LiveRange(BasicBlock block, Operand op, int start, int end)
			{
				this.Block = block;
				this.Op = op;
				this.Reg = null;
				this.Start = start;
				this.End = end;
			}

			/// <summary>
			/// 
			/// </summary>
			public BasicBlock Block;
			/// <summary>
			/// 
			/// </summary>
			public Operand Op;
			/// <summary>
			/// 
			/// </summary>
			public Register Reg;
			/// <summary>
			/// 
			/// </summary>
			public int Start;
			/// <summary>
			/// 
			/// </summary>
			public int End;
		}

		#endregion // Types

		#region Data members

		/// <summary>
		/// Holds the architecture to allocate statements with.
		/// </summary>
		private IArchitecture _architecture;

		/// <summary>
		/// List of live ranges found in the method.
		/// </summary>
		private List<LiveRange> _liveRanges = new List<LiveRange>();

		/// <summary>
		/// Holds the currently live registers and their backing stores.
		/// </summary>
		private List<Register> _registers;

		#endregion // Data members

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value></value>
		string IPipelineStage.Name
		{
			get { return @"LinearRegisterAllocator"; }
		}

		/// <summary>
		/// Gets the pipeline stage order.
		/// </summary>
		/// <value>The pipeline stage order.</value>
		PipelineStageOrder[] IPipelineStage.PipelineStageOrder
		{
			get
			{
				return new PipelineStageOrder[] {
					//new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(IR.CILTransformationStage)),
					new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(ICodeGenerationStage))
				};
			}
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			// Retrieve the architecture
			_architecture = compiler.Architecture;

			// 1st Pass: Number all instructions in the order of appearance
			NumberInstructions();

			// 2nd Pass: Capture all operands and their live ranges
			CaptureLiveRanges();

			// 3rd Pass: Assign registers
			AssignRegisters();
		}

		/// <summary>
		/// Assigns the registers.
		/// </summary>
		private void AssignRegisters()
		{
			List<LiveRange> active = new List<LiveRange>();
			_registers = FillRegisterList();

			for (int i = 0; i < _liveRanges.Count; i++) {
				LiveRange lr = _liveRanges[i];
				ExpireOldRanges(lr.Start, active);

				Register reg = AllocateRegister(lr.Op);
				if (reg == null)
					reg = SpillRegister(active, lr);

				Debug.Assert(reg != null, @"Failed to allocate a register type.");
				RegisterOperand rop = new RegisterOperand(lr.Op.Type, reg);
				ReplaceOperand(lr, rop); ;
				lr.Reg = reg;

				int insIdx = active.FindIndex(delegate(LiveRange match)
				{
					return ((lr.End - match.End) > 0);
				});

				active.Insert(insIdx + 1, lr);
			}
		}

		/// <summary>
		/// Replaces the operand.
		/// </summary>
		/// <param name="lr">The lr.</param>
		/// <param name="replacement">The replacement.</param>
		private void ReplaceOperand(LiveRange lr, RegisterOperand replacement)
		{
			int opIdx;

			// Iterate all definition sites first
			foreach (int index in lr.Op.Definitions.ToArray()) {
				Context def = new Context(InstructionSet, index);
				if (def.Offset == lr.Start) {
					opIdx = 0;
					foreach (Operand r in def.Results) {
						// Is this the operand?
						if (object.ReferenceEquals(r, lr.Op))
							def.SetResult(opIdx, replacement);

						opIdx++;
					}

					break;
				}
			}

			// Iterate all use sites
			foreach (int index in lr.Op.Uses.ToArray()) {
				Context instr = new Context(InstructionSet, index);

				if (instr.Offset <= lr.Start) {
					// A use on instr.Offset == lr.Start is one From a previous definition!!
				}
				else if (instr.Offset <= lr.End) {
					opIdx = 0;
					foreach (Operand r in instr.Operands) {
						// Is this the operand?
						if (object.ReferenceEquals(r, lr.Op))
							instr.SetOperand(opIdx, replacement);

						opIdx++;
					}
				}
				else {
					break;
				}
			}
		}

		/// <summary>
		/// Spills the register.
		/// </summary>
		/// <param name="active">The active.</param>
		/// <param name="current">The current.</param>
		/// <returns></returns>
		private Register SpillRegister(List<LiveRange> active, LiveRange current)
		{
			foreach (LiveRange lr in active) {
				// Does it make sense to spill this register?
				if (lr.Reg.IsValidSigType(current.Op.Type)) {
					// Yes, spill it back to its operand
					RegisterOperand rop = new RegisterOperand(lr.Op.Type, lr.Reg);

					//					MoveInstruction mi = CreateMoveInstruction(lr.Op, rop);		// FIXME PG - hack to allow compile
					//					current.Block.Instructions.Insert(current.Start++, mi);		// FIXME PG - hack to allow compile

					// Load the new value
					//					mi = CreateMoveInstruction(rop, current.Op);				// FIXME PG - hack to allow compile
					//					current.Block.Instructions.Insert(current.Start++, mi);		// FIXME PG - hack to allow compile

					// Remove this live range from the active list
					active.Remove(lr);

					lr.Start = PickNextUse(lr.Op, current.Start, lr.End);
					if (lr.Start != -1)
						ReinsertSpilledRange(lr);

					return lr.Reg;
				}
			}

			throw new InvalidOperationException(@"Failed to spill a register.");
		}

		/// <summary>
		/// Allocates the register.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		private Register AllocateRegister(Operand operand)
		{
			foreach (Register r in _registers) {
				if (r.IsValidSigType(operand.Type)) {
					_registers.Remove(r);
					return r;
				}
			}

			return null;
		}

		/// <summary>
		/// Expires the old ranges.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="active">The active.</param>
		private void ExpireOldRanges(int position, List<LiveRange> active)
		{
			for (int i = 0; i < active.Count; i++) {
				LiveRange lr = active[i];
				if (lr.End > position)
					break;

				_registers.Add(lr.Reg);
				active.RemoveAt(i);
				i--;
			}
		}

		/// <summary>
		/// Reinserts the spilled range.
		/// </summary>
		/// <param name="lr">The lr.</param>
		private void ReinsertSpilledRange(LiveRange lr)
		{
			int index = 0;
			foreach (LiveRange item in _liveRanges) {
				if (item.Start > lr.Start) {
					_liveRanges.Insert(index, lr);
					break;
				}

				index++;
			}
		}

		/// <summary>
		/// Fills the register list.
		/// </summary>
		/// <returns></returns>
		private List<Register> FillRegisterList()
		{
			return new List<Register>(_architecture.RegisterSet);
		}

		/// <summary>
		/// Captures the live ranges.
		/// </summary>
		private void CaptureLiveRanges()
		{
			/*			
		   // Start live ranges for the parameters of the method
		   int paramIdx = 0;
		   foreach (RuntimeParameter rp in compiler.Method.Parameters)
		   {
			   Operand paramOp = compiler.GetParameterOperand(paramIdx++);
			   if (0 != paramOp.Uses.Count)
			   {
				   Sort(paramOp.Definitions);
				   Sort(paramOp.Uses);

				   int lastUse = PickLastUseForDef(paramOp, 0);
				   if (-1 != lastUse)
				   {
					   LiveRange lr = new LiveRange(blockProvider.Blocks[0], paramOp, 0, lastUse);
					   _liveRanges.Add(lr);
				   }
			   }
		   }

		   // Now process all additional definitions
		   foreach (BasicBlock block in blockProvider)
		   {
			   foreach (Instruction instruction in block.Instructions)
			   {
				   foreach (Operand op in instruction.Results)
				   {
					   if (0 != op.Uses.Count)
					   {
						   Sort(op.Uses);
						   Sort(op.Definitions);

						   int lastUse = PickLastUseForDef(op, instruction.Offset);
						   if (instruction.Offset < lastUse)
						   {
							   LiveRange lr = new LiveRange(block, op, instruction.Offset, lastUse);
							   _liveRanges.Add(lr);
						   }
					   }
				   }
			   }
		   }
		   */
		}

		/// <summary>
		/// Picks the last use for def.
		/// </summary>
		/// <param name="op">The op.</param>
		/// <param name="defLine">The def line.</param>
		/// <returns></returns>
		private int PickLastUseForDef(Operand op, int defLine)
		{
			int result = -1;

			// Find the next definition after defLine
			int ubound = Int32.MaxValue;

			foreach (int index in op.Definitions) {
				Context ctx = new Context(InstructionSet, index);
				if (ctx.Offset > defLine) {
					ubound = ctx.Offset;
					break;
				}
			}

			foreach (int index in op.Uses) {
				Context ctx = new Context(InstructionSet, index);
				if (ctx.Offset > defLine && ctx.Offset < ubound) {
					result = ctx.Offset;
				}
			}

			return result;
		}

		/// <summary>
		/// Picks the next use.
		/// </summary>
		/// <param name="op">The op.</param>
		/// <param name="line">The line.</param>
		/// <param name="end">The end.</param>
		/// <returns></returns>
		private int PickNextUse(Operand op, int line, int end)
		{
			int result = -1;

			// Now find the last use between defLine and ubound
			foreach (int index in op.Uses) {
				Context ctx = new Context(InstructionSet, index);
				if (ctx.Offset > line && ctx.Offset < end)
					result = ctx.Offset;
			}

			return result;
		}

		/// <summary>
		/// Sorts the specified list.
		/// </summary>
		/// <param name="list">The list.</param>
		private void Sort(List<Context> list)
		{
			list.Sort(delegate(Context a, Context b)
			{
				return a.Offset - b.Offset;
			});
		}

		#endregion // IMethodCompilerStage Members

		#region Internals

		/// <summary>
		/// Assigns every instruction an increasing offset value.
		/// </summary>
		private void NumberInstructions()
		{
			int offset = 0;

			foreach (BasicBlock block in BasicBlocks)
				for (Context ctx = new Context(InstructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
					ctx.Offset = offset++;
		}

		/// <summary>
		/// Creates a move instruction that moves the value of the operand to the specified register.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="dest">The destination register.</param>
		/// <param name="src">The operand to move.</param>
		private void CreateMoveInstruction(Context ctx, Operand dest, Operand src)
		{
			ctx.AppendInstruction(IR.Instruction.MoveInstruction, dest, src);
		}

		#endregion // Internals
	}
}
