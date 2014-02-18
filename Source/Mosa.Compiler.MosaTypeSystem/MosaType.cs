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
using Mosa.Compiler.Common;
using ElemType = dnlib.DotNet.ElementType;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaType : IResolvable
	{
		public MosaModule Module { get; private set; }

		public TypeDef InternalType { get; private set; }

		public TypeSig TypeSignature { get; private set; }

		public ScopedToken Token { get; private set; }

		public string Namespace { get; private set; }

		public string Name { get; private set; }

		public string FullName { get; private set; }

		public MosaType BaseType { get; internal set; }

		public MosaType EnclosingType { get; private set; }

		public bool HasOpenGenericParams { get; private set; }

		public bool IsValueType { get; private set; }

		public bool IsDelegate { get; private set; }

		public bool IsEnum { get; private set; }

		public bool IsInterface { get; private set; }

		public bool IsNested { get; private set; }

		public bool IsExplicitLayout { get; private set; }

		public bool IsModule { get; private set; }

		public int? ClassSize { get; private set; }

		public int? PackingSize { get; private set; }

		public IList<MosaMethod> Methods { get; private set; }

		public IList<MosaField> Fields { get; private set; }

		public IList<MosaType> Interfaces { get; private set; }

		public bool IsU1 { get { return TypeSignature.ElementType == ElemType.U1; } }

		public bool IsI1 { get { return TypeSignature.ElementType == ElemType.I1; } }

		public bool IsU2 { get { return TypeSignature.ElementType == ElemType.U2; } }

		public bool IsI2 { get { return TypeSignature.ElementType == ElemType.I2; } }

		public bool IsU4 { get { return TypeSignature.ElementType == ElemType.U4; } }

		public bool IsI4 { get { return TypeSignature.ElementType == ElemType.I4; } }

		public bool IsU8 { get { return TypeSignature.ElementType == ElemType.U8; } }

		public bool IsI8 { get { return TypeSignature.ElementType == ElemType.I8; } }

		public bool IsR4 { get { return TypeSignature.ElementType == ElemType.R4; } }

		public bool IsR8 { get { return TypeSignature.ElementType == ElemType.R8; } }

		public bool IsI { get { return TypeSignature.ElementType == ElemType.I; } }

		public bool IsU { get { return TypeSignature.ElementType == ElemType.U; } }

		public bool IsBoolean { get { return TypeSignature.ElementType == ElemType.Boolean; } }

		public bool IsChar { get { return TypeSignature.ElementType == ElemType.Char; } }

		public bool IsVoid { get { return TypeSignature.ElementType == ElemType.Void; } }

		public bool IsManagedPointer { get { return TypeSignature is PinnedSig || TypeSignature is ByRefSig; } }

		public bool IsUnmanagedPointer { get { return TypeSignature is PtrSig || TypeSignature is FnPtrSig; } }

		public bool IsUI1 { get { return IsU1 || IsI1; } }

		public bool IsUI2 { get { return IsU2 || IsI2; } }

		public bool IsUI4 { get { return IsU4 || IsI4; } }

		public bool IsUI8 { get { return IsU8 || IsI8; } }

		public bool IsR { get { return IsR4 || IsR8; } }

		public bool IsN { get { return IsU || IsI; } }

		public bool IsInteger { get { return IsSigned || IsUnsigned; } }

		public bool IsSigned { get { return IsI1 || IsI2 || IsI4 || IsI8 || IsI; } }

		public bool IsUnsigned { get { return IsU1 || IsU2 || IsU4 || IsU8 || IsU; } }

		public bool IsPointer { get { return IsManagedPointer || IsUnmanagedPointer; } }

		public bool IsVar { get { return TypeSignature is GenericMVar; } }

		public bool IsMVar { get { return TypeSignature is GenericVar; } }

		public bool IsArray { get { return TypeSignature is SZArraySig || TypeSignature is ArraySig; } }

		public int? VarOrMVarIndex { get { return TypeSignature is GenericSig ? (int?)((GenericSig)TypeSignature).Number : null; } }

		public MosaType ElementType { get; internal set; }

		public bool HasElementType { get { return ElementType != null; } }

		public IDictionary<MosaMethod, MosaMethod> InheritanceOveride { get; private set; }

		public bool IsLinkerGenerated { get; internal set; }

		internal MosaType(MosaModule module, string name, string @namespace)
			: this(module, new TypeDefUser(@namespace, name, null))
		{
		}

		internal MosaType(MosaModule module, TypeDef type)
			: this(module, type, type.ToTypeSig())
		{
		}

		internal MosaType(MosaModule module, TypeDef type, TypeSig typeSig)
		{
			Module = module;
			InternalType = type;
			if (type != null)
			{
				Token = new ScopedToken(module.InternalModule, type.MDToken);
			}

			IsLinkerGenerated = false;

			Methods = new List<MosaMethod>();
			Fields = new List<MosaField>();
			Interfaces = new List<MosaType>();

			InheritanceOveride = new Dictionary<MosaMethod, MosaMethod>();

			UpdateSignature(typeSig);
		}

		public override string ToString()
		{
			return FullName;
		}

		public bool Matches(MosaType type)
		{
			return type == this;
		}

		public object[] GetAttribute(string attrType)
		{
			var attr = InternalType.CustomAttributes.Find(attrType);
			if (attr == null)
				return null;

			var arguments = attr.ConstructorArguments;

			object[] result = new object[arguments.Count];
			for (int i = 0; i < result.Length; i++)
			{
				if (arguments[i].Value is UTF8String)
					result[i] = ((UTF8String)arguments[i].Value).String;
				else
					result[i] = arguments[i].Value;
			}

			return result;
		}

		public MosaType Clone()
		{
			MosaType result = (MosaType)base.MemberwiseClone();

			result.Methods = new List<MosaMethod>(this.Methods);
			result.Fields = new List<MosaField>(this.Fields);
			result.Interfaces = new List<MosaType>(this.Interfaces);

			result.InheritanceOveride = new Dictionary<MosaMethod, MosaMethod>(this.InheritanceOveride);

			return result;
		}

		/// <summary>
		/// Gets the type on the stack.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// The equivalent stack type code.
		/// </returns>
		/// <exception cref="InvalidCompilerException"></exception>
		public StackTypeCode GetStackType()
		{
			switch (TypeSignature.RemovePinnedAndModifiers().ElementType)
			{
				case ElemType.Boolean:
				case ElemType.Char:
				case ElemType.I1:
				case ElemType.U1:
				case ElemType.I2:
				case ElemType.U2:
				case ElemType.I4:
				case ElemType.U4:
					return StackTypeCode.Int32;

				case ElemType.I8:
				case ElemType.U8:
					return StackTypeCode.Int64;

				case ElemType.R4:
				case ElemType.R8:
					return StackTypeCode.F;

				case ElemType.I:
				case ElemType.U:
					return StackTypeCode.N;

				case ElemType.ByRef:
					return StackTypeCode.ManagedPointer;

				case ElemType.Ptr:
				case ElemType.FnPtr:
					return StackTypeCode.UnmanagedPointer;

				case ElemType.String:
				case ElemType.ValueType:
				case ElemType.Class:
				case ElemType.Var:
				case ElemType.Array:
				case ElemType.GenericInst:
				case ElemType.TypedByRef:
				case ElemType.Object:
				case ElemType.SZArray:
				case ElemType.MVar:
					return StackTypeCode.O;

				case ElemType.Void:
					return StackTypeCode.Unknown;
			}
			throw new InvalidCompilerException(string.Format("Can't transform Type {0} to StackTypeCode.", this));
		}

		internal void UpdateSignature(TypeSig sig)
		{
			TypeSignature = sig;
			Namespace = sig.ReflectionNamespace;
			Name = sig is FnPtrSig ? FullNameCreator.MethodFullName("", "", ((FnPtrSig)sig).MethodSig) : sig.ReflectionName;
			FullName = sig is FnPtrSig ? FullNameCreator.MethodFullName("", "", ((FnPtrSig)sig).MethodSig) : sig.ReflectionFullName;

			var sSig = sig.RemovePinnedAndModifiers();
			TypeDef typeDef = null;

			if (sSig is TypeDefOrRefSig)
				typeDef = ((TypeDefOrRefSig)sSig).TypeDefOrRef.ResolveTypeDef();
			else if (sSig is GenericInstSig)
				typeDef = ((GenericInstSig)sSig).GenericType.TypeDefOrRef.ResolveTypeDef();

			if (typeDef != null)
			{
				IsValueType = typeDef.IsValueType;
				IsDelegate = typeDef.BaseType != null && typeDef.BaseType.DefinitionAssembly.IsCorLib() &&
					(typeDef.BaseType.FullName == "System.Delegate" || typeDef.BaseType.FullName == "System.MulticastDelegate");
				IsEnum = typeDef.IsEnum;
				IsInterface = typeDef.IsInterface;
				IsNested = typeDef.IsNested;
				IsExplicitLayout = typeDef.IsExplicitLayout;
				IsModule = typeDef.IsGlobalModuleType;
				if (typeDef.HasClassLayout)
				{
					ClassSize = (int)typeDef.ClassLayout.ClassSize;
					PackingSize = typeDef.ClassLayout.PackingSize;
				}
				else
				{
					ClassSize = PackingSize = null;
				}
			}
			else
			{
				IsValueType = false;
				IsDelegate = false;
				IsEnum = false;
				IsInterface = false;
				IsNested = false;
				IsExplicitLayout = false;
				IsModule = false;
				ClassSize = PackingSize = null;
			}

			HasOpenGenericParams = sig.HasOpenGenericParameter();
		}

		internal bool Resolved { get; private set; }
		void IResolvable.Resolve(MosaTypeLoader loader)
		{
			// InternalType is null means this instance is generic parameters
			if (!Resolved && InternalType != null)
			{
				GenericArgumentResolver resolver = new GenericArgumentResolver();

				if (TypeSignature.IsArray || TypeSignature.IsSZArray)
					BaseType = loader.GetType(InternalType.ToTypeSig());

				else if (InternalType.BaseType != null)
					BaseType = loader.GetType(InternalType.BaseType.ToTypeSig());

				if (InternalType.DeclaringType != null)
					EnclosingType = loader.GetType(InternalType.DeclaringType.ToTypeSig());

				Interfaces.Clear();
				foreach (var iface in InternalType.Interfaces)
					Interfaces.Add(loader.GetType(iface.Interface.ToTypeSig()));
			}
			Resolved = true;
		}
	}
}