/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Runtime.CompilerFramework.IL;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// The FlowGraph Visualization Stage emits flowgraphs for graphviz.
    /// </summary>
    public class FlowGraphVisualizationStage : IMethodCompilerStage
    {
        #region Data members

        /// <summary>
        /// 
        /// </summary>
        protected IArchitecture arch;
        /// <summary>
        /// 
        /// </summary>
        private List<BasicBlock> blocks;
        /// <summary>
        /// 
        /// </summary>
        private BasicBlock firstBlock;
        /// <summary>
        /// 
        /// </summary>
        protected BitArray workArray;
        /// <summary>
        /// 
        /// </summary>
        protected Stack<BasicBlock> workList;
        /// <summary>
        /// 
        /// </summary>
        static protected Dictionary<string, int> methodCount = new Dictionary<string,int>();

        #endregion // Data members

        #region Properties

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get { return @"FlowGraph Visualization Stage"; }
        }

        #endregion // Properties

        #region IMethodCompilerStage Members

        /// <summary>
        /// A reference to the running instance of this stage
        /// </summary>
        public static readonly FlowGraphVisualizationStage Instance = new FlowGraphVisualizationStage();

        /// <summary>
        /// Runs the specified compiler.
        /// </summary>
        /// <param name="compiler">The compiler.</param>
        public void Run(IMethodCompiler compiler)
        {
            if (!methodCount.ContainsKey(compiler.Method.Name))
                methodCount[compiler.Method.Name] = 0;

            System.IO.StreamWriter dotFile;

            try
            {
                dotFile = new System.IO.StreamWriter("dotGraph_" + compiler.Method.Name + "_" + methodCount[compiler.Method.Name] + ".dot");
            }
            catch (System.Exception)
            {
                return;
            }

            ++methodCount[compiler.Method.Name];

            // Retrieve the basic block provider
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));

            if (blockProvider == null)
                throw new InvalidOperationException(@"Operand Determination stage requires basic blocks.");

            blocks = blockProvider.Blocks;

            // Retreive the first block
            firstBlock = blockProvider.FromLabel(-1);

            workList = new Stack<BasicBlock>();
            workList.Push(firstBlock);
            workArray = new BitArray(blocks.Count);

            IMethodCompilerStage previousStage = compiler.GetPreviousStage() as IMethodCompilerStage;
            dotFile.WriteLine("digraph " + compiler.Method.Name + "_" + methodCount[compiler.Method.Name] + "_FlowGraph {");
            dotFile.WriteLine("label = \"Method: " + compiler.Method.Name + "(" + compiler.Method.Signature + ") after " + previousStage.Name + "\";");
            dotFile.WriteLine("graph [rankdir = \"TB\"];");

            string nodes = string.Empty;
            string edges = string.Empty;

            while (workList.Count != 0)
            {
                BasicBlock block = workList.Pop();

                if (!workArray.Get(block.Index))
                {
                    string nodeName = string.Empty;
                    string nodeContent = string.Empty;
                    string nextNode = string.Empty;

                    nodeName = block.Index.ToString() + "_" + block.Label.ToString();
                    nodeName = nodeName.Replace("-", "_");

                    nodeContent += "<tr><td bgcolor=\"#DDDDDD\" align=\"center\" colspan=\"3\"><font face=\"Courier\">L_" + block.Label.ToString("x4") + "</font></td></tr>";

                    int field = 0;
                    for (int i = 0; i < block.Instructions.Count; i++)
                    {
                        string color;
                        Instruction instruction = block.Instructions[i];
                        string inst = instruction.ToString().Replace("&", "&amp;");
                        inst = inst.Replace("<", "&lt;");
                        inst = inst.Replace(">", "&gt;");

                        if (inst.StartsWith("IL") || inst.StartsWith("T_"))
                            color = "#0000ff5f";
                        else if (inst.StartsWith("IR"))
                            color = "#00ff005f";
                        else
                            color = "#ff00005f";

                        //nodeContent += (" <f" + field + "> " + inst + " |");

                        nodeContent += "<tr><td bgcolor=\"white\" align=\"right\">" + i + "</td><td bgcolor=\"" + color + "\" align=\"center\" colspan=\"2\"><font face=\"Courier\">" + inst + "</font></td></tr>";
                        //nodeContent = nodeContent.Replace(";", string.Empty);

                        ++field;
                    }

                    if (nodeContent != string.Empty && nodeContent[nodeContent.Length - 1] == '|')
                        nodeContent = nodeContent.Substring(0, nodeContent.Length - 2);

                    if (nodeContent != string.Empty)
                        nodes += "\"" + nodeName + "\" [label = <<table border=\"1\" cellborder=\"0\" cellpadding=\"3\" bgcolor=\"white\">" + nodeContent + "</table>> shape = \"Mrecord\"];\r\n";

                    workArray.Set(block.Index, true);

                    foreach (BasicBlock nextBlock in block.NextBlocks)
                    {
                        nextNode = nextBlock.Index.ToString() + "_" + nextBlock.Label.ToString();

                        edges += "\"" + nodeName + "\"" + " -> " + "\"" + nextNode + "\"\r\n";

                        if (!workArray.Get(nextBlock.Index))
                        {
                            workList.Push(nextBlock);
                        }
                    }
                }
            }
            dotFile.WriteLine(nodes);
            dotFile.WriteLine(edges);
            dotFile.WriteLine("};");
            dotFile.Close();
        }

        /// <summary>
        /// Adds to pipeline.
        /// </summary>
        /// <param name="pipeline">The pipeline.</param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertBefore<IL.CilToIrTransformationStage>(this);
        }

        #endregion // Methods
    }
}
