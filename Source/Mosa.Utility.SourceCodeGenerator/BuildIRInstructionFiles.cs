// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator;

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

		Lines.AppendLine("namespace Mosa.Compiler.Framework.IR;");
		Lines.AppendLine();
		Lines.AppendLine("/// <summary>");
		Lines.Append($"/// {node.Name}");

		if (!string.IsNullOrWhiteSpace(node.Description))
		{
			Lines.Append($" - {node.Description}");
		}
		Lines.AppendLine();

		Lines.AppendLine("/// </summary>");
		Lines.AppendLine("/// <seealso cref=\"Mosa.Compiler.Framework.IR.BaseIRInstruction\" />");
		Lines.AppendLine($"public sealed class {node.Name} : BaseIRInstruction");
		Lines.AppendLine("{");
		Lines.AppendLine("\tpublic " + node.Name + "()");
		Lines.AppendLine($"\t\t: base({node.OperandCount}, {node.ResultCount})");
		Lines.AppendLine("\t{");
		Lines.AppendLine("\t}");

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

		if (node.Phi != null && node.Phi == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsPhi => true;");
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

		if (node.IRBranch != null && node.IRBranch == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsIRBranch => true;");
		}

		if (node.IRMove != null && node.IRMove == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsIRMove => true;");
		}

		if (node.IRCompare != null && node.IRCompare == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsIRCompare => true;");
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

		if (node.IRUnspecifiedSideEffect != null && node.IRUnspecifiedSideEffect == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool HasIRUnspecifiedSideEffect => true;");
		}

		if (node.ParameterLoad != null && node.ParameterLoad == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsParameterLoad => true;");
		}

		if (node.ParameterStore != null && node.ParameterStore == "true")
		{
			Lines.AppendLine();
			Lines.AppendLine("\tpublic override bool IsParameterStore => true;");
		}

		Lines.AppendLine("}");
	}
}
