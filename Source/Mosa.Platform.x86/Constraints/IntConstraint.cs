/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Platform.x86.Constraints
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
