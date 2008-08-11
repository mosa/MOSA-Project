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

        #endregion // Data members

        #region Construction

        public BasicBlockBuilderStage()
        {
            _basicBlocks = new List<BasicBlock>();
        }

        #endregion // Construction

        #region IMethodCompilerStage members

        string IMethodCompilerStage.Name
        {
            get { return @"Basic Block Builder"; }
        }

        void IMethodCompilerStage.Run(MethodCompilerBase compiler)
        {
            // Epilogue and current basic block
            BasicBlock currentBlock = null, epilogue;
            // Retrieve the instruction provider
            IInstructionsProvider ip = (IInstructionsProvider)compiler.GetPreviousStage(typeof(IInstructionsProvider));
            // Architecture
            IArchitecture arch = compiler.Architecture;
            // List of scheduled blocks
            List<BasicBlock> scheduledBlocks = new List<BasicBlock>();

            // Add the epilogue block
            epilogue = new BasicBlock(Int32.MaxValue);
            _basicBlocks.Add(epilogue);

            // Start with a prologue block...
            currentBlock = new BasicBlock(-1);
            _basicBlocks.Add(currentBlock);
            // Add a jump instruction to the first block from the prologue
            ip.Instructions.Insert(0, arch.CreateInstruction(typeof(IL.BranchInstruction), OpCode.Br, new int[] { 0 }));
            foreach (Instruction i in ip.Instructions)
            {
                // Is this the next scheduled block?
                if (0 != scheduledBlocks.Count && i.Offset == scheduledBlocks[0].Label)
                {
                    BasicBlock nextBlock = scheduledBlocks[0];
                    scheduledBlocks.RemoveAt(0);

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
                    currentBlock = FindOrCreateBlock(null, i.Offset);
                Debug.Assert(null != currentBlock);

                // Does this instruction end a block?
                switch (i.FlowControl)
                {
                    case FlowControl.Phi: goto case FlowControl.Next;
                    case FlowControl.Break: goto case FlowControl.Next;
                    case FlowControl.Call: goto case FlowControl.Next;
                    case FlowControl.Meta: goto case FlowControl.Next;
                    case FlowControl.Next:
                        // Add the instruction to the current block
                        currentBlock.Instructions.Add(i);
                        break;

                    case FlowControl.Return:
                        // Replace this one by a jump to the method epilogue
                        //BranchInstruction jmp = (BranchInstruction)arch.CreateInstruction(typeof(BranchInstruction), new object[] { OpCode.Br, Int32.MaxValue } );
                        //jmp.BranchTargets = new int[] { Int32.MaxValue };
                        //jmp.Offset = i.Offset;
                        epilogue.PreviousBlocks.Add(currentBlock);
                        currentBlock.Instructions.Add(i);
                        currentBlock = null;
                        break;

                    case FlowControl.Branch: goto case FlowControl.ConditionalBranch;
                    case FlowControl.ConditionalBranch: 
                        IBranchInstruction branch = (IBranchInstruction)i;
                        int[] targets = branch.BranchTargets;
                        //Array.Sort<int>(targets);
                        foreach (int target in targets)
                        {
                            BasicBlock newBlock = FindOrCreateBlock(currentBlock, target);
                            if (target > i.Offset)
                            {
                                // Check for a forward branch, schedule the block
                                bool blockExists = false;
                                int index = scheduledBlocks.FindIndex((Predicate<BasicBlock>)delegate(BasicBlock match)
                                {
                                    blockExists = (match.Label == target);
                                    return (match.Label > target);
                                });

                                // Schedule a new block...
                                if (false == blockExists)
                                {
                                    if (-1 == index)
                                        scheduledBlocks.Add(newBlock);
                                    else
                                        scheduledBlocks.Insert(index, newBlock);
                                }
                            }
                        }
                        goto case FlowControl.Throw;

                    case FlowControl.Throw:
                        // End the block, start a new one on the next statement
                        currentBlock.Instructions.Add(i);
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
            int idx = 0;
            foreach (BasicBlock block in _basicBlocks)
                block.Index = idx++;
        }

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
                result = result.Split(split);
                _basicBlocks.Add(result);
            }

            // Hook the blocks
            if (null != caller)
                LinkBlocks(caller, result);

            return result;
        }

        private void LinkBlocks(BasicBlock caller, BasicBlock callee)
        {
            // Chain the blocks together
            callee.PreviousBlocks.Add(caller);
            caller.NextBlocks.Add(callee);
        }

        #endregion // IMethodCompilerStage members

        #region IBasicBlockProvider members

        public int Count
        {
            get { return _basicBlocks.Count; }
        }

        public BasicBlock this[int index]
        {
            get { return _basicBlocks[index]; }
        }

        public BasicBlock FromLabel(int label)
        {
            return _basicBlocks.Find(delegate(BasicBlock block)
            {
                return (label == block.Label);
            });
        }

        public IEnumerator<BasicBlock> GetEnumerator()
        {
            return _basicBlocks.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _basicBlocks.GetEnumerator();
        }

        #endregion // IBasicBlockProvider members
    }
}
