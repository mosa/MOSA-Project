/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata.Signatures;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Transforms the intermediate representation into a minimal static single assignment form.
    /// </summary>
    /// <remarks>
    /// The minimal form only inserts the really required PHI functions in order to reduce the 
    /// number of live registers used by register allocation.
    /// </remarks>
    public sealed class EnterSSA : BaseStage, IMethodCompilerStage, IPipelineStage
    {
        #region Tracing

        /// <summary>
        /// Controls the tracing of the <see cref="EnterSSA"/> method compiler stage.
        /// </summary>
        /// <remarks>
        /// The trace output happens at the TraceLevel.Info level.
        /// </remarks>
        public static readonly TraceSwitch TRACING = new TraceSwitch(@"Mosa.Runtime.CompilerFramework.EnterSSA", @"Controls tracing of the Mosa.Runtime.CompilerFramework.EnterSSA compilation stage.");

        #endregion // Tracing

        #region Types

        struct WorkItem
        {
            public WorkItem(BasicBlock block, BasicBlock caller, IDictionary<StackOperand, StackOperand> liveIn)
            {
                this.block = block;
                this.caller = caller;
                this.liveIn = liveIn;
            }

            public BasicBlock block;
            public BasicBlock caller;
            public IDictionary<StackOperand, StackOperand> liveIn;
        }

        #endregion // Types

        #region Data members

        /// <summary>
        /// Holds the dominance frontier Blocks of the stage.
        /// </summary>
        private BasicBlock[] _dominanceFrontierBlocks;

        /// <summary>
        /// Holds the dominance provider.
        /// </summary>
        private IDominanceProvider _dominanceProvider;

        /// <summary>
        /// Holds the operand definitions after each block (array of block count length).
        /// </summary>
        private IDictionary<StackOperand, StackOperand>[] _liveness;

        /// <summary>
        /// Holds the version of the next SSA variable. (We use a single version number for all of them)
        /// </summary>
        private int _ssaVersion;

        #endregion // Data members

        #region IPipelineStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        string IPipelineStage.Name { get { return @"EnterSSA"; } }

        private static PipelineStageOrder[] _pipelineOrder = new PipelineStageOrder[] {
			new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(DominanceCalculationStage)),
			new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(StackLayoutStage)),
			new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(IPlatformInstruction)),
		};

        /// <summary>
        /// Gets the pipeline stage order.
        /// </summary>
        /// <value>The pipeline stage order.</value>
        PipelineStageOrder[] IPipelineStage.PipelineStageOrder { get { return _pipelineOrder; } }

        #endregion // IPipelineStage Members

        #region IMethodCompilerStage Members

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        public void Run()
        {
            _dominanceProvider = (IDominanceProvider)MethodCompiler.GetPreviousStage(typeof(IDominanceProvider));
            Debug.Assert(_dominanceProvider != null, @"SSA Conversion requires a dominance provider.");
            if (_dominanceProvider == null)
                throw new InvalidOperationException(@"SSA Conversion requires a dominance provider.");

            // Allocate space for live outs
            _liveness = new IDictionary<StackOperand, StackOperand>[BasicBlocks.Count];
            // Retrieve the dominance frontier Blocks
            _dominanceFrontierBlocks = _dominanceProvider.GetDominanceFrontier();

            // Add ref/out parameters to the epilogue block to have uses there...
            AddPhiFunctionsForOutParameters();

            // Transformation worklist 
            Queue<WorkItem> workList = new Queue<WorkItem>();

            /* Move parameter operands into the dictionary as version 0,
             * because they are live at entry and maybe referenced. Anyways, an
             * assignment to a parameter is also SSA related.
             */
            IDictionary<StackOperand, StackOperand> liveIn = new Dictionary<StackOperand, StackOperand>(s_comparer);
            int i = 0;
            if (MethodCompiler.Method.Signature.HasThis)
            {
                StackOperand param = (StackOperand)MethodCompiler.GetParameterOperand(0);
                liveIn.Add(param, param);
                i++;
            }
            for (int j = 0; j < MethodCompiler.Method.Parameters.Count; j++)
            {
                StackOperand param = (StackOperand)MethodCompiler.GetParameterOperand(i + j);
                liveIn.Add(param, param);
            }

            // Start with the very first block
            workList.Enqueue(new WorkItem(BasicBlocks[0], null, liveIn));

            // Iterate until the worklist is empty
            while (workList.Count != 0)
            {
                // Remove the block From the queue
                WorkItem workItem = workList.Dequeue();

                // Transform the block
                BasicBlock block = workItem.block;
                bool schedule = TransformToSsaForm(new Context(InstructionSet, block), workItem.caller, workItem.liveIn, out liveIn);
                _liveness[block.Sequence] = liveIn;

                if (schedule)
                {
                    // Add all branch targets to the work list
                    foreach (BasicBlock next in block.NextBlocks)
                    {
                        // Only follow backward branches, if we've redefined a variable
                        // this may force us to reinsert a PHI function in a block we
                        // already have completed processing on.
                        workList.Enqueue(new WorkItem(next, block, liveIn));
                    }
                }
            }
        }

        /// <summary>
        /// Adds PHI functions for all ref/out parameters of the method being compiled.
        /// </summary>
        private void AddPhiFunctionsForOutParameters()
        {
            Dictionary<StackOperand, StackOperand> liveIn = null;

            // Retrieve the well known epilogue block
            BasicBlock epilogue = FindBlock(Int32.MaxValue);
            Debug.Assert(epilogue != null, @"Method doesn't have epilogue block?");

            Context ctxEpilogue = new Context(InstructionSet, epilogue);
            ctxEpilogue.GotoLast();

            // Iterate all parameter definitions
            foreach (RuntimeParameter rp in MethodCompiler.Method.Parameters)
            {
                // Retrieve the stack operand for the parameter
                StackOperand paramOp = (StackOperand)MethodCompiler.GetParameterOperand(rp.Position - 1);

                // Only add a PHI if the runtime parameter is out or ref...
                if (rp.IsOut || (paramOp.Type is RefSigType || paramOp.Type is PtrSigType))
                {
                    ctxEpilogue.AppendInstruction(IR.Instruction.PhiInstruction, paramOp);

                    if (liveIn == null)
                        liveIn = new Dictionary<StackOperand, StackOperand>();

                    liveIn.Add(paramOp, paramOp);
                }
            }

            // Save the in versions to force a merge later
            if (liveIn != null)
                _liveness[epilogue.Sequence] = liveIn;
        }

        private bool TransformToSsaForm(Context ctx, BasicBlock caller, IDictionary<StackOperand, StackOperand> liveIn, out IDictionary<StackOperand, StackOperand> liveOut)
        {
            // Is this another entry for this block?
            IDictionary<StackOperand, StackOperand> liveOutPrev = _liveness[ctx.BasicBlock.Sequence];
            if (liveOutPrev != null)
            {
                // FIXME: Merge PHIs with new incoming variables, add new incoming variables to out set
                // and schedule the remaining out nodes/quit
                MergePhiInstructions(ctx.BasicBlock, caller, liveIn);
                liveOut = liveOutPrev;

                return false;
            }
            // Is this a dominance frontier block?
            if (Array.IndexOf(_dominanceFrontierBlocks, ctx.BasicBlock) != -1)
                InsertPhiInstructions(ctx, caller, liveIn);

            // Create a new live out dictionary
            if (liveIn != null)
                liveOut = new Dictionary<StackOperand, StackOperand>(liveIn, s_comparer);
            else
                liveOut = new Dictionary<StackOperand, StackOperand>(s_comparer);

            // Iterate each instruction in the block
            for (Context ctxBlock = CreateContext(ctx.BasicBlock); !ctxBlock.EndOfInstruction; ctxBlock.GotoNext())
            {
                // Replace all operands with their current SSA version
                UpdateUses(ctxBlock, liveOut);

                // Is this an instruction we ignore?
                if (!ctx.Ignore)
                    RenameStackOperands(ctxBlock, liveOut);
            }

            return true;
        }

        private void MergePhiInstructions(BasicBlock block, BasicBlock caller, IDictionary<StackOperand, StackOperand> liveIn)
        {
            for (Context ctx = new Context(InstructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
            {
                IR.PhiInstruction phi = ctx.Instruction as IR.PhiInstruction;

                if (phi != null && liveIn.ContainsKey(ctx.Result as StackOperand))
                {
                    StackOperand value = liveIn[ctx.Result as StackOperand];

                    if (!IR.PhiInstruction.Contains(ctx, value) && (ctx.Result as StackOperand).Version != value.Version)
                        IR.PhiInstruction.AddValue(ctx, caller, value);
                }
            }

        }

        private void InsertPhiInstructions(Context ctx, BasicBlock caller, IDictionary<StackOperand, StackOperand> liveIn)
        {
            // Iterate all incoming variables
            foreach (StackOperand key in liveIn.Keys)
            {
                ctx.AppendInstruction(IR.Instruction.PhiInstruction, key);
                IR.PhiInstruction.AddValue(ctx, caller, key);
            }
        }

        private void RenameStackOperands(Context ctx, IDictionary<StackOperand, StackOperand> liveOut)
        {
            int index = 0;

            // Create new SSA variables for newly defined operands
            foreach (Operand op1 in ctx.Results)
            {
                // Is this a stack operand?
                StackOperand op = op1 as StackOperand;

                if (op != null)
                {
                    StackOperand ssa;
                    if (!liveOut.TryGetValue(op, out ssa))
                        ssa = op;

                    ssa = RedefineOperand(ssa);
                    liveOut[op] = ssa;
                    ctx.SetResult(index++, ssa);

                    if (TRACING.TraceInfo)
                        Trace.WriteLine(String.Format("\tStore to {0} redefined as {1}", op, ssa));
                }
            }
        }

        private void UpdateUses(Context ctx, IDictionary<StackOperand, StackOperand> liveOut)
        {
            int index = 0;

            foreach (Operand op1 in ctx.Operands)
            {
                // Is this a stack operand?
                StackOperand op = op1 as StackOperand;

                if (op != null)
                {
                    StackOperand ssa;
                    // Determine the most recent version
                    Debug.Assert(liveOut.TryGetValue(op, out ssa), @"Stack operand not in live variable list.");

                    ssa = liveOut[op];

                    // Replace the use with the most recent version
                    ctx.SetOperand(index++, ssa);

                    if (TRACING.TraceInfo)
                        Trace.WriteLine(String.Format(@"\tUse {0} has been replaced with {1}", op, ssa));
                }
            }
        }

        class StackOperandComparer : IEqualityComparer<StackOperand>
        {
            #region IEqualityComparer<StackOperand> Members

            bool IEqualityComparer<StackOperand>.Equals(StackOperand x, StackOperand y)
            {
                return (x.Offset.ToInt32() == y.Offset.ToInt32());
            }

            int IEqualityComparer<StackOperand>.GetHashCode(StackOperand obj)
            {
                return obj.Offset.ToInt32();
            }

            #endregion // IEqualityComparer<StackOperand> Members
        }

        private static readonly IEqualityComparer<StackOperand> s_comparer = new StackOperandComparer();

        /// <summary>
        /// Redefines a <see cref="StackOperand"/> with a new SSA version.
        /// </summary>
        /// <param name="cur">The StackOperand to redefine.</param>
        /// <returns>A new StackOperand.</returns>
        private StackOperand RedefineOperand(StackOperand cur)
        {
            string name = cur.Name;
            if (cur.Version == 0)
                name = String.Format(@"T_{0}", name);
            StackOperand op = MethodCompiler.CreateTemporary(cur.Type) as StackOperand;
            //StackOperand op = new LocalVariableOperand(cur.Base, name, idx, cur.Type);
            op.Version = ++_ssaVersion;
            return op;
        }

        #endregion // IMethodCompilerStage Members
    }
}
