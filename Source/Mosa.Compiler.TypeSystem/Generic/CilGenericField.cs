
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.TypeSystem.Generic
{
	public class CilGenericField : RuntimeField
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CilGenericField"/> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="genericField">The generic field.</param>
		/// <param name="signature">The signature.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		public CilGenericField(ITypeModule module, RuntimeField genericField, FieldSignature signature, CilGenericType declaringType) :
			base(module, declaringType)
		{
			this.Signature = signature;
			this.Attributes = genericField.Attributes;
			//TODO
			//this.SetAttributes(genericField.CustomAttributes);

			base.Name = genericField.Name;
		}

	}
}
