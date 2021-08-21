// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Reko.Arch.Arm;
using Reko.Arch.X86;
using Reko.Core;
using Reko.Core.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace Mosa.Utility.Disassembler
{
	public partial class Disassembler
	{
		private byte[] memory;

		public ulong Offset { get; set; } = 0;

		private readonly ProcessorArchitecture arch;
		private MemoryArea memoryArea;

		public Disassembler(string platform)
		{
			var services = new ServiceContainer();
			var options = new Dictionary<string, object>();

			switch (platform.ToLower())
			{
				case "armv8a32": arch = new Arm32Architecture(services, "arm32", options); break;
				case "x86": arch = new X86ArchitectureFlat32(services, "x86-protected-32", options); break;
				case "x64": arch = arch = new X86ArchitectureFlat64(services, "x86-protected-64", options); break;
			}
		}

		public void SetMemory(byte[] memory, ulong address)
		{
			this.memory = memory;
			memoryArea = new ByteMemoryArea(Address.Ptr32((uint)address), memory);
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

					// preference
					instruction = instruction.Replace(",", ", ");

					// fix up
					instruction = ChangeHex(instruction);

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

		private bool IsHex(char c)
		{
			if (c >= '0' && c <= '9')
				return true;

			if (c >= 'a' && c <= 'f')
				return true;

			if (c >= 'A' && c <= 'F')
				return true;

			return false;
		}

		private bool IsNext8Hex(string s, int start)
		{
			if (start + 8 > s.Length)
				return false;

			for (int i = start; i < start + 8; i++)
			{
				char c = s[i];

				if (!IsHex(c))
					return false;
			}

			if (start + 8 == s.Length)
				return true;

			return !IsHex(s[start + 8]);
		}

		private string ChangeHex(string s)
		{
			var sb = new StringBuilder();

			for (int i = 0; i < s.Length; i++)
			{
				char c = s[i];

				if (!IsNext8Hex(s, i))
				{
					sb.Append(c);
					continue;
				}
				else
				{
					string hex = s.Substring(i, 8);
					uint value = uint.Parse(hex, System.Globalization.NumberStyles.HexNumber);
					string hex2 = Convert.ToString(value, 16);

					sb.Append("0x");
					sb.Append(hex2);

					i += 8;
				}
			}

			return sb.ToString();
		}
	}
}
