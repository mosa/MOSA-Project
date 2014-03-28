/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Mosa.Compiler.Common;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.MosaTypeSystem.Metadata
{
	internal class MetadataResolver
	{
		private CLRMetadata metadata;

		public MetadataResolver(CLRMetadata metadata)
		{
			this.metadata = metadata;
		}

		private Queue<MosaUnit> resolveQueue = new Queue<MosaUnit>();

		public void EnqueueForResolve(MosaUnit unit)
		{
			resolveQueue.Enqueue(unit);
		}

		public void Resolve()
		{
			foreach (var unit in metadata.Loader.LoadedUnits)
			{
				if (unit is MosaType)
				{
					MosaType type = (MosaType)unit;
					using (var mosaType = metadata.Controller.MutateType(type))
					{
						TypeDef typeDef = type.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>().Definition;

						if (typeDef.BaseType != null)
							mosaType.BaseType = metadata.Loader.GetType(typeDef.BaseType.ToTypeSig());

						if (typeDef.DeclaringType != null)
							mosaType.DeclaringType = metadata.Loader.GetType(typeDef.DeclaringType.ToTypeSig());

						foreach (var iface in typeDef.Interfaces)
							mosaType.Interfaces.Add(metadata.Loader.GetType(iface.Interface.ToTypeSig()));
					}
					ResolveType(type);
				}
				else if (unit is MosaField || unit is MosaMethod)
					resolveQueue.Enqueue(unit);
			}

			while (resolveQueue.Count > 0)
			{
				MosaUnit unit = resolveQueue.Dequeue();
				if (unit is MosaType)
					ResolveType((MosaType)unit);
				if (unit is MosaField)
					ResolveField((MosaField)unit);
				if (unit is MosaMethod)
					ResolveMethod((MosaMethod)unit);
			}

			foreach (var module in metadata.Cache.Modules.Values)
			{
				ModuleDef moduleDef = module.GetUnderlyingObject<UnitDesc<ModuleDef, object>>().Definition;
				if (moduleDef.EntryPoint != null)
				{
					using (var mosaModule = metadata.Controller.MutateModule(module))
						mosaModule.EntryPoint = metadata.Cache.GetMethodByToken(new ScopedToken(moduleDef, moduleDef.EntryPoint.MDToken));
				}
			}
		}

		private void ResolveCustomAttributes(MosaUnit.MutatorBase unit, IHasCustomAttribute obj)
		{
			// TODO: More detail parsing
			foreach (var attr in obj.CustomAttributes)
			{
				MosaType type = metadata.Loader.GetType(attr.AttributeType.ToTypeSig());
				MethodDef ctor = ((IMethodDefOrRef)attr.Constructor).ResolveMethod();
				MosaMethod mosaCtor = null;
				foreach (var method in type.Methods)
				{
					var desc = method.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>();
					if (desc.Token.Token == ctor.MDToken)
					{
						mosaCtor = method;
						break;
					}
				}
				if (mosaCtor == null)
					throw new AssemblyLoadException();

				object[] values = new object[attr.ConstructorArguments.Count];
				for (int i = 0; i < values.Length; i++)
				{
					if (attr.ConstructorArguments[i].Value is UTF8String)
						values[i] = ((UTF8String)attr.ConstructorArguments[i].Value).String;
					else
						values[i] = attr.ConstructorArguments[i].Value;
				}
				unit.CustomAttributes.Add(new MosaCustomAttribute(mosaCtor, values));
			}
		}

		private void ResolveType(MosaType type)
		{
			GenericArgumentResolver resolver = new GenericArgumentResolver();

			if (type.GenericArguments.Count > 0)
				resolver.PushTypeGenericArguments(type.GenericArguments.GetGenericArguments());

			using (var mosaType = metadata.Controller.MutateType(type))
			{
				if (type.BaseType != null)
					mosaType.BaseType = metadata.Loader.GetType(resolver.Resolve(type.BaseType.GetTypeSig()));

				if (type.DeclaringType != null)
					mosaType.DeclaringType = metadata.Loader.GetType(resolver.Resolve(type.DeclaringType.GetTypeSig()));

				for (int i = 0; i < type.Interfaces.Count; i++)
					mosaType.Interfaces[i] = metadata.Loader.GetType(resolver.Resolve(type.Interfaces[i].GetTypeSig()));

				mosaType.HasOpenGenericParams = type.GetTypeSig().HasOpenGenericParameter();

				ResolveCustomAttributes(mosaType, type.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>().Definition);
			}
		}

		private void ResolveField(MosaField field)
		{
			GenericArgumentResolver resolver = new GenericArgumentResolver();

			if (field.DeclaringType.GenericArguments.Count > 0)
				resolver.PushTypeGenericArguments(field.DeclaringType.GenericArguments.GetGenericArguments());

			using (var mosaField = metadata.Controller.MutateField(field))
			{
				mosaField.FieldType = metadata.Loader.GetType(resolver.Resolve(field.GetFieldSig().Type));

				mosaField.HasOpenGenericParams =
					field.DeclaringType.HasOpenGenericParams ||
					field.FieldType.GetTypeSig().HasOpenGenericParameter();

				ResolveCustomAttributes(mosaField, field.GetUnderlyingObject<UnitDesc<FieldDef, FieldSig>>().Definition);
			}
		}

		private void ResolveMethod(MosaMethod method)
		{
			GenericArgumentResolver resolver = new GenericArgumentResolver();
			bool hasOpening = method.DeclaringType.HasOpenGenericParams;

			if (method.DeclaringType.GenericArguments.Count > 0)
			{
				foreach (var i in method.DeclaringType.GenericArguments.GetGenericArguments())
					hasOpening |= i.HasOpenGenericParameter();
				resolver.PushTypeGenericArguments(method.DeclaringType.GenericArguments.GetGenericArguments());
			}

			if (method.GenericArguments.Count > 0)
			{
				foreach (var i in method.GenericArguments.GetGenericArguments())
					hasOpening |= i.HasOpenGenericParameter();
				resolver.PushMethodGenericArguments(method.GenericArguments.GetGenericArguments());
			}
			else
				hasOpening |= method.GetMethodSig().HasOpenGenericParameter();

			using (var mosaMethod = metadata.Controller.MutateMethod(method))
			{
				var desc = method.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>();

				MosaType returnType = metadata.Loader.GetType(resolver.Resolve(desc.Signature.RetType));
				List<MosaParameter> pars = new List<MosaParameter>();

				Debug.Assert(desc.Signature.GetParamCount() == desc.Definition.ParamDefs.Count);
				for (int i = 0; i < desc.Definition.ParamDefs.Count; i++)
				{
					pars.Add(new MosaParameter(desc.Definition.ParamDefs[i].FullName, metadata.Loader.GetType(resolver.Resolve(desc.Signature.Params[i]))));
				}

				mosaMethod.Signature = new MosaMethodSignature(returnType, pars);

				if (desc.Definition.HasBody)
					ResolveBody(desc.Definition, mosaMethod, desc.Definition.Body, resolver);

				mosaMethod.HasOpenGenericParams = hasOpening;

				ResolveCustomAttributes(mosaMethod, desc.Definition);
			}
		}

		private void ResolveBody(MethodDef methodDef, MosaMethod.Mutator method, CilBody body, GenericArgumentResolver resolver)
		{
			method.LocalVariables.Clear();
			int index = 0;
			foreach (var variable in body.Variables)
			{
				method.LocalVariables.Add(new MosaLocal(
					variable.Name ?? "V_" + index,
					metadata.Loader.GetType(resolver.Resolve(variable.Type)),
					variable.Type.IsPinned));
				index++;
			}

			method.ExceptionBlocks.Clear();
			foreach (var eh in body.ExceptionHandlers)
			{
				method.ExceptionBlocks.Add(new MosaExceptionHandler(
					(ExceptionHandlerType)eh.HandlerType,
					(int)eh.TryStart.Offset,
					(int)eh.TryEnd.Offset,
					(int)eh.HandlerStart.Offset,
					(int)eh.TryEnd.Offset,
					eh.CatchType == null ? null : metadata.Loader.GetType(resolver.Resolve(eh.CatchType.ToTypeSig())),
					eh.FilterStart == null ? null : (int?)eh.FilterStart.Offset
				));
			}

			method.MaxStack = methodDef.Body.MaxStack;

			method.Code.Clear();
			for (int i = 0; i < body.Instructions.Count; i++)
			{
				method.Code.Add(ResolveInstruction(methodDef, body, i, resolver));
			}
		}

		private MosaInstruction ResolveInstruction(MethodDef methodDef, CilBody body, int index, GenericArgumentResolver resolver)
		{
			Instruction instruction = body.Instructions[index];
			int? prev = index == 0 ? null : (int?)body.Instructions[index - 1].Offset;
			int? next = index == body.Instructions.Count - 1 ? null : (int?)body.Instructions[index + 1].Offset;

			object operand = instruction.Operand;

			if (instruction.Operand is ITypeDefOrRef)
			{
				operand = ResolveTypeOperand((ITypeDefOrRef)instruction.Operand, resolver);
			}
			else if (instruction.Operand is MemberRef)
			{
				MemberRef memberRef = (MemberRef)instruction.Operand;
				if (memberRef.IsFieldRef)
					operand = ResolveFieldOperand(memberRef, resolver);
				else
					operand = ResolveMethodOperand(memberRef, resolver);
			}
			else if (instruction.Operand is IField)
			{
				operand = ResolveFieldOperand((IField)instruction.Operand, resolver);
			}
			else if (instruction.Operand is IMethod)
			{
				operand = ResolveMethodOperand((IMethod)instruction.Operand, resolver);
			}
			else if (instruction.Operand is Local)
			{
				operand = ((Local)instruction.Operand).Index;
			}
			else if (instruction.Operand is Parameter)
			{
				operand = ((Parameter)instruction.Operand).Index;
			}
			else if (instruction.Operand is Instruction)
			{
				operand = (int)((Instruction)instruction.Operand).Offset;
			}
			else if (instruction.Operand is Instruction[])
			{
				Instruction[] targets = (Instruction[])instruction.Operand;
				int[] offsets = new int[targets.Length];
				for (int i = 0; i < offsets.Length; i++)
					offsets[i] = (int)targets[i].Offset;
				operand = offsets;
			}
			else if (instruction.Operand is string)
			{
				operand = metadata.Cache.GetStringId((string)instruction.Operand);
			}

			ushort code = (ushort)instruction.OpCode.Code;
			if (code > 0xff)    // To match compiler's opcode values
				code = (ushort)(0x100 + (code & 0xff));

			return new MosaInstruction((int)instruction.Offset, code, operand, prev, next);
		}

		private MosaField ResolveFieldOperand(IField operand, GenericArgumentResolver resolver)
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

			MosaType type = metadata.Loader.GetType(resolver.Resolve(declType));
			foreach (var field in type.Fields)
			{
				var desc = field.GetUnderlyingObject<UnitDesc<FieldDef, FieldSig>>();
				if (desc.Token.Token == fieldToken)
				{
					return field;
				}
			}
			throw new AssemblyLoadException();
		}

		private MosaMethod ResolveMethodOperand(IMethod operand, GenericArgumentResolver resolver)
		{
			if (operand is MethodSpec)
				return metadata.Loader.LoadGenericMethodInstance((MethodSpec)operand, resolver);

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

			MDToken methodToken = methodDef.MDToken;

			MosaType type = metadata.Loader.GetType(resolver.Resolve(declType));
			foreach (var method in type.Methods)
			{
				var desc = method.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>();
				if (desc.Token.Token == methodToken)
				{
					return method;
				}
			}

			throw new AssemblyLoadException();
		}

		private MosaType ResolveTypeOperand(ITypeDefOrRef operand, GenericArgumentResolver resolver)
		{
			return metadata.Loader.GetType(resolver.Resolve(operand.ToTypeSig()));
		}
	}
}
