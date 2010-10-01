using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Metadata.Runtime
{

	public class CilGenericType : RuntimeType
	{
		private readonly GenericInstSigType signature;

		private readonly ISignatureContext signatureContext;

		private RuntimeType genericType;

		private SigType[] genericArguments;

		public CilGenericType(IModuleTypeSystem moduleTypeSystem, RuntimeType type, GenericInstSigType genericTypeInstanceSignature, ISignatureContext signatureContext) :
			base(moduleTypeSystem, type.Token)
		{
			this.signature = genericTypeInstanceSignature;
			this.signatureContext = signatureContext;

			this.Methods = this.GetMethods();
			this.Fields = this.GetFields();
		}

		public SigType[] GenericArguments
		{
			get
			{
				return this.genericArguments;
			}
		}

		protected override string GetName()
		{
			this.ProcessSignature();

			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0}<", this.genericType.Name);

			foreach (SigType genericArgument in this.genericArguments)
			{
				sb.AppendFormat("{0}, ", this.GetRuntimeTypeForSigType(genericArgument).FullName);
			}

			sb.Length -= 2;
			sb.Append(">");

			return sb.ToString();
		}

		protected override string GetNamespace()
		{
			this.ProcessSignature();
			return this.genericType.Namespace;
		}

		protected override RuntimeType GetBaseType()
		{
			ProcessSignature();

			return genericType.BaseType;
		}

		private IList<RuntimeMethod> GetMethods()
		{
			ProcessSignature();

			List<RuntimeMethod> methods = new List<RuntimeMethod>();
			foreach (CilRuntimeMethod method in this.genericType.Methods)
			{
				MethodSignature signature = new MethodSignature();
				signature.LoadSignature(this, method.MetadataModule.Metadata, method.Signature.Token);

				RuntimeMethod genericInstanceMethod = new CilGenericMethod(moduleTypeSystem, method, signature, this);
				methods.Add(genericInstanceMethod);
			}

			return methods;
		}

		private IList<RuntimeField> GetFields()
		{
			ProcessSignature();

			List<RuntimeField> fields = new List<RuntimeField>();
			foreach (CilRuntimeField field in this.genericType.Fields)
			{
				FieldSignature fsig = new FieldSignature();
				fsig.LoadSignature(this, this.genericType.MetadataModule.Metadata, field.Signature.Token);

				CilGenericField genericInstanceField = new CilGenericField(moduleTypeSystem, this, field, fsig);
				fields.Add(genericInstanceField);
			}

			return fields;
		}

		private void ProcessSignature()
		{
			if (this.genericType == null)
			{
				SigType[] signatureArguments = this.signature.GenericArguments;

				this.genericType = moduleTypeSystem.GetType(DefaultSignatureContext.Instance, this.signature.BaseType.Token);
				this.genericArguments = this.signature.GenericArguments;
			}
		}

		private RuntimeType GetRuntimeTypeForSigType(SigType sigType)
		{
			RuntimeType result = null;

			switch (sigType.Type)
			{
				case CilElementType.Class:
					Debug.Assert(sigType is TypeSigType, @"Failing to resolve VarSigType in GenericType.");
					result = moduleTypeSystem.GetType(this, ((TypeSigType)sigType).Token);
					break;

				case CilElementType.ValueType:
					goto case CilElementType.Class;

				case CilElementType.Var:
					sigType = this.GetGenericTypeArgument(((VarSigType)sigType).Index);
					goto case CilElementType.Class;

				case CilElementType.MVar:
					sigType = this.GetGenericTypeArgument(((MVarSigType)sigType).Index);
					goto case CilElementType.Class;

				default:
					{
						BuiltInSigType builtIn = sigType as BuiltInSigType;
						if (builtIn != null)
						{
							result = moduleTypeSystem.GetType(builtIn.TypeName + ", mscorlib");
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

		public override SigType GetGenericTypeArgument(int index)
		{
			this.ProcessSignature();
			return this.genericArguments[index];
		}

		protected override IList<RuntimeType> LoadInterfaces()
		{
			return NoInterfaces;
		}
	}
}
