/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;
using System.Text;

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.TypeSystem.Cil
{
	public class CilGenericType : RuntimeType
	{
		/// <summary>
		///
		/// </summary>
		private readonly CilRuntimeType baseGenericType;

		/// <summary>
		///
		/// </summary>
		private readonly SigType[] genericArguments;

		/// <summary>
		///
		/// </summary>
		private readonly bool containsOpenGenericArguments;

		/// <summary>
		///
		/// </summary>
		public ITypeModule InstantiationModule { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CilGenericType" /> class.
		/// </summary>
		/// <param name="typeModule">The type module.</param>
		/// <param name="token">The token.</param>
		/// <param name="baseGenericType">Type of the base generic.</param>
		/// <param name="genericArguments">The generic arguments.</param>
		public CilGenericType(ITypeModule typeModule, Token token, RuntimeType baseGenericType, SigType[] genericArguments) :
			base(baseGenericType.Module, token, baseGenericType.BaseType)
		{
			Debug.Assert(baseGenericType is CilRuntimeType);

			this.baseGenericType = baseGenericType as CilRuntimeType;
			this.InstantiationModule = typeModule;
			base.Attributes = baseGenericType.Attributes;
			base.Namespace = baseGenericType.Namespace;

			if (this.baseGenericType.IsNested)
			{
				// TODO: find generic type

				;
			}

			// TODO: if this is a nested types, add enclosing type(s) into genericArguments first
			this.genericArguments = genericArguments;

			base.Name = GetName(typeModule);

			ResolveMethods();
			ResolveFields();

			this.containsOpenGenericArguments = CheckContainsOpenGenericParameters();
		}

		/// <summary>
		/// Gets the generic arguments.
		/// </summary>
		/// <value>The generic arguments.</value>
		public SigType[] GenericArguments
		{
			get { return genericArguments; }
		}

		public RuntimeType BaseGenericType
		{
			get { return baseGenericType; }
		}

		private string GetName(ITypeModule typeModule)
		{
			var sb = new StringBuilder();
			sb.AppendFormat("{0}<", baseGenericType.Name);

			foreach (var sigType in genericArguments)
			{
				if (sigType.IsOpenGenericParameter)
				{
					sb.AppendFormat("{0}, ", sigType.ToString());
				}
				else
				{
					var type = GetRuntimeTypeForSigType(sigType, typeModule);
					if (type != null)
						sb.AppendFormat("{0}, ", type.FullName);
					else
						sb.Append("<null>");
				}
			}

			sb.Length -= 2;
			sb.Append(">");

			return sb.ToString();
		}

		private static RuntimeType GetRuntimeTypeForSigType(SigType sigType, ITypeModule typeModule)
		{
			RuntimeType result = null;

			switch (sigType.Type)
			{
				case CilElementType.Class:
					Debug.Assert(sigType is TypeSigType, @"Failing to resolve VarSigType in GenericType.");
					result = typeModule.GetType((sigType as TypeSigType).Token);
					break;

				case CilElementType.ValueType:
					goto case CilElementType.Class;

				case CilElementType.GenericInst:
					result = typeModule.GetType((sigType as GenericInstSigType).BaseType.Token);
					break;

				case CilElementType.Var:
					throw new NotImplementedException(@"Failing to resolve VarSigType in GenericType.");

				case CilElementType.MVar:
					throw new NotImplementedException(@"Failing to resolve VarMSigType in GenericType.");

				case CilElementType.SZArray:

					// FIXME
					return null;

				default:
					var builtIn = sigType as BuiltInSigType;
					if (builtIn != null)
					{
						ITypeModule mscorlib;

						if (typeModule.Name == "mscorlib")
							mscorlib = typeModule;
						else
							mscorlib = typeModule.TypeSystem.ResolveModuleReference("mscorlib");

						result = mscorlib.GetType(builtIn.TypeName);
					}
					else
					{
						throw new NotImplementedException(String.Format("SigType of CilElementType.{0} is not supported.", sigType.Type));
					}
					break;
			}

			return result;
		}

		private void ResolveMethods()
		{
			foreach (CilRuntimeMethod method in baseGenericType.Methods)
			{
				SigType[] sigTypes = GenericSigTypeResolver.Resolve(method.SigParameters, genericArguments);
				SigType returnType = GenericSigTypeResolver.Resolve(method.ReturnType, genericArguments);

				var genericInstanceMethod = new CilRuntimeMethod(Module, method, returnType, method.HasThis, method.HasExplicitThis, sigTypes, this);

				Methods.Add(genericInstanceMethod);
			}
		}

		private void ResolveFields()
		{
			foreach (CilRuntimeField field in baseGenericType.Fields)
			{
				// Resolve variable type using generic arguments, if any
				SigType sigType = (!(field.SigType is VarSigType)) ? field.SigType : genericArguments[(field.SigType as VarSigType).Index];

				var genericInstanceField = new CilRuntimeField(Module, field, sigType, this);
				Fields.Add(genericInstanceField);
			}
		}

		private bool CheckContainsOpenGenericParameters()
		{
			foreach (var sig in genericArguments)
				if (sig.IsOpenGenericParameter)
					return true;

			return false;
		}

		public override bool ContainsOpenGenericParameters
		{
			get { return containsOpenGenericArguments; }
		}

		public void ResolveInterfaces(ITypeModule typeModule)
		{
			foreach (var type in baseGenericType.Interfaces)
			{
				if (!type.ContainsOpenGenericParameters)
				{
					Interfaces.Add(type);
				}
				else
				{
					var genericType = type as CilGenericType;
					Debug.Assert(genericType != null);

					RuntimeType matchedInterfaceType = null;

					// -- only needs to search generic type interfaces
					foreach (var runtimetype in typeModule.GetAllTypes())
					{
						if (runtimetype.IsInterface)
						{
							var runtimetypegeneric = runtimetype as CilGenericType;
							if (runtimetypegeneric != null)
							{
								if (genericType.baseGenericType == runtimetypegeneric.baseGenericType)
								{
									if (SigType.Equals(GenericArguments, runtimetypegeneric.GenericArguments))
									{
										matchedInterfaceType = runtimetype;

										//Interfaces.Add(runtimetype);
										break;
									}
								}
							}
						}
					}

					if (matchedInterfaceType != null)
						Interfaces.Add(matchedInterfaceType);
					else
						continue;
				}
			}
		}
	}
}