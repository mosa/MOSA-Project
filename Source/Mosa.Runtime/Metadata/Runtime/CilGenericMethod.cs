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

		public CilGenericMethod(IModuleTypeSystem moduleTypeSystem, CilRuntimeMethod method, MethodSignature signature, RuntimeType declaringType) :
			base(moduleTypeSystem, method.Token, declaringType)
		{
			this.genericMethod = method;

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

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			result.Append(DeclaringType.ToString());
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

	}
}
