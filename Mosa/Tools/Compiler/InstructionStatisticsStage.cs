using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class InstructionStatisticsStage : IInstructionVisitor<int>, IMethodCompilerStage
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly InstructionStatisticsStage Instance = new InstructionStatisticsStage();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<Type, int> disjointInstructions = new Dictionary<Type,int>();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, int> namespaces = new Dictionary<string, int>();

        /// <summary>
        /// 
        /// </summary>
        private uint numberOfInstructions = 0;

        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return "InstructionStatisticsStage"; } }

        /// <summary>
        /// Visitation method for instructions not caught by more specific visitation methods.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">A visitation context argument.</param>
        public void Visit(Instruction instruction, int arg)
        {
            // Count disjoint instructions
            if (disjointInstructions.ContainsKey(instruction.GetType()))
                ++disjointInstructions[instruction.GetType()];
            else
                disjointInstructions.Add(instruction.GetType(), 1);

            // Count namespaces
            if (namespaces.ContainsKey(instruction.GetType().Namespace))
                ++namespaces[instruction.GetType().Namespace];
            else
                namespaces.Add(instruction.GetType().Namespace, 1);

            ++numberOfInstructions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertAfter<IL.CilToIrTransformationStage>(this);
            pipeline.InsertAfter<IPlatformTransformationStage>(this);
            pipeline.InsertBefore<CodeGenerationStage<int>>(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compiler"></param>
        public void Run(MethodCompilerBase compiler)
        {
            if (null == compiler)
                throw new ArgumentNullException(@"compiler");
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            if (null == blockProvider)
                throw new InvalidOperationException(@"Instruction stream must have been split to basic blocks.");

            CodeTransformationStage.Context ctx = new CodeTransformationStage.Context();
            for (int currentBlock = 0; currentBlock < blockProvider.Blocks.Count; currentBlock++)
            {
                BasicBlock block = blockProvider.Blocks[currentBlock];
                ctx.Block = block;
                for (ctx.Index = 0; ctx.Index < block.Instructions.Count; ctx.Index++)
                {
                    block.Instructions[ctx.Index].Visit(this, 0);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void PrintStatistics()
        {
            System.IO.StreamWriter writer = System.IO.File.CreateText("statistics.txt");
            writer.WriteLine("Instruction statistics:");
            writer.WriteLine("-----------------------");
            writer.WriteLine("  - Total number of instructions:\t {0}", numberOfInstructions);
            writer.WriteLine("  - Number of disjoint instructions:\t {0}", disjointInstructions.Count);
            writer.WriteLine();
            writer.WriteLine("Namespace statistics:");
            writer.WriteLine("---------------------");
            writer.WriteLine("  - Number of instructions visited in namespace:");
            foreach (string name in namespaces.Keys)
            {
                string n = name.Substring(name.LastIndexOf('.') + 1, name.Length - name.LastIndexOf('.') - 1);
                writer.WriteLine("    + {0}\t: {1}", n, namespaces[name]);
            }


            writer.Close();
        }
    }
}
