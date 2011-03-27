
using System;

namespace Mosa.Runtime.Metadata.Signatures
{
	public class TypeSigType : SigType
	{
		private readonly MetadataToken token;

		public MetadataToken Token { get { return this.token; } }

		public TypeSigType(MetadataToken token, CilElementType type) :
			base(type)
		{
			this.token = token;
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return base.ToString() + " [" + this.token.ToString() + "]";
		}

	}
}
