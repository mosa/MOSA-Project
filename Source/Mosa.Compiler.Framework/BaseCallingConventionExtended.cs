// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
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
				throw new ArgumentNullException(@"Architecture");

			this.architecture = architecture;
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
		/// <param name="typeLayout">The type layouts.</param>
		/// <param name="operands">The operands.</param>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		protected static int CalculateStackSizeForParameters(MosaTypeLayout typeLayout, BaseArchitecture architecture, List<Operand> operands, MosaMethod method)
		{
			Debug.Assert((method.Signature.Parameters.Count + (method.HasThis ? 1 : 0) == operands.Count) ||
			(method.DeclaringType.IsDelegate && method.Signature.Parameters.Count == operands.Count), method.FullName);

			int offset = method.Signature.Parameters.Count - operands.Count;
			int result = 0;

			for (int index = operands.Count - 1; index >= 0; index--)
			{
				Operand operand = operands[index];

				int size, alignment;
				architecture.GetTypeRequirements(typeLayout, operand.Type, out size, out alignment);

				var param = (index + offset >= 0) ? method.Signature.Parameters[index + offset] : null;

				if (param != null && operand.IsR8 && param.ParameterType.IsR4)
				{
					//  adjust for parameter size on stack when method parameter is R4 while the calling variable is R8
					architecture.GetTypeRequirements(typeLayout, param.ParameterType, out size, out alignment);
				}

				result = (int)Alignment.AlignUp(result, (uint)alignment) + size;
			}

			return result;
		}

		protected static int CalculateStackSizeForParameters(MosaTypeLayout typeLayout, BaseArchitecture architecture, List<Operand> operands)
		{
			// first operand is the call location
			int result = 0;

			for (int i = 1; i < operands.Count; i++)
			{
				var operand = operands[0];

				int size, alignment;
				architecture.GetTypeRequirements(typeLayout, operand.Type, out size, out alignment);

				result = Alignment.AlignUp(result, alignment) + size;
			}

			return result;
		}

		#endregion Helper Methods
	}
}
