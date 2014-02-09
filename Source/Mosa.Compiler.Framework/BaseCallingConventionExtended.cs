/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// This extended class provides support to emit calling convention
	/// specific code.
	/// </summary>
	public abstract class BaseCallingConventionExtended : BaseCallingConvention
	{

		#region Data members

		/// <summary>
		/// Holds the architecture of the calling convention.
		/// </summary>
		protected BaseArchitecture architecture;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCallingConventionExtended"/>.
		/// </summary>
		/// <param name="architecture">The architecture of the calling convention.</param>
		public BaseCallingConventionExtended(BaseArchitecture architecture)
		{
			if (architecture == null)
				throw new ArgumentNullException(@"architecture");

			this.architecture = architecture;
		}

		#endregion Construction

		#region Members

		public override void GetStackRequirements(Operand stackOperand, out int size, out int alignment)
		{
			// Special treatment for some stack types
			// FIXME: Handle the size and alignment requirements of value types
			architecture.GetTypeRequirements(stackOperand.Type, out size, out alignment);
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Builds the operands.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		protected static List<Operand> BuildOperands(Context context)
		{
			List<Operand> operands = new List<Operand>(context.OperandCount - 1);
			int index = 0;

			foreach (Operand operand in context.Operands)
			{
				if (index++ > 0)
				{
					operands.Add(operand);
				}
			}

			return operands;
		}

		/// <summary>
		/// Calculates the stack size for parameters.
		/// </summary>
		/// <param name="operands">The operands.</param>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		protected static int CalculateStackSizeForParameters(BaseArchitecture architecture, List<Operand> operands, MosaMethod method)
		{
			Debug.Assert((method.Parameters.Count + (method.HasThis ? 1 : 0) == operands.Count) ||
			(method.DeclaringType.IsDelegate && method.Parameters.Count == operands.Count));

			int offset = method.Parameters.Count - operands.Count;
			int result = 0;

			for (int index = operands.Count - 1; index >= 0; index--)
			{
				Operand operand = operands[index];

				int size, alignment;
				architecture.GetTypeRequirements(operand.Type, out size, out alignment);

				var param = (index + offset >= 0) ? method.Parameters[index + offset] : null;

				if (param != null && operand.IsDouble && param.Type.IsSingle)
				{
					size = 4;
					alignment = 4;
				}

				result += size;
			}

			return result;
		}

		#endregion
	}

}