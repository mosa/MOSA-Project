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
using System.Text;
using System.Diagnostics;

using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Metadata.Runtime
{

	internal class CilGenericMethod : RuntimeMethod
	{
		private readonly CilRuntimeMethod genericMethod;

		private readonly ISignatureContext signatureContext;

		public CilGenericMethod(IModuleTypeSystem moduleTypeSystem, CilRuntimeMethod method, MethodSignature signature, ISignatureContext signatureContext) :
			base(moduleTypeSystem, method.Token, method.DeclaringType)
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

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			// TODO: This seems like a bit of a hack
			if (signatureContext is CilGenericType)
			{
				result.Append(signatureContext.ToString());
			}
			else
			{
				result.Append(DeclaringType.ToString());
			}

			result.Append('.');
			result.Append(Name);
			result.Append('(');

			if (0 != this.Parameters.Count)
			{
				MethodSignature sig = this.Signature;
				int i = 0;
				foreach (RuntimeParameter p in this.Parameters)
				{
					result.AppendFormat("{0} {1},", sig.Parameters[i++].Type, p.Name);
				}
				result.Remove(result.Length - 1, 1);
			}

			result.Append(')');

			return result.ToString();
		}

		private RuntimeType GetRuntimeTypeForSigType(SigType sigType)
		{
			RuntimeType result = null;

			switch (sigType.Type)
			{
				case CilElementType.Class:
					Debug.Assert(sigType is TypeSigType, @"Failing to resolve VarSigType in GenericType.");
					result = moduleTypeSystem.GetType(((TypeSigType)sigType).Token);
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
							result = moduleTypeSystem.TypeSystem.GetType(builtIn.TypeName + ", mscorlib");
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

	}
}
