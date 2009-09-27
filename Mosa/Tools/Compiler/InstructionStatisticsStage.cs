using System;
using System.Collections.Generic;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// This stage just saves statistics about the code we're compiling, for example
    /// ratio of IL to IR code, number of compiled instructions, etc.
    /// </summary>
    public class InstructionStatisticsStage : IMethodCompilerStage
    {
        /// <summary>
        /// 
        /// </summary>
        private static DateTime _start;
        /// <summary>
        /// 
        /// </summary>
        private static DateTime _end;
        /// <summary>
        /// A reference to the running instance of this stage
        /// </summary>
        public static readonly InstructionStatisticsStage Instance = new InstructionStatisticsStage();

        /// <summary>
        /// Every instructiontype is stored here to be able to count the number of compiled instructiontypes.
        /// </summary>
        private readonly Dictionary<Type, int> _disjointInstructions = new Dictionary<Type,int>();

        /// <summary>
        /// Every namespace is stored here to be able to iterate over all used
        /// _namespaces (IL, IR, x86, etc)
        /// </summary>
        private readonly Dictionary<string, int> _namespaces = new Dictionary<string, int>();

        /// <summary>
        /// Total number of compiled instructions.
        /// </summary>
        private uint _numberOfInstructions;
        /// <summary>
        /// 
        /// </summary>
        private uint _numberOfMethods;

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
        public void Visit(LegacyInstruction instruction, int arg)
        {
            // Count disjoint instructions
            if (_disjointInstructions.ContainsKey(instruction.GetType()))
                ++_disjointInstructions[instruction.GetType()];
            else
                _disjointInstructions.Add(instruction.GetType(), 1);

            // Count _namespaces
            if (_namespaces.ContainsKey(instruction.GetType().Namespace))
                ++_namespaces[instruction.GetType().Namespace];
            else
                _namespaces.Add(instruction.GetType().Namespace, 1);

            ++_numberOfInstructions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertAfter<CIL.CilToIrTransformationStage>(this);
            pipeline.InsertAfter<IPlatformTransformationStage>(this);
            pipeline.InsertBefore<CodeGenerationStage>(this);
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

            ++_numberOfMethods;

            for (int currentBlock = 0; currentBlock < blockProvider.Blocks.Count; currentBlock++)
            {
                BasicBlock block = blockProvider.Blocks[currentBlock];
				Context ctx = new Context(block);
                for (ctx.Index = 0; ctx.Index < block.Instructions.Count; ctx.Index++)
                {
                    Visit(block.Instructions[ctx.Index], 0);
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
            writer.WriteLine("  - Total number of methods:\t\t\t {0}", _numberOfMethods);
            writer.WriteLine("  - Total number of instructions:\t\t\t {0}", _numberOfInstructions);
            writer.WriteLine("  - Number of disjoint instructions:\t\t\t {0}", _disjointInstructions.Count);
            writer.WriteLine("  - Ratio of disjoint instructions to total number:\t {0}", string.Format("{0:.00}%", ((double)_disjointInstructions.Count / (double)_numberOfInstructions)).Substring(1));
            writer.WriteLine();
            writer.WriteLine("Namespace statistics:");
            writer.WriteLine("---------------------");
            writer.WriteLine("  - Number of instructions visited in namespace:");
            foreach (string name in _namespaces.Keys)
            {
                string n = name.Substring(name.LastIndexOf('.') + 1, name.Length - name.LastIndexOf('.') - 1);
                string percentage = string.Format("{00:.00}%", (double)((double)_namespaces[name] / (double)_numberOfInstructions) * 100);
                writer.WriteLine("    + {0}\t: {1}\t[{2}]", n, _namespaces[name], percentage.Substring(1));
            }
            writer.WriteLine();
            writer.WriteLine("Compilation time: {0}", _end - _start);


            writer.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            _start = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        public void End()
        {
            _end = DateTime.Now;
        }
    }
}
