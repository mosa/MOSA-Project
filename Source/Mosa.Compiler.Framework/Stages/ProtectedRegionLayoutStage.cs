// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System;

namespace Mosa.Compiler.Framework.Stages
{
	public sealed class ProtectedRegionLayoutStage : BaseMethodCompilerStage
	{
		#region Data members

		private BaseCodeEmitter codeEmitter;
		private PatchType NativePatchType;

		#endregion Data members

		protected override void Run()
		{
			if (!HasProtectedRegions)
				return;

			ProtectedRegion.FinalizeAll(BasicBlocks, MethodCompiler.ProtectedRegions);

			codeEmitter = MethodCompiler.Pipeline.FindFirst<CodeGenerationStage>().CodeEmitter;

			if (TypeLayout.NativePointerSize == 4)
				NativePatchType = BuiltInPatch.I4;
			else
				NativePatchType = BuiltInPatch.I8;

			EmitProtectedRegionTable();
		}

		private void EmitProtectedRegionTable()
		{
			var trace = CreateTraceLog("Regions");

			var protectedRegionTableSymbol = MethodCompiler.Linker.CreateSymbol(MethodCompiler.Method.FullName + Metadata.ProtectedRegionTable, SectionKind.ROData, NativeAlignment, 0);
			var writer = new EndianAwareBinaryWriter(protectedRegionTableSymbol.Stream, Architecture.Endianness);

			int sectioncount = 0;

			// 1. Number of Regions (filled in later)
			writer.Write((uint)0);

			foreach (var region in MethodCompiler.ProtectedRegions)
			{
				var handler = (uint)codeEmitter.GetPosition(region.Handler.HandlerStart);

				if (trace.Active)
					trace.Log("Handler: " + region.Handler.TryStart.ToString("X4") + " to " + region.Handler.TryEnd.ToString("X4") + " Handler: " + region.Handler.HandlerStart.ToString("X4") + " Offset: [" + handler.ToString("X4") + "]");

				List<Tuple<int, int>> sections = new List<Tuple<int, int>>();

				foreach (var block in region.IncludedBlocks)
				{
					// Check if block continues to exist
					if (!BasicBlocks.Contains(block))
						continue;

					int start = codeEmitter.GetPosition(block.Label);
					int end = codeEmitter.GetPosition(block.Label + 0x0F000000);

					if (trace.Active)
						trace.Log("   Block: " + block.ToString() + " [" + start.ToString() + "-" + end.ToString() + "]");

					AddSection(sections, start, end);
				}

				foreach (var s in sections)
				{
					int start = s.Item1;
					int end = s.Item2;

					sectioncount++;

					var name = MethodCompiler.Method.FullName + Metadata.ProtectedRegionTable + "$" + sectioncount.ToString();
					var protectedRegionDefinition = CreateProtectedRegionDefinition(name, (uint)start, (uint)end, handler, region.Handler.ExceptionHandlerType, region.Handler.Type);
					MethodCompiler.Linker.Link(LinkType.AbsoluteAddress, NativePatchType, protectedRegionTableSymbol, (int)writer.Position, 0, protectedRegionDefinition, 0);
					writer.WriteZeroBytes(TypeLayout.NativePointerSize);

					if (trace.Active)
						trace.Log("     Section: [" + start.ToString() + "-" + end.ToString() + "]");
				}
			}

			// 1. Number of Regions (now put the real number)
			writer.Position = 0;
			writer.Write(sectioncount);
		}

		private LinkerSymbol CreateProtectedRegionDefinition(string name, uint start, uint end, uint handler, ExceptionHandlerType handlerType, MosaType exceptionType)
		{
			// Emit parameter table
			var protectedRegionDefinitionSymbol = MethodCompiler.Linker.CreateSymbol(name, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(protectedRegionDefinitionSymbol.Stream, Architecture.Endianness);

			// 1. Offset to start
			writer1.Write(start, TypeLayout.NativePointerSize);

			// 2. Offset to end
			writer1.Write(end, TypeLayout.NativePointerSize);

			// 3. Offset to handler
			writer1.Write(handler, TypeLayout.NativePointerSize);

			// 4. Handler type
			writer1.Write((uint)handlerType, TypeLayout.NativePointerSize);

			// 5. Exception object type
			if (handlerType == ExceptionHandlerType.Exception)
			{
				// Store method table pointer of the exception object type
				// The VES exception runtime will uses this to compare exception object types
				MethodCompiler.Linker.Link(LinkType.AbsoluteAddress, NativePatchType, protectedRegionDefinitionSymbol, (int)writer1.Position, 0, exceptionType.FullName + Metadata.TypeDefinition, SectionKind.ROData, 0);
			}
			else if (handlerType == ExceptionHandlerType.Filter)
			{
				// TODO: There are no plans in the short term to support filtered exception clause as C# does not use them
			}
			else
			{
			}

			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// Return protectedRegionSymbol for linker usage
			return protectedRegionDefinitionSymbol;
		}

		private Tuple<int, int> FindConnectingSection(List<Tuple<int, int>> sections, int start, int end)
		{
			foreach (var section in sections)
			{
				if (section.Item1 == end || section.Item2 == start)
				{
					return section;
				}
			}

			return null;
		}

		private void AddSection(List<Tuple<int, int>> sections, int start, int end)
		{
			while (true)
			{
				var find = FindConnectingSection(sections, start, end);

				if (find == null)
				{
					sections.Add(new Tuple<int, int>(start, end));
					return;
				}

				sections.Remove(find);

				if (find.Item1 == end)
				{
					end = find.Item2;
				}
				else if (find.Item2 == start)
				{
					start = find.Item1;
				}
			}
		}
	}
}
