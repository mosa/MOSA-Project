/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Loader;
using System.Diagnostics;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.Metadata.Runtime
{
	/// <summary>
	/// A CIL specialization of <see cref="RuntimeMethod"/>.
	/// </summary>
	sealed class CilRuntimeMethod : RuntimeMethod
	{
		#region Data Members

		/// <summary>
		/// The index of the method name.
		/// </summary>
		private TokenTypes nameIdx;

		/// <summary>
		/// Holds the blob location of the signature.
		/// </summary>
		private TokenTypes signatureBlobIdx;

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeMethod"/> class.
		/// </summary>
		/// <param name="moduleTypeSystem">The module type system.</param>
		/// <param name="token">The token.</param>
		/// <param name="method">The method.</param>
		/// <param name="maxParam">The max param.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		public CilRuntimeMethod(IModuleTypeSystem moduleTypeSystem, int token, MethodDefRow method, TokenTypes maxParam, RuntimeType declaringType) :
			base(moduleTypeSystem, (int)token, declaringType)
		{
			this.nameIdx = method.NameStringIdx;
			this.signatureBlobIdx = method.SignatureBlobIdx;
			base.Attributes = method.Flags;
			base.ImplAttributes = method.ImplFlags;
			base.Rva = method.Rva;

			if (method.ParamList < maxParam)
			{
				int count = maxParam - method.ParamList;
				int start = (int)(method.ParamList & TokenTypes.RowIndexMask) - 1;
				base.Parameters = new ReadOnlyRuntimeParameterListView((IModuleTypeSystemInternalList)moduleTypeSystem, start, count);
			}
			else
			{
				base.Parameters = ReadOnlyRuntimeParameterListView.Empty;
			}
		}

		#endregion // Construction

		#region RuntimeMethod Overrides

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public override bool Equals(RuntimeMethod other)
		{
			CilRuntimeMethod crm = other as CilRuntimeMethod;
			return (crm != null && this.nameIdx == crm.nameIdx && this.signatureBlobIdx == crm.signatureBlobIdx && base.Equals(other) == true);
		}

		/// <summary>
		/// Gets the method signature.
		/// </summary>
		/// <returns>The method signature.</returns>
		protected override MethodSignature GetMethodSignature()
		{
			return new MethodSignature(MetadataModule.Metadata, signatureBlobIdx);
		}

		/// <summary>
		/// Called to retrieve the name of the type.
		/// </summary>
		/// <returns>The name of the type.</returns>
		protected override string GetName()
		{
			string name = this.MetadataModule.Metadata.ReadString(this.nameIdx);
			Debug.Assert(name != null, @"Failed to retrieve CilRuntimeMethod name.");
			return name;
		}

		#endregion // RuntimeMethod Overrides
	}
}
