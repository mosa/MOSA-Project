/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using System.Collections.Generic;
using dnlib.DotNet;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaField : IResolvable
	{
		public MosaModule Module { get; private set; }

		public FieldDef InternalField { get; private set; }

		public FieldSig FieldSignature { get; private set; }

		public ScopedToken Token { get; private set; }

		public string Name { get { return InternalField.Name; } }

		public string FullName { get; private set; }

		public MosaType DeclaringType { get; private set; }

		public MosaType FieldType { get; private set; }

		public bool IsLiteralField { get { return InternalField.IsLiteral; } }

		public bool IsStaticField { get { return InternalField.IsStatic; } }

		public bool HasDefault { get { return InternalField.HasDefault; } }

		public bool HasRVA { get { return InternalField.HasFieldRVA; } }

		public uint? Offset { get { return InternalField.FieldOffset; } }

		public byte[] Data { get { return InternalField.InitialValue; } }

		public bool IsLinkerGenerated { get; internal set; }


		internal MosaField(MosaModule module, MosaType declType, string name, FieldSig signature)
			: this(module, declType, new FieldDefUser(name, signature))
		{
		}

		internal MosaField(MosaModule module, MosaType declType, FieldDef field)
			: this(module, declType, field, field.FieldSig)
		{
		}

		internal MosaField(MosaModule module, MosaType declType, FieldDef field, FieldSig fieldSig)
		{
			Module = module;
			InternalField = field;
			Token = new ScopedToken(module.InternalModule, field.MDToken);

			IsLinkerGenerated = false;

			UpdateSignature(fieldSig, declType);
		}

		public override string ToString()
		{
			return FullName;
		}

		internal MosaField Clone()
		{
			return (MosaField)base.MemberwiseClone();
		}

		internal void UpdateSignature(FieldSig sig, MosaType declaringType)
		{
			DeclaringType = declaringType;
			FieldSignature = sig;

			IList<TypeSig> typeGenericArgs = null;
			if (DeclaringType.TypeSignature.IsGenericInstanceType)
				typeGenericArgs = ((GenericInstSig)DeclaringType.TypeSignature).GenericArguments;

			FullName = FullNameCreator.FieldFullName(DeclaringType.FullName, Name, sig, typeGenericArgs);
		}

		void IResolvable.Resolve(MosaTypeLoader loader)
		{
			FieldType = loader.GetType(FieldSignature.Type);
		}
	}
}