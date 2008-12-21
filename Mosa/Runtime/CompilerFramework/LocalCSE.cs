/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// A stage to compute local common subexpression elimination
    /// according to Steven S. Muchnick, Advanced Compiler Design 
    /// and Implementation (Morgan Kaufmann, 1997) pp. 378-396
    /// </summary>
    public class LocalCSE : IMethodCompilerStage
    {
        /// <summary>
        /// 
        /// </summary>
        struct AEBinExp
        {
            /// <summary>
            /// 
            /// </summary>
            public int Position;
            /// <summary>
            /// 
            /// </summary>
            public Operand Operand1;
            /// <summary>
            /// 
            /// </summary>
            public Operation Operator;
            /// <summary>
            /// 
            /// </summary>
            public Operand Operand2;
            /// <summary>
            /// 
            /// </summary>
            public Operand Var;

            /// <summary>
            /// Initializes a new instance of the <see cref="AEBinExp"/> struct.
            /// </summary>
            /// <param name="pos">The pos.</param>
            /// <param name="op1">The op1.</param>
            /// <param name="opr">The opr.</param>
            /// <param name="op2">The op2.</param>
            /// <param name="var">The var.</param>
            public AEBinExp(int pos, Operand op1, Operation opr, Operand op2, Operand var)
            {
                Position = pos;
                Operand1 = op1;
                Operator = opr;
                Operand2 = op2;
                Var = var;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        enum Operation
        {
            /// <summary>
            /// 
            /// </summary>
            Add,
            /// <summary>
            /// 
            /// </summary>
            Mul,
            /// <summary>
            /// 
            /// </summary>
            And,
            /// <summary>
            /// 
            /// </summary>
            Or,
            /// <summary>
            /// 
            /// </summary>
            Xor
        }

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get { return @"Local Common Subexpression Elimination Stage"; }
        }

        /// <summary>
        /// </summary>
        /// <param name="pipeline"></param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertAfter<ConstantFoldingStage>(this);
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(IMethodCompiler compiler)
        {
            // Retrieve the basic block provider
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            if (null == blockProvider)
                throw new InvalidOperationException(@"Instruction stream must have been split to basic blocks.");

            foreach (BasicBlock block in blockProvider.Blocks)
                EliminateCommonSubexpressions(block);
        }

        /// <summary>
        /// Eliminates the common subexpressions.
        /// </summary>
        /// <param name="block">The block.</param>
        private void EliminateCommonSubexpressions(BasicBlock block)
        {
            List<AEBinExp> AEB = new List<AEBinExp>();
            List<AEBinExp> tmp = new List<AEBinExp>();

            AEBinExp aeb = new AEBinExp();
            Instruction instruction;

            int i = 0;
            int position = 0;
            bool found = false;

            while (i <= block.Instructions.Count)
            {
                instruction = block.Instructions[i];
                found = false;
                RegisterOperand temp = null;

                if (instruction is IL.ArithmeticInstruction)
                {
                    if (instruction is IL.BinaryInstruction)
                    {
                        tmp = new List<AEBinExp>(AEB);

                        while (tmp.Count > 0)
                        {
                            aeb = tmp[0];
                            tmp.RemoveAt(0);

                            // Match current instruction's expression against those
                            // in AEB, including commutativity
                            if (IsCommutative(instruction))
                            {
                                position = aeb.Position;
                                found = true;

                                // If no variable in tuple, create a new temporary and
                                // insert an instruction evaluating the expression
                                // and assigning it to the temporary
                                if (aeb.Var == null)
                                {
                                    // new_tmp()
                                    AEB.Remove(aeb);
                                    AEB.Add(new AEBinExp(aeb.Position, aeb.Operand1, aeb.Operator, aeb.Operand2, temp));

                                    // Insert new assignment to instruction stream in block
                                    Instruction inst = null;

                                    switch (aeb.Operator)
                                    {
                                        case Operation.Add:
                                            inst = new IL.AddInstruction(Mosa.Runtime.CompilerFramework.IL.OpCode.Add, temp, aeb.Operand1, aeb.Operand2);
                                            break;
                                        case Operation.Mul:
                                            inst = new IL.MulInstruction(Mosa.Runtime.CompilerFramework.IL.OpCode.Mul, temp, aeb.Operand1, aeb.Operand2);
                                            break;
                                        default:
                                            break;
                                    }
                                    block.Instructions.Insert(position, inst);

                                    Renumber(AEB, position);

                                    ++position;
                                    ++i;

                                    // Replace current instruction by one that copies
                                    // the temporary instruction
                                    block.Instructions[position] = new IR.MoveInstruction(block.Instructions[position].Results[0], temp);
                                }
                                else
                                {
                                    temp = aeb.Var;
                                }
                                block.Instructions[i] = new IR.MoveInstruction(instruction.Results[0], temp);      
                            }
                        }

                        if (!found)
                        {
                            // Insert new tuple
                            AEB.Add(new AEBinExp(i, instruction.Operands[0], null, instruction.Operands[1], null));
                        }

                        // Remove all tuples that use the variable assigned to by
                        // the current instruction
                        tmp = new List<AEBinExp>(AEB);

                        while (tmp.Count > 0)
                        {
                            aeb = tmp[0];
                            tmp.RemoveAt(0);

                            if (instruction.Operands[0] == aeb.Operand1 || instruction.Operands[0] == aeb.Operand2)
                                AEB.Remove(aeb);
                        }
                    }
                }
                ++i;
            }
        }

        /// <summary>
        /// Renumbers the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="position">The position.</param>
        private void Renumber(List<AEBinExp> list, int position)
        {
        }

        /// <summary>
        /// Determines whether the specified instruction is commutative.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <returns>
        /// 	<c>true</c> if the specified instruction is commutative; otherwise, <c>false</c>.
        /// </returns>
        private bool IsCommutative(Instruction instruction)
        {
            return (instruction is IL.AddInstruction) ||
                   (instruction is IL.MulInstruction) ||
                   (instruction is IR.LogicalAndInstruction) ||
                   (instruction is IR.LogicalOrInstruction) ||
                   (instruction is IR.LogicalXorInstruction);
        }
    }
}
