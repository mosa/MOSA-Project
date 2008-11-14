/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.CompilerFramework.IL;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// This compilation stage is used by method compilers after the
    /// IL decoding stage to build basic blocks out of the instruction list.
    /// </summary>
    public sealed class BasicBlockBuilderStage : IMethodCompilerStage, IBasicBlockProvider
    {
        #region Data members

        /// <summary>
        /// List of basic blocks found during decoding.
        /// </summary>
        private List<BasicBlock> _basicBlocks;

        /// <summary>
        /// List of scheduled blocks.
        /// </summary>
        private List<BasicBlock> _scheduledBlocks;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicBlockBuilderStage"/> class.
        /// </summary>
        public BasicBlockBuilderStage()
        {
            _basicBlocks = new List<BasicBlock>();
            _scheduledBlocks = new List<BasicBlock>();
        }

        #endregion // Construction

        #region IMethodCompilerStage members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value></value>
        string IMethodCompilerStage.Name
        {
            get { return @"Basic Block Builder"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        void IMethodCompilerStage.Run(MethodCompilerBase compiler)
        {
            // Epilogue and current basic block
            BasicBlock currentBlock = null, epilogue;
            // Retrieve the instruction provider
            IInstructionsProvider ip = (IInstructionsProvider)compiler.GetPreviousStage(typeof(IInstructionsProvider));
            // Architecture
            IArchitecture arch = compiler.Architecture;

            // Add the epilogue block
            epilogue = new BasicBlock(Int32.MaxValue);
            _basicBlocks.Add(epilogue);

            // Start with a prologue block...
            currentBlock = new BasicBlock(-1);
            _basicBlocks.Add(currentBlock);
            // Add a jump instruction to the first block from the prologue
            ip.Instructions.Insert(0, arch.CreateInstruction(typeof(IL.BranchInstruction), OpCode.Br, new int[] { 0 }));
            for (int idx = 0; idx < ip.Instructions.Count; idx++)
            {
                // Retrieve the instruction
                Instruction instruction = ip.Instructions[idx];

                // Is this the next scheduled block?
                if (0 != _scheduledBlocks.Count && instruction.Offset == _scheduledBlocks[0].Label)
                {
                    BasicBlock nextBlock = _scheduledBlocks[0];
                    _scheduledBlocks.RemoveAt(0);

                    // Add an unconditional branch to the next block to the current block
                    if (null != currentBlock)
                    {
                        BranchInstruction dummyBranch = new BranchInstruction(OpCode.Br_s, new int[] { nextBlock.Label });
                        currentBlock.Instructions.Add(dummyBranch);

                        // Link the blocks
                        LinkBlocks(currentBlock, nextBlock);
                    }

                    // Done, stack should match...
                    currentBlock = nextBlock;
                }

                // Create a new block, if we need it.
                if (null == currentBlock)
                    currentBlock = FindOrCreateBlock(null, instruction.Offset);
                Debug.Assert(null != currentBlock);

                // Does this instruction end a block?
                switch (instruction.FlowControl)
                {
                    case FlowControl.Break: goto case FlowControl.Next;
                    case FlowControl.Call: goto case FlowControl.Next;
                    case FlowControl.Next:
                        // Add the instruction to the current block
                        currentBlock.Instructions.Add(instruction);
                        break;

                    case FlowControl.Return:
                        // Replace this one by a jump to the method epilogue
                        //BranchInstruction jmp = (BranchInstruction)arch.CreateInstruction(typeof(BranchInstruction), new object[] { OpCode.Br, Int32.MaxValue } );
                        //jmp.BranchTargets = new int[] { Int32.MaxValue };
                        //jmp.Offset = instruction.Offset;
                        LinkBlocks(currentBlock, epilogue);
                        currentBlock.Instructions.Add(instruction);
                        currentBlock = null;
                        break;

                    case FlowControl.Switch:
                        // Switch may fall through
                        int fallThrough = -1;
                        if (idx + 1 < ip.Instructions.Count)
                            fallThrough = ip.Instructions[idx + 1].Offset;
                        ScheduleBranchTargets(currentBlock, (IBranchInstruction)instruction, fallThrough);
                        goto case FlowControl.Throw;

                    case FlowControl.Branch: goto case FlowControl.ConditionalBranch;
                    case FlowControl.ConditionalBranch:
                        ScheduleBranchTargets(currentBlock, (IBranchInstruction)instruction, -1);
                        goto case FlowControl.Throw;

                    case FlowControl.Throw:
                        // End the block, start a new one on the next statement
                        currentBlock.Instructions.Add(instruction);
                        currentBlock = null;
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
            }
            
            // Sort the blocks by their label order
            _basicBlocks.Sort(delegate(BasicBlock left, BasicBlock right)
            {
                return (left.Label - right.Label);
            });

            // Number the blocks
            int blockIdx = 0;
            foreach (BasicBlock block in _basicBlocks)
                block.Index = blockIdx++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertBefore<CilToIrTransformationStage>(this);
        }

        /// <summary>
        /// Schedules the branch targets of the branch instruction.
        /// </summary>
        /// <param name="currentBlock">The currently processed basic block.</param>
        /// <param name="branch">The branch instruction, whose targets need to be scheduled.</param>
        /// <param name="fallThrough">Specifies the next instruction offset, if the branch falls through - otherwise -1.</param>
        private void ScheduleBranchTargets(BasicBlock currentBlock, IBranchInstruction branch, int fallThrough)
        {
            List<int> targets = new List<int>(branch.BranchTargets);
            if (-1 != fallThrough)
                targets.Add(fallThrough);

            foreach (int target in targets)
            {
                BasicBlock newBlock = FindOrCreateBlock(currentBlock, target);
                if (target > branch.Offset)
                {
                    // Check for a forward branch, schedule the block
                    bool blockExists = false;
                    int index = _scheduledBlocks.FindIndex((Predicate<BasicBlock>)delegate(BasicBlock match)
                    {
                        blockExists = (match.Label == target);
                        return (match.Label > target);
                    });

                    // Schedule a new block...
                    if (false == blockExists)
                    {
                        if (-1 == index)
                            _scheduledBlocks.Add(newBlock);
                        else
                            _scheduledBlocks.Insert(index, newBlock);
                    }
                }
            }
        }

        /// <summary>
        /// Finds the or create block.
        /// </summary>
        /// <param name="caller">The caller.</param>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        private BasicBlock FindOrCreateBlock(BasicBlock caller, int label)
        {
            // Return value
            BasicBlock result;
            // Flag, if we need to split an existing block
            int split = -1;

            // Attempt a lookup on the label
            result = _basicBlocks.Find((Predicate<BasicBlock>)delegate(BasicBlock match)
            {
                bool b = (match.Label == label);
                if (false == b)
                {
                    // Check if we need to split this block...
                    List<Instruction> instructions = match.Instructions;
                    if (0 != instructions.Count && instructions[0].Offset < label && label <= instructions[instructions.Count - 1].Offset)
                    {
                        split = instructions.FindIndex((Predicate<Instruction>)delegate(Instruction inst)
                        {
                            return (inst.Offset == label);
                        });
                        b = (split != -1);
                    }
                }

                return b;
            });

            // Did we find one?
            if (null == result)
            {
                // No, create a new block
                result = new BasicBlock(label);

                // Add the block to the list
                _basicBlocks.Add(result);
            }
            else if (-1 != split)
            {
                result = result.Split(split, label);
                _basicBlocks.Add(result);
            }

            // Hook the blocks
            if (null != caller)
                LinkBlocks(caller, result);

            return result;
        }

        /// <summary>
        /// Links the blocks.
        /// </summary>
        /// <param name="caller">The caller.</param>
        /// <param name="callee">The callee.</param>
        private void LinkBlocks(BasicBlock caller, BasicBlock callee)
        {
            // Chain the blocks together
            callee.PreviousBlocks.Add(caller);
            caller.NextBlocks.Add(callee);
        }

        #endregion // IMethodCompilerStage members

        #region IBasicBlockProvider members

        /// <summary>
        /// Gets the basic blocks.
        /// </summary>
        /// <value>The basic blocks.</value>
        public List<BasicBlock> Blocks
        {
            get { return _basicBlocks; }
        }

        /// <summary>
        /// Retrieves a basic block from its label.
        /// </summary>
        /// <param name="label">The label of the basic block.</param>
        /// <returns>
        /// The basic block with the given label or null.
        /// </returns>
        public BasicBlock FromLabel(int label)
        {
            return _basicBlocks.Find(delegate(BasicBlock block)
            {
                return (label == block.Label);
            });
        }

        /// <summary>
        /// Gibt einen Enumerator zurück, der die Auflistung durchläuft.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.Collections.Generic.IEnumerator`1"/>, der zum Durchlaufen der Auflistung verwendet werden kann.
        /// </returns>
        public IEnumerator<BasicBlock> GetEnumerator()
        {
            return _basicBlocks.GetEnumerator();
        }

        /// <summary>
        /// Gibt einen Enumerator zurück, der eine Auflistung durchläuft.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.Collections.IEnumerator"/>-Objekt, das zum Durchlaufen der Auflistung verwendet werden kann.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _basicBlocks.GetEnumerator();
        }

        #endregion // IBasicBlockProvider members
    }
}
