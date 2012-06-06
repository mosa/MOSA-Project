/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.Operands
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class StringOperand : Operand
	{
		private string _string;

		/// <summary>
		/// Gets or sets the string.
		/// </summary>
		/// <value>The string.</value>
		public string String { get { return _string; } set { _string = value; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="StringOperand"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public StringOperand(string value)
			: base(BuiltInSigType.String)
		{
			String = value;
		}

		
	}
}


