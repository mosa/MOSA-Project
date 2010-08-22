
namespace Mosa.Runtime.Metadata.Runtime
{
	using System;

	using Mosa.Runtime.Metadata.Signatures;
	using Mosa.Runtime.Vm;
	using System.Diagnostics;

	internal class CilGenericMethod : RuntimeMethod
	{
		private readonly CilRuntimeMethod genericMethod;

		private readonly ISignatureContext signatureContext;

		public CilGenericMethod(CilRuntimeMethod method, MethodSignature signature, ISignatureContext signatureContext, ITypeSystem typeSystem) :
			base(method.Token, method.Module, method.DeclaringType, typeSystem)
		{
			this.genericMethod = method;
			this.signatureContext = signatureContext;

			this.Signature = signature;

			this.Attributes = method.Attributes;
			this.ImplAttributes = method.ImplAttributes;
			this.Rva = method.Rva;
			this.Parameters = method.Parameters;
		}

		protected override string GetName()
		{
			return this.genericMethod.Name;
		}

		protected override MethodSignature GetMethodSignature()
		{
			throw new NotImplementedException();
		}

		public override SigType GetGenericMethodArgument(int index)
		{
			SigType result = this.signatureContext.GetGenericMethodArgument(index);
			Debug.Assert(result != null, @"Failing to return a generic method argument.");
			return result;
		}

		public override SigType GetGenericTypeArgument(int index)
		{
			SigType result = this.signatureContext.GetGenericTypeArgument(index);
			if (result == null)
			{
				result = base.GetGenericTypeArgument(index);
			}

			Debug.Assert(result != null, @"Failing to return a generic type argument.");
			return result;
		}
	}
}
