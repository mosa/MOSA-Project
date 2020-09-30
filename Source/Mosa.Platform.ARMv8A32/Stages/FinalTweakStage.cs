// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// Final Tweak Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv8A32.BaseTransformationStage" />
	public sealed class FinalTweakStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(ARMv8A32.Mov, Mov);
		}

		#region Visitation Methods

		public void Mov(Context context)
		{
			var source = context.Operand1;
			var result = context.Result;

			Debug.Assert(result.IsCPURegister);

			if (source.IsCPURegister && result.Register == source.Register)
			{
				context.Empty();
			}
		}

		#endregion Visitation Methods
	}
}
