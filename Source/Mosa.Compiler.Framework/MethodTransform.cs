// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

// Note: Most code from BaseMethodCompilerStage

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Method Transform Helper
	/// </summary>
	public class MethodTransform
	{
		#region Properties

		public string CurrenStageName { get; set; }

		/// <summary>
		/// Hold the method compiler
		/// </summary>
		public BaseMethodCompiler MethodCompiler { get; }

		/// <summary>
		/// The architecture of the compilation process
		/// </summary>
		public BaseArchitecture Architecture { get; }

		/// <summary>
		/// Holds the type system
		/// </summary>
		public TypeSystem TypeSystem { get; }

		/// <summary>
		/// Holds the type layout interface
		/// </summary>
		public MosaTypeLayout TypeLayout { get; }

		/// <summary>
		/// Holds the native alignment
		/// </summary>
		public int NativeAlignment { get; }

		/// <summary>
		/// Holds the size of the native pointer.
		/// </summary>
		public int NativePointerSize { get; }

		/// <summary>
		/// The size of the native instruction.
		/// </summary>
		public InstructionSize NativeInstructionSize { get; }

		public BasicBlocks BasicBlocks { get; }

		/// <summary>
		/// The method data.
		/// </summary>
		public CompilerMethodData MethodData { get; }

		public MosaMethod Method { get { return MethodCompiler.Method; } }

		public Operand ConstantZero { get { return MethodCompiler.ConstantZero; } }

		public Operand StackFrame { get { return MethodCompiler.StackFrame; } }

		public Operand StackPointer { get { return MethodCompiler.StackPointer; } }

		/// <summary>
		/// The type of the platform internal runtime.
		/// </summary>
		public MosaType PlatformInternalRuntimeType { get { return MethodCompiler.Compiler.PlatformInternalRuntimeType; } }

		/// <summary>
		/// The type of the internal runtime.
		/// </summary>
		public MosaType InternalRuntimeType { get { return MethodCompiler.Compiler.InternalRuntimeType; } }

		/// <summary>
		/// Gets a value indicating whether this instance has code.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance has code; otherwise, <c>false</c>.
		/// </value>
		public bool HasCode { get { return BasicBlocks.HeadBlocks.Count != 0; } }

		/// <summary>
		/// Gets a value indicating whether this instance has protected regions.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance has protected regions; otherwise, <c>false</c>.
		/// </value>
		public bool HasProtectedRegions { get { return MethodCompiler.Method.ExceptionHandlers.Count != 0; } }

		/// <summary>
		/// The Value indicating whether this instance is plugged.
		/// </summary>
		public bool IsPlugged { get { return MethodCompiler.IsPlugged; } }

		#endregion Properties

		/// <summary>
		/// Method Transform Helper
		/// </summary>
		/// <param name="methodCompiler">The method compiler.</param>
		/// <returns></returns>
		public MethodTransform(BaseMethodCompiler methodCompiler)
		{
			MethodCompiler = methodCompiler;
			BasicBlocks = methodCompiler.BasicBlocks;
			Architecture = methodCompiler.Architecture;
			TypeSystem = methodCompiler.TypeSystem;
			TypeLayout = methodCompiler.TypeLayout;
			NativePointerSize = Architecture.NativePointerSize;
			NativeAlignment = Architecture.NativeAlignment;
			NativeInstructionSize = Architecture.NativeInstructionSize;

			MethodData = MethodCompiler.MethodData;
		}

		#region Methods

		/// <summary>
		/// Allocates the virtual register.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public Operand AllocateVirtualRegister(MosaType type)
		{
			return MethodCompiler.VirtualRegisters.Allocate(type);
		}

		/// <summary>
		/// Allocates the virtual register or stack slot.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public Operand AllocateVirtualRegisterOrStackSlot(MosaType type)
		{
			return MethodCompiler.AllocateVirtualRegisterOrStackSlot(type);
		}

		#endregion Methods

		#region Block Operations

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		protected BasicBlock CreateNewBlock(int label)
		{
			return BasicBlocks.CreateBlock(label);
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <returns></returns>
		protected BasicBlock CreateNewBlock()
		{
			return BasicBlocks.CreateBlock();
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		protected Context CreateNewBlockContext(int label)
		{
			return new Context(CreateNewBlock(label));
		}

		/// <summary>
		/// Creates empty blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <returns></returns>
		protected BasicBlock[] CreateNewBlocks(int blocks)
		{
			// Allocate the block array
			var result = new BasicBlock[blocks];

			for (int index = 0; index < blocks; index++)
			{
				result[index] = CreateNewBlock();
			}

			return result;
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <returns></returns>
		protected Context CreateNewBlockContext()
		{
			return new Context(CreateNewBlock());
		}

		/// <summary>
		/// Creates empty blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <returns></returns>
		protected Context[] CreateNewBlockContexts(int blocks)
		{
			// Allocate the context array
			var result = new Context[blocks];

			for (int index = 0; index < blocks; index++)
			{
				result[index] = CreateNewBlockContext();
			}

			return result;
		}

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns></returns>
		protected BasicBlock Split(InstructionNode node)
		{
			var newblock = CreateNewBlock();

			node.Split(newblock);

			return newblock;
		}

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		protected Context Split(Context context)
		{
			return new Context(Split(context.Node));
		}

		/// <summary>
		/// Determines whether [is empty block with single jump] [the specified block].
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns>
		///   <c>true</c> if [is empty block with single jump] [the specified block]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsEmptyBlockWithSingleJump(BasicBlock block)
		{
			if (block.NextBlocks.Count != 1)
				return false;

			for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmpty)
					continue;

				if (node.Instruction == IRInstruction.Nop)
					continue;

				if (node.Instruction.FlowControl != FlowControl.UnconditionalBranch)
					return false;
			}

			return true;
		}

		/// <summary>
		/// Empties the block of all instructions.
		/// </summary>
		/// <param name="block">The block.</param>
		public void EmptyBlockOfAllInstructions(BasicBlock block)
		{
			for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
			{
				node.Empty();
			}
		}

		/// <summary>
		/// Replaces the branch targets.
		/// </summary>
		/// <param name="block">The current from block.</param>
		/// <param name="oldTarget">The current destination block.</param>
		/// <param name="newTarget">The new target block.</param>
		public void ReplaceBranchTargets(BasicBlock block, BasicBlock oldTarget, BasicBlock newTarget)
		{
			for (var node = block.Last; !node.IsBlockStartInstruction; node = node.Previous)
			{
				if (node.IsEmpty)
					continue;

				if (node.BranchTargetsCount == 0)
					continue;

				var targets = node.BranchTargets;

				for (int index = 0; index < targets.Count; index++)
				{
					if (targets[index] == oldTarget)
					{
						node.UpdateBranchTarget(index, newTarget);
					}
				}
			}
		}

		public void RemoveEmptyBlockWithSingleJump(BasicBlock block)
		{
			Debug.Assert(block.NextBlocks.Count == 1);

			BasicBlock target = block.NextBlocks[0];

			foreach (var previous in block.PreviousBlocks.ToArray())
			{
				ReplaceBranchTargets(previous, block, target);
			}

			EmptyBlockOfAllInstructions(block);

			Debug.Assert(block.NextBlocks.Count == 0);
			Debug.Assert(block.PreviousBlocks.Count == 0);
		}

		public static void UpdatePhiList(BasicBlock removedBlock, BasicBlock[] nextBlocks)
		{
			foreach (var next in nextBlocks)
			{
				for (var node = next.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction != IRInstruction.Phi)
						continue;

					var sourceBlocks = node.PhiBlocks;

					int index = sourceBlocks.IndexOf(removedBlock);

					if (index < 0)
						continue;

					sourceBlocks.RemoveAt(index);

					for (int i = index; index < node.OperandCount - 1; index++)
					{
						node.SetOperand(i, node.GetOperand(i + 1));
					}

					node.SetOperand(node.OperandCount - 1, null);
					node.OperandCount--;
				}
			}
		}

		#endregion Block Operations

		#region Protected Region Methods

		public MosaExceptionHandler FindImmediateExceptionContext(int label)
		{
			foreach (var handler in MethodCompiler.Method.ExceptionHandlers)
			{
				if (handler.IsLabelWithinTry(label) || handler.IsLabelWithinHandler(label))
				{
					return handler;
				}
			}

			return null;
		}

		public MosaExceptionHandler FindNextEnclosingFinallyContext(MosaExceptionHandler exceptionContext)
		{
			int index = MethodCompiler.Method.ExceptionHandlers.IndexOf(exceptionContext);

			for (int i = index + 1; i < MethodCompiler.Method.ExceptionHandlers.Count; i++)
			{
				var entry = MethodCompiler.Method.ExceptionHandlers[i];

				if (!entry.IsLabelWithinTry(exceptionContext.TryStart))
					return null;

				if (entry.ExceptionHandlerType != ExceptionHandlerType.Finally)
					continue;

				return entry;
			}

			return null;
		}

		public MosaExceptionHandler FindFinallyExceptionContext(InstructionNode node)
		{
			int label = node.Label;

			foreach (var handler in MethodCompiler.Method.ExceptionHandlers)
			{
				if (handler.IsLabelWithinHandler(label))
				{
					return handler;
				}
			}

			return null;
		}

		public bool IsSourceAndTargetWithinSameTryOrException(InstructionNode node)
		{
			int leaveLabel = node.Label;
			int targetLabel = node.BranchTargets[0].First.Label;

			foreach (var handler in MethodCompiler.Method.ExceptionHandlers)
			{
				bool one = handler.IsLabelWithinTry(leaveLabel);
				bool two = handler.IsLabelWithinTry(targetLabel);

				if (one && !two)
					return false;

				if (!one && two)
					return false;

				if (one && two)
					return true;

				one = handler.IsLabelWithinHandler(leaveLabel);
				two = handler.IsLabelWithinHandler(targetLabel);

				if (one && !two)
					return false;

				if (!one && two)
					return false;

				if (one && two)
					return true;
			}

			// very odd
			return true;
		}

		#endregion Protected Region Methods

		/// <summary>
		/// Updates the counter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="count">The count.</param>
		public void UpdateCounter(string name, int count)
		{
			MethodData.Counters.Update(name, count);
		}

		#region Helpers

		/// <summary>
		/// Gets the size of the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="align">if set to <c>true</c> [align].</param>
		/// <returns></returns>
		public int GetTypeSize(MosaType type, bool align)
		{
			return MethodCompiler.GetReferenceOrTypeSize(type, align);
		}

		/// <summary>
		/// Gets the size of the instruction.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static InstructionSize GetInstructionSize(MosaType type)
		{
			if (type.IsPointer)
				return InstructionSize.Native;

			if (type.IsI4 || type.IsU4 || type.IsR4)
				return InstructionSize.Size32;

			if (type.IsR8 || type.IsUI8)
				return InstructionSize.Size64;

			if (type.IsUI1 || type.IsBoolean)
				return InstructionSize.Size8;

			if (type.IsUI2 || type.IsChar)
				return InstructionSize.Size16;

			if (type.IsReferenceType)
				return InstructionSize.Native;

			if (type.IsValueType)
				return InstructionSize.Native;

			return InstructionSize.Size32;
		}

		/// <summary>
		/// Gets the size of the instruction.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static InstructionSize GetInstructionSize(InstructionSize size, MosaType type)
		{
			if (size != InstructionSize.None)
				return size;

			return GetInstructionSize(type);
		}

		public IList<BasicBlock> AddMissingBlocks(IList<BasicBlock> blocks, bool cleanUp)
		{
			var list = new List<BasicBlock>(blocks.Count);

			foreach (var block in blocks)
			{
				if (block != null)
				{
					list.Add(block);
				}
			}

			foreach (var block in BasicBlocks)
			{
				if (!blocks.Contains(block))
				{
					if ((!cleanUp) || (block.HasNextBlocks || block.HasPreviousBlocks || block.IsHandlerHeadBlock || block.IsTryHeadBlock))
					{
						list.Add(block);
					}
				}
			}

			return list;
		}

		/// <summary>
		/// Determines if the load should sign extend the given source operand.
		/// </summary>
		/// <param name="source">The source operand to determine sign extension for.</param>
		/// <returns>
		/// True if the given operand should be loaded with its sign extended.
		/// </returns>
		private static bool MustSignExtendOnLoad(MosaType source)
		{
			return source.IsI1 || source.IsI2;
		}

		/// <summary>
		/// Determines if the load should sign extend the given source operand.
		/// </summary>
		/// <param name="source">The source operand to determine sign extension for.</param>
		/// <returns>
		/// True if the given operand should be loaded with its sign extended.
		/// </returns>
		private static bool MustZeroExtendOnLoad(MosaType source)
		{
			return source.IsU1 || source.IsU2 || source.IsChar || source.IsBoolean;
		}

		public static BaseIRInstruction GetLoadInstruction(MosaType type)
		{
			if (MustSignExtendOnLoad(type))
			{
				return IRInstruction.LoadSignExtended;
			}
			else if (MustZeroExtendOnLoad(type))
			{
				return IRInstruction.LoadZeroExtended;
			}
			else if (type.IsR4)
			{
				return IRInstruction.LoadFloatR4;
			}
			else if (type.IsR8)
			{
				return IRInstruction.LoadFloatR8;
			}

			return IRInstruction.LoadInteger;
		}

		public static BaseIRInstruction GetMoveInstruction(MosaType type)
		{
			if (MustSignExtendOnLoad(type))
			{
				return IRInstruction.MoveSignExtended;
			}
			else if (MustZeroExtendOnLoad(type))
			{
				return IRInstruction.MoveZeroExtended;
			}
			else if (type.IsR4)
			{
				return IRInstruction.MoveFloatR4;
			}
			else if (type.IsR8)
			{
				return IRInstruction.MoveFloatR8;
			}

			return IRInstruction.MoveInteger;
		}

		#endregion Helpers

		#region Constant Helper Methods

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

		public static Operand CreateConstant(MosaType type, long value)
		{
			return Operand.CreateConstant(type, (ulong)value);
		}

		public static Operand CreateConstant(MosaType type, ulong value)
		{
			return Operand.CreateConstant(type, value);
		}

		public static Operand CreateConstant(MosaType type, int value)
		{
			return Operand.CreateConstant(type, (long)value);
		}

		public static Operand CreateConstant(MosaType type, uint value)
		{
			return Operand.CreateConstant(type, value);
		}

		public Operand CreateConstant(float value)
		{
			return Operand.CreateConstant(value, TypeSystem);
		}

		public Operand CreateConstant(double value)
		{
			return Operand.CreateConstant(value, TypeSystem);
		}

		#endregion Constant Helper Methods
	}
}
