// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages;

public sealed class ProtectedRegionLayoutStage : BaseMethodCompilerStage
{
	#region Data Members

	private PatchType NativePatchType;

	#endregion Data Members

	protected override void Initialize()
	{
		if (TypeLayout.NativePointerSize == 4)
			NativePatchType = PatchType.I32;
		else
			NativePatchType = PatchType.I64;
	}

	protected override void Run()
	{
		if (!MethodCompiler.HasCILStream)
			return;

		if (MethodCompiler.IsMethodPlugged)
			return;

		if (!HasProtectedRegions)
			return;

		ProtectedRegion.FinalizeAll(BasicBlocks, MethodCompiler.ProtectedRegions);

		EmitProtectedRegionTable();
	}

	private void EmitProtectedRegionTable()
	{
		var trace = CreateTraceLog("Regions");

		var protectedRegionTableSymbol = Linker.DefineSymbol(Metadata.ProtectedRegionTable + Method.FullName, SectionKind.ROData, Architecture.NativeAlignment, 0);
		var writer = new BinaryWriter(protectedRegionTableSymbol.Stream);

		var sectioncount = 0;

		// 1. Number of Regions (filled in later)
		writer.Write((uint)0);

		foreach (var region in MethodCompiler.ProtectedRegions)
		{
			var handler = (uint)MethodCompiler.GetPosition(region.Handler.HandlerStart);

			trace?.Log($"Handler: {region.Handler.TryStart:X4} to {region.Handler.TryEnd:X4} Handler: {region.Handler.HandlerStart:X4} Offset: [{handler:X4}]");

			var sections = new List<Tuple<int, int>>();

			foreach (var block in region.HandlerBlocks)
			{
				// Check if block continues to exist
				if (!BasicBlocks.Contains(block))
					continue;

				var start = MethodCompiler.GetPosition(block.Label);
				var end = MethodCompiler.GetPosition(block.Label + 0x0F000000);

				trace?.Log($"   Block: {block} [{start}-{end}]");

				AddSection(sections, start, end);
			}

			foreach (var s in sections)
			{
				var start = s.Item1;
				var end = s.Item2;

				sectioncount++;

				var name = $"{Metadata.ProtectedRegionTable}{Method.FullName}${sectioncount}";
				var protectedRegionDefinition = CreateProtectedRegionDefinition(name, (uint)start, (uint)end, handler, region.Handler.ExceptionHandlerType, region.Handler.Type);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, protectedRegionTableSymbol, writer.GetPosition(), protectedRegionDefinition, 0);
				writer.WriteZeroBytes(TypeLayout.NativePointerSize);

				trace?.Log($"     Section: [{start}-{end}]");
			}
		}

		// 1. Number of Regions (now put the real number)
		writer.SetPosition(0);
		writer.Write(sectioncount);
	}

	private LinkerSymbol CreateProtectedRegionDefinition(string name, uint start, uint end, uint handler, ExceptionHandlerType handlerType, MosaType exceptionType)
	{
		// Emit parameter table
		var protectedRegionDefinitionSymbol = Linker.DefineSymbol(name, SectionKind.ROData, 0/*TypeLayout.NativePointerAlignment*/, 0);
		var writer1 = new BinaryWriter(protectedRegionDefinitionSymbol.Stream);

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
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, protectedRegionDefinitionSymbol, writer1.GetPosition(), Metadata.TypeDefinition + exceptionType.FullName, 0);
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
