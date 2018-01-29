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

		public string StaticEmitMethod; // __staticMethodName

		public string X86EmitMethodType;
		public byte[] X86EmitBytes;

		public byte[] X86LegacyOpcode;
		public byte? X86LegacyOpcodeRegField;

		//Effect:
		//	X - Changed
		//	1 - Always set to 1
		//	0 - Always set to 0
		//	U - Undefined(assume change)

		public string CarryFlagEffect;
		public string OverflowFlagEffect;
		public string ZeroFlagEffect;
		public string NegativeFlagEffect;

		//Dependency:
		//	Y - Yes
		//	N - No
		//	C - Conditional(based on instruction condition)

		public string CarryFlagDependency;
		public string OverflowFlagDependency;
		public string ZeroFlagDependency;
		public string NegativeFlagDependency;

		public bool X86ThreeTwoAddressConversion;

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append("{ ");

			sb.AppendFormat("\"Name\": \"{0}\", ", Name);
			sb.AppendFormat("\"FamilyName\": \"{0}\", ", FamilyName);

			sb.AppendFormat("\"ResultCount\": {0}, ", ResultCount);
			sb.AppendFormat("\"OperandCount\": {0}, ", OperandCount);

			if (!String.IsNullOrWhiteSpace(ResultType) && ResultType != "None")
				sb.AppendFormat("\"ResultType\": \"{0}\", ", ResultType);

			if (!String.IsNullOrWhiteSpace(ResultType2) && ResultType2 != "None")
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

			if (CarryFlagEffect != null)
				sb.AppendFormat("\"CarryFlagEffect\": \"{0}\", ", CarryFlagEffect);

			if (OverflowFlagEffect != null)
				sb.AppendFormat("\"OverflowFlagEffect\": \"{0}\", ", OverflowFlagEffect);

			if (ZeroFlagEffect != null)
				sb.AppendFormat("\"ZeroFlagEffect\": \"{0}\", ", ZeroFlagEffect);

			if (NegativeFlagEffect != null)
				sb.AppendFormat("\"NegativeFlagEffect\": \"{0}\", ", NegativeFlagEffect);

			if (CarryFlagDependency != null)
				sb.AppendFormat("\"CarryFlagDependency\": \"{0}\", ", CarryFlagDependency);

			if (OverflowFlagDependency != null)
				sb.AppendFormat("\"OverflowFlagDependency\": \"{0}\", ", OverflowFlagDependency);

			if (ZeroFlagDependency != null)
				sb.AppendFormat("\"ZeroFlagDependency\": \"{0}\", ", ZeroFlagDependency);

			if (NegativeFlagDependency != null)
				sb.AppendFormat("\"NegativeFlagDependency\": \"{0}\", ", NegativeFlagDependency);

			if (X86EmitMethodType != null)
				sb.AppendFormat("\"X86EmitMethodType\": \"{0}\", ", X86EmitMethodType);

			if (X86EmitBytes != null)
			{
				sb.AppendFormat("\"X86EmitBytes\": \"");

				foreach (var b in X86EmitBytes)
				{
					sb.AppendFormat("{0:X2} ", b);
				}

				sb.Length--;

				sb.AppendFormat("\", ");
			}

			if (X86LegacyOpcode != null)
			{
				sb.AppendFormat("\"X86LegacyOpcode\": \"");

				foreach (var b in X86LegacyOpcode)
				{
					sb.AppendFormat("{0:X2} ", b);
				}

				sb.Length--;

				sb.AppendFormat("\", ");
			}

			if (X86LegacyOpcodeRegField.HasValue)
				sb.AppendFormat("\"X86LegacyOpcodeRegField\": \"{0:X2}\", ", X86LegacyOpcodeRegField.Value);

			if (StaticEmitMethod != null)
				sb.AppendFormat("\"StaticEmitMethod\": \"{0}\", ", StaticEmitMethod);

			if (X86ThreeTwoAddressConversion)
				sb.AppendFormat("\"X86ThreeTwoAddressConversion\": \"{0}\", ", X86ThreeTwoAddressConversion ? "true" : "false");

			if (Description != null)
				sb.AppendFormat("\"Description\": \"{0}\", ", Description);

			sb.Length--;
			sb.Length--;

			sb.Append(" }");

			return sb.ToString();
		}
	}
}
