using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// This method compiler stage performs constant propagation, e.g. it removes
    /// local variables in favor of constants.
    /// </summary>
    /// <remarks>
    /// Constant propagation has a couple of advantages: First of all it removes
    /// a local variable from the stack and secondly it reduces the register pressure
    /// on systems with only a small number of registers (x86).
    /// <para/>
    /// It is only safe to use this stage on an instruction stream in SSA form.
    /// </remarks>
    public sealed class ConstantPropagationStage : IMethodCompilerStage
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="ConstantPropagationStage"/>.
        /// </summary>
        public ConstantPropagationStage()
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
            get { return @"Constant Propagation"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(IMethodCompiler compiler)
        {
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            if (null == blockProvider)
                throw new InvalidOperationException(@"SSA Conversion requires basic blocks.");

            bool remove = false;

            foreach (BasicBlock block in blockProvider)
            {
                List<Instruction> instructions = block.Instructions;
                for (int i = 0; i < instructions.Count; i++)
                {
                    Instruction instruction = instructions[i];

                    if (instruction is IR.MoveInstruction || instruction is IL.StlocInstruction)
                    {
                        Operand co = instruction.Operands[0];
                        if (co is ConstantOperand)
                        {
                            Operand res = instruction.Results[0];
                            // HACK: We can't track a constant through a register, so we keep those moves
                            if (res is StackOperand)
                            {
                                Debug.Assert(1 == res.Definitions.Count, @"Operand defined multiple times. Instruction stream not in SSA form!");
                                res.Replace(co);
                                remove = true;
                            }
                        }
                    }
                    else if (instruction is IR.PhiInstruction)
                    {
                        IR.PhiInstruction phi = (IR.PhiInstruction)instruction;
                        ConstantOperand co = phi.Operands[0] as ConstantOperand;
                        if (null != co && 1 == phi.Blocks.Count)
                        {
                            // We can remove the phi, as it is only defined once
                            Operand res = instruction.Results[0];
                            // HACK: We can't track a constant through a register, so we keep those moves
                            if (false == res.IsRegister)
                            {
                                Debug.Assert(1 == res.Definitions.Count, @"Operand defined multiple times. Instruction stream not in SSA form!");
                                res.Replace(co);
                                remove = true;
                            }
                        }
                    }

                    // Shall we remove this instruction?
                    if (true == remove)
                    {
                        instructions.RemoveAt(i);
                        i--;
                        remove = false;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertAfter<EnterSSA>(this);
        }

        #endregion // IMethodCompilerStage Members
    }
}
