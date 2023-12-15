// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Phi
{
	public abstract class BasePhiTransform : BaseTransform
	{
		public BasePhiTransform(BaseInstruction instruction, TransformType type, bool log = false)
			: base(instruction, type, log)
		{ }

		#region Helpers

		protected static void ReplaceBranchTarget(BasicBlock source, BasicBlock oldTarget, BasicBlock newTarget)
		{
			for (var node = source.BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (node.Instruction.IsConditionalBranch || node.Instruction.IsUnconditionalBranch)
				{
					if (node.BranchTarget1 == oldTarget)
					{
						node.UpdateBranchTarget(0, newTarget);
					}
					continue;
				}

				break;
			}
		}

		#endregion Helpers

		#region Phi Helpers

		public static void UpdatePhiTarget(BasicBlock target, BasicBlock source, BasicBlock newSource)
		{
			PhiHelper.UpdatePhiTarget(target, source, newSource);
		}

		public static void UpdatePhiTargets(List<BasicBlock> targets, BasicBlock source, BasicBlock newSource)
		{
			PhiHelper.UpdatePhiTargets(targets, source, newSource);
		}

		public static void UpdatePhiBlocks(List<BasicBlock> phiBlocks)
		{
			PhiHelper.UpdatePhiBlocks(phiBlocks);
		}

		public static void UpdatePhiBlock(BasicBlock phiBlock)
		{
			PhiHelper.UpdatePhiBlock(phiBlock);
		}

		public static void UpdatePhi(Node node)
		{
			PhiHelper.UpdatePhi(node);
		}

		#endregion Phi Helpers
	}
}
