/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

// FIXME: Splits this class into platform dependent and independent classes. Move platform independent code into Mosa.Compiler.Framework

namespace Mosa.Platform.x86.Stages
{
	public sealed class ExceptionLayoutStage : BaseMethodCompilerStage
	{
		#region Data members

		private Dictionary<BasicBlock, MosaExceptionHandler> blockExceptions;

		private Dictionary<MosaExceptionHandler, List<BasicBlock>> exceptionBlocks = new Dictionary<MosaExceptionHandler, List<BasicBlock>>();

		private BaseCodeEmitter codeEmitter;

		#endregion Data members

		protected override void Run()
		{
			codeEmitter = MethodCompiler.Pipeline.FindFirst<CodeGenerationStage>().CodeEmitter;

			// Step 1 - Sort the exception clauses into postorder-traversal
			//PER SPEC: The exception clauses table should already be sorted
			//BuildSort();

			// Step 2 - Assign blocks to innermost exception clause
			AssignBlocksToProtectedRegions();

			// Step 3 - Emit table of PC ranges and the clause handler
			EmitProtectedBlockTable();
		}

		private void AssignBlocksToProtectedRegions()
		{
			if (!HasProtectedRegions)
				return;

			blockExceptions = new Dictionary<BasicBlock, MosaExceptionHandler>();

			foreach (var block in BasicBlocks)
			{
				var entry = FindExceptionClause(block);

				if (entry == null)
					return;

				List<BasicBlock> blocks;

				if (!exceptionBlocks.TryGetValue(entry, out blocks))
				{
					blocks = new List<BasicBlock>();
					exceptionBlocks.Add(entry, blocks);
				}

				blocks.Add(block);
				blockExceptions.Add(block, entry);
			}
		}

		private MosaExceptionHandler FindExceptionClause(BasicBlock block)
		{
			var ctx = new Context(InstructionSet, block);
			int label = ctx.Label;

			foreach (var entry in MethodCompiler.Method.ExceptionBlocks)
			{
				if (entry.IsLabelWithinTry(label))
					return entry;
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
			public MosaType Type;

			public ProtectedBlock(uint start, uint end, uint handler, ExceptionHandlerType kind, MosaType type, uint filter)
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
			var entries = new List<ProtectedBlock>();

			foreach (var entry in MethodCompiler.Method.ExceptionBlocks)
			{
				var prev = new ProtectedBlock();

				foreach (var block in exceptionBlocks[entry])
				{
					uint start = (uint)codeEmitter.GetPosition(block.Label);
					uint end = (uint)codeEmitter.GetPosition(block.Label + 0x0F000000);

					var kind = entry.HandlerType;

					uint handler = 0;
					uint filter = 0;
					var type = entry.Type;

					handler = (uint)codeEmitter.GetPosition(entry.HandlerStart);

					if (kind == ExceptionHandlerType.Filter)
					{
						filter = (uint)codeEmitter.GetPosition(entry.FilterOffset.Value);
					}

					// TODO: Optimization - Search for existing exception protected region (before or after) to merge the current block

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
						var protectedBlock = new ProtectedBlock(start, end, handler, kind, type, filter);
						entries.Add(protectedBlock);
						prev = protectedBlock;
					}
				}
			}

			int tableSize = (entries.Count * NativePointerSize * 5) + NativePointerSize;

			var section = MethodCompiler.Linker.CreateSymbol(MethodCompiler.Method.FullName + @"$etable", SectionKind.ROData, NativePointerAlignment, tableSize);
			var stream = section.Stream;

			var writer = new EndianAwareBinaryWriter(stream, Architecture.Endianness);

			foreach (var entry in entries)
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
					MethodCompiler.Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, section, (int)writer.Position, 0, entry.Type.FullName + "$mtable", SectionKind.ROData, 0);

					writer.Position += NativePointerSize;
				}
				else if (entry.Kind == ExceptionHandlerType.Filter)
				{
					// TODO: There are no plans in the short term to support filtered exception clause as C# does not use them
					writer.Position += NativePointerSize;
				}
				else
				{
					writer.Position += NativePointerSize;
				}
			}

			// Mark end of table
			writer.Position += TypeLayout.NativePointerSize;
		}
	}
}