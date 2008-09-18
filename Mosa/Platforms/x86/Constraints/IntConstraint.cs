using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework;
using System.Diagnostics;

namespace Mosa.Platforms.x86.Constraints
{
    /// <summary>
    /// Implements register constraints for the x86 int instruction.
    /// </summary>
    public sealed class IntConstraint : IRegisterConstraint
    {
        #region IRegisterConstraint Members

        bool IRegisterConstraint.IsValidOperand(int opIdx, Operand op)
        {
            Debug.Assert(0 == opIdx, @"Only one operand supported.");
            if (opIdx > 0)
                throw new ArgumentOutOfRangeException(@"opIdx", opIdx, @"Only one operand supported.");

            return (op is ConstantOperand);
        }

        bool IRegisterConstraint.IsValidResult(int resIdx, Operand op)
        {
            throw new NotSupportedException();
        }

        Register[] IRegisterConstraint.GetRegistersForOperand(int opIdx)
        {
            return null;
        }

        Register[] IRegisterConstraint.GetRegistersForResult(int resIdx)
        {
            return null;
        }

        Register[] IRegisterConstraint.GetRegistersUsed()
        {
            return null;
        }

        #endregion // IRegisterConstraint Members
    }
}
