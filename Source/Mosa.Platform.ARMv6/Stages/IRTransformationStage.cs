// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.ARMv6.Stages
{
	/// <summary>
	/// Transforms IR instructions into their appropriate ARMv6.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms IR instructions into their equivalent ARMv6 sequences.
	/// </remarks>
	public sealed class IRTransformationStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.AddSigned32, AddSigned32);
			AddVisitation(IRInstruction.AddUnsigned32, AddUnsigned32);
			AddVisitation(IRInstruction.LogicalOr, LogicalOr);
			AddVisitation(IRInstruction.SubSigned, SubSigned);
			AddVisitation(IRInstruction.SubUnsigned, SubUnsigned);
		}

		#region Visitation Methods

		/// <summary>
		/// Visitation function for AddSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddSigned32(Context context)
		{
			context.ReplaceInstruction(ARMv6.Add);
		}

		/// <summary>
		/// Visitation function for AddUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddUnsigned32(Context context)
		{
			context.ReplaceInstruction(ARMv6.Add);
		}

		/// <summary>
		/// Visitation function for LogicalOrInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void LogicalOr(Context context)
		{
			context.ReplaceInstruction(ARMv6.Orr);
		}

		/// <summary>
		/// Visitation function for SubSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SubSigned(Context context)
		{
			context.ReplaceInstruction(ARMv6.Sub);
		}

		/// <summary>
		/// Visitation function for SubUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SubUnsigned(Context context)
		{
			context.ReplaceInstruction(ARMv6.Sub);
		}

		#endregion Visitation Methods
	}
}
