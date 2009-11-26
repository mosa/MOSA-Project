using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StringOperand : Operand
    {
        /// <summary>
        /// 
        /// </summary>
        private string _string;

        /// <summary>
        /// 
        /// </summary>
        public string String
        {
            get
            {
                return _string;
            }
            set
            {
                _string = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public StringOperand(string value)
            : base(new Metadata.Signatures.SigType(Metadata.CilElementType.String))
        {
            String = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(Operand other)
        {
            if (!(other is StringOperand))
                return false;

            StringOperand stringOp = other as StringOperand;
            return String.Equals(stringOp.String);
        }
    }
}
