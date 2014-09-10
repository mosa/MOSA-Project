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
using Mosa.Compiler.InternalTrace;

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

			EmitProtectedBlockTable();
		}

		private void EmitProtectedBlockTable()
		{
			var trace = CreateTrace("Regions");

			var section = MethodCompiler.Linker.CreateSymbol(MethodCompiler.Method.FullName + @"$etable", SectionKind.ROData, NativePointerAlignment, 0);
			var stream = section.Stream;

			var writer = new EndianAwareBinaryWriter(stream, Architecture.Endianness);

			foreach (var region in MethodCompiler.ProtectedRegions)
			{
				var handler = (uint)codeEmitter.GetPosition(region.Handler.HandlerStart);

				if (trace.Active)
					trace.Log("Handler: " + region.Handler.TryStart.ToString("X4") + " to " + region.Handler.TryEnd.ToString("X4") + " Handler: " + region.Handler.HandlerStart.ToString("X4") + " Offset: #" + handler.ToString("X4"));

				foreach (var block in region.IncludedBlocks)
				{
					uint start = (uint)codeEmitter.GetPosition(block.Label);
					uint end = (uint)codeEmitter.GetPosition(block.Label + 0x0F000000);

					writer.Write((uint)region.Handler.HandlerType);
					writer.Write(start);
					writer.Write(end - start);
					writer.Write(handler);

					if (trace.Active)
						trace.Log("   Block: " + block.ToString() + " Offsets: #" + start.ToString() + "-" + end.ToString() + "]");

					if (region.Handler.HandlerType == ExceptionHandlerType.Exception)
					{
						// Store method table pointer of the exception object type
						// The VES exception runtime will uses this to compare exception object types
						MethodCompiler.Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, section, (int)writer.Position, 0, region.Handler.Type.FullName + "$mtable", SectionKind.ROData, 0);

						writer.Position += NativePointerSize;
					}
					else if (region.Handler.HandlerType == ExceptionHandlerType.Filter)
					{
						// TODO: There are no plans in the short term to support filtered exception clause as C# does not use them
						writer.Position += NativePointerSize;
					}
					else
					{
						writer.Position += NativePointerSize;
					}
				}
			}
		}
	}
}