// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Lowering Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class FinalLoweringStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X86.Add32, Add32);
			AddVisitation(X86.Adc32, Adc32);
			AddVisitation(X86.And32, And32);
			AddVisitation(X86.Btr32, Btr32);
			AddVisitation(X86.Bts32, Bts32);
			AddVisitation(X86.Sub32, Sub32);
			AddVisitation(X86.Test32, Test32);
			AddVisitation(X86.Xor32, Xor32);
			AddVisitation(X86.Or32, Or32);
		}

		#region Visitation Methods

		public void Add32(Context context)
		{
			if (context.Operand2.IsConstant)
			{
				context.SetInstruction(X86.AddConst32, context.Result, context.Operand1, context.Operand2);
			}
		}

		public void Adc32(Context context)
		{
			if (context.Operand2.IsConstant)
			{
				context.SetInstruction(X86.AdcConst32, context.Result, context.Operand1, context.Operand2);
			}
		}

		public void And32(Context context)
		{
			if (context.Operand2.IsConstant)
			{
				context.SetInstruction(X86.AndConst32, context.Result, context.Operand1, context.Operand2);
			}
		}

		public void Btr32(Context context)
		{
			if (context.Operand2.IsConstant)
			{
				context.SetInstruction(X86.BtrConst32, context.Result, context.Operand1, context.Operand2);
			}
		}

		public void Bts32(Context context)
		{
			if (context.Operand2.IsConstant)
			{
				context.SetInstruction(X86.BtsConst32, context.Result, context.Operand1, context.Operand2);
			}
		}

		public void Sub32(Context context)
		{
			if (context.Operand2.IsConstant)
			{
				context.SetInstruction(X86.SubConst32, context.Result, context.Operand1, context.Operand2);
			}
		}

		public void Test32(Context context)
		{
			if (context.Operand2.IsConstant)
			{
				context.SetInstruction(X86.TestConst32, context.Result, context.Operand1, context.Operand2);
			}
		}

		public void Xor32(Context context)
		{
			if (context.Operand2.IsConstant)
			{
				context.SetInstruction(X86.XorConst32, context.Result, context.Operand1, context.Operand2);
			}
		}

		public void Or32(Context context)
		{
			if (context.Operand2.IsConstant)
			{
				context.SetInstruction(X86.OrConst32, context.Result, context.Operand1, context.Operand2);
			}
		}

		#endregion Visitation Methods
	}
}
