// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Utility.Disassembler;

namespace Mosa.Tool.Explorer.Stages;

public class DisassemblyStage : BaseMethodCompilerStage
{
	protected override void Run()
	{
		if (!MosaSettings.EmitBinary)
			return;

		TraceDisassembly();
		TracePatchRequests();
	}

	protected void TraceDisassembly()
	{
		var trace = CreateTraceLog();

		if (trace == null)
			return;

		// Create a byte array from the symbol stream
		var symbol = Linker.GetSymbol(Method.FullName);
		var stream = symbol.Stream;
		var oldPosition = stream.Position;
		var length = (int)stream.Length;
		var memory = new byte[length];

		stream.Position = 0;
		stream.Read(memory, 0, length);
		stream.Position = oldPosition;

		var disassembler = new Disassembler(Architecture.PlatformName, memory, 0);
		var instruction = disassembler.DecodeNext();

		while (instruction != null)
		{
			trace.Log(instruction.Full);
			instruction = disassembler.DecodeNext();
		}
	}

	protected void TracePatchRequests()
	{
		var trace = CreateTraceLog("Patch-Requests");

		if (trace == null)
			return;

		var symbol = Linker.GetSymbol(Method.FullName);

		foreach (var request in symbol.GetLinkRequests())
		{
			trace.Log($"{request.PatchOffset:x8} -> [{request.LinkType}] +{request.ReferenceOffset:x} [{request.ReferenceSymbol.SectionKind}] {request.ReferenceSymbol.Name}");
		}
	}
}
