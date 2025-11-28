// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator;

public static class Program
{
	public static void Main()
	{
		var dataPath = @"../Source/Data";
		var targetPath = @"../Source/";

		// IR

		new BuildInstructionList(
			Path.Combine(dataPath, @"IR-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/"),
			"IR.cs",
			"IR",
			"Framework"
		).Execute();

		new BuildIRInstructionFiles(
			Path.Combine(dataPath, @"IR-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Instructions/")
		).Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-ConstantFolding.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-ConstantMove.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-Simplification.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-StrengthReduction.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-StrengthReduction-Complex.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-Reorder.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-ConstantMove-Expression.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-ConstantFolding-Expression.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-ConstantFolding-MemoryAccess.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-ConstantFolding-MemoryAccess2.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-Rewrite.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-Phi.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-LowerTo32.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-Algebraic.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-Useless.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"IR-Optimizations-BitValue.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformationListFile(
			Path.Combine(targetPath, @"Mosa.Compiler.Framework/Transforms/Optimizations/Auto"),
			"AutoTransforms.cs",
			"Mosa.Compiler.Framework.Transforms.Optimizations.Auto",
			"AutoTransforms",
			new List<string>
			{
				"Mosa.Compiler.Framework.Transforms.Optimizations.Auto",
			}
		).Execute();

		// X86

		new BuildInstructionList(
			Path.Combine(dataPath, @"X86-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86/"),
			"X86.cs",
			"X86",
			"x86")
			.Execute();

		new BuildInstructionFiles(
			Path.Combine(dataPath, @"X86-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86/Instructions"),
			"Mosa.Compiler.x86",
			"X86Instruction")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X86-Optimizations-Standard.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.x86.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X86-Optimizations-ConstantMove.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.x86.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X86-Optimizations-Ordering.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.x86.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X86-Optimizations-Lea.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x86/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.x86.Transforms.Optimizations.Auto")
			.Execute();

		//new BuildTransformations(
		//	Path.Combine(dataPath, @"X86-Optimizations-IRTransform.json"),
		//	Path.Combine(targetPath, @"Mosa.Compiler.x86/Transforms/BaseIR/Auto"),
		//	"Mosa.Compiler.x86.Transforms.BaseIR.Auto")
		//	.Execute();

		new BuildTransformationListFile(
			Path.Combine(targetPath, @"Mosa.Compiler.x86/Transforms/Optimizations/Auto"),
			"AutoTransforms.cs",
			"Mosa.Compiler.x86.Transforms.Optimizations.Auto",
			"AutoTransforms",
			new List<string>
			{
				"Mosa.Compiler.x86.Transforms.Optimizations.Auto."
			})
			.Execute();

		//new BuildTransformationListFile(
		//	Path.Combine(targetPath, @"Mosa.Compiler.x86/Transforms/BaseIR/Auto"),
		//	"AutoTransforms.cs",
		//	"Mosa.Compiler.x86.Transforms.BaseIR.Auto",
		//	"AutoTransforms",
		//	new List<string>
		//	{
		//		"Mosa.Compiler.x86.Transforms.BaseIR.Auto"
		//	})
		//	.Execute();

		// X64

		new BuildInstructionList(
			Path.Combine(dataPath, @"X64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x64/"),
			"X64.cs",
			"X64",
			"x64")
			.Execute();

		new BuildInstructionFiles(
			Path.Combine(dataPath, @"X64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x64/Instructions"),
			"Mosa.Compiler.x64",
			"X64Instruction")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X64-Optimizations-Standard.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x64/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.x64.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X64-Optimizations-ConstantMove.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x64/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.x64.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X64-Optimizations-Ordering.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x64/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.x64.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformations(
			Path.Combine(dataPath, @"X64-Optimizations-Lea.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.x64/Transforms/Optimizations/Auto"),
			"Mosa.Compiler.x64.Transforms.Optimizations.Auto")
			.Execute();

		new BuildTransformationListFile(
			Path.Combine(targetPath, @"Mosa.Compiler.x64/Transforms/Optimizations/Auto"),
			"AutoTransforms.cs",
			"Mosa.Compiler.x64.Transforms.Optimizations.Auto",
			"AutoTransforms",
			new List<string>
			{
				"Mosa.Compiler.x64.Transforms.Optimizations.Auto."
			}
		).Execute();

		// ARM32

		new BuildInstructionList(
			Path.Combine(dataPath, @"ARM32-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.ARM32/"),
			"ARM32.cs",
			"ARM32",
			"ARM32")
			.Execute();

		new BuildInstructionFiles(
			Path.Combine(dataPath, @"ARM32-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.ARM32/Instructions"),
			"Mosa.Compiler.ARM32",
			"ARM32Instruction")
			.Execute();

		// ARM64

		new BuildInstructionList(
			Path.Combine(dataPath, @"ARM64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.ARM64/"),
			"ARM64.cs",
			"ARM64",
			"ARM64")
			.Execute();

		new BuildInstructionFiles(
			Path.Combine(dataPath, @"ARM64-Instructions.json"),
			Path.Combine(targetPath, @"Mosa.Compiler.ARM64/Instructions"),
			"Mosa.Compiler.ARM64",
			"ARM64Instruction")
			.Execute();
	}
}
