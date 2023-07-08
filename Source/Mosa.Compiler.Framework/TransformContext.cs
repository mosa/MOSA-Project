// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Managers;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

public sealed class TransformContext
{
	#region Properties

	public MethodCompiler MethodCompiler { get; private set; }

	public Compiler Compiler { get; private set; }

	public BitValueManager BitValueManager { get; private set; }

	public TypeSystem TypeSystem { get; private set; }

	public TraceLog TraceLog { get; private set; }

	public TraceLog SpecialTraceLog { get; private set; }

	public VirtualRegisters VirtualRegisters { get; private set; }

	public LocalStack LocalStack { get; set; }

	public BasicBlocks BasicBlocks { get; set; }

	public bool LowerTo32 { get; private set; }

	public bool IsInSSAForm { get; private set; }

	public bool AreCPURegistersAllocated { get; private set; }

	public bool Is32BitPlatform { get; private set; }

	public int Window { get; private set; }

	public bool Devirtualization { get; private set; }

	#endregion Properties

	#region Properties - Indirect

	public uint NativePointerSize => Compiler.Architecture.NativePointerSize;

	public BaseArchitecture Architecture => Compiler.Architecture;

	public MosaMethod Method => MethodCompiler.Method;

	public MosaTypeLayout TypeLayout => MethodCompiler.TypeLayout;

	public MosaLinker Linker => Compiler.Linker;

	public MethodScanner MethodScanner => MethodCompiler.MethodScanner;

	public Operand ConstantZero => MethodCompiler.ConstantZero;

	#endregion Properties - Indirect

	#region Properties - Registers

	public Operand StackFrame => MethodCompiler.Compiler.StackFrame;

	public Operand StackPointer => MethodCompiler.Compiler.StackPointer;

	/// <summary>
	/// Gets the link register.
	/// </summary>
	public Operand LinkRegister => MethodCompiler.Compiler.LinkRegister;

	/// <summary>
	/// Gets the program counter
	/// </summary>
	public Operand ProgramCounter => MethodCompiler.Compiler.ProgramCounter;

	/// <summary>
	/// Gets the exception register.
	/// </summary>
	public Operand ExceptionRegister => MethodCompiler.Compiler.ExceptionRegister;

	/// <summary>
	/// Gets the leave target register.
	/// </summary>
	public Operand LeaveTargetRegister => MethodCompiler.Compiler.LeaveTargetRegister;

	#endregion Properties - Registers

	#region Properties - Instructions

	public BaseInstruction LoadInstruction { get; private set; }

	public BaseInstruction StoreInstruction { get; private set; }

	public BaseInstruction MoveInstruction { get; private set; }

	public BaseInstruction AddInstruction { get; private set; }

	public BaseInstruction SubInstruction { get; private set; }

	public BaseInstruction MulSignedInstruction { get; private set; }

	public BaseInstruction MulUnsignedInstruction { get; private set; }

	public BaseInstruction BranchInstruction { get; private set; }

	#endregion Properties - Instructions

	#region Data

	private readonly Dictionary<Type, BaseTransformManager> Managers = new Dictionary<Type, BaseTransformManager>();

	#endregion Data

	public void SetCompiler(Compiler compiler)
	{
		Compiler = compiler;

		Is32BitPlatform = Compiler.Architecture.Is32BitPlatform;
		TypeSystem = Compiler.TypeSystem;

		LowerTo32 = Compiler.MosaSettings.LongExpansion;
		Devirtualization = Compiler.MosaSettings.Devirtualization;
		Window = Math.Max(Compiler.MosaSettings.OptimizationBasicWindow, 1);

		LowerTo32 = Compiler.MosaSettings.LongExpansion;

		LoadInstruction = Is32BitPlatform ? IRInstruction.Load32 : IRInstruction.Load64;
		StoreInstruction = Is32BitPlatform ? IRInstruction.Store32 : IRInstruction.Store64;
		MoveInstruction = Is32BitPlatform ? IRInstruction.Move32 : IRInstruction.Move64;
		AddInstruction = Is32BitPlatform ? IRInstruction.Add32 : IRInstruction.Add64;
		SubInstruction = Is32BitPlatform ? IRInstruction.Sub32 : IRInstruction.Sub64;
		MulSignedInstruction = Is32BitPlatform ? IRInstruction.MulSigned32 : IRInstruction.MulSigned64;
		MulUnsignedInstruction = Is32BitPlatform ? IRInstruction.MulUnsigned32 : IRInstruction.MulUnsigned64;
		BranchInstruction = Is32BitPlatform ? IRInstruction.Branch32 : IRInstruction.Branch64;
	}

