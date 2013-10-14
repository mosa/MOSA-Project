namespace Mosa.Compiler.Metadata.Signatures
{
	public class TypeSigType : SigType
	{
		private readonly Token token;

		public Token Token { get { return this.token; } }

		public TypeSigType(Token token, CilElementType type) :
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
			return base.ToString() + " " + this.token.ToString();
		}
	}
}