// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildX86InstructionFiles : BuildCommonInstructionFiles
	{
		protected override string Platform { get { return "x86"; } }

		public BuildX86InstructionFiles(string jsonFile, string destinationPath)
			: base(jsonFile, destinationPath)
		{
		}
	}
}
