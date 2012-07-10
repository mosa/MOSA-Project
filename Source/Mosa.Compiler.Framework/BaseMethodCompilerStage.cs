/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Basic base class for method compiler pipeline stages
	/// </summary>
	public abstract class BaseMethodCompilerStage
	{
		#region Data members

		/// <summary>
		/// Hold the method compiler
		/// </summary>
		protected BaseMethodCompiler methodCompiler;

		/// <summary>
		/// The architecture of the compilation process
		/// </summary>
		protected IArchitecture architecture;

		/// <summary>
		/// Holds the instruction set
		/// </summary>
		protected InstructionSet instructionSet;

		/// <summary>
		/// List of basic blocks found during decoding
		/// </summary>
		protected BasicBlocks basicBlocks;

		/// <summary>
		/// Holds the type system
		/// </summary>
		protected ITypeSystem typeSystem;

		/// <summary>
		/// Holds the modules type system
		/// </summary>
		protected ITypeModule typeModule;

		/// <summary>
		/// Holds the assembly loader
		/// </summary>
		protected IAssemblyLoader assemblyLoader;

		/// <summary>
		/// Holds the type layout interface
		/// </summary>
		protected ITypeLayout typeLayout;

		/// <summary>
		/// Holds the calling convention interface
		/// </summary>
		protected ICallingConvention callingConvention;

		/// <summary>
		/// Holds the Native Pointer Size
		/// </summary>
		protected int nativePointerSize;

		/// <summary>
		/// Holds the Native Pointer Alignment
		/// </summary>
		protected int nativePointerAlignment;

		#endregion // Data members

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public virtual string Name { get { return this.GetType().Name; } }

		#endregion // IPipelineStage Members

		#region IMethodCompilerStage members

		/// <summary>
		/// Setups the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public void Setup(BaseMethodCompiler compiler)
		{
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");

			methodCompiler = compiler;
			instructionSet = compiler.InstructionSet;
			basicBlocks = compiler.BasicBlocks;
			architecture = compiler.Architecture;
			typeModule = compiler.Method.Module;
			typeSystem = compiler.TypeSystem;
			typeLayout = compiler.TypeLayout;
			callingConvention = architecture.CallingConvention;

			architecture.GetTypeRequirements(BuiltInSigType.IntPtr, out nativePointerSize, out nativePointerAlignment);
		}

		#endregion // IMethodCompilerStage members

		#region Methods

		/// <summary>
		/// Gets a value indicating whether this instance has exception or finally.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has exception or finally; otherwise, <c>false</c>.
		/// </value>
		protected bool HasExceptionOrFinally
		{
			get
			{
				return methodCompiler.ExceptionClauseHeader.Clauses.Count != 0;
			}
		}

		/// <summary>
		/// Creates the context.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		protected Context CreateContext(BasicBlock block)
		{
			return new Context(instructionSet, block);
		}

		/// <summary>
		/// Creates the context.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		protected Context CreateContext(int index)
		{
			return new Context(instructionSet, index);
		}

		/// <summary>
		/// Allocates the virtual register.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		protected Operand AllocateVirtualRegister(SigType type)
		{
			return methodCompiler.VirtualRegisterLayout.AllocateVirtualRegister(type);
		}

		#endregion

		#region Trace Helper Methods

		protected void Trace(CompilerEvent compilerEvent, string message)
		{
			methodCompiler.InternalTrace.CompilerEventListener.SubmitTraceEvent(compilerEvent, message);
		}

		protected void Trace(string line)
		{
			methodCompiler.InternalTrace.TraceListener.SubmitDebugStageInformation(methodCompiler.Method, Name, line);
		}

		protected bool IsLogging { get { return methodCompiler.InternalTrace.TraceFilter.IsLogging; } }

		/// <summary>
		/// Updates the counter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="count">The count.</param>
		protected void UpdateCounter(string name, int count)
		{
			methodCompiler.Compiler.Counters.UpdateCounter(name, count);
		}

		#endregion

		#region Utility Methods

		/// <summary>
		/// Converts the specified opcode.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		/// <returns></returns>
		public static IR.ConditionCode ConvertCondition(CIL.OpCode opcode)
		{
			switch (opcode)
			{
				// Signed
				case CIL.OpCode.Beq_s: return IR.ConditionCode.Equal;
				case CIL.OpCode.Bge_s: return IR.ConditionCode.GreaterOrEqual;
				case CIL.OpCode.Bgt_s: return IR.ConditionCode.GreaterThan;
				case CIL.OpCode.Ble_s: return IR.ConditionCode.LessOrEqual;
				case CIL.OpCode.Blt_s: return IR.ConditionCode.LessThan;
				// Unsigned
				case CIL.OpCode.Bne_un_s: return IR.ConditionCode.NotEqual;
				case CIL.OpCode.Bge_un_s: return IR.ConditionCode.UnsignedGreaterOrEqual;
				case CIL.OpCode.Bgt_un_s: return IR.ConditionCode.UnsignedGreaterThan;
				case CIL.OpCode.Ble_un_s: return IR.ConditionCode.UnsignedLessOrEqual;
				case CIL.OpCode.Blt_un_s: return IR.ConditionCode.UnsignedLessThan;
				// Long form signed
				case CIL.OpCode.Beq: goto case CIL.OpCode.Beq_s;
				case CIL.OpCode.Bge: goto case CIL.OpCode.Bge_s;
				case CIL.OpCode.Bgt: goto case CIL.OpCode.Bgt_s;
				case CIL.OpCode.Ble: goto case CIL.OpCode.Ble_s;
				case CIL.OpCode.Blt: goto case CIL.OpCode.Blt_s;
				// Long form unsigned
				case CIL.OpCode.Bne_un: goto case CIL.OpCode.Bne_un_s;
				case CIL.OpCode.Bge_un: goto case CIL.OpCode.Bge_un_s;
				case CIL.OpCode.Bgt_un: goto case CIL.OpCode.Bgt_un_s;
				case CIL.OpCode.Ble_un: goto case CIL.OpCode.Ble_un_s;
				case CIL.OpCode.Blt_un: goto case CIL.OpCode.Blt_un_s;
				// Compare
				case CIL.OpCode.Ceq: return IR.ConditionCode.Equal;
				case CIL.OpCode.Cgt: return IR.ConditionCode.GreaterThan;
				case CIL.OpCode.Cgt_un: return IR.ConditionCode.UnsignedGreaterThan;
				case CIL.OpCode.Clt: return IR.ConditionCode.LessThan;
				case CIL.OpCode.Clt_un: return IR.ConditionCode.UnsignedLessThan;

				default: throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Gets the unsigned condition code.
		/// </summary>
		/// <param name="conditionCode">The condition code to get an unsigned form from.</param>
		/// <returns>The unsigned form of the given condition code.</returns>
		protected static IR.ConditionCode GetUnsignedConditionCode(IR.ConditionCode conditionCode)
		{
			switch (conditionCode)
			{
				case IR.ConditionCode.Equal: break;
				case IR.ConditionCode.NotEqual: break;
				case IR.ConditionCode.GreaterOrEqual: return IR.ConditionCode.UnsignedGreaterOrEqual;
				case IR.ConditionCode.GreaterThan: return IR.ConditionCode.UnsignedGreaterThan;
				case IR.ConditionCode.LessOrEqual: return IR.ConditionCode.UnsignedLessOrEqual;
				case IR.ConditionCode.LessThan: return IR.ConditionCode.UnsignedLessThan;
				case IR.ConditionCode.UnsignedGreaterOrEqual: break;
				case IR.ConditionCode.UnsignedGreaterThan: break;
				case IR.ConditionCode.UnsignedLessOrEqual: break;
				case IR.ConditionCode.UnsignedLessThan: break;
				default: throw new NotSupportedException();
			}

			return conditionCode;
		}

		/// <summary>
		/// Gets the opposite condition code.
		/// </summary>
		/// <param name="conditionCode">The condition code.</param>
		/// <returns></returns>
		protected static IR.ConditionCode GetOppositeConditionCode(IR.ConditionCode conditionCode)
		{
			switch (conditionCode)
			{
				case IR.ConditionCode.Equal: return IR.ConditionCode.NotEqual;
				case IR.ConditionCode.NotEqual: return IR.ConditionCode.Equal;
				case IR.ConditionCode.GreaterOrEqual: return IR.ConditionCode.LessThan;
				case IR.ConditionCode.GreaterThan: return IR.ConditionCode.LessOrEqual;
				case IR.ConditionCode.LessOrEqual: return IR.ConditionCode.GreaterThan;
				case IR.ConditionCode.LessThan: return IR.ConditionCode.GreaterOrEqual;
				case IR.ConditionCode.UnsignedGreaterOrEqual: return IR.ConditionCode.UnsignedLessThan;
				case IR.ConditionCode.UnsignedGreaterThan: return IR.ConditionCode.UnsignedLessOrEqual;
				case IR.ConditionCode.UnsignedLessOrEqual: return IR.ConditionCode.UnsignedGreaterThan;
				case IR.ConditionCode.UnsignedLessThan: return IR.ConditionCode.UnsignedGreaterOrEqual;
				case IR.ConditionCode.Signed: return IR.ConditionCode.NotSigned;
				case IR.ConditionCode.NotSigned: return IR.ConditionCode.Signed;
				default: throw new NotSupportedException();
			}

		}

		#endregion // Utility Methods

	}
}