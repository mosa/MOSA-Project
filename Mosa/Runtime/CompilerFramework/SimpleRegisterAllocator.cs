using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Mosa.Runtime.Metadata.Signatures;

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
    public sealed class SimpleRegisterAllocator : IMethodCompilerStage
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

        private int[] _activeOpLastUse;

        /// <summary>
        /// The compilation target architecture.
        /// </summary>
        private IArchitecture _architecture;

        /// <summary>
        /// Holds the entire register set of the compilation target architecture.
        /// </summary>
        private Register[] _registerSet;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="SimpleRegisterAllocator"/>.
        /// </summary>
        public SimpleRegisterAllocator()
        {
        }

        #endregion // Construction

        #region IMethodCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get { return @"SimpleRegisterAllocator"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(IMethodCompiler compiler)
        {
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            if (null == blockProvider)
                throw new InvalidOperationException(@"Method compiler requires basic block provider.");

            _architecture = compiler.Architecture;
            _registerSet = _architecture.RegisterSet;
            _activeOperands = new Operand[_registerSet.Length];
            Debug.Assert(0 != _activeOperands.Length, @"No registers in the architecture?");
            _activeOpLastUse = new int[_registerSet.Length];

            // Iterate basic blocks
            foreach (BasicBlock block in blockProvider)
            {
                // Iterate all instructions in the block
                for (int idx = 0; idx < block.Instructions.Count; idx++)
                {
                    // Assign registers to all operands, where this needs to be done
                    AssignRegisters(block, ref idx);
                }

                // Spill active registers at the end of a block (they're reloaded in the next, if necessary.)
                SpillActiveOperands(block);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertBefore<ICodeGenerationStage>(this);
        }

        /// <summary>
        /// Spills all active operands at the end of a basic block.
        /// </summary>
        /// <param name="block">The basic block to spill in.</param>
        private void SpillActiveOperands(BasicBlock block)
        {
            int regIdx = 0;
            foreach (Operand op in _activeOperands)
            {
                if (null != op && op is MemoryOperand)
                {
                    Debug.Assert(block.Instructions.Count > 0, @"No instructions in the block?");
                    InsertMove(block, block.Instructions.Count - 1, op, new RegisterOperand(op.Type, _registerSet[regIdx]));
                }

                regIdx++;
            }
            Array.Clear(_activeOperands, 0, _activeOperands.Length);
        }

        /// <summary>
        /// Assigns registers to operands of the instruction.
        /// </summary>
        /// <param name="block">The basic block currently processed.</param>
        /// <param name="idx">The index of the current instruction.</param>
        private void AssignRegisters(BasicBlock block, ref int idx)
        {
            // Retrieve the current instruction
            Instruction instr = block.Instructions[idx];
            // Retrieve the register constraints for the instruction
            IRegisterConstraint rc = _architecture.GetRegisterConstraint(instr);
            // Operand index
            int opIdx;

            if (null == rc && TRACING.TraceWarning == true)
                Trace.WriteLine(String.Format(@"Failed to get register constraints for instruction {0}!", instr));


            // Only process the instruction, if we have constraints...
            if (null != rc)
            {
                /* FIXME: Spill used registers, if they are in use :(
                 * It is not as simple as it sounds, as the register may be used by the instruction itself,
                 * but dirty afterwards anyways. So we need to decide this at some later point depending on
                 * the arg list. If the register is also a result reg, and they represent the same operand,
                 * we may get away without spilling... Oo?
                 */
                //Register[] used = rc.GetRegistersUsed();

                opIdx = 0;
                foreach (Operand op in instr.Operands)
                {
                    // Only allocate registers, if we really have to.
                    if (false == rc.IsValidOperand(opIdx, op))
                    {
                        // The register operand allocated
                        RegisterOperand rop;
                        // List of compatible registers
                        Register[] regs = rc.GetRegistersForOperand(opIdx);
                        Debug.Assert(null != regs, @"IRegisterConstraint.GetRegistersForOperand returned null.");
                        Debug.Assert(0 != regs.Length, @"WTF IRegisterConstraint.GetRegistersForOperand returned zero-length array - what shall we pass then if no register/memory?");

                        // Is this operand in a register?
                        rop = GetRegisterOfOperand(op);
                        if (null != rop && -1 == Array.IndexOf(regs, rop.Register))
                        {
                            // Spill the register...
                            SpillRegister(block, idx++, op, rop);
                            rop = null;
                        }

                        // Attempt to allocate a free register
                        if (null == rop)
                        {
                            rop = AllocateRegister(regs, op);
                            if (null != rop)
                            {
                                // We need to place a load here... :(
                                InsertMove(block, idx++, rop, op);
                            }
                        }

                        // Still failed to get one? Spill!
                        if (null == rop)
                        {
                            // Darn, need to spill one... Always use the first one.
                            rop = SpillRegister(block, idx++, op.Type, regs);

                            // We need to place a load here... :(
                            InsertMove(block, idx++, rop, op);
                        }

                        // Assign the register
                        Debug.Assert(null != rop, @"Whoa: Failed to allocate a register operand??");
                        AssignRegister(rop, op, idx);
                        instr.SetOperand(opIdx, rop);
                    }

                    opIdx++;
                }

                opIdx = 0;
                foreach (Operand res in instr.Results)
                {
                    // FIXME: Check support first, spill if register is target and in use
                    if (false == rc.IsValidResult(opIdx, res))
                    {
                        // Is this operand in a register?
                        RegisterOperand rop = GetRegisterOfOperand(res);
                        if (null == rop && false == rc.IsValidResult(opIdx, res) && 0 == res.Uses.Count)
                            // Do not allocate: Not in a register, allows memory & has no uses... oO wtf?
                            continue;

                        // Retrieve compliant registers
                        Register[] regs = rc.GetRegistersForResult(opIdx);

                        // Do we already have a register?
                        if (null != rop && -1 == Array.IndexOf(regs, rop.Register))
                        {
                            // Hmm, current register doesn't match, release it - since we're overwriting the operand,
                            // we don't need to spill. This should be safe.
                            _activeOperands[rop.Register.Index] = null;
                        }

                        // Allocate a register
                        if (null == rop)
                            rop = AllocateRegister(regs, res);

                        // Do we need to spill?
                        if (null == rop)
                        {
                            // Darn, need to spill one...
                            rop = SpillRegister(block, idx++, res.Type, regs);
                            // We don't need to place a load here, as we're defining the register...
                        }

                        // Assign the register
                        Debug.Assert(null != rop, @"Whoa: Failed to allocate a register operand??");
                        AssignRegister(rop, res, idx);
                        instr.SetResult(opIdx, rop);
                    }
                    else 
                    {
                        RegisterOperand rop = res as RegisterOperand;
                        if (null != rop)
                            SpillRegisterIfInUse(block, idx, rop);
                    }

                    opIdx++;
                }
            }
        }

        /// <summary>
        /// Spills the given register operand, if its register is in use.
        /// </summary>
        /// <param name="block">The block to spill in.</param>
        /// <param name="idx">The index.</param>
        /// <param name="rop">The register operand to spill.</param>
        private void SpillRegisterIfInUse(BasicBlock block, int idx, RegisterOperand rop)
        {
            int regIdx = rop.Register.Index;
            Operand op = _activeOperands[regIdx];
            if (null != op)
            {
                InsertMove(block, idx, op, rop);
                _activeOperands[regIdx] = null;
                _activeOpLastUse[regIdx] = 0;
            }
        }

        /// <summary>
        /// Inserts the move instruction to load or spill an operand.
        /// </summary>
        /// <param name="block">The block to insert the move in.</param>
        /// <param name="idx">The index of the spot, where the move goes to.</param>
        /// <param name="destination">The destination operand.</param>
        /// <param name="source">The source operand.</param>
        private void InsertMove(BasicBlock block, int idx, Operand destination, Operand source)
        {
            Instruction move = _architecture.CreateInstruction(typeof(IR.MoveInstruction), destination, source);
            block.Instructions.Insert(idx, move);
        }

        /// <summary>
        /// Tries to find the register, which the given operand is assigned to.
        /// </summary>
        /// <param name="op">The operand to find the current register for.</param>
        /// <returns>The register operand of the assigned register.</returns>
        private RegisterOperand GetRegisterOfOperand(Operand op)
        {
            int regIdx = 0;
            foreach (Operand activeOp in _activeOperands)
            {
                if (activeOp == op)
                {
                    return new RegisterOperand(op.Type, _registerSet[regIdx]);
                }

                regIdx++;
            }

            return null;
        }

        /// <summary>
        /// Allocates a free register from the given register set.
        /// </summary>
        /// <param name="regs">The regs.</param>
        /// <param name="op">The res.</param>
        /// <returns>The allocated RegisterOperand or null, if no register is free.</returns>
        private RegisterOperand AllocateRegister(Register[] regs, Operand op)
        {
            foreach (Register reg in regs)
            {
                // Is the register in use?
                if (null == _activeOperands[reg.Index])
                {
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
        /// <param name="block">The block to insert the spill in.</param>
        /// <param name="idx">The index of the instruction, before which to place the spill.</param>
        /// <param name="type">The signature type of the resulting operand.</param>
        /// <param name="regs">The instruction compatible subset of the register set.</param>
        /// <returns>A register from the given set.</returns>
        private RegisterOperand SpillRegister(BasicBlock block, int idx, SigType type, Register[] regs)
        {
            // FIXME: Find the oldest reg from the set. Always use the oldest one.
            KeyValuePair<Register, int>[] lastUses = new KeyValuePair<Register, int>[regs.Length];
            foreach (Register reg in regs)
            {
                lastUses[idx++] = new KeyValuePair<Register, int>(reg, _activeOpLastUse[reg.Index]);
            }

            // Sort the last uses from oldest -> newest
            Array.Sort<KeyValuePair<Register, int>>(lastUses, delegate(KeyValuePair<Register, int> a, KeyValuePair<Register, int> b)
            {
                return b.Value - a.Value;
            });

            // Spill the oldest entry (first index)
            KeyValuePair<Register, int> lastUse = lastUses[0];
            Operand dest = _activeOperands[lastUse.Key.Index];
            RegisterOperand src = new RegisterOperand(dest.Type, lastUse.Key);
            SpillRegister(block, idx, dest, src);

            return new RegisterOperand(type, lastUse.Key);
        }

        /// <summary>
        /// Spills the given register to its source operand.
        /// </summary>
        /// <param name="block">The block to spill in.</param>
        /// <param name="idx">The instruction index of the spill.</param>
        /// <param name="op">The operand to spill to.</param>
        /// <param name="rop">The register operand to spill.</param>
        private void SpillRegister(BasicBlock block, int idx, Operand op, RegisterOperand rop)
        {
            // Set the operand in the active operands list
            int regIdx = rop.Register.Index;
            Debug.Assert(true == _activeOperands[regIdx].Equals(op), @"Register assigned to another operand?");
            InsertMove(block, idx, op, rop);
            _activeOperands[regIdx] = null;
            _activeOpLastUse[regIdx] = 0;
        }

        /// <summary>
        /// Assigns the operand to the register.
        /// </summary>
        /// <param name="rop">The register operand containing the register to assign.</param>
        /// <param name="op">The operand assigned to the register.</param>
        /// <param name="instructionIdx">The index of the instruction referencing the register.</param>
        private void AssignRegister(RegisterOperand rop, Operand op, int instructionIdx)
        {
            // Set the operand in the active operands list
            int regIdx = rop.Register.Index;
            Debug.Assert(null == _activeOperands[regIdx], @"Register not free.");
            _activeOperands[regIdx] = op;
            _activeOpLastUse[regIdx] = instructionIdx;
        }

        #endregion // IMethodCompilerStage Members
    }
}
