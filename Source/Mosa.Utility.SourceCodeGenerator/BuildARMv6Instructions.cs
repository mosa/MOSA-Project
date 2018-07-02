// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildARMv6Instructions : BuildBaseTemplate
	{
		public BuildARMv6Instructions(string jsonFile, string destinationPath)
			: base(jsonFile, destinationPath)
		{
		}

		protected override void Iterator()
		{
			foreach (var entry in Entries.Instructions)
			{
				Lines.Clear();

				DestinationFile = entry.Name + ".cs";
				AddSourceHeader();
				Body(entry);
				Save();
			}
		}

		protected override void Body(dynamic node = null)
		{
			Lines.AppendLine("using Mosa.Compiler.Framework;");

			if (node.ResultType != null || node.ResultType2 != null)
			{
				Lines.AppendLine("using Mosa.Compiler.MosaTypeSystem;");
			}

			if (!string.IsNullOrWhiteSpace(node.ARMv6Opcode))
			{
				Lines.AppendLine("using Mosa.Compiler.Common;");
			}

			Lines.AppendLine();
			Lines.AppendLine("namespace Mosa.Platform.ARMv6.Instructions");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.Append("\t/// " + node.Name);

			if (!string.IsNullOrWhiteSpace(node.Description))
			{
				Lines.Append(" - " + node.Description);
			}

			Lines.AppendLine();
			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine("\t/// <seealso cref=\"Mosa.Platform.ARMv6.ARMv6Instruction\" />");
			Lines.AppendLine("\tpublic sealed class " + node.Name + " : ARMv6Instruction");
			Lines.AppendLine("\t{");

			Lines.AppendLine("\t\tinternal " + node.Name + "()");
			Lines.AppendLine("\t\t\t: base(" + node.ResultCount + ", " + node.OperandCount + ")");
			Lines.AppendLine("\t\t{");
			Lines.AppendLine("\t\t}");

			if (node.AlternativeName != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override string AlternativeName { get { return \"" + node.AlternativeName + "\"; } }");
			}

			if (node.FlowControl != null && node.FlowControl != "Next")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override FlowControl FlowControl { get { return FlowControl." + node.FlowControl + "; } }");
			}

			if (node.ResultType != null && node.ResultType != "")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override BuiltInType ResultType { get { return BuiltInType." + node.ResultType + "; } }");
			}

			if (node.ResultType2 != null && node.ResultType2 != "")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override BuiltInType ResultType2 { get { return BuiltInType." + node.ResultType2 + "; } }");
			}

			if (node.IgnoreDuringCodeGeneration != null && node.IgnoreDuringCodeGeneration == "true")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IgnoreDuringCodeGeneration { get { return true; } }");
			}

			if (node.IgnoreInstructionBasicBlockTargets != null && node.IgnoreInstructionBasicBlockTargets == "true")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IgnoreInstructionBasicBlockTargets { get { return true; } }");
			}

			if (node.VariableOperands != null && node.VariableOperands == "true")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool VariableOperands { get { return true; } }");
			}

			if (node.Commutative != null && node.Commutative == "true")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsCommutative { get { return true; } }");
			}

			if (node.MemoryWrite != null && node.MemoryWrite == "true")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsMemoryWrite { get { return true; } }");
			}

			if (node.MemoryRead != null && node.MemoryRead == "true")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsMemoryRead { get { return true; } }");
			}

			if (node.IOOperation != null && node.IOOperation == "true")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsIOOperation { get { return true; } }");
			}

			if (node.UnspecifiedSideEffect != null && node.UnspecifiedSideEffect == "true")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool HasUnspecifiedSideEffect { get { return true; } }");
			}

			if (node.ARMv6ThreeTwoAddressConversion != null && node.ARMv6ThreeTwoAddressConversion == "true")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool ThreeTwoAddressConversion { get { return true; } }");
			}

			if (node.FlagsUsed != null && node.FlagsUsed.Contains("Z"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsZeroFlagUsed { get { return true; } }");
			}

			if (node.FlagsSet != null && node.FlagsSet.Contains("Z"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsZeroFlagSet { get { return true; } }");
			}

			if (node.FlagsCleared != null && node.FlagsCleared.Contains("Z"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsZeroFlagCleared { get { return true; } }");
			}

			if (node.FlagsModified != null && (node.FlagsModified.Contains("Z") || node.FlagsSet.Contains("Z") || node.FlagsCleared.Contains("Z")))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsZeroFlagModified { get { return true; } }");
			}

			if (node.FlagsUndefined != null && node.FlagsUndefined.Contains("Z"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsZeroFlagUnchanged { get { return true; } }");
			}

			if (node.FlagsUnchanged != null && node.FlagsUndefined.Contains("Z"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsZeroFlagUndefined { get { return true; } }");
			}

			if (node.FlagsUsed != null && node.FlagsUsed.Contains("C"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsCarryFlagUsed { get { return true; } }");
			}

			if (node.FlagsSet != null && node.FlagsSet.Contains("C"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsCarryFlagSet { get { return true; } }");
			}

			if (node.FlagsCleared != null && node.FlagsCleared.Contains("C"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsCarryFlagCleared { get { return true; } }");
			}

			if (node.FlagsModified != null && (node.FlagsModified.Contains("C") || node.FlagsSet.Contains("C") || node.FlagsCleared.Contains("C")))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsCarryFlagModified { get { return true; } }");
			}

			if (node.FlagsUndefined != null && node.FlagsUndefined.Contains("C"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsCarryFlagUnchanged { get { return true; } }");
			}

			if (node.FlagsUnchanged != null && node.FlagsUndefined.Contains("C"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsCarryFlagUndefined { get { return true; } }");
			}

			if (node.FlagsUsed != null && node.FlagsUsed.Contains("S"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsSignFlagUsed { get { return true; } }");
			}

			if (node.FlagsSet != null && node.FlagsSet.Contains("S"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsSignFlagSet { get { return true; } }");
			}

			if (node.FlagsCleared != null && node.FlagsCleared.Contains("S"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsSignFlagCleared { get { return true; } }");
			}

			if (node.FlagsModified != null && (node.FlagsModified.Contains("S") || node.FlagsSet.Contains("S") || node.FlagsCleared.Contains("S")))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsSignFlagModified { get { return true; } }");
			}

			if (node.FlagsUndefined != null && node.FlagsUndefined.Contains("S"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsSignFlagUnchanged { get { return true; } }");
			}

			if (node.FlagsUnchanged != null && node.FlagsUndefined.Contains("S"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsSignFlagUndefined { get { return true; } }");
			}

			if (node.FlagsUsed != null && node.FlagsUsed.Contains("O"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsOverflowFlagUsed { get { return true; } }");
			}

			if (node.FlagsSet != null && node.FlagsSet.Contains("O"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsOverflowFlagSet { get { return true; } }");
			}

			if (node.FlagsCleared != null && node.FlagsCleared.Contains("O"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsOverflowFlagCleared { get { return true; } }");
			}

			if (node.FlagsModified != null && (node.FlagsModified.Contains("O") || node.FlagsSet.Contains("O") || node.FlagsCleared.Contains("O")))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsOverflowFlagModified { get { return true; } }");
			}

			if (node.FlagsUndefined != null && node.FlagsUndefined.Contains("O"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsOverflowFlagUnchanged { get { return true; } }");
			}

			if (node.FlagsUnchanged != null && node.FlagsUndefined.Contains("O"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsOverflowFlagUndefined { get { return true; } }");
			}

			if (node.FlagsUsed != null && node.FlagsUsed.Contains("P"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsParityFlagUsed { get { return true; } }");
			}

			if (node.FlagsSet != null && node.FlagsSet.Contains("P"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsParityFlagSet { get { return true; } }");
			}

			if (node.FlagsCleared != null && node.FlagsCleared.Contains("P"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsParityFlagCleared { get { return true; } }");
			}

			if (node.FlagsModified != null && (node.FlagsModified.Contains("P") || node.FlagsSet.Contains("P") || node.FlagsCleared.Contains("P")))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsParityFlagModified { get { return true; } }");
			}

			if (node.FlagsUndefined != null && node.FlagsUndefined.Contains("P"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsParityFlagUnchanged { get { return true; } }");
			}

			if (node.FlagsUnchanged != null && node.FlagsUndefined.Contains("P"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsParityFlagUndefined { get { return true; } }");
			}

			if (node.LogicalOpposite != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override BaseInstruction GetOpposite()");
				Lines.AppendLine("\t\t{");
				Lines.AppendLine("\t\t\treturn " + node.FamilyName + "." + node.LogicalOpposite + ";");
				Lines.AppendLine("\t\t}");
			}

			if (!string.IsNullOrWhiteSpace(node.ARMv6Emitter))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tprotected override void Emit(InstructionNode node, ARMv6CodeEmitter emitter)");
				Lines.AppendLine("\t\t{");

				if (!string.IsNullOrWhiteSpace(node.ARMv6Opcode))
				{
					Lines.AppendLine("\t\t\t" + node.ARMv6Emitter + "(node, emitter, Bits." + node.ARMv6Opcode + ");");
				}
				else
				{
					Lines.AppendLine("\t\t\t" + node.ARMv6Emitter + "(node, emitter);");
				}

				Lines.AppendLine("\t\t}");
			}

			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}
	}
}
