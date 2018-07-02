// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Expression;
using Mosa.Platform.ARMv6;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Workspace.Experiment.Debug
{
	internal static class Program
	{
		private static void Main()
		{
			Dump2();

			//var tree = ExpressionTest.GetTestExpression2();
			//var basicBlocks = ExpressionTest.CreateBasicBlockInstructionSet();

			//var match = tree.Transform(basicBlocks[0].Last.Previous, null);

			//ExpressionTest.GetTestExpression5();
			//ExpressionTest.GetTestExpression4();
			//ExpressionTest.GetTestExpression3();
			//ExpressionTest.GetTestExpression2();

			return;
		}

		private static List<ARMv6Instruction> list = new List<ARMv6Instruction>()
		{
			ARMv6.Adc32,
			ARMv6.Add32,
			ARMv6.Adr32,
			ARMv6.And32,
			ARMv6.Asr32,
			ARMv6.B32,
			ARMv6.Bic32,
			ARMv6.Bkpt32,
			ARMv6.Bl32,
			ARMv6.Blx32,
			ARMv6.Bx32,
			ARMv6.Cmn32,
			ARMv6.Cmp32,
			ARMv6.Dmb32,
			ARMv6.Dsb32,
			ARMv6.Eor32,
			ARMv6.Isb32,
			ARMv6.Ldm32,
			ARMv6.Ldmfd32,
			ARMv6.Ldmia32,
			ARMv6.Ldr32,
			ARMv6.Ldrb32,
			ARMv6.Ldrh32,
			ARMv6.Ldrsb32,
			ARMv6.Ldrsh32,
			ARMv6.Lsl32,
			ARMv6.Lsr32,
			ARMv6.Mov32,
			ARMv6.Mrs32,
			ARMv6.Msr32,
			ARMv6.Mul32,
			ARMv6.Mvn32,
			ARMv6.Nop32,
			ARMv6.Orr32,
			ARMv6.Pop32,
			ARMv6.Push32,
			ARMv6.Rev32,
			ARMv6.Rev16,
			ARMv6.Revsh32,
			ARMv6.Ror32,
			ARMv6.Rsb32,
			ARMv6.Rsc32,
			ARMv6.Sbc32,
			ARMv6.Sev32,
			ARMv6.Stm32,
			ARMv6.Stmea32,
			ARMv6.Stmia32,
			ARMv6.Str32,
			ARMv6.Strb32,
			ARMv6.Strh32,
			ARMv6.Sub32,
			ARMv6.Svc32,
			ARMv6.Swi32,
			ARMv6.Sxtb32,
			ARMv6.Sxth32,
			ARMv6.Teq32,
			ARMv6.Tst32,
			ARMv6.Uxtb32,
			ARMv6.Uxth32,
			ARMv6.Wfe32,
			ARMv6.Wfi32,
			ARMv6.Yield32
		};

		private static void Dump2()
		{
			StringBuilder sb = new StringBuilder();

			foreach (var arm in list)
			{
				sb.AppendLine("{");
				sb.AppendLine("\"Name\": \"" + arm.Name + "\",");
				sb.AppendLine("\"Commutative\": \"false\",");
				sb.AppendLine("	\"FamilyName\": \"ARMv6\",");
				sb.AppendLine("	\"FlagsCleared\": \"\",");
				sb.AppendLine("	\"FlagsModified\": \"\",");
				sb.AppendLine("	\"FlagsSet\": \"\",");
				sb.AppendLine("	\"FlagsUnchanged\": \"\",");
				sb.AppendLine("	\"FlagsUndefined\": \"\",");
				sb.AppendLine("	\"FlagsUsed\": \"\",");
				sb.AppendLine("	\"OperandCount\": " + arm.DefaultOperandCount + ",");
				sb.AppendLine("	\"ResultCount\": " + arm.DefaultResultCount + ",");
				sb.AppendLine("	\"ARMv6Emitter\": \"" + (arm.__emitter ?? string.Empty) + "\",");
				sb.AppendLine("	\"ARMv6Opcode\": \"" + (arm.__bits ?? string.Empty) + "\",");
				sb.AppendLine("	\"Description\": \"" + (arm.__description ?? string.Empty) + "\"");
				sb.AppendLine("},");
			}

			System.Diagnostics.Debug.WriteLine(sb.ToString());
		}
	}
}
