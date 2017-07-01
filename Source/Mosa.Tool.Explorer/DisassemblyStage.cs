// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Trace;
using SharpDisasm;
using System;

namespace Mosa.Tool.Explorer
{
	public class DisassemblyStage : BaseMethodCompilerStage
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="DisassemblyStage"/> class.
		/// </summary>
		public DisassemblyStage()
		{
		}

		#endregion Construction

		protected override void Run()
		{
			TraceDisassembly();
			TracePatchRequests();
		}

		protected void TraceDisassembly()
		{
			var trace = CreateTraceLog();

			if (!trace.Active)
				return;

			// Determine the architecture mode
			ArchitectureMode mode;
			switch (this.Architecture.MachineType)
			{
				case Compiler.Linker.Elf.MachineType.Intel386:
					mode = ArchitectureMode.x86_32;
					break;

				case Compiler.Linker.Elf.MachineType.IA_64:
					mode = ArchitectureMode.x86_64;
					break;

				case Compiler.Linker.Elf.MachineType.ARM:
				default:
					trace.Log($"Unable to disassemble binary for machine type: {this.Architecture.MachineType}");
					return;
			}

			// Create a byte array from the symbol stream
			var symbol = MethodCompiler.Linker.FindSymbol(MethodCompiler.Method.FullName, SectionKind.Text);
			var stream = symbol.Stream;
			var oldPosition = stream.Position;
			var length = (int)stream.Length;
			var byteArray = new byte[length];

			stream.Position = 0;
			stream.Read(byteArray, 0, length);
			stream.Position = oldPosition;

			try
			{
				// Create the disassembler
				using (var disasm = new Disassembler(byteArray, mode, 0, true))
				{
					// Need a new instance of translator every time as they aren't thread safe
					var translator = new SharpDisasm.Translators.IntelTranslator();

					// Configure the translator to output instruction addresses and instruction binary as hex
					translator.IncludeAddress = true;
					translator.IncludeBinary = true;

					// Disassemble each instruction and output to trace
					foreach (var instruction in disasm.Disassemble())
					{
						var asString = translator.Translate(instruction);
						trace.Log(asString);
					}
				}
			}
			catch (Exception e)
			{
				trace.Log($"Unable to continue disassembly, error encountered\r\n{e.ToString()}");
				NewCompilerTraceEvent(CompilerEvent.Error, $"Failed disassembly for method {this.MethodCompiler.Method}");
			}
		}

		protected void TracePatchRequests()
		{
			var trace = CreateTraceLog("Patch-Requests");

			if (!trace.Active)
				return;

			var symbol = MethodCompiler.Linker.FindSymbol(MethodCompiler.Method.FullName, SectionKind.Text);

			foreach (var request in symbol.LinkRequests)
			{
				trace.Log(String.Format("{0:x8} -> [{1}] +{2:x} [{3}] {4}",
					request.PatchOffset,
					request.LinkType.ToString(),
					request.ReferenceOffset,
					request.ReferenceSymbol.SectionKind.ToString(),
					request.ReferenceSymbol.Name
				));
			}
		}
	}
}
