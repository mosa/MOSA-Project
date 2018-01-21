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

		public bool MemoryWrite;
		public bool MemoryRead;
		public bool IOOperation;
		public bool UnspecifiedSideEffect;

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

			if (MemoryWrite)
				sb.AppendFormat("\"MemoryWrite\": \"{0}\", ", MemoryWrite ? "true" : "false");

			if (MemoryRead)
				sb.AppendFormat("\"MemoryRead\": \"{0}\", ", MemoryRead ? "true" : "false");

			if (IOOperation)
				sb.AppendFormat("\"IOOperation\": \"{0}\", ", IOOperation ? "true" : "false");

			if (UnspecifiedSideEffect)
				sb.AppendFormat("\"UnspecifiedSideEffect\": \"{0}\", ", UnspecifiedSideEffect ? "true" : "false");

			if (Description != null)
				sb.AppendFormat("\"Description\": \"{0}\", ", Description);

			sb.Length--;
			sb.Length--;

			sb.Append(" }");

			return sb.ToString();
		}
	}
}
