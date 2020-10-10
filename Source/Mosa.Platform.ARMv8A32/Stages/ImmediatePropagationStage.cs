// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System.Collections.Generic;

namespace Mosa.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// Runtime Call Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	public sealed class ImmediatePropagationStage : BasePlatformTransformationStage
	{
		private static Dictionary<BaseInstruction, KeyValuePair<BaseInstruction, int>> Map = new Dictionary<BaseInstruction, KeyValuePair<BaseInstruction, int>>()
		{
			{ ARMv8A32.Mov, new KeyValuePair<BaseInstruction, int>(ARMv8A32.MovImm, 1) },
			{ ARMv8A32.Add, new KeyValuePair<BaseInstruction, int>(ARMv8A32.AddImm, 2) },
			{ ARMv8A32.Adc, new KeyValuePair<BaseInstruction, int>(ARMv8A32.AdcImm, 2) },
			{ ARMv8A32.And, new KeyValuePair<BaseInstruction, int>(ARMv8A32.AndImm, 2) },
			{ ARMv8A32.Sub, new KeyValuePair<BaseInstruction, int>(ARMv8A32.SubImm, 2) },
			{ ARMv8A32.Rsb, new KeyValuePair<BaseInstruction, int>(ARMv8A32.RsbImm, 2) },
			{ ARMv8A32.Lsl, new KeyValuePair<BaseInstruction, int>(ARMv8A32.LslImm, 2) },
			{ ARMv8A32.Lsr, new KeyValuePair<BaseInstruction, int>(ARMv8A32.LsrImm, 2) },
			{ ARMv8A32.Orr, new KeyValuePair<BaseInstruction, int>(ARMv8A32.OrrImm, 2) },
			{ ARMv8A32.Eor, new KeyValuePair<BaseInstruction, int>(ARMv8A32.EorImm, 2) },
			{ ARMv8A32.Mvn, new KeyValuePair<BaseInstruction, int>(ARMv8A32.MvnImm, 2) },
			{ ARMv8A32.Asr, new KeyValuePair<BaseInstruction, int>(ARMv8A32.AsrImm, 2) },
			{ ARMv8A32.Cmp, new KeyValuePair<BaseInstruction, int>(ARMv8A32.CmpImm, 2) },
			{ ARMv8A32.Adf, new KeyValuePair<BaseInstruction, int>(ARMv8A32.AdfImm, 2) },
			{ ARMv8A32.Muf, new KeyValuePair<BaseInstruction, int>(ARMv8A32.MufImm, 2) },
			{ ARMv8A32.Suf, new KeyValuePair<BaseInstruction, int>(ARMv8A32.SufImm, 2) },
			{ ARMv8A32.Rsf, new KeyValuePair<BaseInstruction, int>(ARMv8A32.RsfImm, 2) },
			{ ARMv8A32.Dvr, new KeyValuePair<BaseInstruction, int>(ARMv8A32.DvrImm, 2) },
			{ ARMv8A32.Dvf, new KeyValuePair<BaseInstruction, int>(ARMv8A32.DvfImm, 2) },
			{ ARMv8A32.Rmf, new KeyValuePair<BaseInstruction, int>(ARMv8A32.RmfImm, 2) },
			{ ARMv8A32.Fml, new KeyValuePair<BaseInstruction, int>(ARMv8A32.FmlImm, 2) },
			{ ARMv8A32.Fdv, new KeyValuePair<BaseInstruction, int>(ARMv8A32.FdvImm, 2) },
			{ ARMv8A32.Frd, new KeyValuePair<BaseInstruction, int>(ARMv8A32.FrdImm, 2) },
			{ ARMv8A32.Mvf, new KeyValuePair<BaseInstruction, int>(ARMv8A32.MvfImm, 1) },
			{ ARMv8A32.Mnf, new KeyValuePair<BaseInstruction, int>(ARMv8A32.MnfImm, 1) },
			{ ARMv8A32.Abs, new KeyValuePair<BaseInstruction, int>(ARMv8A32.AbsImm, 1) },
			{ ARMv8A32.Rnd, new KeyValuePair<BaseInstruction, int>(ARMv8A32.RndImm, 1) },
			{ ARMv8A32.Sqt, new KeyValuePair<BaseInstruction, int>(ARMv8A32.SqtImm, 1) },
			{ ARMv8A32.Flt, new KeyValuePair<BaseInstruction, int>(ARMv8A32.FltImm, 2) },
		};

		protected override void PopulateVisitationDictionary()
		{
			// Nothing to do
		}

		protected override void Run()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction != ARMv8A32.MovImm)
						continue;

					if (!node.Operand1.IsResolvedConstant)
						continue;

					if (node.Result.Definitions.Count != 1)
						continue;

					var uses = node.Result.Uses;

					if (uses.Count != 0)
					{
						var use = uses[0];

						if (Map.TryGetValue(use.Instruction, out KeyValuePair<BaseInstruction, int> entry))
						{
							var index = entry.Value - 1;

							var operand = use.GetOperand(index);

							if (operand != node.Result)
								continue;

							if (!operand.IsVirtualRegister)
								continue;

							use.Instruction = entry.Key;
							use.SetOperand(index, node.Operand1);
						}
					}

					if (uses.Count == 0)
						node.Empty();
				}
			}
		}
	}
}
