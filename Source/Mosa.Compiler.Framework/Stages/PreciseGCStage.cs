// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Trace;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stage determine were object references are located in code.
	/// </summary>
	public class PreciseGCStage : BaseMethodCompilerStage
	{
		protected class RangeData
		{
			public Operand StackLocal;
			public Register Register;

			public bool IsRegister { get { return Register != null; } }

			public List<int> GenList = new List<int>();
			public List<int> KillList = new List<int>();
			public List<int> UseList = new List<int>();
		}

		private Dictionary<Operand, int> stack = new Dictionary<Operand, int>();

		private TraceLog trace;

		private RangeData[] registerRangeData;
		private RangeData[] stackLocalRangeData;

		protected override void Run()
		{
			if (IsPlugged)
				return;

			trace = CreateTraceLog();

			CollectStackObjects();

			stackLocalRangeData = new RangeData[stack.Count];
			registerRangeData = new RangeData[Architecture.RegisterSet.Length];

			for (int i = 0; i < registerRangeData.Length; i++)
			{
				var range = new RangeData();
				range.Register = Architecture.RegisterSet[i];
				registerRangeData[i] = range;
			}

			foreach (var local in stack)
			{
				var range = new RangeData();
				range.StackLocal = local.Key;
				stackLocalRangeData[local.Value] = range;
			}

			CollectObjectAssignmentsAndUses();
		}

		private void CollectObjectAssignmentsAndUses()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.ResultCount == 0 && node.OperandCount == 0)
						continue;

					var visitor = new OperandVisitor(node);

					foreach (var input in visitor.Input)
					{
						if (input.Type.IsReferenceType || input.Type.IsManagedPointer)
						{
							if (input.IsRegister)
								registerRangeData[input.Register.Index].UseList.Add(node.Offset);
							else if (input.IsStackLocal)
								stackLocalRangeData[stack[input]].UseList.Add(node.Offset);
						}
						else
						{
							if (input.IsRegister)
								registerRangeData[input.Register.Index].KillList.Add(node.Offset);
							else if (input.IsStackLocal)
								stackLocalRangeData[stack[input]].KillList.Add(node.Offset);
						}
					}

					foreach (var output in visitor.Output)
					{
						if (output.Type.IsReferenceType || output.Type.IsManagedPointer)
						{
							if (output.IsRegister)
								registerRangeData[output.Register.Index].GenList.Add(node.Offset);
							else if (output.IsStackLocal)
								stackLocalRangeData[stack[output]].GenList.Add(node.Offset);
						}
					}
				}
			}
		}

		protected override void Finish()
		{
			stack = null;
			stackLocalRangeData = null;
			registerRangeData = null;
		}

		protected void CollectStackObjects()
		{
			foreach (var local in MethodCompiler.StackLayout.LocalStack)
			{
				if (local.Type.IsReferenceType || local.Type.IsManagedPointer)
				{
					stack.Add(local, stack.Count);
				}
			}
		}
	}
}
