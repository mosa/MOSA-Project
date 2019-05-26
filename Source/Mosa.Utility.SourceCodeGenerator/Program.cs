// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

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

			new BuildIRInstructions(
				Path.Combine(dataPath, @"IRInstructions.json"),
				Path.Combine(targetPath, @"Mosa.Compiler.Framework\IR\"),
				"IRInstructions.cs"
			);

			new BuildIRInstructionFiles(
				Path.Combine(dataPath, @"IRInstructions.json"),
				Path.Combine(targetPath, @"Mosa.Compiler.Framework\IR\")
			);

			new BuildX86(
				Path.Combine(dataPath, @"X86Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.x86\"),
				"X86.cs"
			);

			new BuildX86Instructions(
				Path.Combine(dataPath, @"X86Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.x86\"),
				"X86Instructions.cs"
			);

			new BuildX86InstructionFiles(
				Path.Combine(dataPath, @"X86Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.x86\Instructions")
			);

			new BuildX64(
				Path.Combine(dataPath, @"X64Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.x64\"),
				"X64.cs"
			);

			new BuildX64Instructions(
				Path.Combine(dataPath, @"X64Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.x64\"),
				"X64Instructions.cs"
			);

			new BuildX64InstructionFiles(
				Path.Combine(dataPath, @"X64Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.x64\Instructions")
			);

			new BuildARMv6(
				Path.Combine(dataPath, @"ARMv6Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.ARMv6\"),
				"ARMv6.cs"
			);

			new BuildARMv6Instructions(
				Path.Combine(dataPath, @"ARMv6Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.ARMv6\"),
				"ARMv6Instructions.cs"
			);

			new BuildARMv6InstructionFiles(
				Path.Combine(dataPath, @"ARMv6Instructions.json"),
				Path.Combine(targetPath, @"Mosa.Platform.ARMv6\Instructions")
			);

			//new BuildESP32(
			//	Path.Combine(dataPath, @"ESP32Instructions.json"),
			//	Path.Combine(targetPath, @"Mosa.Platform.ESP32\"),
			//	"ESP32.cs"
			//);

			//new BuildESP32Instructions(
			//	Path.Combine(dataPath, @"ESP32Instructions.json"),
			//	Path.Combine(targetPath, @"Mosa.Platform.ESP32\"),
			//	"ESP32Instructions.cs"
			//);

			//new BuildESP32InstructionFiles(
			//	Path.Combine(dataPath, @"ESP32Instructions.json"),
			//	Path.Combine(targetPath, @"Mosa.Platform.ESP32\Instructions")
			//);
		}
	}
}
