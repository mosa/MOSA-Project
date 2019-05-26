// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using static Mosa.Platform.x86.Stages.OptimizationStage;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// X86 Post Optimization Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class PostOptimizationStage : BaseTransformationStage
	{
		private Counter ZeroToXorSubstitutionCount = new Counter("X86.PostOptimizationStage.ZeroToXorSubstitution");

		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X86.Mov32, Mov32);
		}

		protected override void Initialize()
		{
			base.Initialize();

			Register(ZeroToXorSubstitutionCount);
		}

		#region Visitation Methods

		public void Mov32(InstructionNode node)
		{
			if (!node.Operand1.IsConstantZero)
				return;

			if (OptimizationStage.AreStatusFlagsUsed(node.Next, true, true, true, true, true) == TriState.No)
			{
				node.SetInstruction(X86.Xor32, node.Result, node.Result, node.Result);
				ZeroToXorSubstitutionCount++;
			}
		}

		#endregion Visitation Methods
	}
}
