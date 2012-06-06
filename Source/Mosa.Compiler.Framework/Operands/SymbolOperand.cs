/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework.Operands
{
	/// <summary>
	/// An operand, which represents a symbol in the program data.
	/// </summary>
	public sealed class SymbolOperand : Operand
	{
		/// <summary>
		/// Holds the name of the label.
		/// </summary>
		private string name;

		/// <summary>
		/// Initializes a new instance of the <see cref="SymbolOperand"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="name">The name.</param>
		public SymbolOperand(SigType type, string name)
			: base(type)
		{
			this.name = name;
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get { return this.name; }
		}

		/// <summary>
		/// Returns a string representation of <see cref="Operand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return this.name + " " + base.ToString();
		}

		/// <summary>
		/// Creates a symbol operand for the given method.
		/// </summary>
		/// <param name="method">The method to create a symbol operand for.</param>
		/// <returns>The created symbol operand.</returns>
		public static SymbolOperand FromMethod(RuntimeMethod method)
		{
			string symbolName = method.ToString();

			return new SymbolOperand(BuiltInSigType.IntPtr, symbolName);
		}

		/// <summary>
		/// Creates a new SymbolOperand for the given runtime field.
		/// </summary>
		/// <param name="runtimeField">The field to create a symbol operand for.</param>
		/// <returns>The created symbol operand.</returns>
		public static SymbolOperand FromField(RuntimeField runtimeField)
		{
			return new SymbolOperand(runtimeField.SignatureType, runtimeField.ToString());
		}
	}
}


