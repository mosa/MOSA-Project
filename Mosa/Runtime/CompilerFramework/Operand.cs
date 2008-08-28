/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.Metadata;
using System.Diagnostics;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{

	/// <summary>
	/// Abstract base class for IR instruction operands.
	/// </summary>
	public abstract class Operand {

		#region Static data members

		/// <summary>
		/// Undefined operand constant.
		/// </summary>
		public static readonly Operand Undefined = null;

		#endregion // Static data members

		#region Data members

		/// <summary>
		/// The namespace of the operand.
		/// </summary>
		protected SigType _type;

        /// <summary>
        /// Holds a list of instructions, which define this operand.
        /// </summary>
        private List<Instruction> _definitions;

        /// <summary>
        /// Holds a list of instructions, which use this operand.
        /// </summary>
        private List<Instruction> _uses;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Operand"/>.
		/// </summary>
		/// <param name="type">The type of the operand.</param>
		protected Operand(SigType type)
		{
            _type = type;
		}

		#endregion // Construction

		#region Properties

        /// <summary>
        /// Returns a list of instructions, which use this operand.
        /// </summary>
        public List<Instruction> Definitions
        {
            get 
            {
                if (null == _definitions)
                    _definitions = new List<Instruction>();

                return _definitions; 
            }
        }

        /// <summary>
        /// Determines if the operand is a register.
        /// </summary>
        public virtual bool IsRegister { get { return false; } }

        /// <summary>
        /// Returns the stack type of the operand.
        /// </summary>
        public StackTypeCode StackType 
        { 
            get 
            {
                return StackTypeFromSigType(_type);
            } 
        }

        public static StackTypeCode StackTypeFromSigType(SigType type)
        {
            StackTypeCode result = StackTypeCode.Unknown;
            switch (type.Type)
            {
                case CilElementType.Void:
                    break;

                case CilElementType.Boolean: result = StackTypeCode.Int32; break;
                case CilElementType.Char: result = StackTypeCode.Int32; break;
                case CilElementType.I1: result = StackTypeCode.Int32; break;
                case CilElementType.U1: result = StackTypeCode.Int32; break;
                case CilElementType.I2: result = StackTypeCode.Int32; break;
                case CilElementType.U2: result = StackTypeCode.Int32; break;
                case CilElementType.I4: result = StackTypeCode.Int32; break;
                case CilElementType.U4: result = StackTypeCode.Int32; break;
                case CilElementType.I8: result = StackTypeCode.Int64; break;
                case CilElementType.U8: result = StackTypeCode.Int64; break;
                case CilElementType.R4: result = StackTypeCode.F; break;
                case CilElementType.R8: result = StackTypeCode.F; break;
                case CilElementType.I: result = StackTypeCode.N; break;
                case CilElementType.U: result = StackTypeCode.N; break;
                case CilElementType.Ptr: result = StackTypeCode.Ptr; break;
                case CilElementType.ByRef: result = StackTypeCode.Ptr; break;
                case CilElementType.Object: result = StackTypeCode.O; break;
                case CilElementType.String: result = StackTypeCode.O; break;
                case CilElementType.ValueType: result = StackTypeCode.O; break;
                case CilElementType.Type: result = StackTypeCode.O; break;

                default:
                    throw new NotSupportedException(@"Can't transform SigType to StackTypeCode.");
            }

            return result;
        }

        public static SigType SigTypeFromStackType(StackTypeCode typeCode)
        {
            SigType result = null;
            switch (typeCode)
            {
                case StackTypeCode.Int32: result = new SigType(CilElementType.I4); break;
                case StackTypeCode.Int64: result = new SigType(CilElementType.I8); break;
                case StackTypeCode.F: result = new SigType(CilElementType.R8); break;
                case StackTypeCode.O: result = new SigType(CilElementType.Object); break;
                case StackTypeCode.Ptr: result = new SigType(CilElementType.Ptr); break;
                case StackTypeCode.N: result = new SigType(CilElementType.I); break;
                default:
                    throw new NotSupportedException(@"Can't convert stack type code to SigType.");
            }
            return result;
        }

		/// <summary>
		/// Returns the type of the operand.
		/// </summary>
        public SigType Type { get { return _type; } }

        /// <summary>
        /// Returns a list of instructions, which use this operand.
        /// </summary>
        public List<Instruction> Uses
        {
            get 
            {
                if (null == _uses)
                    _uses = new List<Instruction>();

                return _uses; 
            }
        }

        #endregion // Properties

/*
 * This should be removed.
        #region Static methods

        public static TypeReference GetTypeReference(IMetadataProvider provider, StackTypeCode typeCode)
        {
            TypeReference result = null;
            Debug.Assert(typeCode != StackTypeCode.N, @"Native types not supported by this method. Use IArchitecture.NativeType instead.");
            switch (typeCode)
            {
                case StackTypeCode.Int32:
                    result = NativeTypeReference.Int32;
                    break;

                case StackTypeCode.Int64:
                    result = NativeTypeReference.Int64;
                    break;

                case StackTypeCode.N:
                    throw new NotSupportedException(@"Can't generically determine the native unsigned integer type.");

                case StackTypeCode.F:
                    result = NativeTypeReference.Double;
                    break;

                case StackTypeCode.Ptr:
                    result = new ReferenceTypeSpecification(NativeTypeReference.Void);
                    break;

                case StackTypeCode.O:
                    result = TypeReference.FromName(provider, @"System", @"Object");
                    break;

                default:
                    throw new ArgumentException(@"Invalid stack type code.", @"typeCode");
            }

            return result;
        }

        #endregion // Static methods
 */
        #region Methods

        /// <summary>
        /// Replaces this operand in all uses and defs with the given operand.
        /// </summary>
        /// <param name="replacement">The replacement operand.</param>
        public void Replace(Operand replacement)
        {
            int opIdx;

            // Iterate all definition sites first
            foreach (Instruction def in this.Definitions.ToArray())
            {
                opIdx = 0;
                foreach (Operand r in def.Results)
                {
                    // Is this the operand?
                    if (true == Object.ReferenceEquals(r, this))
                        def.SetResult(opIdx, replacement);
                    
                    opIdx++;
                }
            }

            // Iterate all use sites
            foreach (Instruction instr in this.Uses.ToArray())
            {
                opIdx = 0;
                foreach (Operand r in instr.Operands)
                {
                    // Is this the operand?
                    if (true == Object.ReferenceEquals(r, this))
                        instr.SetOperand(opIdx, replacement);

                    opIdx++;
                }
            }
        }

        #endregion // Methods

        #region Object Overrides

        public override string ToString()
        {
            return String.Format("[Type: {0}]", _type);
        }

        #endregion // Object Overrides
    }
}
