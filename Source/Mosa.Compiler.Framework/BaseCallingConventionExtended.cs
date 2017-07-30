// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;

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
			this.architecture = architecture ?? throw new ArgumentNullException("Architecture");
		}

		#endregion Construction

		#region Helper Methods

		/// <summary>
		/// Builds the operands.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		protected static List<Operand> BuildOperands(Context context)
		{
			var operands = new List<Operand>(context.OperandCount - 1);
			int index = 0;

			foreach (var operand in context.Operands)
			{
				if (index++ > 0)
				{
					operands.Add(operand);
				}
			}

			return operands;
		}

		protected static int CalculateStackSizeForParameters(MosaTypeLayout typeLayout, BaseArchitecture architecture, List<Operand> operands)
		{
			// first operand is the call location
			int result = 0;

			foreach (var operand in operands)
			{
				architecture.GetTypeRequirements(typeLayout, operand.Type, out int size, out int alignment);

				result = Alignment.AlignUp(result, alignment) + size;
			}

			return result;
		}

		#endregion Helper Methods
	}
}
