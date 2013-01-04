/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.TypeSystem.Cil
{
	/// <summary>
	/// A CIL specialization of <see cref="RuntimeField"/>.
	/// </summary>
	sealed public class CilRuntimeField : RuntimeField
	{
		#region Data Members

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeField" /> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="name">The name.</param>
		/// <param name="sigType">Type of the sig.</param>
		/// <param name="token">The token.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="rva">The rva.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		/// <param name="attributes">The attributes.</param>
		public CilRuntimeField(ITypeModule module, string name, SigType sigType, Token token, uint offset, uint rva, RuntimeType declaringType, FieldAttributes attributes) :
			base(module, token, declaringType)
		{
			this.Name = name;
			this.SigType = sigType;
			base.Attributes = attributes;
			base.RVA = rva;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeField" /> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="genericField">The generic field.</param>
		/// <param name="sigType">Type of the sig.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		public CilRuntimeField(ITypeModule module, RuntimeField genericField, SigType sigType, CilGenericType declaringType) :
			base(module, declaringType)
		{
			this.Name = genericField.Name;
			this.SigType = sigType;
			this.Attributes = genericField.Attributes;
		}

		#endregion // Construction

	}
}
