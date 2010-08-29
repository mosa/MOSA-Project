
namespace Mosa.Runtime
{
	using System;

	using Mosa.Runtime.Metadata.Signatures;
	using Mosa.Runtime.Vm;

	public class CilGenericField : RuntimeField
	{
		private readonly RuntimeField genericField;

		public CilGenericField(RuntimeType declaringType, RuntimeField genericField, FieldSignature signature, ITypeSystem typeSystem) :
			base(declaringType.Module, declaringType, typeSystem)
		{
			this.genericField = genericField;
			this.Signature = signature;

			// FIXME: RVA, Address of these?
			this.Attributes = genericField.Attributes;
			this.SetAttributes(genericField.CustomAttributes);
		}

		protected override FieldSignature GetSignature()
		{
			throw new NotImplementedException();
		}

		protected override string GetName()
		{
			return this.genericField.Name;
		}
	}
}
