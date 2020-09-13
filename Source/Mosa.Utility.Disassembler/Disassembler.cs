// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Reko.Arch.Arm;
using Reko.Arch.X86;
using Reko.Core;
using Reko.Core.Machine;
using Reko.Core.Types;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;

namespace Mosa.Utility.Disassembler
{
	public partial class Disassembler
	{
		public ulong Offset { get; set; } = 0;

		private ProcessorArchitecture arch;
		private MemoryArea mem;

		public Disassembler(string platform)
		{
			switch (platform)
			{
				case "ARMv8A32": arch = new Arm32Architecture(new ServiceContainer(), "arm32"); break;
				case "x86": arch = new X86ArchitectureFlat32(new ServiceContainer(), "x86-protected-32"); break;
				case "x64": arch = arch = new X86ArchitectureFlat64(new ServiceContainer(), "x86-protected-64"); break;
			}
		}

		public void SetMemory(byte[] memory, ulong address)
		{
			mem = new MemoryArea(Address.Ptr32((uint)address), memory);
		}

		public List<DecodedInstruction> Decode(int count = int.MaxValue)
		{
			var decoded = new List<DecodedInstruction>();

			try
			{
				var dasm = arch.CreateDisassembler(mem.CreateLeReader((uint)Offset));

				foreach (var instr in dasm)
				{
					Offset += (uint)instr.Length;

					var sw = new StringWriter();
					var renderer = new InstrWriter(sw);

					RenderInstruction(mem, arch, instr, renderer);

					decoded.Add(new DecodedInstruction()
					{
						Address = instr.Address.Offset,
						Length = instr.Length,
						Instruction = instr.ToString().Replace('\t', ' '),
						Full = sw.ToString().Replace('\t', ' ')
					});

					count--;

					if (count == 0)
						break;
				}

				return decoded;
			}
			catch
			{
				return decoded;
			}
		}

		private bool RenderInstruction(MemoryArea mem, IProcessorArchitecture arch, MachineInstruction instr, InstrWriter writer)
		{
			var instrAddress = instr.Address;
			var addrBegin = instrAddress;

			writer.WriteFormat("{0} ", addrBegin);

			WriteByteRange(mem, arch, instrAddress, instrAddress + instr.Length, writer);
			if (instr.Length * 3 < 16)
			{
				writer.WriteString(new string(' ', 16 - (instr.Length * 3)));
			}

			writer.WriteString("\t");
			writer.Address = instrAddress;
			instr.Render(writer, MachineInstructionWriterOptions.ResolvePcRelativeAddress);

			return true;
		}

		private void WriteByteRange(MemoryArea image, IProcessorArchitecture arch, Address begin, Address addrEnd, InstrWriter writer)
		{
			var rdr = arch.CreateImageReader(image, begin);
			var byteSize = (7 + arch.InstructionBitSize) / 8;
			string instrByteFormat = $"{{0:X{byteSize * 2}}} "; // each byte is two nybbles.
			var instrByteSize = PrimitiveType.CreateWord(arch.InstructionBitSize);

			while (rdr.Address < addrEnd)
			{
				if (rdr.TryRead(instrByteSize, out var v))
					writer.WriteFormat(instrByteFormat, v.ToUInt64());
			}
		}
	}
}
