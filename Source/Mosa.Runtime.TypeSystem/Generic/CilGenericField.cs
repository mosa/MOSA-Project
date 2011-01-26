using System;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.TypeSystem.Cil;

namespace Mosa.Runtime.TypeSystem.Generic
{
	public class CilGenericField : RuntimeField
	{
		public CilGenericField(RuntimeField genericField, FieldSignature signature, CilGenericType declaringType) :
			base(declaringType)
		{
			this.Signature = signature;
			this.Attributes = genericField.Attributes;
			this.SetAttributes(genericField.CustomAttributes);

			base.Name = genericField.Name;
		}

	}
}
