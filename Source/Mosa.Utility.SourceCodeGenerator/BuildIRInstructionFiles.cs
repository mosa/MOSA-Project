// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildIRInstructionFiles : BuildBaseTemplate
	{
		public BuildIRInstructionFiles(string jsonFile, string destinationPath)
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
			if (node.ResultType != null || node.ResultType2 != null)
			{
				Lines.AppendLine("using Mosa.Compiler.MosaTypeSystem;");
				Lines.AppendLine();
			}

			Lines.AppendLine("namespace Mosa.Compiler.Framework.IR");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.Append($"\t/// {node.Name}");

			if (!string.IsNullOrWhiteSpace(node.Description))
			{
				Lines.Append($" - {node.Description}");
			}
			Lines.AppendLine();

			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine("\t/// <seealso cref=\"Mosa.Compiler.Framework.IR.BaseIRInstruction\" />");
			Lines.AppendLine($"\tpublic sealed class {node.Name} : BaseIRInstruction");
			Lines.AppendLine("\t{");
			Lines.AppendLine("\t\tpublic " + node.Name + "()");
			Lines.AppendLine($"\t\t\t: base({node.OperandCount}, {node.ResultCount})");
			Lines.AppendLine("\t\t{");
			Lines.AppendLine("\t\t}");

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

			if (node.IRUnspecifiedSideEffect != null && node.IRUnspecifiedSideEffect == "true")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool HasIRUnspecifiedSideEffect { get { return true; } }");
			}

			if (node.ParameterLoad != null && node.ParameterLoad == "true")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsParameterLoad { get { return true; } }");
			}

			if (node.ParameterStore != null && node.ParameterStore == "true")
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsParameterStore { get { return true; } }");
			}

			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}
	}
}
