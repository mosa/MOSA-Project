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

		public CilGenericType(RuntimeType baseGenericType, GenericInstSigType genericTypeInstanceSignature, ITypeModule typeModule) :
			base(baseGenericType.Token, baseGenericType.BaseType)
		{
			this.signature = genericTypeInstanceSignature;
			this.genericArguments = signature.GenericArguments;

			this.baseGenericType = baseGenericType;
			base.Attributes = baseGenericType.Attributes;
			base.Namespace = baseGenericType.Namespace;

			this.Methods = GetMethods();
			this.Fields = GetFields();

			base.Name = GetName(typeModule);
		}

		public SigType[] GenericArguments
		{
			get { return genericArguments; }
		}

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
			foreach (CilRuntimeMethod method in this.baseGenericType.Methods)
			{
				MethodSignature signature = new MethodSignature(method.Signature);
				signature.ApplyGenericType(this.genericArguments);

				RuntimeMethod genericInstanceMethod = new CilGenericMethod(method, signature, this);
				methods.Add(genericInstanceMethod);
			}

			return methods;
		}

		private IList<RuntimeField> GetFields()
		{
			List<RuntimeField> fields = new List<RuntimeField>();
			foreach (CilRuntimeField field in this.baseGenericType.Fields)
			{
				FieldSignature signature = new FieldSignature(field.Signature);
				signature.ApplyGenericType(this.genericArguments);

				CilGenericField genericInstanceField = new CilGenericField(field, signature, this);
				fields.Add(genericInstanceField);
			}

			return fields;
		}

		public override bool ContainsOpenGenericParameters
		{
			get { return signature.ContainsGenericParameters; }
		}

		//protected void LoadInterfaces()
		//{
		//    foreach (RuntimeType type in genericType.Interfaces)
		//    {
		//        if (!type.ContainsOpenGenericParameters)
		//        {
		//            base.Interfaces.Add(type);
		//        }
		//        else
		//        {
		//            RuntimeType baseGeneric = (type as CilGenericType).genericType;

		//            // find the enclosed type 
		//            // -- only needs to search generic type interfaces
		//            foreach (RuntimeType runtimetype in ModuleTypeSystem.GetAllTypes())
		//            {
		//                if (runtimetype.IsInterface)
		//                {
		//                    CilGenericType runtimetypegeneric = runtimetype as CilGenericType;
		//                    if (runtimetypegeneric != null)
		//                    {
		//                        if (baseGeneric == runtimetypegeneric.genericType)
		//                        {
		//                            if (SigType.Equals(signature.GenericArguments, runtimetypegeneric.signature.GenericArguments))
		//                            {
		//                                base.Interfaces.Add(runtimetype);
		//                                break;
		//                            }
		//                        }
		//                    }
		//                }
		//            }
		//        }
		//    }
		//}


	}
}

