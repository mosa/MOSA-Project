// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Explorer.Avalonia;

public class InstructionRecord
{
	public readonly string Data;

	public readonly string[] Parts;

	public readonly string Type;

	public readonly bool IsInstruction;

	public readonly bool IsMethod;

	public readonly bool IsStartBlock;

	public readonly bool IsEndBlock;

	public string Label => Parts[1];

	// Block
	public string BlockLabel => Parts[1];

	// Start Block
	public string BlockSequence => Parts[2];

	public string BlockType => Parts[3];

	public readonly int PreviousBlockCount;

	public string GetPreviousBlocks(int index) => Parts[5 + index];

	// End Block
	public int NextBlockCount => Convert.ToInt32(Parts[2]);

	// Instruction
	public string Mark => Parts[2];

	public string Instruction => Parts[3];

	public string Condition => Parts[4];

	public readonly int ResultCount = 0;

	public readonly int OperandCount = 0;

	public readonly int BranchTargetCount = 0;

	public readonly int PhiBlockCount = 0;

	public string GetResult(int index) => Parts[9 + index];

	public string GetOperand(int index) => Parts[9 + ResultCount + index];

	public string GetBranchTarget(int index) => Parts[9 + ResultCount + OperandCount + index];

	public string GetPhilBlock(int index) => Parts[9 + ResultCount + OperandCount + BranchTargetCount + index];

	// Method

	public string MethodName => Parts[1];

	public int Version => Convert.ToInt32(Parts[2]);

	public string Stage => Parts[3];

	public int Step => Convert.ToInt32(Parts[4]);

	public InstructionRecord(string data)
	{
		Data = data;
		Parts = data.Split('\t');

		Type = Parts[0];

		IsInstruction = Type == "I";
		IsMethod = Type == "M";
		IsStartBlock = Type == "S";
		IsEndBlock = Type == "E";

		if (IsInstruction)
		{
			ResultCount = Convert.ToInt32(Parts[5]);
			OperandCount = Convert.ToInt32(Parts[6]);
			BranchTargetCount = Convert.ToInt32(Parts[7]);
			PhiBlockCount = Convert.ToInt32(Parts[8]);
		}
		else if (IsStartBlock)
		{
			PreviousBlockCount = Convert.ToInt32(Parts[4]);
		}
	}
}
