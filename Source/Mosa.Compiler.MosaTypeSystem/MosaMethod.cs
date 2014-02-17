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
using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaMethod : IResolvable
	{
		public MosaModule Module { get; private set; }

		public MethodDef InternalMethod { get; private set; }

		public MethodSig MethodSignature { get; private set; }

		public ScopedToken Token { get; private set; }

		public string Name { get { return InternalMethod.Name; } }

		public string FullName { get; private set; }

		public MosaType DeclaringType { get; private set; }

		public bool HasOpenGenericParams { get; private set; }

		public bool IsAbstract { get { return InternalMethod.IsAbstract; } }

		public bool IsStatic { get { return InternalMethod.IsStatic; } }

		public bool HasThis { get { return InternalMethod.HasThis; } }

		public bool HasExplicitThis { get { return InternalMethod.ExplicitThis; } }

		public MosaType ReturnType { get; private set; }

		public IList<MosaParameter> Parameters { get; private set; }

		public bool IsInternal { get { return InternalMethod.IsInternalCall; } }

		public bool IsNoInlining { get { return InternalMethod.IsNoInlining; } }

		public bool IsSpecialName { get { return InternalMethod.IsSpecialName; } }

		public bool IsRTSpecialName { get { return InternalMethod.IsRuntimeSpecialName; } }

		public bool IsVirtual { get { return InternalMethod.IsVirtual; } }

		public bool IsPInvokeImpl { get { return InternalMethod.IsPinvokeImpl; } }

		public bool IsNewSlot { get { return InternalMethod.IsNewSlot; } }

		public bool IsFinal { get { return InternalMethod.IsFinal; } }

		public uint RVA { get { return (uint)InternalMethod.RVA; } }

		public IList<MosaType> GenericArguments { get; private set; }

		public IList<MosaType> LocalVariables { get; private set; }

		public IList<Instruction> Code { get; private set; }

		public IList<MosaExceptionHandler> ExceptionBlocks { get; private set; }

		public string ExternMethod { get { return InternalMethod.ImplMap == null ? null : InternalMethod.ImplMap.Name; } }

		public bool HasCode { get { return InternalMethod.HasBody; } }

		public bool IsLinkerGenerated { get; internal set; }

		internal MosaMethod(MosaModule module, MosaType declType, string name, MethodSig signature)
			: this(module, declType, new MethodDefUser(name, signature)
			{
				Body = new CilBody(false, new List<Instruction>(), new List<ExceptionHandler>(), new List<Local>())
			})
		{
		}

		internal MosaMethod(MosaModule module, MosaType declType, MethodDef method)
			: this(module, declType, method, method.MethodSig)
		{
		}

		internal MosaMethod(MosaModule module, MosaType declType, MethodDef method, MethodSig methodSig)
		{
			Module = module;
			InternalMethod = method;
			Token = new ScopedToken(module.InternalModule, method.MDToken);

			IsLinkerGenerated = false;

			Parameters = new List<MosaParameter>();
			LocalVariables = new List<MosaType>();
			if (method.HasBody)
			{
				Code = new List<Instruction>();
				ExceptionBlocks = new List<MosaExceptionHandler>();
			}
			else
			{
				Code = null;
				ExceptionBlocks = null;
			}

			GenericArguments = new List<MosaType>();

			UpdateSignature(methodSig, declType);
		}

		public override string ToString()
		{
			return FullName;
		}

		public bool Matches(MosaMethod method)
		{
			return new SigComparer().Equals(method.MethodSignature, this.MethodSignature);
		}

		public object[] GetAttribute(string attrType)
		{
			var attr = InternalMethod.CustomAttributes.Find(attrType);
			if (attr == null)
				return null;

			var arguments = attr.ConstructorArguments;

			object[] result = new object[arguments.Count];
			for (int i = 0; i < result.Length; i++)
				result[i] = arguments[i].Value;

			return result;
		}

		internal MosaMethod Clone()
		{
			MosaMethod result = (MosaMethod)base.MemberwiseClone();

			result.Parameters = new List<MosaParameter>(this.Parameters);
			result.LocalVariables = new List<MosaType>(this.LocalVariables);
			if (result.InternalMethod.HasBody)
			{
				result.Code = new List<Instruction>(this.Code);
				result.ExceptionBlocks = new List<MosaExceptionHandler>(this.ExceptionBlocks);
			}

			result.GenericArguments = new List<MosaType>(this.GenericArguments);

			return result;
		}

		internal void UpdateSignature(MethodSig sig, MosaType declaringType)
		{
			DeclaringType = declaringType;
			MethodSignature = sig;

			IList<TypeSig> typeGenericArgs = null;
			if (DeclaringType.TypeSignature.IsGenericInstanceType)
				typeGenericArgs = ((GenericInstSig)DeclaringType.TypeSignature).GenericArguments;

			List<TypeSig> methodGenericArgs = null;
			if (this.GenericArguments.Count > 0)
			{
				methodGenericArgs = new List<TypeSig>();
				foreach (var genericArg in this.GenericArguments)
					methodGenericArgs.Add(genericArg.TypeSignature);
			}

			FullName = FullNameCreator.MethodFullName(DeclaringType.FullName, Name, MethodSignature, typeGenericArgs, methodGenericArgs);

			HasOpenGenericParams = DeclaringType.HasOpenGenericParams || sig.HasOpenGenericParameter();

			// TODO: update parameters
		}

		void IResolvable.Resolve(MosaTypeLoader loader)
		{
			GenericArgumentResolver resolver = new GenericArgumentResolver();
			if (GenericArguments.Count > 0)
			{
				List<TypeSig> genericArgs = new List<TypeSig>();
				foreach (var genericArg in this.GenericArguments)
					genericArgs.Add(genericArg.TypeSignature);

				resolver.PushMethodGenericArguments(genericArgs);
			}


			ReturnType = loader.GetType(resolver.Resolve(MethodSignature.RetType));

			Debug.Assert(MethodSignature.GetParamCount() == InternalMethod.ParamDefs.Count);
			Parameters.Clear();
			for (int i = 0; i < InternalMethod.ParamDefs.Count; i++)
			{
				Parameters.Add(new MosaParameter(InternalMethod.ParamDefs[i].FullName, loader.GetType(resolver.Resolve(MethodSignature.Params[i]))));
			}

			if (InternalMethod.HasBody)
			{
				ResolveBody(loader, resolver);
			}
		}

		internal void ResolveBody(MosaTypeLoader loader, GenericArgumentResolver resolver)
		{
			LocalVariables.Clear();
			foreach (var variable in InternalMethod.Body.Variables)
				LocalVariables.Add(loader.GetType(resolver.Resolve(variable.Type)));

			ExceptionBlocks.Clear();
			foreach (var eh in InternalMethod.Body.ExceptionHandlers)
			{
				ExceptionBlocks.Add(new MosaExceptionHandler(eh)
				{
					Type = eh.CatchType == null ? null : loader.GetType(resolver.Resolve(eh.CatchType.ToTypeSig()))
				});
			}

			Code.Clear();
			foreach (var instr in InternalMethod.Body.Instructions)
			{
				Code.Add(ResolveInstruction(instr, loader, resolver));
			}
		}

		Instruction ResolveInstruction(Instruction instruction, MosaTypeLoader loader, GenericArgumentResolver resolver)
		{
			Instruction result = instruction.Clone();

			if (result.Operand is ITypeDefOrRef)
				result.Operand = ResolveType((ITypeDefOrRef)result.Operand, loader, resolver);

			else if (result.Operand is MemberRef)
			{
				MemberRef memberRef = (MemberRef)result.Operand;
				if (memberRef.IsFieldRef)
					result.Operand = ResolveField(memberRef, loader, resolver);
				else
					result.Operand = ResolveMethod(memberRef, loader, resolver);
			}
			else if (result.Operand is IField)
				result.Operand = ResolveField((IField)result.Operand, loader, resolver);

			else if (result.Operand is IMethod)
				result.Operand = ResolveMethod((IMethod)result.Operand, loader, resolver);

			return result;
		}

		MosaField ResolveField(IField operand, MosaTypeLoader loader, GenericArgumentResolver resolver)
		{
			TypeSig declType;
			FieldDef fieldDef = operand as FieldDef;
			if (fieldDef == null)
			{
				MemberRef memberRef = (MemberRef)operand;
				fieldDef = memberRef.ResolveFieldThrow();
				declType = memberRef.DeclaringType.ToTypeSig();
			}
			else
				declType = fieldDef.DeclaringType.ToTypeSig();

			MDToken fieldToken = fieldDef.MDToken;

			MosaType type = loader.GetType(resolver.Resolve(declType));
			foreach (var field in type.Fields)
			{
				if (field.Token.Token == fieldToken)
					return field;
			}
			throw new AssemblyLoadException();
		}

		MosaMethod ResolveMethod(IMethod operand, MosaTypeLoader loader, GenericArgumentResolver resolver)
		{
			if (operand is MethodSpec)
				return loader.LoadGenericMethodInstance((MethodSpec)operand, resolver);

			TypeSig declType;
			MethodDef methodDef = operand as MethodDef;
			if (methodDef == null)
			{
				MemberRef memberRef = (MemberRef)operand;
				methodDef = memberRef.ResolveMethodThrow();
				declType = memberRef.DeclaringType.ToTypeSig();
			}
			else
				declType = methodDef.DeclaringType.ToTypeSig();

			MDToken fieldToken = methodDef.MDToken;

			MosaType type = loader.GetType(resolver.Resolve(declType));
			foreach (var field in type.Methods)
			{
				if (field.Token.Token == fieldToken)
					return field;
			}

			throw new AssemblyLoadException();
		}

		MosaType ResolveType(ITypeDefOrRef operand, MosaTypeLoader loader, GenericArgumentResolver resolver)
		{
			return loader.GetType(resolver.Resolve(operand.ToTypeSig()));
		}
	}
}