/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System.Collections;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// The FlowGraph Visualization Stage emits flowgraphs for graphviz.
	/// </summary>
	public class FlowGraphVisualizationStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		protected IArchitecture arch;
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
		static protected Dictionary<string, int> methodCount = new Dictionary<string, int>();
		/// <summary>
		/// 
		/// </summary>
		System.IO.StreamWriter dotFile = null;

		#endregion // Data members

		#region IMethodCompilerStage Members

		/// <summary>
		/// A reference to the running instance of this stage
		/// </summary>
		public static readonly FlowGraphVisualizationStage Instance = new FlowGraphVisualizationStage();

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			if (!methodCount.ContainsKey(methodCompiler.Method.Name))
				methodCount[methodCompiler.Method.Name] = 0;

			++methodCount[methodCompiler.Method.Name];

			// Retreive the first block
			firstBlock = FindBlock(-1);

			workList = new Stack<BasicBlock>();
			workList.Push(firstBlock);
			workArray = new BitArray(basicBlocks.Count);

			string methodName = methodCompiler.Method.Name;
			methodName = methodName.Replace("<", "");
			methodName = methodName.Replace(">", "");
			methodName = methodName.Replace("$", "");
			methodName = methodName.Replace(".", "");
			
			//IPipelineStage previousStage = methodCompiler.GetPreviousStage(typeof(IMethodCompilerStage));
			//BROKE THIS:
			IPipelineStage previousStage = methodCompiler.GetStage(typeof(IMethodCompilerStage));

			dotFile.WriteLine("subgraph cluster" + methodName + "_FlowGraph {");
			dotFile.WriteLine("label = \"Method: " + methodName + "(" + methodCompiler.Method.Signature + ") after " + previousStage.Name + "\"");
			//dotFile.WriteLine("graph [rankdir = \"TB\"];");

			string nodes = string.Empty;
			string edges = string.Empty;

			foreach (BasicBlock block in basicBlocks)
			{
				string nodeName = string.Empty;
				string nodeContent = string.Empty;
				string nextNode = string.Empty;

				nodeName = methodName + "_" + block.ToString();
				//nodeName = nodeName.Replace("-", "_");

				nodeContent += "<tr><td bgcolor=\"black\" align=\"center\" colspan=\"4\"><font face=\"Courier\" color=\"white\">L_" + block.Label.ToString("x4") + "</font></td></tr>";

				int field = 0;
				int i = 0;

				for (Context ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					if (ctx.Instruction == null)
						continue;

					string color;
					string inst = ctx.Instruction.ToString(ctx).Replace("&", "&amp;");
					inst = inst.Replace("<", "&lt;");
					inst = inst.Replace(">", "&gt;");

					if (inst.StartsWith("IL") || inst.StartsWith("T_"))
						color = "#0000ff5f";
					else if (inst.StartsWith("IR"))
						color = "#ff00005f";
					else
						color = "#CFD6CEff";


					nodeContent += "<tr height=\"20\"><td bgcolor=\"white\" align=\"right\" width=\"20\"><img src=\"icon.png\"/></td><td bgcolor=\"white\" align=\"right\">" + (i++) + "</td><td bgcolor=\"" + color + "\" align=\"center\" colspan=\"2\"><font face=\"Courier\">" + inst + "</font></td></tr>";

					++field;
				}

				if (nodeContent != string.Empty && nodeContent[nodeContent.Length - 1] == '|')
					nodeContent = nodeContent.Substring(0, nodeContent.Length - 2);

				if (nodeContent != string.Empty)
					nodes += "\"" + nodeName + "\" [label = <<table border=\"1\" cellborder=\"0\" cellpadding=\"3\" bgcolor=\"white\">" + nodeContent + "</table>> shape = \"Mrecord\"];\r\n";


				foreach (BasicBlock nextBlock in block.NextBlocks)
				{
					nextNode = methodName + "_" + nextBlock.ToString();

					edges += "\"" + nodeName + "\"" + " -> " + "\"" + nextNode + "\";\r\n";
				}
			}

			dotFile.WriteLine(nodes);
			dotFile.WriteLine(edges);
			dotFile.WriteLine("};");
		}

		/// <summary>
		/// 
		/// </summary>
		public void Open()
		{
			try
			{
				//dotFile = new System.IO.StreamWriter("dotGraph_" + compiler.Method.Name + "_" + methodCount[compiler.Method.Name] + ".dot");
				dotFile = new System.IO.StreamWriter("dotGraph.dot");
				dotFile.WriteLine("digraph \"\" {");
				dotFile.WriteLine("label = \"" + methodCompiler.Assembly.Name + "\"");
			}
			catch (System.Exception)
			{
				return;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Close()
		{
			dotFile.WriteLine("}");
			dotFile.Close();
		}

		#endregion // Methods
	}
}
