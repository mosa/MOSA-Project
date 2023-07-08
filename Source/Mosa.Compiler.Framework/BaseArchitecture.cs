// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Reflection;
using Mosa.Compiler.Framework.Linker.Elf;
using Mosa.Utility.Configuration;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Base Architecture
/// </summary>
public abstract class BaseArchitecture
{
	#region Properties

	/// <summary>
	/// Gets the type of the elf machine.
	/// </summary>
	public abstract MachineType ElfMachineType { get; }

	/// <summary>
	/// Gets the register set of the architecture.
	/// </summary>
	public abstract PhysicalRegister[] RegisterSet { get; }

	/// <summary>
	/// Gets the stack frame register of the architecture.
	/// </summary>
	public abstract PhysicalRegister StackFrameRegister { get; }

	/// <summary>
	/// Returns the stack pointer register of the architecture.
	/// </summary>
	public abstract PhysicalRegister StackPointerRegister { get; }

	/// <summary>
	/// Gets the return register.
	/// </summary>
	public abstract PhysicalRegister ReturnRegister { get; }

	/// <summary>
	/// Gets the return register for the high portion of the 64bit result.
	/// </summary>
	public abstract PhysicalRegister ReturnHighRegister { get; }

	/// <summary>
	/// Gets the return floating point register.
	/// </summary>
	public abstract PhysicalRegister ReturnFloatingPointRegister { get; }

	/// <summary>
	/// Retrieves the program counter register of the architecture.
	/// </summary>
	public abstract PhysicalRegister ProgramCounter { get; }

	/// <summary>
	/// Retrieves the exception register of the architecture.
	/// </summary>
	public abstract PhysicalRegister ExceptionRegister { get; }

	/// <summary>
	/// Gets the finally return block register.
	/// </summary>
	public abstract PhysicalRegister LeaveTargetRegister { get; }

	/// <summary>
	/// Returns the link register register of the architecture.
	/// </summary>
	public abstract PhysicalRegister LinkRegister { get; }

	/// <summary>
	/// Gets the name of the platform.
	/// </summary>
	public abstract string PlatformName { get; }

	/// <summary>
	/// Gets the native alignment of the architecture in bytes.
	/// </summary>
	public uint NativeAlignment => NativePointerSize;

	/// <summary>
	/// Gets the native size of architecture in bytes.
	/// </summary>
	public abstract uint NativePointerSize { get; }

	/// <summary>
	/// Is the platform is 32 bit
	/// </summary>
	/// <value>
	///   <c>true</c> if [is32 bit]; otherwise, <c>false</c>.
	/// </value>
	public virtual bool Is32BitPlatform => NativePointerSize == 4;

	/// <summary>
	/// Is the platform is 64 bit
	/// </summary>
	public virtual bool Is64BitPlatform => NativePointerSize == 8;

	/// <summary>
	/// Gets the offset of first local.
	/// </summary>
	public virtual int OffsetOfFirstLocal => 0;

	/// <summary>
	/// Gets the offset of first parameter.
	/// </summary>
	public virtual int OffsetOfFirstParameter => (int)NativePointerSize * 2;

	/// <summary>
	/// Gets the instructions.
	/// </summary>
	public virtual List<BaseInstruction> Instructions { get; }

	#endregion Properties

	#region Members

	protected Dictionary<string, IntrinsicMethodDelegate> PlatformIntrinsicMethods { get; }

	#endregion Members

	#region Constructor

	protected BaseArchitecture()
	{
		PlatformIntrinsicMethods = GetPlatformIntrinsicMethods();
	}

	#endregion Constructor

	#region Methods

	public abstract OpcodeEncoder GetOpcodeEncoder();

	/// <summary>
	/// Extends the compiler pipeline with architecture specific compiler stages.
	/// </summary>
	/// <param name="pipeline">The pipeline to extend.</param>
	public abstract void ExtendCompilerPipeline(Pipeline<BaseCompilerStage> pipeline, MosaSettings mosaSettings);

	/// <summary>
	///	Requests the architecture to add architecture specific compilation stages to the pipeline. These
	/// may depend upon the current state of the pipeline.</summary>
	/// <param name="pipeline">The pipeline of the method compiler to add architecture specific compilation stages to.</param>
	/// <param name="mosaSettings">The compiler options.</param>
	public abstract void ExtendMethodCompilerPipeline(Pipeline<BaseMethodCompilerStage> pipeline, MosaSettings mosaSettings);

	/// <summary>
	/// Create platform move.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <param name="destination">The destination.</param>
	/// <param name="source">The source.</param>
	public abstract void InsertMoveInstruction(Context context, Operand destination, Operand source);

	/// <summary>
	/// Inserts the store instruction.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <param name="destination">The destination.</param>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public abstract void InsertStoreInstruction(Context context, Operand destination, Operand offset, Operand value);

	/// <summary>
	/// Inserts the load instruction.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <param name="destination">The destination.</param>
	/// <param name="source">The source.</param>
	/// <param name="offset">The offset.</param>
	public abstract void InsertLoadInstruction(Context context, Operand destination, Operand source, Operand offset);

	/// <summary>
	/// Create platform exchange registers.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <param name="destination">The destination.</param>
	/// <param name="source">The source.</param>
	public abstract void InsertExchangeInstruction(Context context, Operand destination, Operand source);

	/// <summary>
	/// Inserts the jump instruction.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <param name="destination">The destination.</param>
	public abstract void InsertJumpInstruction(Context context, BasicBlock destination);

	/// <summary>
	/// Determines whether [is instruction move] [the specified instruction].
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <returns></returns>
	public abstract bool IsInstructionMove(BaseInstruction instruction);

	/// <summary>
	/// Gets the platform intrinsic method.
	/// </summary>
	/// <param name="name">The name.</param>
	/// <returns></returns>
	public IntrinsicMethodDelegate GetInstrinsicMethod(string name)
	{
		PlatformIntrinsicMethods.TryGetValue(name, out IntrinsicMethodDelegate value);

		return value;
	}

	protected Dictionary<string, IntrinsicMethodDelegate> GetPlatformIntrinsicMethods()
	{
		var platformIntrinsicMethods = new Dictionary<string, IntrinsicMethodDelegate>();

		foreach (var type in GetType().Assembly.GetTypes())
		{
			if (!type.IsClass)
				continue;

			foreach (var method in type.GetRuntimeMethods())
			{
				// Now get all the IntrinsicMethodAttribute attributes
				var attributes = (IntrinsicMethodAttribute[])method.GetCustomAttributes(typeof(IntrinsicMethodAttribute), true);

				for (var i = 0; i < attributes.Length; i++)
				{
					var d = (IntrinsicMethodDelegate)System.Delegate.CreateDelegate(typeof(IntrinsicMethodDelegate), method);

					// Finally add the dictionary entry mapping the target name and the delegate
					platformIntrinsicMethods.Add(attributes[i].Target, d);
				}
			}
		}

		return platformIntrinsicMethods;
	}

	#endregion Methods
}
