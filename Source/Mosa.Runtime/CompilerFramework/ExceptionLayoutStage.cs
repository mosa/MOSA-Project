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

using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;

using CIL = Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
	public sealed class ExceptionLayoutStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		#region Data members

		//FIXME: Assumes LittleEndian architecture 
		private static readonly DataConverter LittleEndianBitConverter = DataConverter.LittleEndian;

		private List<ExceptionClauseNode> sortedClauses;

		private Dictionary<BasicBlock, ExceptionClause> blockExceptions;
		private Dictionary<int, ExceptionClause> exceptionLabelMap;

		private Dictionary<ExceptionClause, List<BasicBlock>> exceptionBlocks = new Dictionary<ExceptionClause, List<BasicBlock>>();

		private ICodeEmitter codeEmitter;

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
			codeEmitter = methodCompiler.Pipeline.FindFirst<CodeGenerationStage>().CodeEmitter;

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

		private struct ExceptionEntry
		{
			public int Start;
			public int Length;
			public int Handler;
			public int Filter;

			public int End { get { return Start + Length - 1; } }

			public ExceptionEntry(int start, int length, int handler, int filter)
			{
				Start = start;
				Length = length;
				Handler = handler;
				Filter = filter;
			}

		}

		static private void Write(ExceptionEntry clause, Stream stream)
		{
			//FIXME:
			WriteLittleEndian4(stream, clause.Start);
			WriteLittleEndian4(stream, clause.Length);
			WriteLittleEndian4(stream, clause.Handler);
			WriteLittleEndian4(stream, clause.Filter);
		}

		static private void WriteLittleEndian4(Stream stream, int value)
		{
			byte[] bytes = LittleEndianBitConverter.GetBytes(value);
			stream.Write(bytes, 0, bytes.Length);
		}

		private void EmitExceptionTable()
		{
			List<ExceptionEntry> entries = new List<ExceptionEntry>();

			foreach (ExceptionClauseNode node in sortedClauses)
			{
				ExceptionClause clause = node.Clause;

				List<BasicBlock> blocks = this.exceptionBlocks[clause];

				ExceptionEntry prev = new ExceptionEntry();

				foreach (BasicBlock block in blocks)
				{
					int start = (int)codeEmitter.GetPosition(block.Label);
					int length = (int)codeEmitter.GetPosition(block.Label + 0x0F000000) - start;
					int handler = (int)codeEmitter.GetPosition(clause.TryOffset);
					int filter = (int)codeEmitter.GetPosition(clause.FilterOffset);

					if (prev.End + 1 == start && prev.Handler == handler && prev.Filter == filter)
					{
						// merge protected blocks sequence
						prev.Length = prev.Length + (int)codeEmitter.GetPosition(block.Label + 0x0F000000) - start;
					}
					else
					{
						// new protection block sequence
						ExceptionEntry entry = new ExceptionEntry(start, length, handler, filter);
						entries.Add(entry);
						prev = entry;
					}
				}
			}

			int tableSize = (entries.Count * nativePointerSize * 4) + nativePointerSize;

			using (Stream stream = methodCompiler.Linker.Allocate(this.methodCompiler.Method.FullName + @"$etable", SectionKind.Text, tableSize, nativePointerAlignment))
			{
				foreach (ExceptionEntry entry in entries)
				{
					Write(entry, stream);
				}

				stream.Write(new Byte[nativePointerSize], 0, nativePointerSize);
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