	public void SetMethodCompiler(MethodCompiler methodCompiler)
	{
		MethodCompiler = methodCompiler;

		VirtualRegisters = methodCompiler.VirtualRegisters;
		LocalStack = methodCompiler.LocalStack;

		BasicBlocks = methodCompiler.BasicBlocks;

		AreCPURegistersAllocated = methodCompiler.AreCPURegistersAllocated;
		IsInSSAForm = methodCompiler.IsInSSAForm;

		LowerTo32 = false;
		TraceLog = null;
		Managers.Clear();

		BitValueManager = null;
	}

	public void SetStageOptions(bool lowerTo32)
	{
		LowerTo32 = Compiler.MosaSettings.LongExpansion && lowerTo32 && Is32BitPlatform;
	}

	#region Manager

	public T GetManager<T>() where T : class
	{
		Managers.TryGetValue(typeof(T), out var manager);

		return manager as T;
	}

	public void AddManager(BaseTransformManager transformManager)
	{
		Managers.Add(transformManager.GetType(), transformManager);

		// minor hack for now
		BitValueManager = transformManager as BitValueManager;
	}

	#endregion Manager

	public void ClearLogs()
	{
		TraceLog = null;
		SpecialTraceLog = null;
	}

	public void SetLog(TraceLog traceLog)
	{
		TraceLog = traceLog;
	}

	public void SetLogs(TraceLog traceLog = null, TraceLog specialTraceLog = null)
	{
		TraceLog = traceLog;
		SpecialTraceLog = specialTraceLog;
	}

	public bool ApplyTransform(Context context, BaseTransform transform, int count)
	{
		if (!transform.Match(context, this))
			return false;

		TraceBefore(context, transform, count);

		transform.Transform(context, this);

		TraceAfter(context);

		return true;
	}

	#region Trace

	public void TraceBefore(Context context, BaseTransform transformation, int count)
	{
		TraceLog?.Log($"[{context.Block}-{count}] {transformation.Name}");

		if (transformation.Log)
			SpecialTraceLog?.Log($"{transformation.Name}\t{Method.FullName} at {context}");

		TraceLog?.Log($"BEFORE:\t{context}");
	}

	public void TraceAfter(Context context)
	{
		TraceLog?.Log($"AFTER: \t{context}");
		TraceLog?.Log();
	}

	#endregion Trace

	#region Basic Block Helpers

	/// <summary>
	/// Splits the block.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <returns></returns>
	public Context Split(Context context)
	{
		return new Context(Split(context.Node));
	}

	/// <summary>
	/// Splits the block.
	/// </summary>
	/// <param name="node">The node.</param>
	/// <returns></returns>
	public BasicBlock Split(InstructionNode node)
	{
		var newblock = CreateNewBlock(-1, node.Label);

		node.Split(newblock);

		return newblock;
	}

	/// <summary>
	/// Creates the new block.
	/// </summary>
	/// <param name="blockLabel">The label.</param>
	/// <param name="instructionLabel">The instruction label.</param>
	/// <returns></returns>
	private BasicBlock CreateNewBlock(int blockLabel, int instructionLabel)
	{
		return BasicBlocks.CreateBlock(blockLabel, instructionLabel);
	}

	/// <summary>
	/// Creates empty blocks.
	/// </summary>
	/// <param name="blocks">The Blocks.</param>
	/// <param name="instructionLabel">The instruction label.</param>
	/// <returns></returns>
	public Context[] CreateNewBlockContexts(int blocks, int instructionLabel)
	{
		// Allocate the context array
		var result = new Context[blocks];

		for (var index = 0; index < blocks; index++)
		{
			result[index] = CreateNewBlockContext(instructionLabel);
		}

		return result;
	}

