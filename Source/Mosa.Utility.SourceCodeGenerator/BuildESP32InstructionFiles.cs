// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildESP32InstructionFiles : BuildCommonInstructionFiles
	{
		protected override string Platform { get { return "ESP32"; } }

		public BuildESP32InstructionFiles(string jsonFile, string destinationPath)
			: base(jsonFile, destinationPath)
		{
		}
	}
}
