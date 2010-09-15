using System;

using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime
{
	public class CilGenericField : RuntimeField
	{
		private readonly RuntimeField genericField;

		public CilGenericField(IModuleTypeSystem moduleTypeSystem, RuntimeType declaringType, RuntimeField genericField, FieldSignature signature) :
			base(moduleTypeSystem, declaringType)
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