	/// <summary>
	/// Create an empty block.
	/// </summary>
	/// <param name="instructionLabel">The instruction label.</param>
	/// <returns></returns>
	private Context CreateNewBlockContext(int instructionLabel)
	{
		return new Context(CreateNewBlock(-1, instructionLabel));
	}

	#endregion Basic Block Helpers

	#region Phi Helpers

	public static void UpdatePhiTarget(BasicBlock target, BasicBlock source, BasicBlock newSource)
	{
		BaseMethodCompilerStage.UpdatePhiTarget(target, source, newSource);
	}

	public static void UpdatePhiTargets(List<BasicBlock> targets, BasicBlock source, BasicBlock newSource)
	{
		BaseMethodCompilerStage.UpdatePhiTargets(targets, source, newSource);
	}

	public static void UpdatePhiBlocks(List<BasicBlock> phiBlocks)
	{
		BaseMethodCompilerStage.UpdatePhiBlocks(phiBlocks);
	}

	public static void UpdatePhiBlock(BasicBlock phiBlock)
	{
		BaseMethodCompilerStage.UpdatePhiBlock(phiBlock);
	}

	public static void UpdatePhi(InstructionNode node)
	{
		BaseMethodCompilerStage.UpdatePhi(node);
	}

	#endregion Phi Helpers

	#region Move Helpers

	public void MoveOperand1ToVirtualRegister(Context context, BaseInstruction moveInstruction)
	{
		Debug.Assert(!context.Operand1.IsVirtualRegister);

		var operand1 = context.Operand1;

		var v1 = VirtualRegisters.Allocate(operand1);

		context.InsertBefore().AppendInstruction(moveInstruction, v1, operand1);
		context.Operand1 = v1;
	}

	public void MoveOperand2ToVirtualRegister(Context context, BaseInstruction moveInstruction)
	{
		Debug.Assert(!context.Operand2.IsVirtualRegister);

		var operand2 = context.Operand2;

		var v1 = VirtualRegisters.Allocate(operand2);

		context.InsertBefore().AppendInstruction(moveInstruction, v1, operand2);
		context.Operand2 = v1;
	}

	public void MoveOperand1And2ToVirtualRegisters(Context context, BaseInstruction moveInstruction)
	{
		Debug.Assert(!context.Operand1.IsVirtualRegister || !context.Operand2.IsVirtualRegister);

		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		if (operand1.IsConstant && operand2.IsConstant && operand1.ConstantUnsigned64 == operand2.ConstantUnsigned64)
		{
			var v1 = VirtualRegisters.Allocate(operand1);

			context.InsertBefore().AppendInstruction(moveInstruction, v1, operand1);
			context.Operand1 = v1;
			context.Operand2 = v1;
			return;
		}
		else if (operand1.IsConstant && operand2.IsConstant)
		{
			var v1 = VirtualRegisters.Allocate(operand1);
			var v2 = VirtualRegisters.Allocate(operand2);

			context.InsertBefore().AppendInstruction(moveInstruction, v1, operand1);
			context.InsertBefore().AppendInstruction(moveInstruction, v2, operand2);
			context.Operand1 = v1;
			context.Operand2 = v2;
			return;
		}
		else if (operand1.IsConstant)
		{
			var v1 = VirtualRegisters.Allocate(operand1);

			context.InsertBefore().AppendInstruction(moveInstruction, v1, operand1);
			context.Operand1 = v1;
		}
		else if (operand2.IsConstant)
		{
			var v1 = VirtualRegisters.Allocate(operand2);

			context.InsertBefore().AppendInstruction(moveInstruction, v1, operand2);
			context.Operand2 = v1;
		}
	}

	#endregion Move Helpers

	#region Linker Helpers

	public Operand CreateR4Label(float value)
	{
		var symbol = Linker.GetConstantSymbol(value);

		var label = Operand.CreateLabelR4(symbol.Name);

		return label;
	}

	public Operand CreateR8Label(double value)
	{
		var symbol = Linker.GetConstantSymbol(value);

		var label = Operand.CreateLabelR8(symbol.Name);

		return label;
	}

