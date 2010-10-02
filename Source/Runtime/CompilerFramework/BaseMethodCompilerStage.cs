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
using System.Collections.Generic;
using System.Diagnostics;

using Mosa.Runtime.Vm;
using Mosa.Runtime.Loader;
using CIL = Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
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
		protected IMethodCompiler MethodCompiler;

		/// <summary>
		/// The architecture of the compilation process
		/// </summary>
		protected IArchitecture Architecture;

		/// <summary>
		/// Holds the instruction set
		/// </summary>
		protected InstructionSet InstructionSet;

		/// <summary>
		/// List of basic blocks found during decoding
		/// </summary>
		protected List<BasicBlock> BasicBlocks;

		/// <summary>
		/// Holds the type loader 
		/// </summary>
		protected ITypeSystem typeSystem;

		/// <summary>
		/// Holds the modules type system
		/// </summary>
		protected IModuleTypeSystem moduleTypeSystem; 

		/// <summary>
		/// Holds the assembly loader
		/// </summary>
		protected IAssemblyLoader assemblyLoader;

		/// <summary>
		/// Holds the type layout interface
		/// </summary>
		protected ITypeLayout typeLayout;

		#endregion // Data members

		#region IMethodCompilerStage members

		/// <summary>
		/// Setups the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public void Setup(IMethodCompiler compiler)
		{
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");

			MethodCompiler = compiler;
			InstructionSet = compiler.InstructionSet;
			BasicBlocks = compiler.BasicBlocks;
			Architecture = compiler.Architecture;
			moduleTypeSystem = compiler.Method.ModuleTypeSystem;
			typeSystem = compiler.TypeSystem;
			typeLayout = compiler.TypeLayout;
		}

		#endregion // IMethodCompilerStage members

		#region Methods

		/// <summary>
		/// Gets block by label
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		protected BasicBlock FindBlock(int label)
		{
			return MethodCompiler.FromLabel(label);
		}

		/// <summary>
		/// Creates the context.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		protected Context CreateContext(BasicBlock block)
		{
			return new Context(InstructionSet, block);
		}

		/// <summary>
		/// Creates the context.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		protected Context CreateContext(int index)
		{
			return new Context(InstructionSet, index);
		}

		/// <summary>
		/// Creates the block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		protected BasicBlock CreateBlock(int label, int index)
		{
			return MethodCompiler.CreateBlock(label, index);
		}

		/// <summary>
		/// Creates the block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		protected BasicBlock CreateBlock(int label)
		{
			return MethodCompiler.CreateBlock(label, -1);
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
