// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Base class of a method compiler.
/// </summary>
/// <remarks>
/// A method compiler is responsible for compiling a single method of an object.
/// </remarks>
public abstract class MethodCompiler
{
	#region Properties

	/// <summary>
	/// Gets the Architecture to compile for.
	/// </summary>
	public BaseArchitecture Architecture { get; set; }

	/// <summary>
	/// Gets the linker used to resolve external symbols.
	/// </summary>
	public MosaLinker Linker { get; set; }

	/// <summary>
	/// Gets the method implementation being compiled.
	/// </summary>
	public MosaMethod Method { get; set; }

	/// <summary>
	/// Gets the basic blocks.
	/// </summary>
	/// <value>The basic blocks.</value>
	public BasicBlocks BasicBlocks { get; set; }

	/// <summary>
	/// Retrieves the compilation scheduler.
	/// </summary>
	/// <value>The compilation scheduler.</value>
	public MethodScheduler MethodScheduler { get; set; }

	/// <summary>
	/// Provides access to the pipeline of this compiler.
	/// </summary>
	public Pipeline<BaseMethodCompilerStage> Pipeline { get; set; }

	/// <summary>
	/// Gets the type system.
	/// </summary>
	/// <value>The type system.</value>
	public TypeSystem TypeSystem { get; set; }

	/// <summary>
	/// Gets the type layout.
	/// </summary>
	/// <value>The type layout.</value>
	public MosaTypeLayout TypeLayout { get; set; }

	/// <summary>
	/// Gets the local variables.
	/// </summary>
	public Operand[] LocalVariables { get; set; }

	/// <summary>
	/// Gets the assembly compiler.
	/// </summary>
	public Compiler Compiler { get; set; }

	/// <summary>
	/// Gets the stack.
	/// </summary>
	public List<Operand> LocalStack { get; set; }

	/// <summary>
	/// Gets or sets the size of the stack.
	/// </summary>
	/// <value>
	/// The size of the stack memory.
	/// </value>
	public int StackSize { get; set; }

	/// <summary>
	/// Gets the virtual register layout.
	/// </summary>
	public VirtualRegisters VirtualRegisters { get; set; }

	/// <summary>
	/// Gets the parameters.
	/// </summary>
	public Operand[] Parameters { get; set; }

	/// <summary>
	/// Gets the protected regions.
	/// </summary>
	public List<ProtectedRegion> ProtectedRegions { get; set; }

	public bool HasProtectedRegions { get; set; }

	/// <summary>
	/// The labels
	/// </summary>
	public Dictionary<int, int> Labels { get; set; }

	/// <summary>
	/// Gets the thread identifier.
	/// </summary>
	public int ThreadID { get; set; }

	/// <summary>
	/// Gets the compiler method data.
	/// </summary>
	public MethodData MethodData { get; set; }

	/// <summary>
	/// Gets the platform constant zero
	/// </summary>
	public Operand ConstantZero { get; set; }

	/// <summary>
	/// Gets the 32-bit constant zero.
	/// </summary>
	public Operand Constant32_0 { get; set; }

	/// <summary>
	/// Gets the 64-bit constant zero.
	/// </summary>
	public Operand Constant64_0 { get; set; }

	/// <summary>
	/// Gets the R4 constant zero.
	/// </summary>
	public Operand ConstantR4_0 { get; set; }

