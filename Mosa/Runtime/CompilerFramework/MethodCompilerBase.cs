/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Base class of a method compiler.
	/// </summary>
	/// <remarks>
	/// A _method compiler is responsible for compiling a single function
	/// of an object. There are various classes derived From MethodCompilerBase,
	/// which provide specific features, such as jit compilation, runtime
	/// optimized jitting and others. MethodCompilerBase instances are usually
	/// created by invoking CreateMethodCompiler on a specific compiler
	/// instance.
	/// </remarks>
	public class MethodCompilerBase : CompilerBase, IMethodCompiler, IDisposable
	{

		#region Data Members

		/// <summary>
		/// Holds a list of operands, which represent _method _parameters.
		/// </summary>
		private readonly List<Operand> _parameters;

		/// <summary>
		/// The Architecture of the compilation target.
		/// </summary>
		private IArchitecture _architecture;

		/// <summary>
		/// Holds the _linker used to resolve external symbols.
		/// </summary>
		private IAssemblyLinker _linker;

		/// <summary>
		/// Holds a list of operands, which represent local variables.
		/// </summary>
		private List<Operand> _locals;

		/// <summary>
		/// Optional signature of stack local variables.
		/// </summary>
		private LocalVariableSignature _localsSig;

		/// <summary>
		/// The _method definition being compiled.
		/// </summary>
		private RuntimeMethod _method;

		/// <summary>
		/// The metadata _module, that contains the _method.
		/// </summary>
		private IMetadataModule _module;

		/// <summary>
		/// Holds the next free stack slot index.
		/// </summary>
		protected int _nextStackSlot;

		/// <summary>
		/// Holds the _type, which owns the _method.
		/// </summary>
		private RuntimeType _type;

		/// <summary>
		/// Holds the instruction set
		/// </summary>
		private InstructionSet _instructionSet;

		/// <summary>
		/// Holds the basic blocks
		/// </summary>
		private List<BasicBlock> _basicBlocks;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodCompilerBase"/> class.
		/// </summary>
		/// <param name="linker">The _linker.</param>
		/// <param name="architecture">The target compilation Architecture.</param>
		/// <param name="module">The metadata module, that contains the type.</param>
		/// <param name="type">The type, which owns the _method to compile.</param>
		/// <param name="method">The method to compile by this instance.</param>
		protected MethodCompilerBase(
			IAssemblyLinker linker,
			IArchitecture architecture,
			IMetadataModule module,
			RuntimeType type,
			RuntimeMethod method)
		{
			if (architecture == null)
				throw new ArgumentNullException(@"architecture");

			if (linker == null)
				throw new ArgumentNullException(@"linker");

			_architecture = architecture;
			_linker = linker;
			_method = method;
			_module = module;
			_parameters = new List<Operand>(new Operand[_method.Parameters.Count]);
			_type = type;
			_nextStackSlot = 0;
			_basicBlocks = new List<BasicBlock>();
			_instructionSet = null; // this will be set later
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the Architecture to compile for.
		/// </summary>
		public IArchitecture Architecture
		{
			get { return _architecture; }
		}

		/// <summary>
		/// Gets the assembly, which contains the _method.
		/// </summary>
		public IMetadataModule Assembly
		{
			get { return _module; }
		}

		/// <summary>
		/// Gets the _linker used to resolve external symbols.
		/// </summary>
		public IAssemblyLinker Linker
		{
			get { return _linker; }
		}

		/// <summary>
		/// Gets the _method implementation being compiled.
		/// </summary>
		public RuntimeMethod Method
		{
			get { return _method; }
		}

		/// <summary>
		/// Gets the owner _type of the _method.
		/// </summary>
		public RuntimeType Type
		{
			get { return _type; }
		}

		/// <summary>
		/// Gets the instruction set.
		/// </summary>
		/// <value>The instruction set.</value>
		public InstructionSet InstructionSet
		{
			get { return _instructionSet; }
			set { _instructionSet = value; }
		}

		/// <summary>
		/// Gets the basic Blocks.
		/// </summary>
		/// <value>The basic blocks.</value>
		public List<BasicBlock> BasicBlocks
		{
			get { return _basicBlocks; }
			set { _basicBlocks = value; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Requests a stream to emit native instructions to.
		/// </summary>
		/// <returns>A stream object, which can be used to store emitted instructions.</returns>
		public virtual Stream RequestCodeStream()
		{
			return _linker.Allocate(_method, SectionKind.Text, 0, 0);
		}

		/// <summary>
		/// Compiles the _method referenced by this method compiler.
		/// </summary>
		public void Compile()
		{
			BeginCompile();

			Pipeline.Execute<IMethodCompilerStage>(delegate(IMethodCompilerStage stage)
				{
					stage.Setup(this);
					stage.Run();
				}
			);

			EndCompile();
		}

		/// <summary>
		/// Creates a result operand for an instruction.
		/// </summary>
		/// <param name="type">The signature type of the operand to be created.</param>
		/// <returns>A new temporary result operand.</returns>
		public Operand CreateResultOperand(SigType type)
		{
			if (type.Type != CilElementType.I8 && type.Type != CilElementType.U8) {
				return _architecture.CreateResultOperand(type, _nextStackSlot, _nextStackSlot++);
			}
			return CreateTemporary(type);
		}

		/// <summary>
		/// Creates a new temporary local variable operand.
		/// </summary>
		/// <param name="type">The signature _type of the temporary.</param>
		/// <returns>An operand, which represents the temporary.</returns>
		public Operand CreateTemporary(SigType type)
		{
			int stackSlot = _nextStackSlot++;
			return new LocalVariableOperand(
				_architecture.StackFrameRegister, String.Format(@"T_{0}", stackSlot), stackSlot, type);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (_pipeline == null)
				throw new ObjectDisposedException(@"MethodCompilerBase");

			foreach (IMethodCompilerStage mcs in _pipeline) {
				IDisposable d = mcs as IDisposable;
				if (null != d)
					d.Dispose();
			}

			_pipeline.Clear();
			_pipeline = null;

			_architecture = null;
			_linker = null;
			_method = null;
			_module = null;
			_type = null;
			_instructionSet = null;
			_basicBlocks = null;
		}

		/// <summary>
		/// Provides access to the instructions of the method.
		/// </summary>
		/// <returns>A stream, which represents the IL of the method.</returns>
		public Stream GetInstructionStream()
		{
			return _method.Module.GetInstructionStream(_method.Rva);
		}

		/// <summary>
		/// Retrieves the local stack operand at the specified <paramref name="index"/>.
		/// </summary>
		/// <param name="index">The index of the stack operand to retrieve.</param>
		/// <returns>The operand at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index"/> is not valid.</exception>
		public Operand GetLocalOperand(int index)
		{
			// HACK: Returning a new instance here breaks object identity. We should reuse operands,
			// which represent the same memory location. If we need to move a variable in an optimization
			// stage to a different memory location, it should actually be a new one so sharing object
			// only saves runtime space/perf.
			Debug.Assert(_localsSig != null, @"Method doesn't have _locals.");
			Debug.Assert(index <= _localsSig.Types.Length, @"Invalid local index requested.");
			if (_localsSig == null || _localsSig.Types.Length <= index)
				throw new ArgumentOutOfRangeException(@"index", index, @"Invalid parameter index");


			Operand local = null;
			if (_locals.Count > index)
				local = _locals[index];

			if (local == null) {
				local = new LocalVariableOperand(
					_architecture.StackFrameRegister, String.Format("L_{0}", index), index, _localsSig.Types[index]);
				_locals[index] = local;
			}

			return local;
		}

		/// <summary>
		/// Retrieves the parameter operand at the specified <paramref name="index"/>.
		/// </summary>
		/// <param name="index">The index of the parameter operand to retrieve.</param>
		/// <returns>The operand at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">The <paramref name="index"/> is not valid.</exception>
		public Operand GetParameterOperand(int index)
		{
			// HACK: Returning a new instance here breaks object identity. We should reuse operands,
			// which represent the same memory location. If we need to move a variable in an optimization
			// stage to a different memory location, it should actually be a new one so sharing object
			// only saves runtime space/perf.
			MethodSignature sig = _method.Signature;
			if (sig.HasThis || sig.HasExplicitThis) {
				if (index == 0) {
					return new ParameterOperand(
						_architecture.StackFrameRegister,
						new RuntimeParameter(_method.Module, @"this", 0, ParameterAttributes.In),
						new ClassSigType((TokenTypes)_type.Token));
				}
				// Decrement the index, as the caller actually wants a real parameter
				index--;
			}

			// A normal argument, decode it...
			IList<RuntimeParameter> parameters = _method.Parameters;
			Debug.Assert(parameters != null, @"Method doesn't have arguments.");
			Debug.Assert(index < parameters.Count, @"Invalid argument index requested.");
			if (parameters == null || parameters.Count <= index)
				throw new ArgumentOutOfRangeException(@"index", index, @"Invalid parameter index");

			Operand param = null;
			if (_parameters.Count > index)
				param = _parameters[index];

			if (param == null) {
				param = new ParameterOperand(
					_architecture.StackFrameRegister, parameters[index], sig.Parameters[index]);
				_parameters[index] = param;
			}

			return param;
		}

		/// <summary>
		/// Sets the signature of local variables in the method.
		/// </summary>
		/// <param name="localVariableSignature">The local variable signature of the _method.</param>
		public void SetLocalVariableSignature(LocalVariableSignature localVariableSignature)
		{
			if (localVariableSignature == null)
				throw new ArgumentNullException(@"localVariableSignature");


			_localsSig = localVariableSignature;
			_locals = new List<Operand>(new Operand[_localsSig.Types.Length]);
			_nextStackSlot = _locals.Count + 1;
		}

		/// <summary>
		/// Called before the _method compiler begins compiling the method.
		/// </summary>
		protected virtual void BeginCompile()
		{
		}

		/// <summary>
		/// Called after the _method compiler has finished compiling the method.
		/// </summary>
		protected virtual void EndCompile()
		{
		}

		#region IBasicBlockProvider members

		/// <summary>
		/// Retrieves a basic block From its label.
		/// </summary>
		/// <param name="label">The label of the basic block.</param>
		/// <returns>
		/// The basic block with the given label or null.
		/// </returns>
		public BasicBlock FromLabel(int label)
		{
			return _basicBlocks.Find(delegate(BasicBlock block)
			{
				return (label == block.Label);
			});
		}

		/// <summary>
		/// Creates the block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public BasicBlock CreateBlock(int label, int index)
		{
			// HACK: BasicBlock.Count for the sequence works for now since blocks are not removed
			BasicBlock basicBlock = new BasicBlock(BasicBlocks.Count, label, index);
			BasicBlocks.Add(basicBlock);
			return basicBlock;
		}

		#endregion // IBasicBlockProvider members

		#endregion // Methods

	}
}