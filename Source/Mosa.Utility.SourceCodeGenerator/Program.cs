// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace Mosa.Utility.SourceCodeGenerator
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var dataPath = @"..\Source\Data";
			var targetPath = @"..\Source\";

			new BuildIRInstruction(
				Path.Combine(dataPath, @"IRInstructions.json"),
				Path.Combine(targetPath, @"Mosa.Compiler.Framework\IR\"),
				"IRInstruction.cs"
			);

			new BuildIRInstructionMap(
				Path.Combine(dataPath, @"IRInstructions.json"),
				Path.Combine(targetPath, @"Mosa.Compiler.Framework\IR\"),
				"IRInstructionMap.cs"
			);

			new BuildIRInstructions(
				Path.Combine(dataPath, @"IRInstructions.json"),
				Path.Combine(targetPath, @"Mosa.Compiler.Framework\IR\")
			);

			new BuildX86(
				Path.Combine(dataPath, @"X86Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.x86\"),
				"X86.cs"
			);

			new BuildX86InstructionMap(
				Path.Combine(dataPath, @"X86Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.x86\"),
				"X86InstructionMap.cs"
			);

			new BuildX86Instructions(
				Path.Combine(dataPath, @"X86Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.x86\Instructions")
			);

			new BuildX64(
				Path.Combine(dataPath, @"X64Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.x64\"),
				"X64.cs"
			);

			new BuildX64InstructionMap(
				Path.Combine(dataPath, @"X64Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.x64\"),
				"X64InstructionMap.cs"
			);

			new BuildX64Instructions(
				Path.Combine(dataPath, @"X64Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.x64\Instructions")
			);

			new BuildARMv6(
				Path.Combine(dataPath, @"ARMv6Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.ARMv6\"),
				"ARMv6.cs"
			);

			new BuildARMv6InstructionMap(
				Path.Combine(dataPath, @"ARMv6Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.ARMv6\"),
				"ARMv6InstructionMap.cs"
			);

			new BuildARMv6Instructions(
				Path.Combine(dataPath, @"ARMv6Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.ARMv6\Instructions")
			);
		}
	}
}