	/// <summary>
	/// Gets the R4 constant zero.
	/// </summary>
	public Operand ConstantR8_0 { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether this instance is execute pipeline.
	/// </summary>
	public bool IsExecutePipeline { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether this method requires CIL decoding .
	/// </summary>
	public bool IsCILStream { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether this method is plugged.
	/// </summary>
	public bool IsMethodPlugged { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether this instance is stack frame required.
	/// </summary>
	public bool IsStackFrameRequired { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether this instance is method inlined.
	/// </summary>
	public bool IsMethodInlined { get; set; }

	/// <summary>
	/// Holds flag that will stop method compiler
	/// </summary>
	public bool IsStopped { get; set; }

	public bool Statistics { get; set; }

	/// <summary>
	/// Gets the linker symbol.
	/// </summary>
	public LinkerSymbol Symbol
	{ get => MethodData.Symbol; set => MethodData.Symbol = value; }

	/// <summary>
	/// Gets the method scanner.
	/// </summary>
	public MethodScanner MethodScanner { get; set; }

	public CompilerHooks CompilerHooks { get; set; }

	public bool IsInSSAForm { get; set; }

	public bool AreCPURegistersAllocated { get; set; }

	public bool IsLocalStackFinalized { get; set; }

	public bool Is32BitPlatform { get; set; }

	public bool Is64BitPlatform { get; set; }

	public int? MethodTraceLevel { get; set; }

	#endregion Properties

	#region Methods

	/// <summary>
	/// Adds the stack local.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <returns></returns>
	public Operand AddStackLocal(MosaType type)
	{
		return AddStackLocal(type, false);
	}

	/// <summary>
	/// Adds the stack local.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="pinned">if set to <c>true</c> [pinned].</param>
	/// <returns></returns>
	public Operand AddStackLocal(MosaType type, bool pinned)
	{
		var local = Operand.CreateStackLocal(type, LocalStack.Count, pinned);
		LocalStack.Add(local);
		return local;
	}

	/// <summary>
	/// Compiles the method referenced by this method compiler.
	/// </summary>
	public void Compile()
	{
		try
		{
			PlugMethod();

			PatchDelegate();

			ExternalMethod();

			InternalMethod();

			StubMethod();

			ExecutePipeline();

			Symbol.SetReplacementStatus(MethodData.Inlined);

			if (Statistics)
			{
				var log = new TraceLog(TraceType.MethodCounters, Method, string.Empty, MethodData.Version);

				log.Log(MethodData.Counters.Export());

				Compiler.PostTraceLog(log);
			}
		}
		catch (Exception exception)
		{
			Compiler.PostEvent(CompilerEvent.Exception, $"Method: {Method} -> {exception.Message}");

			var exceptionLog = new TraceLog(TraceType.MethodDebug, Method, "Exception", MethodData.Version);

			exceptionLog.Log(exception.Message);
			exceptionLog.Log("");
			exceptionLog.Log(exception.ToString());

			Compiler.PostTraceLog(exceptionLog);

			Stop();
			Compiler.Stop();
		}
	}

	public abstract void ExecutePipeline();

	public abstract void CreateTranformInstructionTrace(BaseMethodCompilerStage stage, int step);

	public abstract void PlugMethod();

	public bool IsTraceable(int tracelevel)
	{
		if (MethodTraceLevel.HasValue)
			return MethodTraceLevel.Value >= tracelevel;
		else
			return Compiler.IsTraceable(tracelevel);
	}

	public void PostTraceLog(TraceLog traceLog)
	{
		Compiler.PostTraceLog(traceLog);
	}

	private void PatchDelegate()
	{
		if (Method.HasImplementation)
			return;

		if (!Method.DeclaringType.IsDelegate)
			return;

		if (!DelegatePatcher.Patch(this))
			return;

		IsCILStream = false;
		IsExecutePipeline = true;

		if (IsTraceable(5))
		{
			var traceLog = new TraceLog(TraceType.MethodDebug, Method, "XX-Delegate Patched", MethodData.Version);
			traceLog?.Log("This delegate method was patched");
			PostTraceLog(traceLog);
		}
	}

	public abstract void ExternalMethod();

	public abstract void InternalMethod();

	public abstract void StubMethod();

	/// <summary>
	/// Stops the method compiler.
	/// </summary>
	/// <returns></returns>
	public void Stop()
	{
		IsStopped = true;
	}

	/// <summary>
	/// Creates a new virtual register operand.
	/// </summary>
	/// <param name="type">The signature type of the virtual register.</param>
	/// <returns>
	/// An operand, which represents the virtual register.
	/// </returns>
	public Operand CreateVirtualRegister(MosaType type)
	{
		return VirtualRegisters.Allocate(type);
	}

	/// <summary>
	/// Splits the long operand.
	/// </summary>
	/// <param name="longOperand">The long operand.</param>
	public void SplitLongOperand(Operand longOperand)
	{
		VirtualRegisters.SplitLongOperand(TypeSystem, longOperand);
	}

	/// <summary>
	/// Splits the long operand.
	/// </summary>
	/// <param name="operand">The operand.</param>
	/// <param name="operandLow">The operand low.</param>
	/// <param name="operandHigh">The operand high.</param>
	public void SplitLongOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
	{
		var is64Bit = operand.IsInteger64;

		if (!operand.IsInteger64 && !operand.IsInteger32)
		{
			// figure it out
			var underlyingType = MosaTypeLayout.GetUnderlyingType(operand.Type);

			if (underlyingType.IsUI8)
				is64Bit = true;

			if (Is64BitPlatform)
			{
				if (underlyingType.IsPointer
					|| underlyingType.IsFunctionPointer
					|| underlyingType.IsN
					|| underlyingType.IsManagedPointer
					|| underlyingType.IsReferenceType)
					is64Bit = true;
			}
		}

		if (is64Bit)
		{
			SplitLongOperand(operand);
			operandLow = operand.Low;
			operandHigh = operand.High;
		}
		else
		{
			operandLow = operand;
			operandHigh = Constant32_0;
		}
	}

	/// <summary>
	/// Allocates the virtual register or stack slot.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <returns></returns>
	public Operand AllocateVirtualRegisterOrStackSlot(MosaType type)
	{
		if (MosaTypeLayout.IsUnderlyingPrimitive(type))
		{
			var resultType = Compiler.GetStackType(type);
			return CreateVirtualRegister(resultType);
		}
		else
		{
			return AddStackLocal(type);
		}
	}

	/// <summary>
	/// Allocates the local variable virtual registers.
	/// </summary>
	/// <param name="locals">The locals.</param>
	public void SetLocalVariables(IList<MosaLocal> locals)
	{
		LocalVariables = new Operand[locals.Count];

		var index = 0;
		foreach (var local in locals)
		{
			var localtype = local.Type;
			var underlyingType = localtype;
			//var underlyingType = MosaTypeLayout.GetUnderlyingType(local.Type);

			if (MosaTypeLayout.IsUnderlyingPrimitive(underlyingType) && !local.IsPinned)
			{
				var stacktype = Compiler.GetStackType(underlyingType);
				LocalVariables[index++] = CreateVirtualRegister(stacktype);
			}
			else
			{
				LocalVariables[index++] = AddStackLocal(underlyingType, local.IsPinned);
			}
		}
	}

	/// <summary>
	/// Gets the size of the reference or type.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="aligned">if set to <c>true</c> [aligned].</param>
	/// <returns></returns>
	public uint GetReferenceOrTypeSize(MosaType type, bool aligned)
	{
		if (type.IsValueType)
		{
			if (aligned)
				return Alignment.AlignUp(TypeLayout.GetTypeSize(type), Architecture.NativeAlignment);
			else
				return TypeLayout.GetTypeSize(type);
		}
		else
		{
			return Architecture.NativeAlignment;
		}
	}

	/// <summary>
	/// Gets the position.
	/// </summary>
	/// <param name="label">The label.</param>
	/// <returns></returns>
	public int GetPosition(int label)
	{
		return Labels[label];
	}

	/// <summary>
	/// Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>
	/// A <see cref="System.String" /> that represents this instance.
	/// </returns>
	public override string ToString()
	{
		return Method.ToString();
	}

	#endregion Methods

	#region Constant Helper Methods

	public Operand CreateConstant(byte value)
	{
		return Operand.CreateConstant(TypeSystem.BuiltIn.U1, value);
	}

	public Operand CreateConstant(int value)
	{
		return Operand.CreateConstant(TypeSystem.BuiltIn.I4, value);
	}

	public Operand CreateConstant(uint value)
	{
		return Operand.CreateConstant(TypeSystem.BuiltIn.U4, value);
	}

	public Operand CreateConstant(long value)
	{
		return Operand.CreateConstant(TypeSystem.BuiltIn.I8, value);
	}

	public Operand CreateConstant(ulong value)
	{
		return Operand.CreateConstant(TypeSystem.BuiltIn.U8, value);
	}

	public Operand CreateConstant(float value)
	{
		return Operand.CreateConstant(TypeSystem.BuiltIn.R4, value);
	}

	public Operand CreateConstant(double value)
	{
		return Operand.CreateConstant(TypeSystem.BuiltIn.R8, value);
	}

	#endregion Constant Helper Methods
}
