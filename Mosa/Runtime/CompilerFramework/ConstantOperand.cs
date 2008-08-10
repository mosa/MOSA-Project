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
using System.Text;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{

	/// <summary>
	/// Represent a constant operand.
	/// </summary>
	public sealed class ConstantOperand : Operand {
		#region Static data members
/*
		public static Constant MinusOne = new Constant(TypeReference.Int32, -1);
		public static Constant Zero = new Constant(TypeReference.Int32, 0);
		public static Constant One = new Constant(TypeReference.Int32, 1);
		public static Constant Two = new Constant(TypeReference.Int32, 2);
		public static Constant Three = new Constant(TypeReference.Int32, 3);
		public static Constant Four = new Constant(TypeReference.Int32, 4);
		public static Constant Five = new Constant(TypeReference.Int32, 5);
		public static Constant Six = new Constant(TypeReference.Int32, 6);
		public static Constant Seven = new Constant(TypeReference.Int32, 7);
		public static Constant Eight = new Constant(TypeReference.Int32, 8);
		public static Constant Null = new Constant(TypeReference.Object, null);
*/
        private static SigType s_object;

		#endregion // Static data members

		#region Data members

		/// <summary>
		/// Constant value.
		/// </summary>
		private object _value;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Constant"/>.
		/// </summary>
		/// <param name="value">The value of the contant.</param>
		public ConstantOperand(SigType typeRef, object value) 
			: base(typeRef)
		{
			_value = value;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Returns the value of the constant.
		/// </summary>
		public object Value
		{
			get { return _value; }
		}

		#endregion // Properties

        #region Methods

        public static ConstantOperand FromValue(IMetadataProvider provider, int value)
        {
            return new ConstantOperand(new SigType(CilElementType.I4), value);
        }

        public static ConstantOperand GetNull(IMetadataProvider provider)
        {
            if (null == s_object)
                s_object = new SigType(CilElementType.Object);

            return new ConstantOperand(s_object, null);
        }

        #endregion // Methods

        #region Operand Overrides

        public override string ToString()
        {
            return String.Format("const {0} {1}", _value, _type);
        }

        #endregion // Operand Overrides
    }
}
