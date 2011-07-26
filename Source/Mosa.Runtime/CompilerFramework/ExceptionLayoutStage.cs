/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

using Mosa.Compiler.Linker;

using CIL = Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
	public sealed class ExceptionLayoutStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		#region Data members

		private List<ExceptionClauseNode> sortedClauses;

		private Dictionary<BasicBlock, ExceptionClause> blockExceptions;
		private Dictionary<int, ExceptionClause> exceptionLabelMap;

		private Dictionary<ExceptionClause, List<BasicBlock>> exceptionBlocks = new Dictionary<ExceptionClause, List<BasicBlock>>();

		#endregion // Data members

		#region IPipelineStage members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value></value>
		string IPipelineStage.Name { get { return @"ExceptionLayoutStage"; } }

		#endregion // IPipelineStage Members

		#region IMethodCompilerStage members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
			// Step 1 - Sort the exception clauses into postorder-traversal
			BuildSort();

			// Step 2 - Assign blocks to innermost exception clause
			AssignBlockstoClauses();

			// Step 3 - Emit table of PC ranges and the clause handler
			EmitExceptionTable();
		}

		#endregion // IMethodCompilerStage members

		private void AssignBlockstoClauses()
		{
			BuildExceptionLabelMap();

			blockExceptions = new Dictionary<BasicBlock, ExceptionClause>();

			foreach (BasicBlock block in basicBlocks)
			{
				ExceptionClause clause = FindExceptionClause(block);

				if (clause != null)
				{
					List<BasicBlock> blocks;

					if (!exceptionBlocks.TryGetValue(clause, out blocks))
					{
						blocks = new List<BasicBlock>();
						exceptionBlocks.Add(clause, blocks);
					}

					blocks.Add(block);
					blockExceptions.Add(block, clause);
				}
			}

		}

		private ExceptionClause FindExceptionClause(BasicBlock block)
		{
			Context ctx = new Context(instructionSet, block);
			int label = ctx.Index;

			foreach (ExceptionClauseNode node in sortedClauses)
			{
				if (label >= node.Clause.TryOffset && label < node.Clause.TryEnd)
					return node.Clause;

				//if (node.Clause.TryEnd > label)
				//    return null; // early out
			}

			return null;
		}

		private void BuildExceptionLabelMap()
		{
			exceptionLabelMap = new Dictionary<int, ExceptionClause>();

			foreach (ExceptionClause clause in methodCompiler.ExceptionClauseHeader.Clauses)
				exceptionLabelMap.Add(clause.TryOffset, clause);
		}

		private void EmitExceptionTable()
		{
			if (exceptionBlocks.Count == 0)
				return;

			// TODO:

			List<string> entries = new List<string>();

			foreach (ExceptionClauseNode node in sortedClauses)
			{
				ExceptionClause clause = node.Clause;

				List<BasicBlock> blocks = this.exceptionBlocks[clause];

				foreach (BasicBlock block in blocks)
				{

					// 1. Start 
					// 2. End
					// 3. Handler

				}
			}

			int tableSize = entries.Count * nativePointerSize;

			using (Stream stream = methodCompiler.Linker.Allocate(this.methodCompiler.Method.FullName + @"$etable", SectionKind.Text, tableSize, nativePointerAlignment))
			{
				stream.Position = tableSize;
			}

		}

		#region Postorder-traversal Sort

		private class ExceptionClauseNode : IComparable<ExceptionClauseNode>
		{
			public ExceptionClause Clause { get; private set; }

			public int Start { get { return Clause.TryOffset; } }
			public int Length { get { return Clause.TryLength; } }

			public ExceptionClauseNode Parent;
			public List<ExceptionClauseNode> Children;

			public ExceptionClauseNode(ExceptionClause clause)
			{
				Clause = clause;
			}

			public bool IsInside(ExceptionClauseNode pair)
			{
				return this.Start > pair.Start && (this.Start + this.Length < pair.Start + pair.Length);
			}

			public int CompareTo(ExceptionClauseNode other)
			{
				return this.Start.CompareTo(other.Start);
			}

		}

		private void BuildSort()
		{
			sortedClauses = new List<ExceptionClauseNode>(methodCompiler.ExceptionClauseHeader.Clauses.Count);

			foreach (ExceptionClause clause in methodCompiler.ExceptionClauseHeader.Clauses)
				sortedClauses.Add(new ExceptionClauseNode(clause));
		}

		private List<ExceptionClauseNode> Sort(List<ExceptionClauseNode> listToSort)
		{
			var result = new List<ExceptionClauseNode>();
			listToSort.Sort();
			foreach (var pair in listToSort)
			{
				pair.Children = this.GetChildren(pair, listToSort);
				pair.Children.Sort();
			}

			var root = listToSort[0];
			result.Add(root);
			InsertChildren(result, root);
			return result;
		}

		private void InsertChildren(List<ExceptionClauseNode> result, ExceptionClauseNode root)
		{
			result.InsertRange(result.IndexOf(root), root.Children);
			foreach (var pair in root.Children)
				InsertChildren(result, pair);
		}

		private List<ExceptionClauseNode> GetChildren(ExceptionClauseNode pair, List<ExceptionClauseNode> list)
		{
			var children = new List<ExceptionClauseNode>();
			foreach (var other in list)
			{
				if (other.IsInside(pair))
					children.Add(other);
			}

			var result = new List<ExceptionClauseNode>(children);
			foreach (var x in children)
			{
				foreach (var y in children)
				{
					if (x.IsInside(y))
						result.Remove(x);
				}
			}

			foreach (var x in result)
				x.Parent = pair;

			return result;
		}

		#endregion

	}
}
