// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildCommonInstructionFiles : BuildBaseTemplate
	{
		protected virtual string Platform { get; }

		protected string NormalizedPlatform { get { return Platform.Substring(0, 1).ToUpper() + Platform.Substring(1); } }

		private Dictionary<string, string> EncodingTemplates = new Dictionary<string, string>();

		public BuildCommonInstructionFiles(string jsonFile, string destinationPath)
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
			//int id = Identifiers.GetInstructionID();

			Lines.AppendLine("using Mosa.Compiler.Framework;");

			if (node.ResultType != null || node.ResultType2 != null)
			{
				Lines.AppendLine("using Mosa.Compiler.MosaTypeSystem;");
			}

			Lines.AppendLine();
			Lines.AppendLine("namespace Mosa.Platform." + Platform + ".Instructions");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.Append("\t/// " + node.Name);

			if (!string.IsNullOrWhiteSpace(node.Description))
			{
				Lines.Append(" - " + node.Description);
			}

			Lines.AppendLine();
			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine("\t/// <seealso cref=\"Mosa.Platform." + Platform + "." + NormalizedPlatform + "Instruction\" />");
			Lines.AppendLine("\tpublic sealed class " + node.Name + " : " + NormalizedPlatform + "Instruction");
			Lines.AppendLine("\t{");
			Lines.AppendLine("\t\tinternal " + node.Name + "()");
			Lines.AppendLine("\t\t\t: base(" + node.ResultCount + ", " + node.OperandCount + ")");
			Lines.AppendLine("\t\t{");
			Lines.AppendLine("\t\t}");

			var FlagsUsed = node.FlagsUsed == null ? string.Empty : node.FlagsUsed.ToUpper(); // tested_f
			var FlagsSet = node.FlagsSet == null ? string.Empty : node.FlagsSet.ToUpper(); // values_f (upper=set, lower=cleared)
			var FlagsCleared = node.FlagsCleared == null ? string.Empty : node.FlagsCleared.ToUpper(); // above
			var FlagsModified = node.FlagsModified == null ? string.Empty : node.FlagsModified.ToUpper(); // modif_f
			var FlagsUndefined = node.FlagsUndefined == null ? string.Empty : node.FlagsUndefined.ToUpper(); // undef_f

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

			if (node.ThreeTwoAddressConversion != null && node.ThreeTwoAddressConversion == "true")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool ThreeTwoAddressConversion { get { return true; } }");
			}

			if (FlagsUsed.Contains("Z") || FlagsUsed == "CONDITIONAL")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsZeroFlagUsed { get { return true; } }");
			}

			if (FlagsSet.Contains("Z"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsZeroFlagSet { get { return true; } }");
			}

			if (FlagsCleared.Contains("Z"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsZeroFlagCleared { get { return true; } }");
			}

			if (FlagsModified.Contains("Z") || FlagsSet.Contains("Z") || FlagsCleared.Contains("Z"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsZeroFlagModified { get { return true; } }");
			}

			if (FlagsUndefined.Contains("Z"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsZeroFlagUnchanged { get { return true; } }");
			}

			if (FlagsUndefined.Contains("Z"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsZeroFlagUndefined { get { return true; } }");
			}

			if (FlagsUsed.Contains("C") || FlagsUsed == "CONDITIONAL")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsCarryFlagUsed { get { return true; } }");
			}

			if (FlagsSet.Contains("C"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsCarryFlagSet { get { return true; } }");
			}

			if (FlagsCleared.Contains("C"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsCarryFlagCleared { get { return true; } }");
			}

			if (FlagsModified.Contains("C") || FlagsSet.Contains("C") || FlagsCleared.Contains("C"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsCarryFlagModified { get { return true; } }");
			}

			if (FlagsUndefined.Contains("C"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsCarryFlagUnchanged { get { return true; } }");
			}

			if (FlagsUndefined.Contains("C"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsCarryFlagUndefined { get { return true; } }");
			}

			if (FlagsUsed.Contains("S") || FlagsUsed.Contains("N") || FlagsUsed == "CONDITIONAL")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsSignFlagUsed { get { return true; } }");
			}

			if (FlagsSet.Contains("S") || FlagsSet.Contains("N"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsSignFlagSet { get { return true; } }");
			}

			if (FlagsCleared.Contains("S") || FlagsCleared.Contains("N"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsSignFlagCleared { get { return true; } }");
			}

			if (FlagsModified.Contains("S") || FlagsSet.Contains("S") || FlagsCleared.Contains("S")
				|| FlagsModified.Contains("N") || FlagsSet.Contains("N") || FlagsCleared.Contains("N")
				)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsSignFlagModified { get { return true; } }");
			}

			if (FlagsUndefined.Contains("S") || FlagsUndefined.Contains("N"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsSignFlagUnchanged { get { return true; } }");
			}

			if (FlagsUndefined.Contains("S") || FlagsUndefined.Contains("N"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsSignFlagUndefined { get { return true; } }");
			}

			if (FlagsUsed.Contains("O") || FlagsUsed.Contains("V") || FlagsUsed == "CONDITIONAL")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsOverflowFlagUsed { get { return true; } }");
			}

			if (FlagsSet.Contains("O") || FlagsSet.Contains("V"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsOverflowFlagSet { get { return true; } }");
			}

			if (FlagsCleared.Contains("O") || FlagsCleared.Contains("V"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsOverflowFlagCleared { get { return true; } }");
			}

			if (FlagsModified.Contains("O") || FlagsSet.Contains("O") || FlagsCleared.Contains("O")
				|| FlagsModified.Contains("V") || FlagsSet.Contains("V") || FlagsCleared.Contains("V"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsOverflowFlagModified { get { return true; } }");
			}

			if (FlagsUndefined.Contains("O") || FlagsUndefined.Contains("V"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsOverflowFlagUnchanged { get { return true; } }");
			}

			if (FlagsUndefined.Contains("O") || FlagsUndefined.Contains("V"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsOverflowFlagUndefined { get { return true; } }");
			}

			if (FlagsUsed.Contains("P") || FlagsUsed == "CONDITIONAL")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsParityFlagUsed { get { return true; } }");
			}

			if (FlagsSet.Contains("P"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsParityFlagSet { get { return true; } }");
			}

			if (FlagsCleared.Contains("P"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsParityFlagCleared { get { return true; } }");
			}

			if (FlagsModified.Contains("P") || FlagsSet.Contains("P") || FlagsCleared.Contains("P"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsParityFlagModified { get { return true; } }");
			}

			if (FlagsUndefined.Contains("P"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsParityFlagUnchanged { get { return true; } }");
			}

			if (FlagsUndefined.Contains("P"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsParityFlagUndefined { get { return true; } }");
			}

			if (FlagsUsed != null && FlagsUsed == "CONDITIONAL")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool AreFlagUseConditional { get { return true; } }");
			}

			if (node.StaticEmitMethod != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)");
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
				Lines.AppendLine("\t\tpublic override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)");
				Lines.AppendLine("\t\t{");
				if (node.VariableOperands == null || node.VariableOperands == "false")
				{
					Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.ResultCount == " + node.ResultCount + ");");
					Lines.AppendLine("\t\t\tSystem.Diagnostics.Debug.Assert(node.OperandCount == " + node.OperandCount + ");");

					if (node.ThreeTwoAddressConversion != null && node.ThreeTwoAddressConversion == "true")
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

		private void CreateEncoding(dynamic node)
		{
			bool first = true;
			bool cond = false;

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

				encoding = encoding.Replace("\t", string.Empty);

				if (!String.IsNullOrEmpty(condition))
				{
					cond = true;
					EmitCondition(condition, encoding, true, 0);
				}
				else
				{
					EmitBits(encoding, 0);
				}
			}

			if (cond)
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
					if (!part.Contains("="))
					{
						encoding += part;
					}
					else
					{
						// substitution
						var split = part.Split('=');

						encoding = encoding.Replace('[' + split[0] + ']', split[1]);
					}
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
						case "signedbyte": cond1 = ".IsConstant"; cond2 = ".ConstantSigned32 >= " + sbyte.MinValue.ToString(); cond3 = ".ConstantSigned32 <= " + sbyte.MaxValue.ToString(); break;
						case "signedshort": cond1 = ".IsConstant"; cond2 = ".ConstantSigned32 >= " + short.MinValue.ToString(); cond3 = ".ConstantSigned32 <= " + short.MaxValue.ToString(); break;
						case "signint": cond1 = ".IsConstant"; cond2 = ".ConstantSigned32 >= " + int.MinValue.ToString(); cond3 = ".ConstantSigned32 <= " + int.MaxValue.ToString(); break;
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

				if (s.StartsWith("0x") | s.StartsWith("x"))
				{
					// hex
					string hex = s.StartsWith("x") ? s.Substring(1) : s.Substring(2);

					Lines.Append(tabs);

					switch (hex.Length)
					{
						case 1:
							Lines.AppendLine("opcodeEncoder.Append4Bits(0x" + hex + ");");
							break;

						case 2:
							Lines.AppendLine("opcodeEncoder.Append8Bits(0x" + hex + ");");
							break;

						case 3:
							Lines.AppendLine("opcodeEncoder.Append8Bits(0x" + hex.Substring(0, 2) + ");");
							Lines.Append(tabs);
							Lines.AppendLine("opcodeEncoder.Append4Bits(0x" + hex.Substring(1) + ");");
							break;

						case 4:
							Lines.AppendLine("opcodeEncoder.AppendShort(0x" + hex + ");");
							break;

						case 5:
							Lines.AppendLine("opcodeEncoder.AppendShort(0x" + hex.Substring(0, 4) + ");");
							Lines.Append(tabs);
							Lines.AppendLine("opcodeEncoder.Append4Bits(0x" + hex.Substring(5) + ");");
							break;

						case 8:
							Lines.AppendLine("opcodeEncoder.Append32Bits(0x" + hex + ");");
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
							Lines.AppendLine("opcodeEncoder.Append1Bit(0b" + binary + ");");
							break;

						case 2:
							Lines.AppendLine("opcodeEncoder.Append2Bits(0b" + binary + ");");
							break;

						case 3:
							Lines.AppendLine("opcodeEncoder.Append3Bits(0b" + binary + ");");
							break;

						case 4:
							Lines.AppendLine("opcodeEncoder.Append4Bits(0b" + binary + ");");
							break;

						case 5:
							Lines.AppendLine("opcodeEncoder.Append4Bits(0b" + binary.Substring(0, 4) + ");");
							Lines.Append(tabs);
							Lines.AppendLine("opcodeEncoder.Append1Bit(0b" + binary.Substring(4) + ");");
							break;

						case 6:
							Lines.AppendLine("opcodeEncoder.Append4Bits(0b" + binary.Substring(0, 4) + ");");
							Lines.Append(tabs);
							Lines.AppendLine("opcodeEncoder.Append2Bits(0b" + binary.Substring(4) + ");");
							break;

						case 7:
							Lines.AppendLine("opcodeEncoder.Append4Bits(0b" + binary.Substring(0, 4) + ");");
							Lines.Append(tabs);
							Lines.AppendLine("opcodeEncoder.Append3Bits(0b" + binary.Substring(4) + ");");
							break;

						case 8:
							Lines.AppendLine("opcodeEncoder.Append4Bits(0b" + binary.Substring(0, 4) + ");");
							Lines.Append(tabs);
							Lines.AppendLine("opcodeEncoder.Append4Bits(0b" + binary.Substring(4) + ");");
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
						Lines.AppendLine("opcodeEncoder." + code + "(" + operand + postcode + ");");
					}
					else
					{
						Lines.AppendLine("opcodeEncoder." + code + "(" + operand + postcode + ", " + operand2 + ");");
					}
				}
			}
		}

		private static void GetCodes(string part, ref string code, ref string postcode)
		{
			postcode = string.Empty;

			switch (part)
			{
				case "reg3": code = "Append3Bits"; postcode = ".Register.RegisterCode"; return;
				case "reg3s1": code = "Append3Bits("; postcode = ".Register.RegisterCode >> 1) & 0b111"; return;
				case "reg4x": code = "Append1Bit("; postcode = ".Register.RegisterCode >> 3) & 0x1"; return;
				case "reg4": code = "Append4Bits"; postcode = ".Register.RegisterCode"; return;
				case "reg5": code = "Append5Bits"; postcode = ".Register.RegisterCode"; return;
				case "reg6": code = "Append6Bits"; postcode = ".Register.RegisterCode"; return;
				case "imm1": code = "Append1BitImmediate"; return;
				case "imm2": code = "Append2BitImmediate"; return;
				case "imm4": code = "Append4BitImmediate"; return;
				case "imm4hn": code = "Append4BitImmediateHighNibble"; return;
				case "imm5": code = "Append5BitImmediate"; return;
				case "imm8": code = "Append8BitImmediate"; return;
				case "imm12": code = "Append12BitImmediate"; return;
				case "imm16": code = "Append16BitImmediate"; return;
				case "imm32": code = "Append32BitImmediate"; return;
				case "imm32+": code = "Append32BitImmediateWithOffset"; return;
				case "imm64": code = "Append64BitImmediate"; return;
				case "rel24": code = "EmitRelative24"; return;
				case "rel32": code = "EmitRelative32"; return;
				case "rel64": code = "EmitRelative64"; return;
				case "forward32": code = "EmitForward32"; return;
				case "supress8": code = "SuppressByte"; return;
				case "conditional": code = "Append4Bits"; postcode = "GetConditionCode(node.ConditionCode)"; return;
				case "status": code = "Append1Bit"; postcode = " == StatusRegister.Set ? 1 : 0"; return;
				case "updir": code = "Append1Bit"; postcode = " == StatusRegister.UpDirection ? 1 : 0"; return;
				case "downdir": code = "Append1Bit"; postcode = " == StatusRegister.DownDirection ? 1 : 0"; return;
				case "fp": code = "Append1Bit"; postcode = ".IsR4 ? 0 : 1"; return;

				//case "signed": code = "Append1Bit"; postcode = ".ConstantSigned64 < 0 ? 1 : 0"; return;
				//case "imm12unsigned": code = "Append12BitImmediate"; postcode = ""; return;

				case "": return;

				default: break;
			}

			if (part.StartsWith("reg("))
			{
				int open = part.IndexOf('(');
				int comma = part.IndexOf('-');
				int closed = part.IndexOf(')');

				if (open >= 0)
				{
					int start = Convert.ToInt32(part.Substring(open + 1, comma - open - 1).Trim());
					int end = comma < 0 ? start : Convert.ToInt32(part.Substring(comma + 1, closed - comma - 1).Trim());
					int length = end - start + 1;

					if (start == 0)
					{
						postcode = ".Register.RegisterCode) & 0x" + ("111111111111111111111111111111".Substring(0, length));
					}
					else
					{
						postcode = ".Register.RegisterCode >> " + start.ToString() + ") & 0x" + ("111111111111111111111111111111".Substring(0, length));
					}

					switch (length)
					{
						case 1: code = "Append1Bit("; break;
						case 2: code = "Append2Bits("; break;
						case 3: code = "Append3Bits("; break;
						case 4: code = "Append4Bits("; break;
						case 5: code = "Append5Bits("; break;
						case 6: code = "Append6Bits("; break;
						case 7: code = "Append7Bits("; break;
						case 8: code = "Append8Bits("; break;
						default: code = "AppendBits("; postcode += ", " + length.ToString(); break;
					}

					return;
				}
			}

			throw new Exception("ERROR!");
		}

		private static string GetOperand(string part)
		{
			switch (part)
			{
				case "o1": return "node.Operand1";
				case "o2": return "node.Operand2";
				case "o3": return "node.Operand3";
				case "o4": return "node.GetOperand(3)";
				case "r": return "node.Result";
				case "r1": return "node.Result";
				case "r2": return "node.Result2";
				case "label": return "node.BranchTargets[0].Label";

				case "status": return "node.StatusRegister";

				default: return part; // pass as is
			}
		}
	}
}
