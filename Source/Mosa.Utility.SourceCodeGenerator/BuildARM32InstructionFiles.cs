// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator;

public class BuildARM32InstructionFiles : BuildCommonInstructionFiles
{
	protected override string Platform => "ARM32";

	public BuildARM32InstructionFiles(string jsonFile, string destinationPath)
		: base(jsonFile, destinationPath)
	{
	}
}
