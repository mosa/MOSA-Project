// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.ComponentModel.Design;
using System.Text;
using Reko.Arch.Arm;
using Reko.Arch.X86;
using Reko.Core;
using Reko.Core.Machine;
using Reko.Core.Memory;

namespace Mosa.Utility.Disassembler;

public class Disassembler
{
	public ulong Offset { get; set; }

	private readonly byte[] memory;
	private readonly IEnumerator<MachineInstruction> instructions;
	private readonly StringBuilder sb = new StringBuilder(100);

	public Disassembler(string platform, byte[] bytes, ulong address)
	{
		var services = new ServiceContainer();
		var options = new Dictionary<string, object>();
		var memoryArea = new ByteMemoryArea(Address.Ptr32((uint)address), bytes);

		ProcessorArchitecture arch = platform.ToLowerInvariant() switch
		{
			"arm32" => new Arm32Architecture(services, "arm32", options),
			"arm64" => new Arm64Architecture(services, "arm64", options),
			"x86" => new X86ArchitectureFlat32(services, "x86-protected-32", options),
			"x64" => new X86ArchitectureFlat64(services, "x86-protected-64", options),
			_ => throw new PlatformNotSupportedException(platform)
		};

		memory = bytes;
		instructions = arch.CreateDisassembler(memoryArea.CreateLeReader((long)Offset)).GetEnumerator();
	}

	public DecodedInstruction DecodeNext()
	{
		if (!instructions.MoveNext())
			return null;

		var machineInstruction = instructions.Current;
		if (machineInstruction == null)
			return null;

		var length = (uint)machineInstruction.Length;
		var address = machineInstruction.Address.Offset;
		var instruction = machineInstruction.ToString()
			.Replace('\t', ' ')
			.Replace(",", ", ")
			.Replace("+", " + ")
			.Replace("-", " - ")
			.Replace("*", " * ");

		// FIXME
		//instruction = ChangeHex(instruction);

		sb.Append(address.ToString("X8"));
		sb.Append(' ');
		for (var i = 0U; i < length; i++)
		{
			var b = memory[i + Offset];
			sb.Append(b.ToString("X2"));
			sb.Append(' ');
		}
		for (var i = 0; i < 41 - sb.Length; i++)
			sb.Append(' ');
		sb.Append(instruction);

		var decodedInstruction = new DecodedInstruction
		{
			Address = address,
			Length = length,
			Instruction = instruction,
			Full = sb.ToString()
		};

		sb.Clear();

		Offset += length;

		return decodedInstruction;
	}

	private static bool IsHex(char c) => c switch
	{
		>= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F' => true,
		_ => false
	};

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

			var hex = s.Substring(i, l);
			var value = long.Parse(hex, System.Globalization.NumberStyles.HexNumber);
			var hex2 = Convert.ToString(value, 16).ToUpper();

			sb.Append("0x");
			sb.Append(hex2);

			i += l;
		}

		return sb.ToString();
	}
}
