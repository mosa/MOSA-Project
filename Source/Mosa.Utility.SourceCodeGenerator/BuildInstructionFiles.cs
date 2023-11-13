// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Utility.SourceCodeGenerator;

public class BuildInstructionFiles : BuildBaseTemplate
{
	protected virtual string Platform { get; }

	protected string NormalizedPlatform => Platform.Substring(0, 1).ToUpperInvariant() + Platform.Substring(1);

	private readonly Dictionary<string, string> EncodingTemplates = new();

	public BuildInstructionFiles(string jsonFile, string destinationPath, string platform)
		: base(jsonFile, destinationPath)
	{
		Platform = platform;
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

			var name = line.Substring(0, position);
			var value = line.Substring(position + 1);

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
		Lines.AppendLine($"namespace Mosa.Compiler.{Platform}.Instructions;");
		Lines.AppendLine();
		Lines.AppendLine("/// <summary>");
		Lines.Append("/// " + node.Name);

		if (!string.IsNullOrWhiteSpace(node.Description))
		{
			Lines.Append(" - " + node.Description);
		}

		Lines.AppendLine();
		Lines.AppendLine("/// </summary>");
		Lines.AppendLine("public sealed class " + node.Name + " : " + NormalizedPlatform + "Instruction");
		Lines.AppendLine("{");
		Lines.AppendLine("\tinternal " + node.Name + "()");
		Lines.AppendLine("\t\t: base(" + node.ResultCount + ", " + node.OperandCount + ")");
		Lines.AppendLine("\t{");
		Lines.AppendLine("\t}");

		var FlagsUsed = node.FlagsUsed == null ? string.Empty : node.FlagsUsed.ToUpperInvariant(); // tested_f
		var FlagsSet = node.FlagsSet == null ? string.Empty : node.FlagsSet.ToUpperInvariant(); // values_f (upper=set, lower=cleared)
		var FlagsCleared = node.FlagsCleared == null ? string.Empty : node.FlagsCleared.ToUpperInvariant(); // above
		var FlagsModified = node.FlagsModified == null ? string.Empty : node.FlagsModified.ToUpperInvariant(); // modif_f
		var FlagsUndefined = node.FlagsUndefined == null ? string.Empty : node.FlagsUndefined.ToUpperInvariant(); // undef_f

		if (node.AlternativeName != null)
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override string AlternativeName => \"" + node.AlternativeName + "\";");
		}

		//if (node.FlowControl != null && node.FlowControl != "Next")
		//{
		//	Lines.AppendLine();
		//	Lines.AppendLine("\tpublic override FlowControl FlowControl => FlowControl." + node.FlowControl + ";");
		//}

		if (node.FlowControl != null && node.FlowControl != "Next")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsFlowNext => false;");
		}

		if (node.FlowControl != null && node.FlowControl == "Call")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsCall => true;");
		}

		if (node.FlowControl != null && node.FlowControl == "Return")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsReturn => true;");
		}

		if (node.FlowControl != null && node.FlowControl == "ConditionalBranch")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsConditionalBranch => true;");
		}

		if (node.FlowControl != null && node.FlowControl == "UnconditionalBranch")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsUnconditionalBranch => true;");
		}

		if (node.IgnoreDuringCodeGeneration != null && node.IgnoreDuringCodeGeneration == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IgnoreDuringCodeGeneration => true;");
		}

		if (node.IgnoreInstructionBasicBlockTargets != null && node.IgnoreInstructionBasicBlockTargets == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IgnoreInstructionBasicBlockTargets => true;");
		}

		if (node.VariableOperands != null && node.VariableOperands == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool HasVariableOperands => true;");
		}

		if (node.Commutative != null && node.Commutative == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsCommutative => true;");
		}

		if (node.MemoryWrite != null && node.MemoryWrite == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsMemoryWrite => true;");
		}

		if (node.MemoryRead != null && node.MemoryRead == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsMemoryRead => true;");
		}

		if (node.IOOperation != null && node.IOOperation == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsIOOperation => true;");
		}

		if (node.UnspecifiedSideEffect != null && node.UnspecifiedSideEffect == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool HasUnspecifiedSideEffect => true;");
		}

		//if (node.ThreeTwoAddressConversion != null && node.ThreeTwoAddressConversion == "true")
		//{
		//	Lines.AppendLine();
		//	Lines.AppendLine("\tpublic override bool ThreeTwoAddressConversion => true;");
		//}

		if (FlagsUsed.Contains("Z") || FlagsUsed == "CONDITIONAL")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsZeroFlagUsed => true;");
		}

		if (FlagsSet.Contains("Z"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsZeroFlagSet => true;");
		}

		if (FlagsCleared.Contains("Z"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsZeroFlagCleared => true;");
		}

		if (FlagsModified.Contains("Z") || FlagsSet.Contains("Z") || FlagsCleared.Contains("Z"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsZeroFlagModified => true;");
		}

		if (FlagsUndefined.Contains("Z"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsZeroFlagUnchanged => true;");
		}

		if (FlagsUndefined.Contains("Z"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsZeroFlagUndefined => true;");
		}

		if (FlagsUsed.Contains("C") || FlagsUsed == "CONDITIONAL")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsCarryFlagUsed => true;");
		}

		if (FlagsSet.Contains("C"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsCarryFlagSet => true;");
		}

		if (FlagsCleared.Contains("C"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsCarryFlagCleared => true;");
		}

		if (FlagsModified.Contains("C") || FlagsSet.Contains("C") || FlagsCleared.Contains("C"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsCarryFlagModified => true;");
		}

		if (FlagsUndefined.Contains("C"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsCarryFlagUnchanged => true;");
		}

		if (FlagsUndefined.Contains("C"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsCarryFlagUndefined => true;");
		}

		if (FlagsUsed.Contains("S") || FlagsUsed.Contains("N") || FlagsUsed == "CONDITIONAL")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsSignFlagUsed => true;");
		}

		if (FlagsSet.Contains("S") || FlagsSet.Contains("N"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsSignFlagSet => true;");
		}

		if (FlagsCleared.Contains("S") || FlagsCleared.Contains("N"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsSignFlagCleared => true;");
		}

		if (FlagsModified.Contains("S") || FlagsSet.Contains("S") || FlagsCleared.Contains("S")
			|| FlagsModified.Contains("N") || FlagsSet.Contains("N") || FlagsCleared.Contains("N")
		   )
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsSignFlagModified => true;");
		}

		if (FlagsUndefined.Contains("S") || FlagsUndefined.Contains("N"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsSignFlagUnchanged => true;");
		}

		if (FlagsUndefined.Contains("S") || FlagsUndefined.Contains("N"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsSignFlagUndefined => true;");
		}

		if (FlagsUsed.Contains("O") || FlagsUsed.Contains("V") || FlagsUsed == "CONDITIONAL")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsOverflowFlagUsed => true;");
		}

		if (FlagsSet.Contains("O") || FlagsSet.Contains("V"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsOverflowFlagSet => true;");
		}

		if (FlagsCleared.Contains("O") || FlagsCleared.Contains("V"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsOverflowFlagCleared => true;");
		}

		if (FlagsModified.Contains("O") || FlagsSet.Contains("O") || FlagsCleared.Contains("O")
			|| FlagsModified.Contains("V") || FlagsSet.Contains("V") || FlagsCleared.Contains("V"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsOverflowFlagModified => true;");
		}

		if (FlagsUndefined.Contains("O") || FlagsUndefined.Contains("V"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsOverflowFlagUnchanged => true;");
		}

		if (FlagsUndefined.Contains("O") || FlagsUndefined.Contains("V"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsOverflowFlagUndefined => true;");
		}

		if (FlagsUsed.Contains("P") || FlagsUsed == "CONDITIONAL")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsParityFlagUsed => true;");
		}

		if (FlagsSet.Contains("P"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsParityFlagSet => true;");
		}

		if (FlagsCleared.Contains("P"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsParityFlagCleared => true;");
		}

		if (FlagsModified.Contains("P") || FlagsSet.Contains("P") || FlagsCleared.Contains("P"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsParityFlagModified => true;");
		}

		if (FlagsUndefined.Contains("P"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsParityFlagUnchanged => true;");
		}

		if (FlagsUndefined.Contains("P"))
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsParityFlagUndefined => true;");
		}

		if (FlagsUsed != null && FlagsUsed == "CONDITIONAL")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool AreFlagUseConditional => true;");
		}

		if (node.StaticEmitMethod != null)
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override void Emit(Node node, OpcodeEncoder opcodeEncoder)");
			Lines.AppendLine("\t{");

			if (node.VariableOperands == null || node.VariableOperands == "false")
			{
				Lines.AppendLine("\t\tSystem.Diagnostics.Debug.Assert(node.ResultCount == DefaultResultCount);");
				Lines.AppendLine("\t\tSystem.Diagnostics.Debug.Assert(node.OperandCount == DefaultOperandCount);");
				Lines.AppendLine();
			}

			Lines.AppendLine("\t\t" + node.StaticEmitMethod.Replace("%", node.Name) + "(node, emitter);");
			Lines.AppendLine("\t}");
		}

		if (node.OpcodeEncoding != null)
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override void Emit(Node node, OpcodeEncoder opcodeEncoder)");
			Lines.AppendLine("\t{");
			if (node.VariableOperands == null || node.VariableOperands == "false")
			{
				Lines.AppendLine("\t\tSystem.Diagnostics.Debug.Assert(node.ResultCount == " + node.ResultCount + ");");
				Lines.AppendLine("\t\tSystem.Diagnostics.Debug.Assert(node.OperandCount == " + node.OperandCount + ");");

				if (node.ThreeTwoAddressConversion != null && node.ThreeTwoAddressConversion == "true")
				{
					Lines.AppendLine("\t\tSystem.Diagnostics.Debug.Assert(node.Result.IsPhysicalRegister);");
					Lines.AppendLine("\t\tSystem.Diagnostics.Debug.Assert(node.Operand1.IsPhysicalRegister);");
					Lines.AppendLine("\t\tSystem.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);");
				}
				Lines.AppendLine();
			}

			CreateEncoding(node);

			Lines.AppendLine("\t}");
		}

		Lines.AppendLine("}");
	}

	private void CreateEncoding(dynamic node)
	{
		var first = true;
		var cond = false;

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
				Lines.Append("\t\t // ");
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
			Lines.AppendLine("\t\tthrow new Compiler.Common.Exceptions.CompilerException(\"Invalid Opcode\");");
		}
	}

	private string ReduceEncoding(string template)
	{
		if (string.IsNullOrWhiteSpace(template))
			return string.Empty;

		var encoding = string.Empty;

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
				var subTemplate = string.Empty;

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

		var rawEncoding = line.Trim();

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

		var expression = string.Empty;

		var parts = condition.Split(']');
		for (var i = 0; i < parts.Length; i++)
		{
			var part = parts[i];
			var normalized = part.Replace(" ", string.Empty).TrimStart('[').ToLowerInvariant();

			if (string.IsNullOrWhiteSpace(normalized))
				continue;

			var subparts = normalized.Split(':');

			var partexpression = string.Empty;

			foreach (var subpart in subparts)
			{
				var operand = string.Empty;

				if (string.IsNullOrEmpty(subpart))
					continue;

				if (subpart.StartsWith("#"))
					continue;

				var opp = subpart.StartsWith("!");

				var subpart2 = opp ? subpart.Substring(1) : subpart;

				operand = i switch
				{
					0 => "Operand1",
					1 => "Operand2",
					2 => "Operand3",
					3 => "Operand4",
					4 => "Operand5",
					_ => $"GetOperand({i})"
				};

				var cond1 = string.Empty;
				var cond2 = string.Empty;
				var cond3 = string.Empty;

				switch (subpart2.ToLowerInvariant())
				{
					case "skip": continue;
					case "ignore": continue;
					case "register": cond1 = ".IsPhysicalRegister"; break;
					case "constant": cond1 = ".IsConstant"; break;
					case "eax": cond1 = ".IsPhysicalRegister"; cond2 = ".Register.RegisterCode == 0"; break;
					case "ecx": cond1 = ".IsPhysicalRegister"; cond2 = ".Register.RegisterCode == 1"; break;
					case "edx": cond1 = ".IsPhysicalRegister"; cond2 = ".Register.RegisterCode == 2"; break;
					case "ebx": cond1 = ".IsPhysicalRegister"; cond2 = ".Register.RegisterCode == 3"; break;
					case "esp": cond1 = ".IsPhysicalRegister"; cond2 = ".Register.RegisterCode == 4"; break;
					case "ebp": cond1 = ".IsPhysicalRegister"; cond2 = ".Register.RegisterCode == 5"; break;
					case "esi": cond1 = ".IsPhysicalRegister"; cond2 = ".Register.RegisterCode == 6"; break;
					case "edi": cond1 = ".IsPhysicalRegister"; cond2 = ".Register.RegisterCode == 7"; break;
					case "zero":
					case "0": cond1 = ".IsConstantZero"; break;
					case "one":
					case "1": cond1 = ".IsConstantOne"; break;
					case "constant_imm8": cond1 = ".IsConstant"; cond2 = ".ConstantSigned32 >= " + sbyte.MinValue; cond3 = ".ConstantSigned32 <= " + sbyte.MaxValue; break;
					case "constant_imm16": cond1 = ".IsConstant"; cond2 = ".ConstantSigned32 >= " + short.MinValue; cond3 = ".ConstantSigned32 <= " + short.MaxValue; break;
					case "constant_imm32": cond1 = ".IsConstant"; cond2 = ".ConstantSigned32 >= " + int.MinValue; cond3 = ".ConstantSigned32 <= " + int.MaxValue; break;
					case "constant_1to4": cond1 = ".IsConstant"; cond2 = ".ConstantUnsigned32 >= 1"; cond3 = ".ConstantUnsigned32 <= 4"; break;
					case "constant_0or1": cond1 = ".IsConstant"; cond2 = ".ConstantUnsigned32 >= 0"; cond3 = ".ConstantUnsigned32 <= 1"; break;
				}

				var subexpression = $"node.{operand}{cond1}";

				if (!string.IsNullOrWhiteSpace(cond3))
				{
					subexpression = $"{subexpression} && node.{operand}{cond2} && node.{operand}{cond3}";
				}
				else if (!string.IsNullOrWhiteSpace(cond2))
				{
					subexpression = $"{subexpression} && node.{operand}{cond2}";
				}

				if (opp)
				{
					subexpression = $"!{subexpression}";
				}

				partexpression = subexpression;
			}

			if (string.IsNullOrWhiteSpace(expression))
			{
				expression = partexpression;
			}
			else
			{
				expression = $"{expression} && {partexpression}";
			}
		}

		return expression;
	}

	private void EmitCondition(string condition, string encoding, bool end, int index = 0)
	{
		var tabs = "\t\t\t\t\t\t\t\t\t".Substring(0, index + 2);
		Lines.Append(tabs);

		Lines.AppendLine($"if ({condition})");

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
		var tabs = "\t\t\t\t\t\t\t\t\t".Substring(0, index + 2);

		foreach (var s in steps)
		{
			if (string.IsNullOrWhiteSpace(s))
				continue;

			if (s.StartsWith("[")) // ignore these
				continue;

			if (s.StartsWith("0x") | s.StartsWith("x"))
			{
				// hex
				var hex = s.StartsWith("x") ? s.Substring(1) : s.Substring(2);

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
				var binary = s;

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

				var operand = parts.Length > 1 ? GetOperand(parts[1]) : string.Empty;
				var operand2 = parts.Length > 2 ? GetOperand(parts[2]) : null;

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
			case "reg1": code = "Append1Bits"; postcode = ".Register.RegisterCode"; return;
			case "reg3": code = "Append3Bits"; postcode = ".Register.RegisterCode"; return;
			case "reg4": code = "Append4Bits"; postcode = ".Register.RegisterCode"; return;
			case "reg5": code = "Append5Bits"; postcode = ".Register.RegisterCode"; return;
			case "reg6": code = "Append6Bits"; postcode = ".Register.RegisterCode"; return;
			case "reg4x": code = "Append1Bit"; postcode = ".Register.RegisterCode >> 3"; return;
			case "reg4xnot": code = "Append1BitNot"; postcode = ".Register.RegisterCode >> 3"; return;
			case "reg1not": code = "Append1BitsNot"; postcode = ".Register.RegisterCode"; return;
			case "reg3not": code = "Append3BitsNot"; postcode = ".Register.RegisterCode"; return;
			case "reg4not": code = "Append4BitsNot"; postcode = ".Register.RegisterCode"; return;
			case "reg3s1": code = "Append3Bits"; postcode = ".Register.RegisterCode >> 1"; return;
			case "imm1": code = "Append1BitImmediate"; return;
			case "imm2": code = "Append2BitImmediate"; return;
			case "imm2scale": code = "Append2BitSScale"; return;
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
			case "int": code = "Append1Bit"; postcode = ".IsInteger ? 1 : 0"; return;
			case "float": code = "Append1Bit"; postcode = ".IsFloatingPoint ? 1 : 0"; return;

			//case "signed": code = "Append1Bit"; postcode = ".ConstantSigned64 < 0 ? 1 : 0"; return;
			//case "imm12unsigned": code = "Append12BitImmediate"; postcode = ""; return;

			case "": return;

			default: break;
		}

		if (part.StartsWith("reg("))
		{
			var open = part.IndexOf('(');
			var comma = part.IndexOf('-');
			var closed = part.IndexOf(')');

			if (open >= 0)
			{
				var start = Convert.ToInt32(part.Substring(open + 1, comma - open - 1).Trim());
				var end = comma < 0 ? start : Convert.ToInt32(part.Substring(comma + 1, closed - comma - 1).Trim());
				var length = end - start + 1;

				if (start == 0)
				{
					postcode = ".Register.RegisterCode) & 0x" + "111111111111111111111111111111".Substring(0, length);
				}
				else
				{
					postcode = ".Register.RegisterCode >> " + start + ") & 0x" + "111111111111111111111111111111".Substring(0, length);
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
					default: code = "AppendBits("; postcode += ", " + length; break;
				}

				return;
			}
		}

		throw new Exception("ERROR!");
	}

	private static string GetOperand(string part)
	{
		return part switch
		{
			"o1" => "node.Operand1",
			"o2" => "node.Operand2",
			"o3" => "node.Operand3",
			"o4" => "node.Operand4",
			"o5" => "node.Operand5",
			"o6" => "node.GetOperand(5)",
			"r" => "node.Result",
			"r1" => "node.Result",
			"r2" => "node.Result2",
			"label" => "node.BranchTargets[0].Label",
			"status" => "node.StatusRegister",
			_ => part
		};
	}
}
