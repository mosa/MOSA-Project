// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.ESP32.Stages
{
	/// <summary>
	/// Transforms IR instructions into their appropriate ESP32.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms IR instructions into their equivalent ESP32 sequences.
	/// </remarks>
	public sealed class IRTransformationStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.Add32, Add32);
		}

		#region Visitation Methods

		/// <summary>
		/// Visitation function for AddSigned.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Add32(Context context)
		{
			//context.ReplaceInstruction(ESP32.Add32);
		}

		#endregion Visitation Methods
	}
}
