﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.Framework.Transform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Bit Tracker Stage
	/// </summary>
	public sealed class DemandBitStage : BaseMethodCompilerStage
	{
		private const int MaxInstructions = 1024;

		private readonly Counter InstructionsRemovedCount = new Counter("DemandBitStage.InstructionsRemoved");
		private TraceLog trace;

		private NodeVisitationDelegate[] affected = new NodeVisitationDelegate[MaxInstructions];
		private NodeVisitationDelegate[] demanded = new NodeVisitationDelegate[MaxInstructions];

		private delegate ulong NodeVisitationDelegate(InstructionNode node, TransformContext transformContext);

		private TransformContext TransformContext;

		protected override void Finish()
		{
			trace = null;
			TransformContext = null;
		}

		protected override void Initialize()
		{
			Register(InstructionsRemovedCount);

			RegisterAffected(IRInstruction.Or32, AffectedBits_Or32);
			RegisterAffected(IRInstruction.Or64, AffectedBits_Or64);
			RegisterAffected(IRInstruction.And32, AffectedBits_And32);
			RegisterAffected(IRInstruction.And64, AffectedBits_And64);

			RegisterDemanded(IRInstruction.Or32, DemandedBits_And32);
			RegisterDemanded(IRInstruction.Or64, DemandedBits_Or64);
			RegisterDemanded(IRInstruction.And32, DemandedBits_And32);
			RegisterDemanded(IRInstruction.And64, DemandedBits_And64);

			RegisterDemanded(IRInstruction.ShiftRight32, DemandedBits_ShiftRight32);
			RegisterDemanded(IRInstruction.ShiftRight64, DemandedBits_ShiftRight64);
		}

		private void RegisterAffected(BaseInstruction instruction, NodeVisitationDelegate method)
		{
			affected[instruction.ID] = method;
		}

		private void RegisterDemanded(BaseInstruction instruction, NodeVisitationDelegate method)
		{
			demanded[instruction.ID] = method;
		}

		protected override void Run()
		{
			if (HasProtectedRegions)
				return;

			// Method is empty - must be a plugged method
			if (BasicBlocks.HeadBlocks.Count != 1)
				return;

			if (BasicBlocks.PrologueBlock == null)
				return;

			trace = CreateTraceLog(5);

			var bitValueManager = new BitValueManager(Is32BitPlatform);

			TransformContext = new TransformContext(MethodCompiler, bitValueManager);
			TransformContext.SetLog(trace);

			// TODO

			if (CompilerSettings.FullCheckMode)
				CheckAllPhiInstructions();
		}

		#region IR Instructions

		private static ulong AffectedBits_Or32(InstructionNode node, TransformContext transformContext)
		{
			var value1 = transformContext.GetBitValue(node.Operand1);
			var value2 = transformContext.GetBitValue(node.Operand2);

			var affected = value1.BitsSet32 | value1.BitsUnknown32 | value2.BitsSet32 | value2.BitsUnknown32;

			return affected;
		}

		private static ulong AffectedBits_Or64(InstructionNode node, TransformContext transformContext)
		{
			var value1 = transformContext.GetBitValue(node.Operand1);
			var value2 = transformContext.GetBitValue(node.Operand2);

			var affected = value1.BitsSet | value1.BitsUnknown32 | value2.BitsSet | value2.BitsUnknown32;

			return affected;
		}

		private static ulong AffectedBits_And32(InstructionNode node, TransformContext transformContext)
		{
			var value1 = transformContext.GetBitValue(node.Operand1);
			var value2 = transformContext.GetBitValue(node.Operand2);

			var affected = value1.BitsClear32 | value1.BitsUnknown32 | value2.BitsClear32 | value2.BitsUnknown32;

			return affected;
		}

		private static ulong AffectedBits_And64(InstructionNode node, TransformContext transformContext)
		{
			var value1 = transformContext.GetBitValue(node.Operand1);
			var value2 = transformContext.GetBitValue(node.Operand2);

			var affected = value1.BitsClear | value1.BitsUnknown | value2.BitsClear | value2.BitsUnknown;

			return affected;
		}

		private static ulong DemandedBits_And32(InstructionNode node, TransformContext transformContext)
		{
			if (node.Operand1.IsVirtualRegister && node.Operand2.IsResolvedConstant)
				return node.Operand2.ConstantUnsigned32;

			if (node.Operand1.IsResolvedConstant && node.Operand2.IsVirtualRegister)
				return node.Operand1.ConstantUnsigned32;

			return uint.MaxValue;
		}

		private static ulong DemandedBits_And64(InstructionNode node, TransformContext transformContext)
		{
			if (node.Operand1.IsVirtualRegister && node.Operand2.IsResolvedConstant)
				return node.Operand2.ConstantUnsigned64;

			if (node.Operand1.IsResolvedConstant && node.Operand2.IsVirtualRegister)
				return node.Operand1.ConstantUnsigned64;

			return ulong.MaxValue;
		}

		private static ulong DemandedBits_Or32(InstructionNode node, TransformContext transformContext)
		{
			if (node.Operand1.IsVirtualRegister && node.Operand2.IsResolvedConstant)
				return ~node.Operand2.ConstantUnsigned32;

			if (node.Operand1.IsResolvedConstant && node.Operand2.IsVirtualRegister)
				return ~node.Operand1.ConstantUnsigned32;

			return uint.MaxValue;
		}

		private static ulong DemandedBits_Or64(InstructionNode node, TransformContext transformContext)
		{
			if (node.Operand1.IsVirtualRegister && node.Operand2.IsResolvedConstant)
				return ~node.Operand2.ConstantUnsigned64;

			if (node.Operand1.IsResolvedConstant && node.Operand2.IsVirtualRegister)
				return ~node.Operand1.ConstantUnsigned64;

			return ulong.MaxValue;
		}

		private static ulong DemandedBits_ShiftRight32(InstructionNode node, TransformContext transformContext)
		{
			if (node.Operand1.IsVirtualRegister && node.Operand2.IsResolvedConstant)
				return (uint.MaxValue << (node.Operand2.ConstantSigned32 & 0x3F)) & uint.MaxValue;

			if (node.Operand1.IsResolvedConstant && node.Operand2.IsVirtualRegister)
				return (uint.MaxValue << (node.Operand1.ConstantSigned32 & 0x3F)) & uint.MaxValue;

			return uint.MaxValue;
		}

		private static ulong DemandedBits_ShiftRight64(InstructionNode node, TransformContext transformContext)
		{
			if (node.Operand1.IsVirtualRegister && node.Operand2.IsResolvedConstant)
				return ulong.MaxValue << (node.Operand2.ConstantSigned32 & 0x3F);

			if (node.Operand1.IsResolvedConstant && node.Operand2.IsVirtualRegister)
				return ulong.MaxValue << (node.Operand1.ConstantSigned32 & 0x3F);

			return ulong.MaxValue;
		}

		#endregion IR Instructions
	}
}

//Register(IRInstruction.ShiftRight32, ShiftRight32);
//Register(IRInstruction.ShiftRight64, ShiftRight64);
