// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using System;
using System.Collections.Generic;

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
			visitationDictionary[IRInstruction.AddSigned] = AddSigned;
			visitationDictionary[IRInstruction.AddUnsigned] = AddUnsigned;
			visitationDictionary[IRInstruction.LogicalOr] = LogicalOr;
			visitationDictionary[IRInstruction.SubSigned] = SubSigned;
			visitationDictionary[IRInstruction.SubUnsigned] = SubUnsigned;
		}

		#region Visitation Methods

		/// <summary>
		/// Visitation function for AddSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddSigned(Context context)
		{
			context.ReplaceInstructionOnly(ARMv6.Add);
		}

		/// <summary>
		/// Visitation function for AddUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void AddUnsigned(Context context)
		{
			context.ReplaceInstructionOnly(ARMv6.Add);
		}

		/// <summary>
		/// Visitation function for LogicalOrInstruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void LogicalOr(Context context)
		{
			context.ReplaceInstructionOnly(ARMv6.Orr);
		}

		/// <summary>
		/// Visitation function for SubSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SubSigned(Context context)
		{
			context.ReplaceInstructionOnly(ARMv6.Sub);
		}

		/// <summary>
		/// Visitation function for SubUnsigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SubUnsigned(Context context)
		{
			context.ReplaceInstructionOnly(ARMv6.Sub);
		}

		#endregion Visitation Methods
	}
}
