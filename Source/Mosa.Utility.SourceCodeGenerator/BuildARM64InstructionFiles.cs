// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator;

public class BuildARM64InstructionFiles : BuildCommonInstructionFiles
{
	protected override string Platform => "ARM64";

	public BuildARM64InstructionFiles(string jsonFile, string destinationPath)
		: base(jsonFile, destinationPath)
	{
	}
}
