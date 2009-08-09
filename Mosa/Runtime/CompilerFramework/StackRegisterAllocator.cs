// -----------------------------------------------------------------------------------------------------------
// <copyright file="StackRegisterAllocator.cs" company="(C) 2008-2009 MOSA - The Managed Operating System Alliance">
//  
// (c) 2008-2009 MOSA - The Managed Operating System Alliance
// 
// Licensed under the terms of the New BSD License.
//  
// Authors:
//   Michael Ruck (mailto:sharpos@michaelruck.de)
//   
// </copyright>
// -----------------------------------------------------------------------------------------------------------
namespace Mosa.Runtime.CompilerFramework
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using IR;
    using Metadata;
    using Metadata.Signatures;

    /// <summary>
    /// Allocates the registers according to the IL stack.
    /// </summary>
    /// <remarks>
    /// This is a very simple register allocator. It simulates the IL stack using the
    /// registers and native stack of the target architecture. Right now this allocator
    /// only supports x86 and is specifically designed to use an IL stack with the following
    /// properties:
    /// <para />
    /// - The EAX register always contains the topmost stack value. This implies that
    ///   EAX is always the destination of any operation.
    /// <para />
    /// - The ECX holds the second to top stack value. This implies that most x86
    ///   instructions will be in the format EAX = EAX op ECX, which incidentally matches 
    ///   all x86 instructions, including shifts and divisions.
    /// <para />
    /// - For floating point arithmetic XMM#0 is used instead of EAX and XMM#1 is used instead of
    ///   ECX.
    /// <para />
    /// - If a value is loaded onto the evaluation stack, the following sequence of instructions
    ///   is emitted: push ecx, mov ecx, eax, [load] eax. This sequence is emitted depending on
    ///   the state of the IL stack: An empty IL stack does not cause the first two instructions
    ///   to be emitted, while a stack depth of 1 only causes the mov ecx, eax to be emitted.
    /// <para />
    /// - If a single value of the stack is used, the following sequence is emitted: 
    ///   mov eax, ecx, pop ecx. The pop ecx is only emitted, if the IL stack size is larger than 1.
    /// <para />
    /// - If the two topmost values  of the stack are used, they are replaced by pop eax, pop ecx. 
    ///   Again depending on the size of the IL stack.
    /// <para />
    /// - If the instruction pushes a result onto the stack, this result goes into EAX.
    /// <para />
    /// - EAX and ECX are not touched for floating point. They keep their last state.
    /// </remarks>
    public class StackRegisterAllocator : IMethodCompilerStage
    {
        /// <summary>
        /// Holds the number of registers used for the evaluation stack.
        /// </summary>
        private const int RegisterStackSize = 2;

        /// <summary>
        /// The evaluation stack.
        /// </summary>
        private readonly Stack<Operand> evaluationStack;

        /// <summary>
        /// An array of register operands to hold the top of the evaluation stack for fp operands.
        /// </summary>
        private readonly RegisterOperand[] stackRegistersFp;

        /// <summary>
        /// An array of register operands to hold the top of the evaluation stack for integer operands.
        /// </summary>
        private readonly RegisterOperand[] stackRegistersI;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackRegisterAllocator"/> class.
        /// </summary>
        public StackRegisterAllocator()
        {
            this.evaluationStack = new Stack<Operand>();
            this.stackRegistersI = new RegisterOperand[RegisterStackSize];
            this.stackRegistersFp = new RegisterOperand[RegisterStackSize];
        }

        /// <summary>
        /// Gets the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get { return @"StackRegisterAllocator"; }
        }

        /// <summary>
        /// Adds the stage to the pipeline.
        /// </summary>
        /// <param name="pipeline">The pipeline to add to.</param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertBefore<ICodeGenerationStage>(this);
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(IMethodCompiler compiler)
        {
            // Prepare the registers used for the evaluation stack
            this.PrepareEvaluationStack(compiler.Architecture);

            // Enumerate the basic Blocks and process the instructions in each of them
            IBasicBlockProvider blockProvider = compiler.GetPreviousStage<IBasicBlockProvider>();
            foreach (BasicBlock block in blockProvider)
            {
                foreach (Instruction instruction in block.Instructions)
                {
                    this.ProcessInstruction(instruction);
                }
            }
        }

        /// <summary>
        /// Pops the operands of an instruction from the evaluation stack.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <returns>The number of operands popped.</returns>
        private int PopOperands(Instruction instruction)
        {
            Operand[] ops = instruction.Operands;
            for (int i = ops.Length - 1; i > -1; i--)
            {
                Operand op = ops[i];
                Operand evalOp = this.evaluationStack.Pop();
                Debug.Assert(ReferenceEquals(evalOp, op), @"Operand's are not equal?");
            }

            return ops.Length;
        }

        /// <summary>
        /// Prepares the evaluation stack.
        /// </summary>
        /// <param name="architecture">The architecture.</param>
        private void PrepareEvaluationStack(IArchitecture architecture)
        {
            /*
             * This register allocator uses two sets of registers to keep
             * parts of the evaluation stack in memory. The first set is used
             * for integers and the second set for floating point values.
             * 
             */
            SigType i = new SigType(CilElementType.I);
            SigType fp = new SigType(CilElementType.R8);
            Register[] registerSet = architecture.RegisterSet;
            int iregs = 0, fpregs = 0;

            // Enumerate all registers, until we've found all needed registers
            foreach (Register reg in registerSet)
            {
                if (reg.IsFloatingPoint == false)
                {
                    // A general purpose register. Do we need to allocate a stack register?
                    if (iregs < RegisterStackSize)
                    {
                        this.stackRegistersI[iregs++] = new RegisterOperand(i, reg);
                    }
                }
                else
                {
                    // A floating point register. Do we need to allocate a stack register?
                    if (fpregs < RegisterStackSize)
                    {
                        this.stackRegistersFp[fpregs++] = new RegisterOperand(fp, reg);
                    }
                }

                // If we've allocated all registers, break the loop
                if (iregs == RegisterStackSize && fpregs == RegisterStackSize)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Processes the instruction.
        /// </summary>
        /// <param name="instruction">The instruction to process.</param>
        private void ProcessInstruction(Instruction instruction)
        {
            if (instruction is MoveInstruction)
            {
                this.evaluationStack.Push(instruction.Results[0]);
                return;
            }

            // If the instruction has operands, these are popped from the IL stack.
            int pops = this.PopOperands(instruction);

            // If an instruction has a result, it is pushed onto the evaluation stack.
            int pushes = this.PushResults(instruction, pops);

            this.SyncEvalStack(pops - pushes);
        }

        /// <summary>
        /// Pushes the results of an instruction onto the evaluation stack.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="pops">The number of pops performed.</param>
        /// <returns>The number of pushes performed.</returns>
        private int PushResults(Instruction instruction, int pops)
        {
            // Get the result operands
            Operand[] ops = instruction.Results;

            Debug.Assert(
                ops == null || ops.Length == 1, @"Not tested for more than one result. Which order should they take?");

            // Enumerate the result operands
            foreach (Operand result in ops)
            {
                // Move the result to the top of the eval stack
                this.evaluationStack.Push(result);
            }

            return ops != null ? ops.Length : 0;
        }

        /// <summary>
        /// Synchronizes the eval stack after an instruction.
        /// </summary>
        /// <param name="entries">The number of stack entries to retrieve from the processor stack.</param>
        private void SyncEvalStack(int entries)
        {
        }
    }
}