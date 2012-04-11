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
		public Operand Operand = null;
		public int SsaVersion = 0;

		public SsaOperand(Operand operand, int ssaVersion) : base (operand.Type)
		{
			this.Operand = operand;
			this.SsaVersion = ssaVersion;
		}

		public override bool Equals(Operand other)
		{
			return this.Operand.Equals(other);
		}

		public override bool IsRegister
		{
			get
			{
				return this.Operand.IsRegister;
			}
		}

		public override bool IsStackLocal
		{
			get
			{
				return this.Operand.IsStackLocal;
			}
		}

		public override string ToString()
		{
			return "<" + this.SsaVersion + "> " + this.Operand.ToString();
		}
	}
}
