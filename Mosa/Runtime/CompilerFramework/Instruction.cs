/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Represents an instruction in a three-address code.
    /// </summary>
    /// <remarks>
    /// The compiler engine uses a three-address code to represent all operations. In a temporary
    /// state the instructions may use more than three operands, however before emitting code these
    /// instructions should expand themselves to ensure a valid three-address code.
    /// </remarks>
    public abstract class Instruction
    {
        #region Constant data members

        /// <summary>
        /// Empty operand array.
        /// </summary>
        public static readonly Operand[] NoOperands = new Operand[0];

        /// <summary>
        /// Indicates an empty instruction expansion.
        /// </summary>
        public static readonly Instruction[] Empty = new Instruction[0];

        #endregion // Constant data members

        #region Static data members

        /// <summary>
        /// Holds the id of the next virtual register.
        /// </summary>
        private static int _vregs = 1;

        #endregion // Static data members

        #region Data members

        /// <summary>
        /// Holds the block index of this instruction.
        /// </summary>
        private int _block;

        /// <summary>
        /// Determines if this instruction is ignored.
        /// </summary>
        protected bool _ignore;

        /// <summary>
        /// IL offset of the instruction from the start of the method.
        /// </summary>
        protected int _offset;

        /// <summary>
        /// Holds the operands of the instruction.
        /// </summary>
        /// <remarks>
        /// This array holds at most 3 operands of the instruction, where index zero represents
        /// the destination and index one and two represent instruction arguments.
        /// </remarks>
        private Operand[] _operands;

        /// <summary>
        /// Holds the result operands of the instruction.
        /// </summary>
        private Operand[] _results;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="Instruction"/>.
        /// </summary>
        protected Instruction()
        {
            _ignore = false;
            _offset = 0;
            _operands = NoOperands;
            _results = NoOperands;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Instruction"/>.
        /// </summary>
        /// <param name="operandCount">The number of operands of the instruction.</param>
        protected Instruction(int operandCount)
        {
            _ignore = false;
            _offset = 0;
            _operands = new Operand[operandCount];
            _results = NoOperands;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Instruction"/>.
        /// </summary>
        /// <param name="operandCount">The number of operands of the instruction.</param>
        /// <param name="resultCount">The number of results pushed by the instruction.</param>
        protected Instruction(int operandCount, int resultCount)
        {
            _ignore = false;
            _offset = 0;
            _operands = new Operand[operandCount];
            _results = new Operand[resultCount];
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Returns the block index of this instruction.
        /// </summary>
        public int Block
        {
            get { return _block; }
            set { _block = value; }
        }

        /// <summary>
        /// Determines flow behavior of this instruction.
        /// </summary>
        /// <remarks>
        /// Knowledge of control flow is required for correct basic block
        /// building. Any instruction that alters the control flow must override
        /// this property and correctly identify its control flow modifications.
        /// </remarks>
        public virtual FlowControl FlowControl
        {
            get { return FlowControl.Next; }
        }

        /// <summary>
        /// Determines if this instruction is ignored, that is, that it is not added
        /// to the instruction stream.
        /// </summary>
        public bool Ignore
        {
            get { return _ignore; }
            set { _ignore = value; }
        }

        /// <summary>
        /// Gets or sets the offset of the instruction from the start of the method.
        /// </summary>
        /// <remarks>
        /// Offsets are used by branch instructions to define their target. During basic block
        /// building these offsets are used to insert labels at appropriate positions and the
        /// jumps or modified to target one of these labels. During code generation, the offset
        /// can be used to indicate native code offsets.
        /// </remarks>
        public int Offset
        {
            get { return _offset; }
            set
            {
                if (0 > value)
                    throw new ArgumentOutOfRangeException(@"Offset can not be negative.", @"value");

                _offset = value;
            }
        }

        /// <summary>
        /// Returns an array of operands used by this instruction.
        /// </summary>
        public virtual Operand[] Operands
        {
            get 
            {
                if (null != _operands)
                    return _operands;

                _operands = new Operand[3] { Operand.Undefined, Operand.Undefined, Operand.Undefined };
                return _operands; 
            }
        }

        /// <summary>
        /// Determines the number of results on the stack for this instruction.
        /// </summary>
        public Operand[] Results
        {
            get { return _results; }
            set
            {
                if (null == value)
                    throw new ArgumentNullException(@"value");
                if (Instruction.NoOperands == _results)
                    throw new InvalidOperationException(@"ILInstruction does not provide results or hasn't been decoded yet.");
                if (value.Length != _results.Length)
                    throw new ArgumentException(@"Can't change result count of an instruction.", @"value");
                if (true == IsAnyOperandMuted(value, _results))
                    throw new ArgumentException(@"Result operands can't change type.", @"value");

                _results = value;
            }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Sets the operand count of the instruction.
        /// </summary>
        /// <param name="operands">The number of supported operands.</param>
        /// <param name="results">The number of supported results.</param>
        protected void SetOperandCount(int operands, int results)
        {
            if (operands != _operands.Length)
            {
                if (null != _operands && operands < _operands.Length)
                {
                    for (int i = operands; i < _operands.Length; i++)
                    {
                        Operand op = _operands[i];
                        if (null != op)
                            op.Uses.Remove(this);
                    }
                }
                if (0 != operands)
                    Array.Resize<Operand>(ref _operands, operands);
                else
                    _operands = Instruction.NoOperands;
            }

            if (results != _results.Length)
            {
                if (null != _results && results < _results.Length)
                {
                    for (int i = results; i < _results.Length; i++)
                    {
                        Operand op = _results[i];
                        if (null != op)
                            op.Definitions.Remove(this);
                    }
                }

                if (0 != results)
                    Array.Resize<Operand>(ref _results, results);
                else
                    _results = Instruction.NoOperands;
            }
        }

        /// <summary>
        /// Adds the operand at the given position in the operand list and maintains the operands the use list.
        /// </summary>
        /// <param name="index">The index, where this operand is added.</param>
        /// <param name="operand">The operand to add.</param>
        public void SetOperand(int index, Operand operand)
        {
            Debug.Assert(index < _operands.Length, @"Invalid operand index.");
            if (index >= _operands.Length)
                throw new ArgumentOutOfRangeException(@"index", @"Invalid operand index.");
            Debug.Assert(null != operand);
            if (null == operand)
                throw new ArgumentNullException("operand");

            Operand old = _operands[index];

            _operands[index] = operand;
            operand.Uses.Add(this);

            if (null != old)
                old.Uses.Remove(this);
        }

        /// <summary>
        /// Adds the operand at the given position in the result list and maintains the operands the definitions list.
        /// </summary>
        /// <param name="index">The index, where this operand is added.</param>
        /// <param name="operand">The operand to add.</param>
        public void SetResult(int index, Operand operand)
        {
            Debug.Assert(index < _results.Length, @"Invalid result index.");
            if (index >= _results.Length)
                throw new ArgumentOutOfRangeException(@"index", @"Invalid result index.");
            Debug.Assert(null != operand);
            if (null == operand)
                throw new ArgumentNullException("operand");

            Operand old = _results[index];
            
            _results[index] = operand;
            operand.Definitions.Add(this);

            if (null != old)
                old.Definitions.Remove(this);
        }

        /// <summary>
        /// Called by the intermediate to machine intermediate representation transformation
        /// to expand compound instructions into their basic instructions.
        /// </summary>
        /// <param name="methodCompiler">The executing method compiler.</param>
        /// <returns>
        /// The default expansion keeps the original instruction by 
        /// returning the instruction itself. A derived class may return an 
        /// IEnumerable&lt;Instruction&gt; to replace the instruction with a set of other
        /// instructions or null to remove the instruction itself from the stream.
        /// </returns>
        /// <remarks>
        /// If a derived class returns <see cref="Instruction.Empty"/> from this method, the 
        /// instruction is essentially removed from the instruction stream.
        /// </remarks>
        public virtual object Expand(MethodCompilerBase methodCompiler)
        {
            object result = null;
            if (false == this.Ignore)
                result = this;

            return result;
        }

        /// <summary>
        /// Determines if any operand is muted.
        /// </summary>
        /// <param name="value">The new operand set to validate.</param>
        /// <param name="_result">The old operand set.</param>
        /// <returns>True if the type of any operand is changed.</returns>
        private bool IsAnyOperandMuted(Operand[] value, Operand[] _result)
        {
            bool result = false;
            for (int i = 0; false == result && i < value.Length; i++)
            {
                if (null != _results[i] && null != value[i])
                    result = (_results[i].Type == value[i].Type);
            }
            return result;
        }

        /// <summary>
        /// Updates the operands of the instruction.
        /// </summary>
        /// <param name="compiler">The compiler, which performs the update.</param>
        /// <param name="operands">The array of operands to set.</param>
        public void SetOperands(MethodCompilerBase compiler, Operand[] operands)
        {
            if (null == compiler)
                throw new ArgumentNullException(@"compiler");
            if (null == operands)
                throw new ArgumentNullException(@"value");
            if (Instruction.NoOperands == _results)
                throw new InvalidOperationException(@"ILInstruction does not take operands or hasn't been decoded yet.");
            if (operands.Length != _results.Length)
                throw new ArgumentException(@"Can't change operand count of an instruction.", @"value");
            if (true == IsAnyOperandMuted(operands, _results))
                throw new ArgumentException(@"Operands can't change type.", @"value");

            _operands = operands;
            Validate(compiler);
        }

        /// <summary>
        /// Validates the current set of stack operands.
        /// </summary>
        /// <exception cref="System.ExecutionEngineException">One of the stack operands is invalid.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="compiler"/> is null.</exception>
        public virtual void Validate(MethodCompilerBase compiler)
        {
            if (null == compiler)
                throw new ArgumentNullException(@"compiler");
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public abstract void Visit<ArgType>(IInstructionVisitor<ArgType> visitor, ArgType arg);

        #endregion // Methods
    }
}
