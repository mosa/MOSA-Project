// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Managers;

public class CodeMotionManager : BaseTransformManager
{
	private HashSet<InstructionNode> Motion = new HashSet<InstructionNode>();

	public override void Reset()
	{
		Motion.Clear();
	}

	public void MarkMotion(InstructionNode node)
	{
		Motion.Add(node);
	}

	public bool CheckMotion(InstructionNode node)
	{
		return Motion.Contains(node);
	}
}
