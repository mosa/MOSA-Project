using System;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeLoader;
using Mosa.Runtime.TypeLoader.Cil;

namespace Mosa.Runtime.TypeLoader.Generic
{
	public class CilGenericField : RuntimeField
	{
		//private readonly RuntimeField genericField;

		public CilGenericField(RuntimeField genericField, FieldSignature signature, CilGenericType declaringType) :
			base(declaringType)
		{
			//this.genericField = genericField;
			this.Signature = signature;
			this.Attributes = genericField.Attributes;
			this.SetAttributes(genericField.CustomAttributes);

			base.Name = genericField.Name;
		}

	}
}
