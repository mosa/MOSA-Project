// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildX64InstructionFiles : BuildCommonInstructionFiles
	{
		protected override string Platform { get { return "x64"; } }

		public BuildX64InstructionFiles(string jsonFile, string destinationPath)
			: base(jsonFile, destinationPath)
		{
		}
	}
}
