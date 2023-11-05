// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			Path.Combine(dataPath, @"IR-Optimizations-ConstantFolding-MemoryAccess2.json"),
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
			Path.Combine(dataPath, @"IR-Optimizations-LowerTo32.json"),
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
			Path.Combine(dataPath, @"IR-Optimizations-Useless.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.Framework",
			"Mosa.Compiler.Framework").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-BitValue.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.Framework",
			"Mosa.Compiler.Framework").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X86-Optimizations-Standard.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.x86",
			"Mosa.Compiler.x86").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X86-Optimizations-ConstantMove.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.x86",
			"Mosa.Compiler.x86").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X86-Optimizations-Ordering.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.x86",
			"Mosa.Compiler.x86").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X64-Optimizations-Standard.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x64\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.x64",
			"Mosa.Compiler.x64").Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X64-Optimizations-Ordering.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x64\Transforms\Optimizations\Auto"),
			"Mosa.Compiler.x64",
			"Mosa.Compiler.x64").Execute();

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
			Path.Combine(targetPath, @"Mosa.Compiler.x86\Transforms\Optimizations\Auto"),
			"AutoTransforms.cs",
			"Mosa.Compiler.x86.Transforms.Optimizations.Auto",
			"AutoTransforms",
			new List<string>
			{
				"x86."
			}
		).Execute();

		new BuildTransformationListFile(
			Path.Combine(targetPath, @"Mosa.Compiler.x64\Transforms\Optimizations\Auto"),
			"AutoTransforms.cs",
			"Mosa.Compiler.x64.Transforms.Optimizations.Auto",
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

		new BuildIRInstructionFiles(
			Path.Combine(dataPath, @"IR-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework\IR\")
		).Execute();

		new BuildX86(
			Path.Combine(dataPath, @"X86-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86\"),
			"X86.cs"
		).Execute();

		new BuildX86Instructions(
			Path.Combine(dataPath, @"X86-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86\"),
			"X86Instructions.cs"
		).Execute();

		new BuildX86InstructionFiles(
			Path.Combine(dataPath, @"X86-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86\Instructions")
		).Execute();

		new BuildX64(
			Path.Combine(dataPath, @"X64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x64\"),
			"X64.cs"
		).Execute();

		new BuildX64Instructions(
			Path.Combine(dataPath, @"X64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x64\"),
			"X64Instructions.cs"
		).Execute();

		new BuildX64InstructionFiles(
			Path.Combine(dataPath, @"X64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x64\Instructions")
		).Execute();

		new BuildARM32(
			Path.Combine(dataPath, @"ARM32-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.ARM32\"),
			"ARM32.cs"
		).Execute();

		new BuildARM32Instructions(
			Path.Combine(dataPath, @"ARM32-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.ARM32\"),
			"ARM32Instructions.cs"
		).Execute();

		new BuildARM32InstructionFiles(
			Path.Combine(dataPath, @"ARM32-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.ARM32\Instructions")
		).Execute();

		new BuildARM64(
			Path.Combine(dataPath, @"ARM64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.ARM64\"),
			"ARM64.cs"
		).Execute();

		new BuildARM64Instructions(
			Path.Combine(dataPath, @"ARM64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.ARM64\"),
			"ARM64Instructions.cs"
		).Execute();

		new BuildARM64InstructionFiles(
			Path.Combine(dataPath, @"ARM64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.ARM64\Instructions")
		).Execute();
	}
}
