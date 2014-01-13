/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Text;

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	/// Specifies an instance of a generic type as a signature type.
	/// </summary>
	public sealed class GenericInstSigType : SigType
	{
		#region Properties

		/// <summary>
		/// Gets the generic type of this signature type.
		/// </summary>
		/// <value>The type of the generic type.</value>
		public TypeSigType BaseType { get; private set; }

		/// <summary>
		/// Gets the generic parameter type signatures.
		/// </summary>
		/// <value>The generic type signatures.</value>
		public SigType[] GenericArguments { get; private set; }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericInstSigType"/> class.
		/// </summary>
		/// <param name="baseType">Type of the base.</param>
		/// <param name="genericArguments">The generic args.</param>
		internal GenericInstSigType(TypeSigType baseType, SigType[] genericArguments) :
			base(CilElementType.GenericInst)
		{
			BaseType = baseType;
			GenericArguments = genericArguments;
		}

		#endregion Construction

		#region SigType Overrides

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(base.ToString());
			sb.Append(' ');
			sb.Append(BaseType.ToString());

			if (GenericArguments.Length != 0)
			{
				sb.Append(" [ ");

				int index = 0;
				foreach (var type in GenericArguments)
				{
					sb.Append(index.ToString());
					sb.Append(":");
					sb.Append(type.ToString());
					sb.Append(", ");
					index++;
				}

				sb.Length = sb.Length - 2;

				sb.Append(" ]");
			}

			return sb.ToString();
		}

		#endregion SigType Overrides

		/// <summary>
		/// Expresses the generic instance type in a meaningful, symbol-friendly string form
		/// </summary>
		public override string ToSymbolPart()
		{
			StringBuilder sb = new StringBuilder();

			SigType[] genericArguments = this.GenericArguments;
			sb.Append(BaseType.ToSymbolPart());
			sb.Append('<');
			for (int x = 0; x < genericArguments.Length; x++)
			{
				if (x > 0)
					sb.Append(',');
				sb.Append(genericArguments[x].ToSymbolPart());
			}
			sb.Append('>');

			return sb.ToString();
		}
	}
}