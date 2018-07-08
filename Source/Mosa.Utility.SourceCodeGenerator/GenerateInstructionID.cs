// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator
{
	static public class GenerateInstructionID
	{
		private static int id = 0;

		public static int GetInstructionID()
		{
			return id++;
		}
	}
}
