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
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Transforms the intermediate representation out of SSA form.
	/// </summary>
	/// <remarks>
	/// This transformation simplifies and expands all PHI functions and
	/// unifies variable version.
	/// </remarks>
	public sealed class LeaveSSA : BaseStage, IMethodCompilerStage, IPipelineStage
	{

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name
		{
			get { return @"LeaveSSA"; }
		}

		private static PipelineStageOrder[] _pipelineOrder = new PipelineStageOrder[] {
			new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(EnterSSA)),
			new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(StackLayoutStage)),
			new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(IPlatformTransformationStage)),
		};

		/// <summary>
		/// Gets the pipeline stage order.
		/// </summary>
		/// <value>The pipeline stage order.</value>
		PipelineStageOrder[] IPipelineStage.PipelineStageOrder { get { return _pipelineOrder; } }

		#endregion IPipelineStage Members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
			// FIXME PG - this is too complex to modify at the moment. It's not used so this will be tabled until later.

			//foreach (BasicBlock block in blockProvider) {
			//                    if (block.Instructions.Count > 0) {
			//    {
			//        Context ctx = new Context(InstructionSet, block);

			//        while (!ctx.EndOfInstruction) {

			//            if (!ctx.Instruction is IR.PhiInstruction)
			//                break;

			//            Operand res = ctx.Result;
			//            for (int i = 0; i < phi.Blocks.Count; i++) {
			//                IR.PhiData phi = ctx.Other as IR.PhiData;

			//                Operand op = phi.Operands[i];

			//                 HACK: Remove phi from the operand use list
			//                op.Uses.Remove(phi);	// FIXME PG - ???

			//                if (!Object.ReferenceEquals(res, op)) {
			//                    if (op.Definitions.Count == 1 && op.Uses.Count == 0) {
			//                         Replace the operand, as it is only defined but never used again
			//                        op.Replace(res);
			//                    }
			//                    else {
			//                        List<LegacyInstruction> insts = phi.Blocks[i].Instructions;
			//                        int insIdx = insts.Count - 1;
			//                        Context insts = new Context(InstructionSet, phi.Blocks);

			//                        /* If there's a use, insert the move right after the last use
			//                         * this really helps the register allocator as it keeps the lifetime
			//                         * of the temporary short.
			//                         */
			//                        if (op.Uses.Count != 0) {
			//                             FIXME: Depends on sortable instruction offsets, we really need a custom collection here
			//                            op.Uses.Sort(delegate(LegacyInstruction a, LegacyInstruction b)
			//                            {
			//                                return (a.Offset - b.Offset);
			//                            });

			//                            insIdx = insts.IndexOf(op.Uses[op.Uses.Count - 1]) + 1;
			//                        }


			//                         Make sure we're inserting at a valid position
			//                        if (insIdx == -1)
			//                            insIdx = 0;

			//                        LegacyInstruction move = arch.CreateInstruction(typeof(IR.MoveInstruction), res, op);
			//                        insts.Insert(insIdx, move);
			//                    }
			//                }
			//            }

			//            /* HACK: Hide the PHI instruction.
			//             * 
			//             * We're not removing the PHI instruction as it may still be valuable to calculate
			//             * live ranges in later stages (e.g. register allocation) in those cases, the PHI
			//             * function causes the live range to be virtually "extended".
			//             * 
			//             */
			//            phi.Ignore = true;

			//             HACK: Remove phi From the operand def list
			//            res.Definitions.Remove(phi);

			//            ctx.GotoNext();
			//        }


			//    }
			//}
		}

		#endregion // IMethodCompilerStage Members
	}
}
