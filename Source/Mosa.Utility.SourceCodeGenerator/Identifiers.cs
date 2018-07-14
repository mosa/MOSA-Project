// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator
{
	static public class Identifiers
	{
		private static int instructionID = 0;
		private static int ruleID = 0;

		public static int GetInstructionID()
		{
			return ++instructionID;
		}

		public static int GetRuleID()
		{
			return ++ruleID;
		}
	}
}
