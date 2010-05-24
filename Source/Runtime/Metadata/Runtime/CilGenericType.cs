
namespace Mosa.Runtime.Metadata.Runtime
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Text;
	
	using Mosa.Runtime.Loader;
	using Mosa.Runtime.Metadata.Signatures;
	using Mosa.Runtime.Vm;

	public class CilGenericType : RuntimeType
	{	
		private readonly GenericInstSigType signature;
		
		private readonly IMetadataModule signatureModule;
		
		private readonly ISignatureContext signatureContext;

		private RuntimeType genericType;
		
		private SigType[] genericArguments;
		
		public CilGenericType(RuntimeType type, IMetadataModule referencingModule, GenericInstSigType genericTypeInstanceSignature, ISignatureContext signatureContext) :
			base(type.Token, type.Module)
		{
			this.signature = genericTypeInstanceSignature;
			this.signatureContext = signatureContext;
			this.signatureModule = referencingModule;
			
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
			this.ProcessSignature();
			
			return this.genericType.BaseType;
		}
		
		private IEnumerable<RuntimeMethod> GetMethods()
		{
			this.ProcessSignature();
			
			List<RuntimeMethod> methods = new List<RuntimeMethod>();
			foreach (CilRuntimeMethod method in this.genericType.Methods)
			{
				MethodSignature signature = new MethodSignature();
				signature.LoadSignature(this, method.Module.Metadata, method.Signature.Token);
					
				RuntimeMethod genericInstanceMethod = new CilGenericMethod(method, signature, this);
				methods.Add(genericInstanceMethod);
			}
			
			return methods;
		}
		
		private IList<RuntimeField> GetFields()
		{
			this.ProcessSignature();
			
			List<RuntimeField> fields = new List<RuntimeField>();
			foreach (CilRuntimeField field in this.genericType.Fields)
			{
	            FieldSignature fsig = new FieldSignature();
    		        fsig.LoadSignature(this, this.genericType.Module.Metadata, field.Signature.Token);
				
				CilGenericField genericInstanceField = new CilGenericField(this, field, fsig);
				fields.Add(genericInstanceField);
			}
			
			return fields;
		}
		
		private void ProcessSignature()
		{
			if (this.genericType == null)
			{
				ITypeSystem typeSystem = RuntimeBase.Instance.TypeLoader;
				SigType[] signatureArguments = this.signature.GenericArguments;
				
				this.genericType = typeSystem.GetType(DefaultSignatureContext.Instance, this.signatureModule, this.signature.BaseType.Token);
				this.genericArguments = this.signature.GenericArguments;
			}
		}
		
		private RuntimeType GetRuntimeTypeForSigType(SigType sigType)
		{
			RuntimeType result = null;
			ITypeSystem typeSystem = RuntimeBase.Instance.TypeLoader;
			
			switch (sigType.Type)
			{
			case CilElementType.Class:
				Debug.Assert(sigType is TypeSigType, @"Failing to resolve VarSigType in GenericType.");
				result = typeSystem.GetType(this, this.signatureModule, ((TypeSigType)sigType).Token);
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
						result = typeSystem.GetType(builtIn.TypeName + ", mscorlib");
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