	public Operand CreateFloatingPointLabel(Operand operand)
	{
		var symbol = operand.IsR4
			? Linker.GetConstantSymbol(operand.ConstantFloat)
			: Compiler.Linker.GetConstantSymbol(operand.ConstantDouble);

		var label = operand.IsR4
			? Operand.CreateLabelR4(symbol.Name)
			: Operand.CreateLabelR8(symbol.Name);

		return label;
	}

	#endregion Linker Helpers

	#region BitValue (experimental)

	public BitValue GetBitValue(Operand operand)
	{
		return BitValueManager.GetBitValue(operand);
	}

	public BitValue GetBitValueWithDefault(Operand operand)
	{
		return BitValueManager.GetBitValueDefaultAny(operand);
	}

	#endregion BitValue (experimental)

	#region Floating Point Helpers

	public Operand LoadValueR4(Context context, float value, BaseInstruction loadInstruction)
	{
		var label = CreateR4Label(value);

		var v1 = VirtualRegisters.AllocateR4();

		context.InsertBefore().SetInstruction(loadInstruction, v1, label, Operand.Constant32_0);

		return v1;
	}

	public Operand LoadValueR8(Context context, double value, BaseInstruction loadInstruction)
	{
		var label = CreateR8Label(value);

		var v1 = VirtualRegisters.AllocateR8();

		context.InsertBefore().SetInstruction(loadInstruction, v1, label, Operand.Constant32_0);

		return v1;
	}

	public Operand MoveConstantToFloatRegister(Context context, Operand operand, BaseInstruction instructionR4, BaseInstruction instructionR8)
	{
		if (!operand.IsConstant)
			return operand;

		var label = CreateFloatingPointLabel(operand);

		var v1 = operand.IsR4 ? VirtualRegisters.AllocateR4() : VirtualRegisters.AllocateR8();

		var instruction = operand.IsR4 ? instructionR4 : instructionR8;

		context.InsertBefore().SetInstruction(instruction, v1, label, Operand.Constant32_0);

		return v1;
	}

	#endregion Floating Point Helpers

	public void SplitOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
	{
		MethodCompiler.SplitOperand(operand, out operandLow, out operandHigh);
	}

	public (Operand Low, Operand High) SplitOperand(Operand operand)
	{
		MethodCompiler.SplitOperand(operand, out var low, out var high);

		return (low, high);
	}

	public MosaMethod GetMethod(string fullName, string methodName)
	{
		var type = TypeSystem.GetTypeByName(fullName);

		if (type == null)
			return null;

		var method = type.FindMethodByName(methodName);

		return method;
	}

	public void ReplaceWithCall(Context context, string fullName, string methodName)
	{
		var method = GetMethod(fullName, methodName);

		Debug.Assert(method != null, $"Cannot find method: {methodName}");

		// FUTURE: throw compiler exception

		var symbol = Operand.CreateLabel(method, Is32BitPlatform);

		if (context.OperandCount == 1)
		{
			context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, context.Operand1);
		}
		else if (context.OperandCount == 2)
		{
			context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, context.Operand1, context.Operand2);
		}
		else
		{
			// FUTURE: throw compiler exception
		}

		MethodScanner.MethodInvoked(method, Method);
	}

	public static void MoveConstantRight(Context context) // static
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		if (operand2.IsConstant || operand1.IsVirtualRegister)
			return;

		// Move constant to the right
		context.Operand1 = operand2;
		context.Operand2 = operand1;
		context.ConditionCode = context.ConditionCode.GetReverse();
	}

	public void OrderLoadStoreOperands(Context context)  // FUTURE: Rename
	{
		if (context.Operand1.IsResolvedConstant && context.Operand2.IsResolvedConstant)
		{
			context.Operand1 = Operand.CreateConstant(context.Operand1.ConstantUnsigned64 + context.Operand2.ConstantUnsigned64);
			context.Operand2 = Operand.Constant32_0;
		}

		if (context.Operand1.IsConstant && !context.Operand2.IsConstant)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			context.Operand2 = operand1;
			context.Operand1 = operand2;
		}
	}
}
