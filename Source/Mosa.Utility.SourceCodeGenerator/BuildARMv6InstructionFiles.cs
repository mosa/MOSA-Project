// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildARMv6InstructionFiles : BuildCommonInstructionFiles
	{
		protected override string Platform { get { return "ARMv6"; } }

		public BuildARMv6InstructionFiles(string jsonFile, string destinationPath)
			: base(jsonFile, destinationPath)
		{
		}
	}
}
