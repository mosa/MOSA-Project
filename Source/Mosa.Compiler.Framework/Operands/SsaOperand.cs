/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Framework.Operands
{
	public sealed class SsaOperand : Operand
	{
		private int ssaVersion;
		private Operand operand;

		/// <summary>
		/// Gets the operand.
		/// </summary>
		public Operand Operand { get { return operand; } }

		/// <summary>
		/// Gets the ssa version.
		/// </summary>
		public int SsaVersion { get { return ssaVersion; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="SsaOperand"/> class.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <param name="ssaVersion">The ssa version.</param>
		public SsaOperand(Operand operand, int ssaVersion)
			: base(operand.Type)
		{
			this.operand = operand;
			this.ssaVersion = ssaVersion;
		}

		/// <summary>
		/// Compares with the given operand for equality.
		/// </summary>
		/// <param name="other">The other operand to compare with.</param>
		/// <returns>
		/// The return value is true if the operands are equal; false if not.
		/// </returns>
		public override bool Equals(Operand other)
		{
			return operand.Equals(other);
		}

		/// <summary>
		/// Determines if the operand is a register.
		/// </summary>
		public override bool IsRegister { get { return operand.IsRegister; } }

		/// <summary>
		/// Determines if the operand is a stack local variable.
		/// </summary>
		public override bool IsStackLocal { get { return operand.IsStackLocal; } }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			string name = operand.ToString();
			int pos = name.IndexOf(' ');

			if (pos < 0)
				return name + "<" + ssaVersion + ">";
			else
				return name.Substring(0, pos) + "<" + ssaVersion + ">" + name.Substring(pos);
		}
	}
}
