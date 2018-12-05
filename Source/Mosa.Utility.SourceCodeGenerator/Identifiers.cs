// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Utility.SourceCodeGenerator
{
	static public class Identifiers
	{
		private static int instructionID = 0;
		private static int ruleID = 0;
		private static int gapMod = 10;

		public static int GetInstructionID()
		{
			return ++instructionID;
		}

		public static int GetRuleID()
		{
			return ++ruleID;
		}

		public static void InstructionGap()
		{
			instructionID = Alignment.AlignUp(instructionID, gapMod);
		}
	}
}
