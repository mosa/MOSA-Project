// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework;

public abstract class BaseBlockTransform : IComparable<BaseBlockTransform>
{
	#region Properties

	public bool Log { get; private set; }

	public string Name { get; }

	public virtual int Priority { get; } = 0;

	#endregion Properties

	#region Constructors

	public BaseBlockTransform(bool log = false)
	{
		Log = log;

		Name = GetType().FullName.Replace("Mosa.Compiler.", string.Empty).Replace("Mosa.Compiler.Framework.Transforms", "IR").Replace("Transforms.", string.Empty);
	}

	#endregion Constructors

	#region Abstract Methods

	public abstract bool Process(TransformContext transformContext);

	#endregion Abstract Methods

	#region Internals

	int IComparable<BaseBlockTransform>.CompareTo(BaseBlockTransform other)
	{
		return Priority.CompareTo(other.Priority);
	}

	#endregion Internals

	#region Block Helpers

	protected static void RemoveRemainingInstructionInBlock(Context context)
	{
		BaseTransform.RemoveRemainingInstructionInBlock(context);
	}

	protected static BasicBlock GetOtherBranchTarget(BasicBlock block, BasicBlock target)
	{
		return BaseTransform.GetOtherBranchTarget(block, target);
	}

	#endregion Block Helpers
}
