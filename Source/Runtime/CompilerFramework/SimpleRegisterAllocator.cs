using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata.Signatures;

// FIXME PG - Probably very buggy after modifications!

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// A simple register allocator.
	/// </summary>
	/// <remarks>
	/// The simple register allocator does not try to optimize register usage. It loads values 
	/// into registers as soon as it sees a load statement and spills them back as soon as it
	/// finds a matching store instruction or the register set is fully utilized. It performs one
	/// optimization: Values accessed only once (e.g. Parameters) are not loaded into a register,
	/// if the use-location supports a memory operand at that position.
	/// <para/>
	/// The simple register allocator requires register constraints for all instructions it finds.
	/// </remarks>
	public sealed class SimpleRegisterAllocator : BaseStage, IMethodCompilerStage, IPipelineStage
	{
		#region Tracing Switch

		/// <summary>
		/// Controls tracing of the <see cref="SimpleRegisterAllocator"/>.
		/// </summary>
		public static readonly TraceSwitch TRACING = new TraceSwitch(@"Mosa.Runtime.CompilerFramework.SimpleRegisterAllocator", @"Controls traces of the simple register allocator.", "Info");

		#endregion // Tracing Switch

		#region Data members

		/// <summary>
		/// An array of operands in registers.
		/// </summary>
		/// <remarks>
		/// The array index is the physical register index in the register set.
		/// </remarks>
		private Operand[] _activeOperands;

		private Context[] _activeOpLastUse;

		/// <summary>
		/// Holds the entire register set of the compilation target architecture.
		/// </summary>
		private Register[] _registerSet;

		#endregion // Data members

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"SimpleRegisterAllocator"; } }

		#endregion // IPipelineStage Members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
			_registerSet = Architecture.RegisterSet;
			_activeOperands = new Operand[_registerSet.Length];
			Debug.Assert(0 != _activeOperands.Length, @"No registers in the architecture?");
			_activeOpLastUse = new Context[_registerSet.Length];

			// Iterate basic Blocks
			foreach (BasicBlock block in BasicBlocks) {

				// Iterate all instructions in the block
				// Assign registers to all operands, where this needs to be done
				for (Context ctx = new Context(InstructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
					AssignRegisters(ctx);

				// Spill active registers at the end of a block (they're reloaded in the next, if necessary.)
				SpillActiveOperands(block);
			}
		}

		/// <summary>
		/// Spills all active operands at the end of a basic block.
		/// </summary>
		/// <param name="block">The basic block to spill in.</param>
		private void SpillActiveOperands(BasicBlock block)
		{
			int regIdx = 0;
			foreach (Operand op in _activeOperands) {
				if (op != null && op is MemoryOperand) {

					Context ctx = new Context(InstructionSet, block);
					ctx.GotoLast();

					InsertMove(ctx, op, new RegisterOperand(op.Type, _registerSet[regIdx]));
				}

				regIdx++;
			}
			Array.Clear(_activeOperands, 0, _activeOperands.Length);
		}

		/// <summary>
		/// Assigns registers to operands of the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		private void AssignRegisters(Context ctx)
		{
			// Retrieve the register constraints for the instruction
			IRegisterConstraint rc = null; // _architecture.GetRegisterConstraint(instr);	// FIXME PG - pass context instead???

			// Operand index
			int opIdx;

			if (rc == null && TRACING.TraceWarning)
				Trace.WriteLine(String.Format(@"Failed to get register constraints for instruction {0}!", ctx.Instruction.ToString(ctx)));

			// Only process the instruction, if we have constraints...
			if (rc != null) {
				/* FIXME: Spill used registers, if they are in use :(
				 * It is not as simple as it sounds, as the register may be used by the instruction itself,
				 * but dirty afterwards anyways. So we need to decide this at some later point depending on
				 * the arg list. If the register is also a result reg, and they represent the same operand,
				 * we may get away without spilling... Oo?
				 */
				//Register[] used = rc.GetRegistersUsed();

				opIdx = 0;

				Context at = ctx.Clone();

				foreach (Operand op in ctx.Operands) {
					// Only allocate registers, if we really have to.
					if (!rc.IsValidOperand(opIdx, op)) {
						// The register operand allocated
						RegisterOperand rop;
						// List of compatible registers
						Register[] regs = rc.GetRegistersForOperand(opIdx);
						Debug.Assert(null != regs, @"IRegisterConstraint.GetRegistersForOperand returned null.");
						Debug.Assert(0 != regs.Length, @"WTF IRegisterConstraint.GetRegistersForOperand returned zero-length array - what shall we pass then if no register/memory?");

						// Is this operand in a register?
						rop = GetRegisterOfOperand(op);
						if (rop != null && Array.IndexOf(regs, rop.Register) == -1) {
							// Spill the register...
							SpillRegister(ctx, op, rop);
							rop = null;
						}

						// Attempt to allocate a free register
						if (rop == null) {
							rop = AllocateRegister(regs, op);
							if (rop != null) {
								// We need to place a load here... :(
								InsertMove(ctx, rop, op);
							}
						}

						// Still failed to get one? Spill!
						if (rop == null) {
							// Darn, need to spill one... Always use the first one.
							rop = SpillRegister(at, op.Type, regs);

							// We need to place a load here... :(
							InsertMove(at, rop, op);
						}

						// Assign the register
						Debug.Assert(rop != null, @"Whoa: Failed to allocate a register operand??");
						AssignRegister(rop, op, at);
						ctx.SetOperand(opIdx, rop); // FIXME PG - opIdx? hmmm. is this phidata?
					}

					opIdx++;
				}

				opIdx = 0;
				foreach (Operand res in ctx.Results) {
					// FIXME: Check support first, spill if register is target and in use
					if (!rc.IsValidResult(opIdx, res)) {
						// Is this operand in a register?
						RegisterOperand rop = GetRegisterOfOperand(res);
						if (rop == null && !rc.IsValidResult(opIdx, res) && res.Uses.Count == 0)
							// Do not allocate: Not in a register, allows memory & has no uses... oO wtf?
							continue;

						// Retrieve compliant registers
						Register[] regs = rc.GetRegistersForResult(opIdx);

						// Do we already have a register?
						if (rop != null && Array.IndexOf(regs, rop.Register) == -1) {
							// Hmm, current register doesn't match, release it - since we're overwriting the operand,
							// we don't need to spill. This should be safe.
							_activeOperands[rop.Register.Index] = null;
						}

						// Allocate a register
						if (rop == null)
							rop = AllocateRegister(regs, res);

						// Do we need to spill?
						if (rop == null) {
							// Darn, need to spill one...
							rop = SpillRegister(at, res.Type, regs);
							// We don't need to place a load here, as we're defining the register...
						}

						// Assign the register
						Debug.Assert(null != rop, @"Whoa: Failed to allocate a register operand??");
						AssignRegister(rop, res, at);
						ctx.SetResult(opIdx, rop); // FIXME PG - same
					}
					else {
						RegisterOperand rop = res as RegisterOperand;
						if (rop != null)
							SpillRegisterIfInUse(ctx, rop);
					}

					opIdx++;
				}
			}
		}

		/// <summary>
		/// Spills the given register operand, if its register is in use.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="rop">The register operand to spill.</param>
		private void SpillRegisterIfInUse(Context ctx, RegisterOperand rop)
		{
			int regIdx = rop.Register.Index;
			Operand op = _activeOperands[regIdx];
			if (op != null) {
				InsertMove(ctx, op, rop);
				_activeOperands[regIdx] = null;
				_activeOpLastUse[regIdx] = null;
			}
		}

		/// <summary>
		/// Inserts the move instruction to load or spill an operand.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		private void InsertMove(Context ctx, Operand destination, Operand source)
		{
			//LegacyInstruction move = _architecture.CreateInstruction(typeof(IR.MoveInstruction), destination, source);
			//block.Instructions.Insert(idx, move);
			ctx.AppendInstruction(IR.Instruction.MoveInstruction, destination, source);
		}

		/// <summary>
		/// Tries to find the register, which the given operand is assigned to.
		/// </summary>
		/// <param name="op">The operand to find the current register for.</param>
		/// <returns>The register operand of the assigned register.</returns>
		private RegisterOperand GetRegisterOfOperand(Operand op)
		{
			int regIdx = 0;
			foreach (Operand activeOp in _activeOperands) {
				if (activeOp == op) {
					return new RegisterOperand(op.Type, _registerSet[regIdx]);
				}

				regIdx++;
			}

			return null;
		}

		/// <summary>
		/// Allocates a free register From the given register set.
		/// </summary>
		/// <param name="regs">The regs.</param>
		/// <param name="op">The res.</param>
		/// <returns>The allocated RegisterOperand or null, if no register is free.</returns>
		private RegisterOperand AllocateRegister(Register[] regs, Operand op)
		{
			foreach (Register reg in regs) {
				// Is the register in use?
				if (_activeOperands[reg.Index] == null) {
					// No, use it...
					_activeOperands[reg.Index] = op;
					return new RegisterOperand(op.Type, reg);
				}
			}

			return null;
		}

		/// <summary>
		/// Spills a register from the given register set.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="type">The signature type of the resulting operand.</param>
		/// <param name="regs">The instruction compatible subset of the register set.</param>
		/// <returns>A register From the given set.</returns>
		private RegisterOperand SpillRegister(Context ctx, SigType type, Register[] regs)
		{
			// FIXME: Find the oldest reg from the set. Always use the oldest one.

			KeyValuePair<Register, Context>[] lastUses = new KeyValuePair<Register, Context>[regs.Length];

			int i = 0;
			foreach (Register reg in regs)
				lastUses[i++] = new KeyValuePair<Register, Context>(reg, _activeOpLastUse[reg.Index]);

			// Sort the last uses from oldest -> newest
			Array.Sort<KeyValuePair<Register, Context>>(lastUses, delegate(KeyValuePair<Register, Context> a, KeyValuePair<Register, Context> b)
			{
				return b.Value.Offset - a.Value.Offset;
			});

			// Spill the oldest entry (first index)
			KeyValuePair<Register, Context> lastUse = lastUses[0];
			Operand dest = _activeOperands[lastUse.Key.Index];
			RegisterOperand src = new RegisterOperand(dest.Type, lastUse.Key);
			SpillRegister(lastUse.Value, dest, src);

			return new RegisterOperand(type, lastUse.Key);
		}

		/// <summary>
		/// Spills the given register to its source operand.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="op">The operand to spill to.</param>
		/// <param name="rop">The register operand to spill.</param>
		private void SpillRegister(Context ctx, Operand op, RegisterOperand rop)
		{
			// Set the operand in the active operands list
			int regIdx = rop.Register.Index;
			Debug.Assert(_activeOperands[regIdx].Equals(op), @"Register assigned to another operand?");
			InsertMove(ctx, op, rop);
			_activeOperands[regIdx] = null;
			_activeOpLastUse[regIdx] = null;
		}

		/// <summary>
		/// Assigns the operand to the register.
		/// </summary>
		/// <param name="rop">The register operand containing the register to assign.</param>
		/// <param name="op">The operand assigned to the register.</param>
		/// <param name="ctx">The context.</param>
		private void AssignRegister(RegisterOperand rop, Operand op, Context ctx)
		{
			// Set the operand in the active operands list
			int regIdx = rop.Register.Index;
			Debug.Assert(_activeOperands[regIdx] == null, @"Register not free.");
			_activeOperands[regIdx] = op;
			_activeOpLastUse[regIdx] = ctx;
		}

		#endregion // IMethodCompilerStage Members
	}
}
