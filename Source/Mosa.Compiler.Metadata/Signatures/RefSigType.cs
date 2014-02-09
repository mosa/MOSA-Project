/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Text;

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	/// Reference signature type.
	/// </summary>
	public sealed class RefSigType : SigType
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RefSigType"/> class.
		/// </summary>
		/// <param name="type">The referenced type.</param>
		public RefSigType(SigType type) :
			base(CilElementType.ByRef)
		{
			if (null == type)
				throw new ArgumentNullException(@"type");

			ElementType = type;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the type referenced by this signature type.
		/// </summary>
		/// <value>The referenced type.</value>
		public SigType ElementType { get; private set; }

		#endregion Properties

		/// <summary>
		/// Expresses the byref parameter signature component in a meaningful, symbol-friendly string form
		/// </summary>
		/// <returns></returns>
		public override string ToSymbolPart()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("ref ");
			sb.Append(ElementType.ToSymbolPart());
			return sb.ToString();
		}
	}
}