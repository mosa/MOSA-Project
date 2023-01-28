// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator;

public class BuildARMv8A32InstructionFiles : BuildCommonInstructionFiles
{
	protected override string Platform => "ARMv8A32";

	public BuildARMv8A32InstructionFiles(string jsonFile, string destinationPath)
		: base(jsonFile, destinationPath)
	{
	}
}
