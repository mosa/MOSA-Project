using System;

using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Metadata.Runtime
{
	public class CilGenericField : RuntimeField
	{
		private readonly RuntimeField genericField;

		public CilGenericField(IModuleTypeSystem moduleTypeSystem, RuntimeField genericField, FieldSignature signature, CilGenericType declaringType) :
			base(moduleTypeSystem, declaringType)
		{
			this.Signature = signature;
			this.genericField = genericField;
			this.Attributes = genericField.Attributes;
			this.SetAttributes(genericField.CustomAttributes);
			
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
