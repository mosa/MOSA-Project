// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildX86InstructionFiles : BuildBaseTemplate
	{
		private Dictionary<string, string> EncodingTemplates = new Dictionary<string, string>();

		public BuildX86InstructionFiles(string jsonFile, string destinationPath)
			: base(jsonFile, destinationPath)
		{
		}

		protected override void Iterator()
		{
			ReadEncodingTemplates();

			foreach (var entry in Entries.Instructions)
			{
				Lines.Clear();

				DestinationFile = entry.Name + ".cs";
				AddSourceHeader();
				Body(entry);
				Save();
			}
		}

		protected void ReadEncodingTemplates()
		{
			if (Entries.Encoding == null)
				return;

			foreach (var array in Entries.Encoding)
			{
				if (string.IsNullOrWhiteSpace(array))
					continue;

				var line = (string)array;

				var position = line.IndexOf('=');

				string name = line.Substring(0, position);
				string value = line.Substring(position + 1);

				EncodingTemplates.Add(name, value);
			}
		}

		protected override void Body(dynamic node = null)
		{
			int id = Identifiers.GetInstructionID();

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
			Lines.AppendLine("namespace Mosa.Platform.x86.Instructions");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.Append("\t/// " + node.Name);

			if (!string.IsNullOrWhiteSpace(node.Description))
			{
				Lines.Append(" - " + node.Description);
			}

			Lines.AppendLine();
			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine("\t/// <seealso cref=\"Mosa.Platform.x86.X86Instruction\" />");
			Lines.AppendLine("\tpublic sealed class " + node.Name + " : X86Instruction");
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

			if (node.X86EmitBytes != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic static readonly byte[] opcode = new byte[] { " + bytes + " };");
			}

			if (node.X86LegacyOpcode != null)
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

			if (node.ThreeTwoAddressConversion != null && node.ThreeTwoAddressConversion == "true")
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

				Lines.AppendLine("\t\t\t" + node.StaticEmitMethod.Replace("%", node.Name) + "(node, emitter);");
				Lines.AppendLine("\t\t}");
			}

			if (node.OpcodeEncoding != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override void Emit(InstructionNode node, BaseCodeEmitter emitter)");
				Lines.AppendLine("\t\t{");
				if (node.VariableOperands == null || node.VariableOperands == "false")
				{
					Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.ResultCount == " + node.ResultCount + ");");
					Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.OperandCount == " + node.OperandCount + ");");

					if (node.ThreeTwoAddressConversion == null || node.ThreeTwoAddressConversion == "true")
					{
						Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.Result.IsCPURegister);");
						Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.Operand1.IsCPURegister);");
						Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);");
					}
					Lines.AppendLine();
				}

				CreateEncoding(node);

				Lines.AppendLine("\t\t}");
			}

			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}

		private static string EncodeLegacyOpecodeRegField(dynamic node)
		{
			if (node.X86LegacyOpcodeRegField == null)
				return string.Empty;

			return ", 0x" + node.X86LegacyOpcodeRegField.Replace("0x", string.Empty);
		}

		private static string EncodeOperandsOrder(dynamic node)
		{
			if (string.IsNullOrWhiteSpace(node.X86LegacyOpcodeOperandOrder))
				return null;

			var sb = new StringBuilder();

			foreach (var c in node.X86LegacyOpcodeOperandOrder)
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
			if (node.X86LegacyOpcode == null)
				return null;

			string legacy = string.Empty;
			bool first = true;

			foreach (var b in node.X86LegacyOpcode.Split(' '))
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
			if (node.X86EmitBytes == null)
				return null;

			string bytes = string.Empty;
			bool first = true;

			foreach (var b in node.X86EmitBytes.Split(' '))
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

		private void CreateEncoding(dynamic node)
		{
			bool first = true;
			int count = 0;

			foreach (var entry in node.OpcodeEncoding)
			{
				string end = entry.End;
				string comment = entry.Comment;

				if (first)
				{
					first = false;
				}
				else
				{
					Lines.AppendLine();
				}

				if (!String.IsNullOrEmpty(comment))
				{
					Lines.Append("\t\t\t // ");
					Lines.AppendLine(comment);
				}

				var condition = DecodeExperimentalCondition(entry.Condition) ?? string.Empty;
				var encoding = DecodeExperimentalEncoding(entry.Encoding, node.OpcodeEncodingAppend);

				count++;
				if (!String.IsNullOrEmpty(condition))
				{
					EmitCondition(condition, encoding, true, 0);
				}
				else
				{
					EmitBits(encoding, 0);
				}
			}

			if (count > 1)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\t\tthrow new Compiler.Common.Exceptions.CompilerException(\"Invalid Opcode\");");
			}
		}

		private string ReduceEncoding(string template)
		{
			if (string.IsNullOrWhiteSpace(template))
				return string.Empty;

			string encoding = string.Empty;

			var parts = template.Split(',');

			foreach (var p in parts)
			{
				var part = p.Trim(' ');  // trim away spaces

				if (string.IsNullOrWhiteSpace(part))
					continue;

				if (part.StartsWith("[") && part.Contains("|"))
				{
					if (string.IsNullOrEmpty(encoding))
						encoding = part;
					else
						encoding = encoding + '|' + part;

					continue;
				}
				else if (part.StartsWith("["))
				{
					// template
					string subTemplate = string.Empty;

					EncodingTemplates.TryGetValue(part, out subTemplate);

					if (!string.IsNullOrWhiteSpace(subTemplate))
					{
						if (string.IsNullOrEmpty(encoding))
							encoding = ReduceEncoding(subTemplate);
						else
							encoding = encoding + '|' + ReduceEncoding(subTemplate);
					}
					continue;
				}

				if (parts.Length == 1)
				{
					encoding = part;
				}
				else
				{
					// substitution
					var split = part.Split('=');

					encoding = encoding.Replace('[' + split[0] + ']', split[1]);
				}
			}

			Debug.Assert(!encoding.Contains("="));
			Debug.Assert(!encoding.Contains(","));

			return encoding;
		}

		private string DecodeExperimentalEncoding(string line, string append)
		{
			if (string.IsNullOrWhiteSpace(line))
				return null;

			string rawEncoding = line.Trim();

			if (!string.IsNullOrWhiteSpace(append))
			{
				rawEncoding = rawEncoding + "," + append.Trim();
			}

			return ReduceEncoding(rawEncoding);
		}

		private string DecodeExperimentalCondition(string condition)
		{
			if (string.IsNullOrWhiteSpace(condition))
				return null;

			string expression = string.Empty;

			var parts = condition.Split(']');
			for (int i = 0; i < parts.Length; i++)
			{
				var part = parts[i];
				var normalized = part.Replace(" ", string.Empty).TrimStart('[').ToLower();

				if (string.IsNullOrWhiteSpace(normalized))
					continue;

				var subparts = normalized.Split(':');

				var orexpression = string.Empty;

				foreach (var subpart in subparts)
				{
					var operand = string.Empty;

					if (string.IsNullOrEmpty(subpart))
						continue;

					if (subpart.StartsWith("#"))
						continue;

					var opp = subpart.StartsWith("!");

					var subpart2 = opp ? subpart.Substring(1) : subpart;

					switch (i)
					{
						case 0: operand = "Operand1"; break;
						case 1: operand = "Operand2"; break;
						case 2: operand = "Operand3"; break;
						default: operand = "GetOperand(" + i.ToString() + ")"; break;
					}

					string cond1 = string.Empty;
					string cond2 = string.Empty;
					string cond3 = string.Empty;

					switch (subpart2.ToLower())
					{
						case "skip": continue;
						case "ignore": continue;
						case "register": cond1 = ".IsCPURegister"; break;
						case "constant": cond1 = ".IsConstant"; break;
						case "eax": cond1 = ".IsCPURegister"; cond2 = ".Register.RegisterCode == 0"; break;
						case "ecx": cond1 = ".IsCPURegister"; cond2 = ".Register.RegisterCode == 1"; break;
						case "edx": cond1 = ".IsCPURegister"; cond2 = ".Register.RegisterCode == 2"; break;
						case "ebx": cond1 = ".IsCPURegister"; cond2 = ".Register.RegisterCode == 3"; break;
						case "esp": cond1 = ".IsCPURegister"; cond2 = ".Register.RegisterCode == 4"; break;
						case "ebp": cond1 = ".IsCPURegister"; cond2 = ".Register.RegisterCode == 5"; break;
						case "esi": cond1 = ".IsCPURegister"; cond2 = ".Register.RegisterCode == 6"; break;
						case "edi": cond1 = ".IsCPURegister"; cond2 = ".Register.RegisterCode == 7"; break;
						case "zero":
						case "0": cond1 = ".IsConstantZero"; break;
						case "one":
						case "1": cond1 = ".IsConstantOne"; break;
						case "sbyte":
						case "signedbyte": cond1 = ".IsConstant"; cond2 = ".ConstantSignedInteger >= " + sbyte.MinValue.ToString(); cond3 = ".ConstantSignedInteger <= " + sbyte.MaxValue.ToString(); break;
						case "signedshort": cond1 = ".IsConstant"; cond2 = ".ConstantSignedInteger >= " + short.MinValue.ToString(); cond3 = ".ConstantSignedInteger <= " + short.MaxValue.ToString(); break;
						case "signint": cond1 = ".IsConstant"; cond2 = ".ConstantSignedInteger >= " + int.MinValue.ToString(); cond3 = ".ConstantSignedInteger <= " + int.MaxValue.ToString(); break;
					}

					string subexpression = "node." + operand + cond1;

					if (!string.IsNullOrWhiteSpace(cond3))
					{
						subexpression = "(" + subexpression + " && node." + operand + cond2 + " && node." + operand + cond3 + ")";
					}
					else if (!string.IsNullOrWhiteSpace(cond2))
					{
						subexpression = "(" + subexpression + " && node." + operand + cond2 + ")";
					}

					if (opp)
					{
						subexpression = "!" + subexpression;
					}

					if (string.IsNullOrWhiteSpace(orexpression))
					{
						orexpression = subexpression;
					}
					else
					{
						orexpression = orexpression + " || " + subexpression;
					}
				}

				if (string.IsNullOrWhiteSpace(expression))
				{
					expression = orexpression;
				}
				else
				{
					expression = expression + " && " + orexpression;
				}
			}

			return expression;
		}

		private void EmitCondition(string condition, string encoding, bool end, int index = 0)
		{
			var tabs = "\t\t\t\t\t\t\t\t\t\t".Substring(0, index + 3);
			Lines.Append(tabs);

			Lines.AppendLine("if (" + condition + ")");

			Lines.Append(tabs);
			Lines.AppendLine("{");

			EmitBits(encoding, index + 1);

			if (end)
			{
				Lines.Append(tabs);
				Lines.AppendLine("\treturn;");
			}

			Lines.Append(tabs);
			Lines.AppendLine("}");
		}

		private void EmitBits(string bits, int index = 0)
		{
			var steps = bits.Split('|');
			var tabs = "\t\t\t\t\t\t\t\t\t\t".Substring(0, index + 3);

			foreach (var s in steps)
			{
				if (string.IsNullOrWhiteSpace(s))
					continue;

				if (s.StartsWith("[")) // ignore these
					continue;
				else if (s.StartsWith("0x") | s.StartsWith("x"))
				{
					// hex
					string hex = s.StartsWith("x") ? s.Substring(1) : s.Substring(2);

					Lines.Append(tabs);

					switch (hex.Length)
					{
						case 1:
							Lines.AppendLine("emitter.OpcodeEncoder.AppendNibble(0x" + hex + ");");
							break;

						case 2:
							Lines.AppendLine("emitter.OpcodeEncoder.AppendByte(0x" + hex + ");");
							break;

						case 3:
							Lines.AppendLine("emitter.OpcodeEncoder.AppendByte(0x" + hex.Substring(0, 2) + ");");
							Lines.Append(tabs);
							Lines.AppendLine("emitter.OpcodeEncoder.AppendNibble(0x" + hex.Substring(1) + ");");
							break;

						case 4:
							Lines.AppendLine("emitter.OpcodeEncoder.AppendShort(0x" + hex + ");");
							break;

						case 5:
							Lines.AppendLine("emitter.OpcodeEncoder.AppendShort(0x" + hex.Substring(0, 4) + ");");
							Lines.Append(tabs);
							Lines.AppendLine("emitter.OpcodeEncoder.AppendNibble(0x" + hex.Substring(5) + ");");
							break;

						default: throw new Exception("ERROR!");
					}
				}
				else if (s.StartsWith("0b") | s.StartsWith("b") | s.StartsWith("0") | s.StartsWith("1"))
				{
					// binary
					string binary = s;

					if (binary.StartsWith("0b"))
						binary = s.Substring(2);

					if (binary.StartsWith("b"))
						binary = s.Substring(1);

					Lines.Append(tabs);

					switch (binary.Length)
					{
						case 1:
							Lines.AppendLine("emitter.OpcodeEncoder.AppendBit(0b" + binary + ");");
							break;

						case 2:
							Lines.AppendLine("emitter.OpcodeEncoder.Append2Bits(0b" + binary + ");");
							break;

						case 3:
							Lines.AppendLine("emitter.OpcodeEncoder.Append3Bits(0b" + binary + ");");
							break;

						case 4:
							Lines.AppendLine("emitter.OpcodeEncoder.AppendNibble(0b" + binary + ");");
							break;

						case 5:
							Lines.AppendLine("emitter.OpcodeEncoder.AppendNibble(0b" + binary.Substring(0, 4) + ");");
							Lines.Append(tabs);
							Lines.AppendLine("emitter.OpcodeEncoder.AppendBit(0b" + binary.Substring(4) + ");");
							break;

						case 6:
							Lines.AppendLine("emitter.OpcodeEncoder.AppendNibble(0b" + binary.Substring(0, 4) + ");");
							Lines.Append(tabs);
							Lines.AppendLine("emitter.OpcodeEncoder.Append2Bits(0b" + binary.Substring(4) + ");");
							break;

						case 7:
							Lines.AppendLine("emitter.OpcodeEncoder.AppendNibble(0b" + binary.Substring(0, 4) + ");");
							Lines.Append(tabs);
							Lines.AppendLine("emitter.OpcodeEncoder.Append3Bits(0b" + binary.Substring(4) + ");");
							break;

						case 8:
							Lines.AppendLine("emitter.OpcodeEncoder.AppendNibble(0b" + binary.Substring(0, 4) + ");");
							Lines.Append(tabs);
							Lines.AppendLine("emitter.OpcodeEncoder.AppendNibble(0b" + binary.Substring(4) + ");");
							break;

						default: throw new Exception("ERROR!");
					}
				}
				else
				{
					var parts = s.Split(':');

					var code = string.Empty;
					var postcode = string.Empty;

					GetCodes(parts[0], ref code, ref postcode);

					var operand = (parts.Length > 1) ? GetOperand(parts[1]) : string.Empty;
					var operand2 = (parts.Length > 2) ? GetOperand(parts[2]) : null;

					Lines.Append(tabs);

					if (operand2 == null)
					{
						Lines.AppendLine("emitter.OpcodeEncoder." + code + "(node." + operand + postcode + ");");
					}
					else
					{
						Lines.AppendLine("emitter.OpcodeEncoder." + code + "(node." + operand + postcode + ", node." + operand2 + ");");
					}
				}
			}
		}

		private static string GetOperand(string part)
		{
			switch (part)
			{
				case "o1": return "Operand1";
				case "o2": return "Operand2";
				case "o3": return "Operand3";
				case "o4": return "GetOperand(3)";
				case "r": return "Result";
				case "r1": return "Result";
				case "r2": return "Result2";
				case "label": return "BranchTargets[0].Label";
				case "cond": return string.Empty; // TODO
				case "shifter": return string.Empty; // TODO
				case "": return string.Empty;

				default: throw new Exception("ERROR!");
			}
		}

		private static void GetCodes(string part, ref string code, ref string postcode)
		{
			postcode = string.Empty;

			switch (part)
			{
				case "reg3": code = "Append3Bits"; postcode = ".Register.RegisterCode"; return;
				case "regx4": code = "Append1Bit"; postcode = ".Register.RegisterCode"; return;
				case "reg4": code = "AppendNibble"; postcode = ".Register.RegisterCode"; return;
				case "imm32": code = "Append32BitImmediate"; return;
				case "imm32+": code = "Append32BitImmediateWithOffset"; return;
				case "imm8": code = "Append8BitImmediate"; return;
				case "rel32": code = "EmitRelative32"; return;
				case "rel64": code = "EmitRelative64"; return;
				case "": return;

				default: throw new Exception("ERROR!");
			}
		}
	}
}
