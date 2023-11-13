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
			Path.Combine(dataPath, @"X86-Optimizations-IRTransform.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86\Transforms\IR\Auto"),
			"Mosa.Compiler.x86",
			"Mosa.Compiler.x86").Execute();

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

		// X86

		new BuildX86(
			Path.Combine(dataPath, @"X86-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86\"),
			"X86.cs"
		).Execute();

		new BuildInstructionFiles(
			Path.Combine(dataPath, @"X86-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86\Instructions"),
			"x86"
		).Execute();

		// X64

		new BuildX64(
			Path.Combine(dataPath, @"X64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x64\"),
			"X64.cs"
		).Execute();

		new BuildInstructionFiles(
			Path.Combine(dataPath, @"X64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x64\Instructions"),
			"x64"
		).Execute();

		// ARM32

		new BuildARM32(
			Path.Combine(dataPath, @"ARM32-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.ARM32\"),
			"ARM32.cs"
		).Execute();

		new BuildInstructionFiles(
			Path.Combine(dataPath, @"ARM32-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.ARM32\Instructions"),
			"ARM32"
		).Execute();

		// ARM64

		new BuildARM64(
			Path.Combine(dataPath, @"ARM64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.ARM64\"),
			"ARM64.cs"
		).Execute();

		new BuildInstructionFiles(
			Path.Combine(dataPath, @"ARM64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.ARM64\Instructions"),
			"ARM64"
		).Execute();
	}
}
