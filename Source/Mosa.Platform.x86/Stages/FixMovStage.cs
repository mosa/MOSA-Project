// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	public sealed class FixMovStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			visitationDictionary[IRInstruction.Move] = Move;
		}

		#region Visitation Methods

		private void Move(Context context)
		{
			if (context.Result.IsStackLocal)
			{
				BaseInstruction storeInstruction = null;

				if (context.Result.IsR8)
				{
					storeInstruction = X86.MovsdStore;
				}
				else if (context.Result.IsR4)
				{
					storeInstruction = X86.MovssStore;
				}
				else
				{
					storeInstruction = X86.MovStore;
				}

				context.SetInstruction(storeInstruction, context.Size, null, context.Operand1, context.Operand2, context.Operand3);
			}
			else if (context.Operand1.IsStackLocal)
			{
				BaseInstruction loadInstruction = null;

				if (context.Operand1.IsR8)
				{
					loadInstruction = X86.MovsdLoad;
				}
				else if (context.Operand1.IsR4)
				{
					loadInstruction = X86.MovssLoad;
				}
				else
				{
					loadInstruction = X86.MovLoad;
				}

				context.SetInstruction(loadInstruction, context.Size, context.Result, context.Operand1, context.Operand2);
			}
		}

		#endregion Visitation Methods
	}
}
