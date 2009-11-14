/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

using IR = Mosa.Runtime.CompilerFramework.IR;
using CIL = Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SimplePeepholeOptimizationStage : BaseTransformationStage, IMethodCompilerStage, IPlatformTransformationStage, IPipelineStage, IBlockOptimizationStage
    {

        #region IMethodCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        string IPipelineStage.Name { get { return @"X86.SimplePeepholeOptimizationStage"; } }

        private static PipelineStageOrder[] _pipelineOrder = new PipelineStageOrder[] {
				new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(IBlockReorderStage)),
				new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(MemToMemConversionStage)),
				new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(CodeGenerationStage))
			};

        /// <summary>
        /// Gets the pipeline stage order.
        /// </summary>
        /// <value>The pipeline stage order.</value>
        PipelineStageOrder[] IPipelineStage.PipelineStageOrder { get { return _pipelineOrder; } }

        #endregion // IMethodCompilerStage Members

        #region Window Class

        /// <summary>
        /// Window Class
        /// </summary>
        public class Window
        {
            private int _size;
            private Context[] _history;
            private int _count;

            /// <summary>
            /// Initializes a new instance of the <see cref="Window"/> class.
            /// </summary>
            /// <param name="size">The size.</param>
            public Window(int size)
            {
                _size = size;
                _history = new Context[size];
                _count = 0;
            }

            /// <summary>
            /// Nexts the specified CTX.
            /// </summary>
            /// <param name="ctx">The CTX.</param>
            public void Next(Context ctx)
            {
                for (int i = Math.Min(_count, _size) - 1; i > 0; i--)
                    _history[i] = _history[i - 1];

                _history[0] = ctx.Clone();
                if (_count < _size)
                    _count++;
            }

            /// <summary>
            /// Deletes the current.
            /// </summary>
            public void DeleteCurrent()
            {
                _history[0].Remove();
                _count--;
                for (int i = 0; i < _count; i++)
                    _history[i] = _history[i + 1];
            }

            /// <summary>
            /// Deletes the previous.
            /// </summary>
            public void DeletePrevious()
            {
                _history[1].Remove();
                _count--;
                for (int i = 1; i < _count; i++)
                    _history[i] = _history[i + 1];
            }

            /// <summary>
            /// Deletes the previous previous.
            /// </summary>
            public void DeletePreviousPrevious()
            {
                _history[2].Remove();
                _count--;
                for (int i = 2; i < _count; i++)
                    _history[i] = _history[i + 1];
            }

            /// <summary>
            /// Gets the current.
            /// </summary>
            /// <value>The current.</value>
            public Context Current
            {
                get
                {
                    if (_count == 0)
                        return null;
                    else
                        return _history[0];
                }
            }

            /// <summary>
            /// Gets the previous.
            /// </summary>
            /// <value>The previous.</value>
            public Context Previous
            {
                get
                {
                    if (_count < 2)
                        return null;
                    else
                        return _history[1];
                }
            }

            /// <summary>
            /// Gets the previous previous.
            /// </summary>
            /// <value>The previous previous.</value>
            public Context PreviousPrevious
            {
                get
                {
                    if (_count < 3)
                        return null;
                    else
                        return _history[2];
                }
            }
        }

        #endregion  // Windows Class

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        public override void Run()
        {
            Window window = new Window(5);

            foreach (BasicBlock block in BasicBlocks)
                for (Context ctx = CreateContext(block); !ctx.EndOfInstruction; ctx.GotoNext())
                    if (ctx.Instruction != null && !ctx.Ignore)
                    {
                        window.Next(ctx);

                        RemoveMultipleStores(window);
                        RemoveSingleLineJump(window);
                        ImproveBranchAndJump(window);
                    }
        }

        /// <summary>
        /// Remove multiple occuring stores, for e.g. before:
        /// <code>
        /// mov eax, operand
        /// mov operand, eax
        /// </code>
        /// after:
        /// <code>
        /// mov eax, operand
        /// </code>
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns>True if an instruction has been removed</returns>
        private bool RemoveMultipleStores(Window window)
        {
            if (window.Current == null || window.Previous == null)
                return false;

            if (window.Current.BasicBlock != window.Previous.BasicBlock)
                return false;

            if (window.Current.Instruction is CPUx86.MovInstruction && window.Previous.Instruction is CPUx86.MovInstruction)
            {
                if (window.Previous.Result == window.Current.Operand1 && window.Previous.Operand1 == window.Current.Result)
                {
                    window.DeleteCurrent();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the single line jump.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns></returns>
        private bool RemoveSingleLineJump(Window window)
        {
            if (window.Current == null || window.Previous == null)
                return false;

            if (window.Previous.Instruction is CPUx86.JmpInstruction)
                if (window.Current.BasicBlock != window.Previous.BasicBlock)	// should always be true
                    if (window.Previous.Branch.Targets[0] == window.Current.BasicBlock.Label)
                    {
                        window.DeletePrevious();
                        return true;
                    }

            return false;
        }

        /// <summary>
        /// Removes the single line jump.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns></returns>
        private bool ImproveBranchAndJump(Window window)
        {
            if (window.Current == null || window.Previous == null || window.PreviousPrevious == null)
                return false;

            if (window.Previous.Instruction is CPUx86.JmpInstruction)
                if (window.PreviousPrevious.Instruction is CPUx86.BranchInstruction)
                    if (window.Previous.BasicBlock == window.PreviousPrevious.BasicBlock)
                        if (window.Current.BasicBlock != window.Previous.BasicBlock)
                            if (window.PreviousPrevious.Branch.Targets[0] == window.Current.BasicBlock.Label)
                            {
                                Debug.Assert(window.PreviousPrevious.Branch.Targets.Length == 1);

                                // Negate branch condition
                                window.PreviousPrevious.ConditionCode = GetOppositeConditionCode(window.PreviousPrevious.ConditionCode);

                                // Change branch target
                                window.PreviousPrevious.Branch.Targets[0] = window.Previous.Branch.Targets[0];

                                // Delete jump
                                window.DeletePrevious();

                                return true;
                            }

            return false;
        }
    }
}
