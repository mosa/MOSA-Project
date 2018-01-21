// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosa.Workspace.Experiment.Debug
{
	internal class Instruction
	{
		public string Name;
		public string FamilyName;
		public int ResultCount;
		public int OperandCount;
		public string ResultType;
		public string ResultType2;
		public string FlowControl;
		public bool IgnoreDuringCodeGeneration;
		public bool IgnoreInstructionBasicBlockTargets;
		public bool VariableOperands;
		public bool Commutative;
		public bool HasSideEffect;
		public string Description = null;

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append("{ ");

			sb.AppendFormat("\"Name\": \"{0}\", ", Name);
			sb.AppendFormat("\"FamilyName\": \"{0}\", ", FamilyName);

			sb.AppendFormat("\"ResultCount\": {0}, ", ResultCount);
			sb.AppendFormat("\"OperandCount\": {0}, ", OperandCount);

			if (ResultType != null)
				sb.AppendFormat("\"ResultType\": \"{0}\", ", ResultType);
			if (ResultType2 != null)
				sb.AppendFormat("\"ResultType2\": \"{0}\", ", ResultType2);

			if (FlowControl != null && FlowControl != "Next")
				sb.AppendFormat("\"FlowControl\": \"{0}\", ", FlowControl);

			if (IgnoreDuringCodeGeneration)
				sb.AppendFormat("\"IgnoreDuringCodeGeneration\": \"{0}\", ", IgnoreDuringCodeGeneration ? "true" : "false");

			if (IgnoreInstructionBasicBlockTargets)
				sb.AppendFormat("\"IgnoreInstructionBasicBlockTargets\": \"{0}\", ", IgnoreInstructionBasicBlockTargets ? "true" : "false");

			if (VariableOperands)
				sb.AppendFormat("\"VariableOperands\": \"{0}\", ", VariableOperands ? "true" : "false");

			if (Commutative)
				sb.AppendFormat("\"Commutative\": \"{0}\", ", Commutative ? "true" : "false");

			if (HasSideEffect)
				sb.AppendFormat("\"HasSideEffect\": \"{0}\", ", HasSideEffect ? "true" : "false");

			if (Description != null)
				sb.AppendFormat("\"Description\": \"{0}\", ", Description);

			sb.Length--;
			sb.Length--;

			sb.Append(" }");

			return sb.ToString();
		}
	}
}
