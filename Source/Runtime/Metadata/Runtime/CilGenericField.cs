using System;

using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Metadata.Runtime
{
	public class CilGenericField : RuntimeField
	{
		private readonly RuntimeField genericField;

		public CilGenericField(IModuleTypeSystem moduleTypeSystem, CilGenericType declaringType, RuntimeField genericField) :
			base(moduleTypeSystem, declaringType)
		{
			this.Signature = new FieldSignature(MetadataModule.Metadata, genericField.Signature.Token);

			this.genericField = genericField;

			// FIXME: RVA, Address of these?
			this.Attributes = genericField.Attributes;
			this.SetAttributes(genericField.CustomAttributes);

			Signature.ApplyGenericType(declaringType.GenericArguments);

			return;
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
