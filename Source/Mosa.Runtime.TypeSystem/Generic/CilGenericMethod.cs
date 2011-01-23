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

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.TypeSystem.Cil;

namespace Mosa.Runtime.TypeSystem.Generic
{

	internal class CilGenericMethod : RuntimeMethod
	{
		//private readonly CilRuntimeMethod genericMethod;

		public CilGenericMethod(CilRuntimeMethod genericMethod, MethodSignature signature, RuntimeType declaringType) :
			base(genericMethod.Token, declaringType)
		{
			//this.genericMethod = genericMethod;
			this.Signature = signature;
			this.Attributes = genericMethod.Attributes;
			this.ImplAttributes = genericMethod.ImplAttributes;
			this.Rva = genericMethod.Rva;
			this.Parameters = genericMethod.Parameters;
			base.Name = genericMethod.Name;
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
