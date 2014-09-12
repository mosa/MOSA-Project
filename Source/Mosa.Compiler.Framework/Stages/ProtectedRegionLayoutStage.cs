/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	public sealed class ProtectedRegionLayoutStage : BaseMethodCompilerStage
	{
		#region Data members

		private BaseCodeEmitter codeEmitter;

		#endregion Data members

		protected override void Run()
		{
			if (!HasProtectedRegions)
				return;

			ProtectedRegion.FinalizeAll(BasicBlocks, MethodCompiler.ProtectedRegions);

			codeEmitter = MethodCompiler.Pipeline.FindFirst<CodeGenerationStage>().CodeEmitter;

			EmitProtectedRegionTable();
		}

		private void EmitProtectedRegionTable()
		{
			var trace = CreateTrace("Regions");

			var section = MethodCompiler.Linker.CreateSymbol(MethodCompiler.Method.FullName + Metadata.ProtectedRegionTable, SectionKind.ROData, NativePointerAlignment, 0);
			var stream = section.Stream;

			var writer = new EndianAwareBinaryWriter(stream, Architecture.Endianness);

			foreach (var region in MethodCompiler.ProtectedRegions)
			{
				var handler = (uint)codeEmitter.GetPosition(region.Handler.HandlerStart);

				if (trace.Active)
					trace.Log("Handler: " + region.Handler.TryStart.ToString("X4") + " to " + region.Handler.TryEnd.ToString("X4") + " Handler: " + region.Handler.HandlerStart.ToString("X4") + " Offset: [" + handler.ToString("X4") + "]");

				List<Tuple<int, int>> sections = new List<Tuple<int, int>>();

				foreach (var block in region.IncludedBlocks)
				{
					int start = codeEmitter.GetPosition(block.Label);
					int end = codeEmitter.GetPosition(block.Label + 0x0F000000);

					if (trace.Active)
						trace.Log("   Block: " + block.ToString() + " [" + start.ToString() + "-" + end.ToString() + "]");

					AddSection(sections, start, end);
				}

				writer.Write(sections.Count);

				foreach (var s in sections)
				{
					int start = s.Item1;
					int end = s.Item2;

					writer.Write((uint)region.Handler.HandlerType);

					if (NativePointerSize == 4)
					{
						writer.Write((uint)start);
						writer.Write((uint)(end - start));
						writer.Write((uint)handler);
					}
					else
					{
						writer.Write((ulong)start);
						writer.Write((uint)(end - start));
						writer.Write((ulong)handler);
					}

					if (trace.Active)
						trace.Log("     Section: [" + start.ToString() + "-" + end.ToString() + "]");

					if (region.Handler.HandlerType == ExceptionHandlerType.Exception)
					{
						// Store method table pointer of the exception object type
						// The VES exception runtime will uses this to compare exception object types
						MethodCompiler.Linker.Link(LinkType.AbsoluteAddress, (NativePointerSize == 4) ? BuiltInPatch.I4 : BuiltInPatch.I8, section, (int)writer.Position, 0, region.Handler.Type.FullName + Metadata.TypeDefinition, SectionKind.ROData, 0);
					}
					else if (region.Handler.HandlerType == ExceptionHandlerType.Filter)
					{
						// TODO: There are no plans in the short term to support filtered exception clause as C# does not use them
					}
					else
					{
					}

					writer.WriteZeroBytes(NativePointerSize);
				}
			}
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