// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.ComponentModel.Design;
using System.Text;
using Reko.Arch.Arm;
using Reko.Arch.X86;
using Reko.Core;
using Reko.Core.Memory;

namespace Mosa.Utility.Disassembler;

public partial class Disassembler
{
	private byte[] memory;

	public ulong Offset { get; set; }

	private readonly ProcessorArchitecture arch;
	private MemoryArea memoryArea;

	public Disassembler(string platform)
	{
		var services = new ServiceContainer();
		var options = new Dictionary<string, object>();

		arch = platform.ToLowerInvariant() switch
		{
			"arm32" => new Arm32Architecture(services, "arm32", options),
			"arm64" => new Arm64Architecture(services, "arm64", options),
			"x86" => new X86ArchitectureFlat32(services, "x86-protected-32", options),
			"x64" => new X86ArchitectureFlat64(services, "x86-protected-64", options),
			_ => arch
		};
	}

	public void SetMemory(byte[] memory, ulong address)
	{
		this.memory = memory;
		memoryArea = new ByteMemoryArea(Address.Ptr32((uint)address), memory);
	}

	public List<DecodedInstruction> Decode(int count = int.MaxValue)
	{
		var decoded = new List<DecodedInstruction>();

		var sb = new StringBuilder(100);

		try
		{
			var dasm = arch.CreateDisassembler(memoryArea.CreateLeReader((long)Offset));

			foreach (var instr in dasm)
			{
				var len = instr.Length;
				var address = instr.Address.Offset;
				var instruction = instr.ToString().Replace('\t', ' ');

				// preference
				instruction = instruction.Replace(",", ", ");

				// fix up
				//instruction = ChangeHex(instruction);

				sb.AppendFormat("{0:x8}", address);
				sb.Append(' ');
				var bytes = BytesToHex(memory, (uint)Offset, len);
				sb.Append(bytes == null ? string.Empty : bytes.ToString());
				sb.Append(string.Empty.PadRight(41 - sb.Length, ' '));
				sb.Append(instruction);

				decoded.Add(new DecodedInstruction
				{
					Address = address,
					Length = len,
					Instruction = instruction,
					Full = sb.ToString()
				});

				sb.Clear();

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

	private static StringBuilder BytesToHex(byte[] memory, uint offset, int length)
	{
		if (length == 0)
			return null;

		var sb = new StringBuilder();

		for (uint i = 0; i < length; i++)
		{
			var b = memory[i + offset];

			sb.AppendFormat("{0:x2} ", b);
		}

		sb.Length--;

		return sb;
	}

	private static bool IsHex(char c)
	{
		if (c >= '0' && c <= '9')
			return true;

		if (c >= 'a' && c <= 'f')
			return true;

		if (c >= 'A' && c <= 'F')
			return true;

		return false;
	}

	private static int IsNextWordHex(string s, int start)
	{
		for (var i = 0; i < s.Length - start; i++)
		{
			var c = s[start + i];

			if (c == 'h' && i != 0)
				return i;

			if (!IsHex(c))
				return 0;
		}

		return 0;
	}

	private static string ChangeHex(string s)
	{
		var sb = new StringBuilder();

		for (var i = 0; i < s.Length; i++)
		{
			var c = s[i];

			var l = IsNextWordHex(s, i);

			if (l == 0)
			{
				sb.Append(c);
				continue;
			}
			else
			{
				var hex = s.Substring(i, l);
				var value = long.Parse(hex, System.Globalization.NumberStyles.HexNumber);
				var hex2 = Convert.ToString(value, 16).ToUpper();

				sb.Append("0x");
				sb.Append(hex2);

				i += l;
			}
		}

		return sb.ToString();
	}
}
