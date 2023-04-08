// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.IO;

namespace Mosa.Utility.SourceCodeGenerator;

public static class Program
{
	public static void Main()
	{
		var dataPath = @"..\Source\Data";
		var targetPath = @"..\Source\";

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-ConstantFolding.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.Framework",
			"Mosa.Compiler.Framework").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-ConstantMove.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.Framework",
			"Mosa.Compiler.Framework").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-Simplification.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.Framework",
			"Mosa.Compiler.Framework").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-StrengthReduction.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.Framework",
			"Mosa.Compiler.Framework").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-StrengthReduction-Complex.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.Framework",
			"Mosa.Compiler.Framework").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-Reorder.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.Framework",
			"Mosa.Compiler.Framework").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-ConstantMove-Expression.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.Framework",
			"Mosa.Compiler.Framework").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-ConstantFolding-Expression.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.Framework",
			"Mosa.Compiler.Framework").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-ConstantFolding-MemoryAccess.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.Framework",
			"Mosa.Compiler.Framework").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-Rewrite.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.Framework",
			"Mosa.Compiler.Framework").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-Phi.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.Framework",
			"Mosa.Compiler.Framework"
		).Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-Algebraic.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.Framework",
			"Mosa.Compiler.Framework").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X86-Optimizations-Standard.json"),
			Path.Combine(targetPath, @"Mosa.Platform.x86\Transforms\Optimizations\Auto"),
			"Mosa.Platform.x86",
			"Mosa.Platform.x86").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X86-Optimizations-Ordering.json"),
			Path.Combine(targetPath, @"Mosa.Platform.x86\Transforms\Optimizations\Auto"),
			"Mosa.Platform.x86",
			"Mosa.Platform.x86").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X64-Optimizations-Standard.json"),
			Path.Combine(targetPath, @"Mosa.Platform.x64\Transforms\Optimizations\Auto"),
			"Mosa.Platform.x64",
			"Mosa.Platform.x64").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X64-Optimizations-Ordering.json"),
			Path.Combine(targetPath, @"Mosa.Platform.x64\Transforms\Optimizations\Auto"),
			"Mosa.Platform.x64",
			"Mosa.Platform.x64").Execute();

		new BuildTransformationListFile(
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"AutoTransforms.cs",
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto",
			"AutoTransforms",
			new List<string>
			{
				"IR."
			}
		).Execute();

		new BuildTransformationListFile(
			Path.Combine(targetPath, @"Mosa.Platform.x86\Transforms\Optimizations\Auto"),
			"AutoTransforms.cs",
			"Mosa.Platform.x86.Transforms.Optimizations.Auto",
			"AutoTransforms",
			new List<string>
			{
				"x86."
			}
		).Execute();

		new BuildTransformationListFile(
			Path.Combine(targetPath, @"Mosa.Platform.x64\Transforms\Optimizations\Auto"),
			"AutoTransforms.cs",
			"Mosa.Platform.x64.Transforms.Optimizations.Auto",
			"AutoTransforms",
			new List<string>
			{
				"X64."
			}
		).Execute();

		new BuildIRInstruction(
			Path.Combine(dataPath, @"IR-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\"),
			"IRInstruction.cs"
		).Execute();

		new BuildIRInstructions(
			Path.Combine(dataPath, @"IR-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\"),
			"IRInstructions.cs"
		).Execute();

		new BuildIRInstructionFiles(
			Path.Combine(dataPath, @"IR-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\IR\")
		).Execute();

		new BuildX86(
			Path.Combine(dataPath, @"X86-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Platform.x86\"),
			"X86.cs"
		).Execute();

		new BuildX86Instructions(
			Path.Combine(dataPath, @"X86-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Platform.x86\"),
			"X86Instructions.cs"
		).Execute();

		new BuildX86InstructionFiles(
			Path.Combine(dataPath, @"X86-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Platform.x86\Instructions")
		).Execute();

		new BuildX64(
			Path.Combine(dataPath, @"X64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Platform.x64\"),
			"X64.cs"
		).Execute();

		new BuildX64Instructions(
			Path.Combine(dataPath, @"X64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Platform.x64\"),
			"X64Instructions.cs"
		).Execute();

		new BuildX64InstructionFiles(
			Path.Combine(dataPath, @"X64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Platform.x64\Instructions")
		).Execute();

		new BuildARMv8A32(
			Path.Combine(dataPath, @"ARMv8A32-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Platform.ARMv8A32\"),
			"ARMv8A32.cs"
		).Execute();

		new BuildARMv8A32Instructions(
			Path.Combine(dataPath, @"ARMv8A32-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Platform.ARMv8A32\"),
			"ARMv8A32Instructions.cs"
		).Execute();

		new BuildARMv8A32InstructionFiles(
			Path.Combine(dataPath, @"ARMv8A32-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Platform.ARMv8A32\Instructions")
		).Execute();

		//new BuildESP32(
		//	Path.Combine(dataPath, @"ESP32-Instructions.json"),
		//	Path.Combine(targetPath, @"Mosa.Platform.ESP32\"),
		//	"ESP32.cs"
		//).Execute();

		//new BuildESP32Instructions(
		//	Path.Combine(dataPath, @"ESP32-Instructions.json"),
		//	Path.Combine(targetPath, @"Mosa.Platform.ESP32\"),
		//	"ESP32Instructions.cs"
		//).Execute();

		//new BuildESP32InstructionFiles(
		//	Path.Combine(dataPath, @"ESP32-Instructions.json"),
		//	Path.Combine(targetPath, @"Mosa.Platform.ESP32\Instructions")
		//).Execute();
	}
}
