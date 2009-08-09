using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// This stage just saves statistics about the code we're compiling, for example
    /// ratio of IL to IR code, number of compiled instructions, etc.
    /// </summary>
    public class InstructionStatisticsStage : IInstructionVisitor<int>, IMethodCompilerStage
    {
        /// <summary>
        /// A reference to the running instance of this stage
        /// </summary>
        public static readonly InstructionStatisticsStage Instance = new InstructionStatisticsStage();

        /// <summary>
        /// Every instructiontype is stored here to be able to count the number of compiled instructiontypes.
        /// </summary>
        private Dictionary<Type, int> disjointInstructions = new Dictionary<Type,int>();

        /// <summary>
        /// Every namespace is stored here to be able to iterate over all used
        /// namespaces (IL, IR, x86, etc)
        /// </summary>
        private Dictionary<string, int> namespaces = new Dictionary<string, int>();

        /// <summary>
        /// Total number of compiled instructions.
        /// </summary>
        private uint numberOfInstructions = 0;

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
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
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(IMethodCompiler compiler)
        {
            if (null == compiler)
                throw new ArgumentNullException(@"compiler");
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            if (null == blockProvider)
                throw new InvalidOperationException(@"Instruction stream must have been split to basic Blocks.");

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
            writer.WriteLine("  - Total number of instructions:\t\t\t {0}", numberOfInstructions);
            writer.WriteLine("  - Number of disjoint instructions:\t\t\t {0}", disjointInstructions.Count);
            writer.WriteLine("  - Ratio of disjoint instructions to total number:\t {0}", string.Format("{0:.00}%", ((double)disjointInstructions.Count / (double)numberOfInstructions)).Substring(1));
            writer.WriteLine();
            writer.WriteLine("Namespace statistics:");
            writer.WriteLine("---------------------");
            writer.WriteLine("  - Number of instructions visited in namespace:");
            foreach (string name in namespaces.Keys)
            {
                string n = name.Substring(name.LastIndexOf('.') + 1, name.Length - name.LastIndexOf('.') - 1);
                string percentage = string.Format("{0:.00}%", ((double)namespaces[name] / (double)numberOfInstructions));
                writer.WriteLine("    + {0}\t: {1}\t[{2}]", n, namespaces[name], percentage.Substring(1));
            }


            writer.Close();
        }
    }
}
