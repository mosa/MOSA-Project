// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildX64InstructionFiles : BuildBaseTemplate
	{
		public BuildX64InstructionFiles(string jsonFile, string destinationPath)
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
			int id = GenerateInstructionID.GetInstructionID();

			string bytes = EncodeOpcodeBytes(node);
			string legacy = EncodeLegacyOpcode(node);
			string reg = EncodeLegacyOpecodeRegField(node);
			string operands = EncodeOperandsOrder(node);

			Lines.AppendLine("using Mosa.Compiler.Framework;");

			if (node.ResultType != null || node.ResultType2 != null)
			{
				Lines.AppendLine("using Mosa.Compiler.MosaTypeSystem;");
			}

			Lines.AppendLine();
			Lines.AppendLine("namespace Mosa.Platform.x64.Instructions");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.Append("\t/// " + node.Name);

			if (!string.IsNullOrWhiteSpace(node.Description))
			{
				Lines.Append(" - " + node.Description);
			}

			Lines.AppendLine();
			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine("\t/// <seealso cref=\"Mosa.Platform.x64.X64Instruction\" />");
			Lines.AppendLine("\tpublic sealed class " + node.Name + " : X64Instruction");
			Lines.AppendLine("\t{");
			Lines.AppendLine("\t\tpublic override int ID { get { return " + id.ToString() + "; } }");
			Lines.AppendLine();
			Lines.AppendLine("\t\tinternal " + node.Name + "()");
			Lines.AppendLine("\t\t\t: base(" + node.ResultCount + ", " + node.OperandCount + ")");
			Lines.AppendLine("\t\t{");
			Lines.AppendLine("\t\t}");

			if (node.AlternativeName != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override string AlternativeName { get { return \"" + node.AlternativeName + "\"; } }");
			}

			if (node.X64EmitBytes != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic static readonly byte[] opcode = new byte[] { " + bytes + " };");
			}

			if (node.X64LegacyOpcode != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic static readonly LegacyOpCode LegacyOpcode = new LegacyOpCode(new byte[] { " + legacy + " }" + reg + ");");
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

			if (node.X64ThreeTwoAddressConversion != null && node.X64ThreeTwoAddressConversion == "true")
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

			if (node.X64EmitBytes != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override void Emit(InstructionNode node, BaseCodeEmitter emitter)");
				Lines.AppendLine("\t\t{");

				if (node.VariableOperands == null || node.VariableOperands == "false")
				{
					Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.ResultCount == " + node.ResultCount + ");");
					Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.OperandCount == " + node.OperandCount + ");");

					if (node.X64EmitRelativeBranchTarget != null || node.X64EmitRelativeBranchTarget == "true")
					{
						Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.BranchTargets.Count >= 1);");
						Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.BranchTargets[0] != null);");
					}
					Lines.AppendLine();
				}

				Lines.AppendLine("\t\t\temitter.Write(opcode);");

				if (node.X64EmitRelativeBranchTarget != null || node.X64EmitRelativeBranchTarget == "true")
				{
					Lines.AppendLine("\t\t\t(emitter as X64CodeEmitter).EmitRelativeBranchTarget(node.BranchTargets[0].Label);");
				}

				Lines.AppendLine("\t\t}");
			}

			if (node.StaticEmitMethod != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override void Emit(InstructionNode node, BaseCodeEmitter emitter)");
				Lines.AppendLine("\t\t{");

				if (node.VariableOperands == null || node.VariableOperands == "false")
				{
					Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.ResultCount == DefaultResultCount);");
					Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.OperandCount == DefaultOperandCount);");
					Lines.AppendLine();
				}

				Lines.AppendLine("\t\t\t//" + node.StaticEmitMethod.Replace("%", node.Name) + "(node, emitter);");
				Lines.AppendLine("\t\t}");
			}

			if (node.X64LegacyOpcodeOperandOrder != null && node.X64LegacyOpcode != null && node.StaticEmitMethod == null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tinternal override void EmitLegacy(InstructionNode node, X64CodeEmitter emitter)");
				Lines.AppendLine("\t\t{");
				if (node.VariableOperands == null || node.VariableOperands == "false")
				{
					Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.ResultCount == " + node.ResultCount + ");");
					Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.OperandCount == " + node.OperandCount + ");");

					if (node.X64ThreeTwoAddressConversion == null || node.X64ThreeTwoAddressConversion == "true")
					{
						Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.Result.IsCPURegister);");
						Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.Operand1.IsCPURegister);");
						Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);");
					}
					Lines.AppendLine();
				}

				if (operands == null)
				{
					Lines.AppendLine("\t\t\temitter.Emit(LegacyOpcode);");
				}
				else
				{
					Lines.AppendLine("\t\t\temitter.Emit(LegacyOpcode, " + operands + ");");
				}
				Lines.AppendLine("\t\t}");
			}

			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}

		private static string EncodeLegacyOpecodeRegField(dynamic node)
		{
			if (node.X64LegacyOpcodeRegField == null)
				return string.Empty;

			return ", 0x" + node.X64LegacyOpcodeRegField.Replace("0x", string.Empty);
		}

		private static string EncodeOperandsOrder(dynamic node)
		{
			if (string.IsNullOrWhiteSpace(node.X64LegacyOpcodeOperandOrder))
				return null;

			var sb = new StringBuilder();

			foreach (var c in node.X64LegacyOpcodeOperandOrder)
			{
				if (c == 'R' || c == 'r')
				{
					sb.Append("node.Result");
				}
				else if (c == '1')
				{
					sb.Append("node.Operand1");
				}
				else if (c == '2')
				{
					sb.Append("node.Operand2");
				}
				else if (c == '3')
				{
					sb.Append("node.Operand3");
				}
				else if (c == 'N' || c == 'n')
				{
					sb.Append("node.Result");
				}
				sb.Append(", ");
			}

			sb.Length--;
			sb.Length--;

			return sb.ToString();
		}

		private static string EncodeLegacyOpcode(dynamic node)
		{
			if (node.X64LegacyOpcode == null)
				return null;

			string legacy = string.Empty;
			bool first = true;

			foreach (var b in node.X64LegacyOpcode.Split(' '))
			{
				string b2 = b.Replace("0x", string.Empty).Replace(",", string.Empty);

				if (!first)
				{
					legacy += ", ";
				}

				legacy = legacy + "0x" + b2;

				first = false;
			}

			return legacy;
		}

		private static string EncodeOpcodeBytes(dynamic node)
		{
			if (node.X64EmitBytes == null)
				return null;

			string bytes = string.Empty;
			bool first = true;

			foreach (var b in node.X64EmitBytes.Split(' '))
			{
				string b2 = b.Replace("0x", string.Empty).Replace(",", string.Empty);

				if (!first)
				{
					bytes += ", ";
				}

				bytes = bytes + "0x" + b2;

				first = false;
			}

			return bytes;
		}
	}
}
