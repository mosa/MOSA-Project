/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Text;

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	/// Specifies an instance of a generic type as a signature type.
	/// </summary>
	public sealed class GenericInstSigType : SigType
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericInstSigType"/> class.
		/// </summary>
		/// <param name="baseType">Type of the base.</param>
		/// <param name="genericArguments">The generic args.</param>
		public GenericInstSigType(TypeSigType baseType, SigType[] genericArguments) :
			base(CilElementType.GenericInst)
		{
			BaseType = baseType;
			GenericArguments = genericArguments;
			ContainsGenericParameters = CheckContainsOpenGenericParameters();
		}

		#endregion Construction

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

		/// <summary>
		/// Gets a value indicating whether the the signature's types are closed.
		/// </summary>
		/// <value><c>true</c> if this signature's types are closed; otherwise, <c>false</c>.</value>
		public bool ContainsGenericParameters { get; private set; }

		#endregion Properties

		#region SigType Overrides

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public override bool Equals(SigType other)
		{
			GenericInstSigType gist = other as GenericInstSigType;
			if (gist == null)
				return false;

			// TEMP
			if (!base.Equals(other))
				return false;

			if (BaseType != gist.BaseType)
				return false;

			if (!SigType.Equals(GenericArguments, gist.GenericArguments))
				return false;

			// END TEMP

			return (base.Equals(other) && BaseType == gist.BaseType && SigType.Equals(GenericArguments, gist.GenericArguments));
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return base.ToString() + " " + BaseType.ToString();
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

		private bool CheckContainsOpenGenericParameters()
		{
			foreach (SigType type in GenericArguments)
				if (type.IsOpenGenericParameter)
					return true;

			return false;
		}
	}
}