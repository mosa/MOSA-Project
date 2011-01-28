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
		private readonly GenericInstSigType signature;

		private RuntimeType genericType;

		private SigType[] genericArguments;

		public CilGenericType(IMetadataProvider metadataProvider, RuntimeType genericType, GenericInstSigType genericTypeInstanceSignature) :
			base(genericType.Token, genericType.BaseType)
		{
			this.signature = genericTypeInstanceSignature;
			this.genericArguments = signature.GenericArguments;

			this.genericType = genericType;
			base.Attributes = genericType.Attributes;
			base.Namespace = genericType.Namespace;

			this.Methods = GetMethods(metadataProvider);
			this.Fields = GetFields(metadataProvider);

			//base.Name = GetName();
		}

		public SigType[] GenericArguments
		{
			get { return genericArguments; }
		}

		//private string GetName()
		//{
		//    StringBuilder sb = new StringBuilder();
		//    sb.AppendFormat("{0}<", Name);

		//    foreach (SigType sigType in genericArguments)
		//    {
		//        if (sigType.IsOpenGenericParameter)
		//        {
		//            sb.AppendFormat("<{0}>, ", sigType.ToString());
		//        }
		//        else
		//        {
		//            RuntimeType type = GetRuntimeTypeForSigType(sigType);
		//            if (type != null)
		//                sb.AppendFormat("{0}, ", type.FullName);
		//            else
		//                sb.Append("<null>");
		//        }
		//    }

		//    sb.Length -= 2;
		//    sb.Append(">");

		//    return sb.ToString();
		//}

		private IList<RuntimeMethod> GetMethods(IMetadataProvider metadataProvider)
		{
			List<RuntimeMethod> methods = new List<RuntimeMethod>();
			foreach (CilRuntimeMethod method in this.genericType.Methods)
			{
				MethodSignature signature = new MethodSignature(metadataProvider, method.Signature.Token);
				signature.ApplyGenericType(this.genericArguments);

				RuntimeMethod genericInstanceMethod = new CilGenericMethod(method, signature, this);
				methods.Add(genericInstanceMethod);
			}

			return methods;
		}

		private IList<RuntimeField> GetFields(IMetadataProvider metadataProvider)
		{
			List<RuntimeField> fields = new List<RuntimeField>();
			foreach (CilRuntimeField field in this.genericType.Fields)
			{
				FieldSignature signature = new FieldSignature(metadataProvider, field.Signature.Token);
				signature.ApplyGenericType(this.genericArguments);

				CilGenericField genericInstanceField = new CilGenericField(field, signature, this);
				fields.Add(genericInstanceField);
			}

			return fields;
		}

		//private RuntimeType GetRuntimeTypeForSigType(SigType sigType)
		//{
		//    RuntimeType result = null;

		//    switch (sigType.Type)
		//    {
		//        case CilElementType.Class:
		//            Debug.Assert(sigType is TypeSigType, @"Failing to resolve VarSigType in GenericType.");
		//            result = moduleTypeSystem.GetType(((TypeSigType)sigType).Token);
		//            break;

		//        case CilElementType.ValueType:
		//            goto case CilElementType.Class;

		//        case CilElementType.Var:
		//            throw new NotImplementedException(@"Failing to resolve VarSigType in GenericType.");

		//        case CilElementType.MVar:
		//            throw new NotImplementedException(@"Failing to resolve VarMSigType in GenericType.");

		//        case CilElementType.SZArray:
		//            {
		//                return null; // FIXME (rootnode)
		//            }

		//        default:
		//            {
		//                BuiltInSigType builtIn = sigType as BuiltInSigType;
		//                if (builtIn != null)
		//                {
		//                    result = moduleTypeSystem.TypeSystem.GetType(builtIn.TypeName + ", mscorlib");
		//                }
		//                else
		//                {
		//                    throw new NotImplementedException(String.Format("SigType of CilElementType.{0} is not supported.", sigType.Type));
		//                }
		//                break;
		//            }
		//    }

		//    return result;
		//}

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

