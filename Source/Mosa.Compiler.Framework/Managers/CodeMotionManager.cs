// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Managers;

public class CodeMotionManager : BaseTransformManager
{
	private readonly HashSet<Node> Motion = new HashSet<Node>();

	public void MarkMotion(Node node)
	{
		Motion.Add(node);
	}

	public bool CheckMotion(Node node)
	{
		return Motion.Contains(node);
	}
}
