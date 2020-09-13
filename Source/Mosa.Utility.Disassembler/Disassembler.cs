// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Reko.Arch.Arm;
using Reko.Arch.X86;
using Reko.Core;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace Mosa.Utility.Disassembler
{
	public partial class Disassembler
	{
		private byte[] memory;
		private ulong address;

		public ulong Offset { get; set; } = 0;

		private ProcessorArchitecture arch;
		private MemoryArea memoryArea;

		public Disassembler(string platform)
		{
			switch (platform.ToLower())
			{
				case "armv8a32": arch = new Arm32Architecture(new ServiceContainer(), "arm32"); break;
				case "x86": arch = new X86ArchitectureFlat32(new ServiceContainer(), "x86-protected-32"); break;
				case "x64": arch = arch = new X86ArchitectureFlat64(new ServiceContainer(), "x86-protected-64"); break;
			}
		}

		public void SetMemory(byte[] memory, ulong address)
		{
			this.memory = memory;
			this.address = address;
			memoryArea = new MemoryArea(Address.Ptr32((uint)address), memory);
		}

		public List<DecodedInstruction> Decode(int count = int.MaxValue)
		{
			var decoded = new List<DecodedInstruction>();

			try
			{
				var dasm = arch.CreateDisassembler(memoryArea.CreateLeReader((uint)Offset));

				foreach (var instr in dasm)
				{
					var len = instr.Length;
					var address = instr.Address.Offset;
					var instruction = instr.ToString().Replace('\t', ' ');

					var sb = new StringBuilder();

					sb.AppendFormat("{0:x8}", address);
					sb.Append(' ');
					sb.Append(BytesToHex(memory, (uint)Offset, len));
					sb.Append(string.Empty.PadRight(41 - sb.Length, ' '));
					sb.Append(instruction);

					decoded.Add(new DecodedInstruction()
					{
						Address = address,
						Length = len,
						Instruction = instruction,
						Full = sb.ToString()
					});

					count--;

					if (count == 0)
						break;

					Offset += (uint)len;
				}

				return decoded;
			}
			catch
			{
				return decoded;
			}
		}

		private string BytesToHex(byte[] memory, uint offset, int length)
		{
			if (length == 0)
				return string.Empty;

			var sb = new StringBuilder();

			for (uint i = 0; i < length; i++)
			{
				var b = memory[i + offset];

				sb.AppendFormat("{0:x2} ", b);
			}

			sb.Length--;

			return sb.ToString();
		}
	}
}
