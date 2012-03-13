/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.IO;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.TypeSystem;

// FIXME: Splits this class into platform dependent and independent classes. Move platform independent code into Mosa.Compiler.Framework

namespace Mosa.Platform.x86
{
	public sealed class ExceptionLayoutStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		#region Data members

		private Dictionary<BasicBlock, ExceptionHandlingClause> blockExceptions;

		private Dictionary<ExceptionHandlingClause, List<BasicBlock>> exceptionBlocks = new Dictionary<ExceptionHandlingClause, List<BasicBlock>>();

		private ICodeEmitter codeEmitter;

		#endregion // Data members

		#region IMethodCompilerStage members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			codeEmitter = methodCompiler.Pipeline.FindFirst<CodeGenerationStage>().CodeEmitter;

			// Step 1 - Sort the exception clauses into postorder-traversal
			//PER SPEC: The exception clauses table should already be sorted
			//BuildSort();

			// Step 2 - Assign blocks to innermost exception clause
			AssignBlocksToClauses();

			// Step 3 - Emit table of PC ranges and the clause handler
			EmitProtectedBlockTable();
		}

		#endregion // IMethodCompilerStage members

		private void AssignBlocksToClauses()
		{
			if (methodCompiler.ExceptionClauseHeader.Clauses.Count == 0)
				return;

			blockExceptions = new Dictionary<BasicBlock, ExceptionHandlingClause>();

			foreach (BasicBlock block in basicBlocks)
			{
				ExceptionHandlingClause clause = FindExceptionClause(block);

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

		private ExceptionHandlingClause FindExceptionClause(BasicBlock block)
		{
			Context ctx = new Context(instructionSet, block);
			int label = ctx.Label;

			foreach (ExceptionHandlingClause clause in methodCompiler.ExceptionClauseHeader.Clauses)
			{
				if (clause.IsLabelWithinTry(label))
					return clause;

				//if (clause.IsLabelWithinHandler(label))
				//    return null;
			}

			return null;
		}

		private struct ProtectedBlock
		{
			public ExceptionHandlerType Kind;
			public uint Start;
			public uint End;
			public uint Length { get { return End - Start; } }
			public uint Handler;

			public uint Filter;
			public RuntimeType Type;

			public ProtectedBlock(uint start, uint end, uint handler, ExceptionHandlerType kind, RuntimeType type, uint filter)
			{
				Start = start;
				End = end;
				Kind = kind;
				Handler = handler;
				Type = type;
				Filter = filter;
			}

		}

		private void EmitProtectedBlockTable()
		{
			List<ProtectedBlock> entries = new List<ProtectedBlock>();

			foreach (ExceptionHandlingClause clause in methodCompiler.ExceptionClauseHeader.Clauses)
			{
				ProtectedBlock prev = new ProtectedBlock();

				foreach (BasicBlock block in exceptionBlocks[clause])
				{
					uint start = (uint)codeEmitter.GetPosition(block.Label);
					uint end = (uint)codeEmitter.GetPosition(block.Label + 0x0F000000);

					ExceptionHandlerType kind = clause.ExceptionHandler;

					uint handler = 0;
					uint filter = 0;
					RuntimeType type = null;


					handler = (uint)codeEmitter.GetPosition(clause.HandlerOffset);

					if (kind == ExceptionHandlerType.Exception)
					{
						// Get runtime type
						type = typeModule.GetType(new Token(clause.ClassToken));
					}
					else if (kind == ExceptionHandlerType.Filter)
					{
						filter = (uint)codeEmitter.GetPosition(clause.FilterOffset);
					}

					// TODO: Optimization - Search for existing exception protected region  (before or after) to merge the current block

					// Simple optimization assuming blocks are somewhat sorted by position
					if (prev.End == start && prev.Kind == kind && prev.Handler == handler && prev.Filter == filter && prev.Type == type)
					{
						// merge protected blocks sequence
						prev.End = end;
					}
					else if (prev.Start == end && prev.Kind == kind && prev.Handler == handler && prev.Filter == filter && prev.Type == type)
					{
						// merge protected blocks sequence						
						prev.Start = start;
					}
					else
					{
						// new protection block sequence
						ProtectedBlock entry = new ProtectedBlock(start, end, handler, kind, type, filter);
						entries.Add(entry);
						prev = entry;
					}
				}
			}

			int tableSize = (entries.Count * nativePointerSize * 5) + nativePointerSize;

			string section = methodCompiler.Method.FullName + @"$etable";

			using (Stream stream = methodCompiler.Linker.Allocate(section, SectionKind.ROData, tableSize, nativePointerAlignment))
			{
				using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(stream, architecture.IsLittleEndian))
				{
					foreach (ProtectedBlock entry in entries)
					{
						// FIXME: Assumes x86 platform
						writer.Write((uint)entry.Kind);
						writer.Write(entry.Start);
						writer.Write(entry.Length);
						writer.Write(entry.Handler);

						if (entry.Kind == ExceptionHandlerType.Exception)
						{
							// Store method table pointer of the exception object type
							// The VES exception runtime will uses this to compare exception object types
							methodCompiler.Linker.Link(LinkType.AbsoluteAddress | LinkType.NativeI4, section, (int)writer.Position, 0, entry.Type.FullName + "$mtable", IntPtr.Zero);

							writer.Position += nativePointerSize;
						}
						else if (entry.Kind == ExceptionHandlerType.Filter)
						{
							// TODO: There are no plans in the short term to support filtered exception clause as C# does not use them 
							writer.Position += nativePointerSize;
						}
						else
						{
							writer.Position += nativePointerSize;
						}
					}

					// Mark end of table
					writer.Position += typeLayout.NativePointerSize;
				}
			}

		}

	}
}