using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.TypeSystem.Cil;

namespace Mosa.Runtime.TypeSystem.Generic
{

	public class CilGenericType : RuntimeType
	{
		private RuntimeType baseGenericType;

		private readonly GenericInstSigType signature;

		private SigType[] genericArguments;

		public CilGenericType(RuntimeType baseGenericType, GenericInstSigType genericTypeInstanceSignature, TokenTypes token, ITypeModule typeModule) :
			base(token, baseGenericType.BaseType)
		{
			this.signature = genericTypeInstanceSignature;
			this.genericArguments = signature.GenericArguments;

			this.baseGenericType = baseGenericType;
			base.Attributes = baseGenericType.Attributes;
			base.Namespace = baseGenericType.Namespace;

			base.Name = GetName(typeModule);
			this.Methods = GetMethods();
			this.Fields = GetFields();
			this.Interfaces = new List<RuntimeType>(); //GetInterfaces();
		}

		/// <summary>
		/// Gets the generic arguments.
		/// </summary>
		/// <value>The generic arguments.</value>
		public SigType[] GenericArguments
		{
			get { return genericArguments; }
		}

		//public RuntimeType BaseGenericType
		//{
		//    get { return baseGenericType; }
		//}

		private string GetName(ITypeModule typeModule)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0}<", baseGenericType.Name);

			foreach (SigType sigType in genericArguments)
			{
				if (sigType.IsOpenGenericParameter)
				{
					sb.AppendFormat("{0}, ", sigType.ToString());
				}
				else
				{
					RuntimeType type = GetRuntimeTypeForSigType(sigType, typeModule);
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

		private RuntimeType GetRuntimeTypeForSigType(SigType sigType, ITypeModule typeModule)
		{
			RuntimeType result = null;

			switch (sigType.Type)
			{
				case CilElementType.Class:
					Debug.Assert(sigType is TypeSigType, @"Failing to resolve VarSigType in GenericType.");
					result = typeModule.GetType(((TypeSigType)sigType).Token);
					break;

				case CilElementType.ValueType:
					goto case CilElementType.Class;

				case CilElementType.Var:
					throw new NotImplementedException(@"Failing to resolve VarSigType in GenericType.");

				case CilElementType.MVar:
					throw new NotImplementedException(@"Failing to resolve VarMSigType in GenericType.");

				case CilElementType.SZArray:
					{
						return null; // FIXME (rootnode)
					}

				default:
					{
						BuiltInSigType builtIn = sigType as BuiltInSigType;
						if (builtIn != null)
						{
							ITypeModule mscorlib = typeModule.TypeSystem.ResolveModuleReference("mscorlib");
							result = mscorlib.GetType(builtIn.TypeName);
						}
						else
						{
							throw new NotImplementedException(String.Format("SigType of CilElementType.{0} is not supported.", sigType.Type));
						}
						break;
					}
			}

			return result;
		}

		private IList<RuntimeMethod> GetMethods()
		{
			List<RuntimeMethod> methods = new List<RuntimeMethod>();
			foreach (CilRuntimeMethod method in baseGenericType.Methods)
			{
				MethodSignature signature = new MethodSignature(method.Signature, genericArguments);

				RuntimeMethod genericInstanceMethod = new CilGenericMethod(method, signature, this);
				methods.Add(genericInstanceMethod);
			}

			return methods;
		}

		private IList<RuntimeField> GetFields()
		{
			List<RuntimeField> fields = new List<RuntimeField>();
			foreach (CilRuntimeField field in baseGenericType.Fields)
			{
				FieldSignature signature = new FieldSignature(field.Signature, genericArguments);

				CilGenericField genericInstanceField = new CilGenericField(field, signature, this);
				fields.Add(genericInstanceField);
			}

			return fields;
		}

		public override bool ContainsOpenGenericParameters
		{
			get { return signature.ContainsGenericParameters; }
		}

		public void ResolveInterfaces(ITypeModule typeModule)
		{
			//IList<RuntimeType> interfaces = new List<RuntimeType>();

			foreach (RuntimeType type in baseGenericType.Interfaces)
			{
				if (!type.ContainsOpenGenericParameters)
				{
					Interfaces.Add(type);
				}
				else
				{
					CilGenericType genericType = type as CilGenericType;
					Debug.Assert(genericType != null);

					RuntimeType matchedInterfaceType = null;

					// -- only needs to search generic type interfaces
					foreach (RuntimeType runtimetype in typeModule.GetAllTypes())
					{
						if (runtimetype.IsInterface)
						{
							CilGenericType runtimetypegeneric = runtimetype as CilGenericType;
							if (runtimetypegeneric != null)
							{
								if (genericType.baseGenericType == runtimetypegeneric.baseGenericType)
								{
									if (SigType.Equals(signature.GenericArguments, runtimetypegeneric.signature.GenericArguments))
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

