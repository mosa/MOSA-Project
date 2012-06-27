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

using Mosa.Compiler.Framework;

namespace Mosa.Tool.Compiler.Stages
{
	/// <summary>
	/// This stage just saves statistics about the code we're compiling, for example
	/// ratio of IL to IR code, number of compiled instructions, etc.
	/// </summary>
	public class InstructionStatisticsStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		/// <summary>
		/// 
		/// </summary>
		private static DateTime start;
		/// <summary>
		/// 
		/// </summary>
		private static DateTime end;

		/// <summary>
		/// Every instruction type is stored here to be able to count the number of compiled instruction types.
		/// </summary>
		private readonly Dictionary<Type, int> disjointInstructions = new Dictionary<Type, int>();

		/// <summary>
		/// Every namespace is stored here to be able to iterate over all used
		/// namespaces (IL, IR, x86, etc)
		/// </summary>
		private readonly Dictionary<string, int> namespaces = new Dictionary<string, int>();

		/// <summary>
		/// Total number of compiled instructions.
		/// </summary>
		private uint numberOfInstructions;
		/// <summary>
		/// 
		/// </summary>
		private uint numberOfMethods;

		/// <summary>
		/// Visitation method for instructions not caught by more specific visitation methods.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Visit(Context ctx)
		{
			if (ctx.IsEmpty)
				return;

			// Count disjoint instructions
			if (disjointInstructions.ContainsKey(ctx.Instruction.GetType()))
				++disjointInstructions[ctx.Instruction.GetType()];
			else
				disjointInstructions.Add(ctx.Instruction.GetType(), 1);

			// Count namespaces
			if (namespaces.ContainsKey(ctx.Instruction.GetType().Namespace))
				++namespaces[ctx.Instruction.GetType().Namespace];
			else
				namespaces.Add(ctx.Instruction.GetType().Namespace, 1);

			++numberOfInstructions;
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			numberOfMethods++;

			foreach (BasicBlock block in basicBlocks)
				for (Context ctx = CreateContext(block); !ctx.EndOfInstruction; ctx.GotoNext())
					Visit(ctx);
		}

		/// <summary>
		/// Prints the statistics.
		/// </summary>
		public void PrintStatistics()
		{
			System.IO.StreamWriter writer = System.IO.File.CreateText("statistics.txt");
			writer.WriteLine("Instruction statistics:");
			writer.WriteLine("-----------------------");
			writer.WriteLine("  - Total number of methods:\t\t\t {0}", numberOfMethods);
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
				string percentage = string.Format("{00:.00}%", (double)((double)namespaces[name] / (double)numberOfInstructions) * 100);
				writer.WriteLine("    + {0}\t: {1}\t[{2}]", n, namespaces[name], percentage.Substring(1));
			}
			writer.WriteLine();
			writer.WriteLine("Compilation time: {0}", end - start);

			writer.Close();
		}

		/// <summary>
		/// Starts this instance.
		/// </summary>
		public void Start()
		{
			start = DateTime.Now;
		}

		/// <summary>
		/// Ends this instance.
		/// </summary>
		public void End()
		{
			end = DateTime.Now;
		}
	}
}
