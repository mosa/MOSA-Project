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
using System.Diagnostics;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Metadata.Signatures
{
	/// <summary>
	/// 
	/// </summary>
	public class MethodSignature : Signature
	{
		/// <summary>
		/// 
		/// </summary>
		private CallingConvention callingConvention;

		/// <summary>
		/// 
		/// </summary>
		private int genericParameterCount;

		/// <summary>
		/// 
		/// </summary>
		private bool hasExplicitThis;

		/// <summary>
		/// 
		/// </summary>
		private bool hasThis;

		/// <summary>
		/// 
		/// </summary>
		private SigType[] parameters;

		/// <summary>
		/// 
		/// </summary>
		private SigType returnType;

		/// <summary>
		/// Gets the calling convention.
		/// </summary>
		/// <value>The calling convention.</value>
		public CallingConvention CallingConvention
		{
			get { return callingConvention; }
			protected set { callingConvention = value; }
		}

		/// <summary>
		/// Gets the generic parameter count.
		/// </summary>
		/// <value>The generic parameter count.</value>
		public int GenericParameterCount
		{
			get { return genericParameterCount; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance has explicit this.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has explicit this; otherwise, <c>false</c>.
		/// </value>
		public bool HasExplicitThis
		{
			get { return hasExplicitThis; }
			protected set { hasExplicitThis = value; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance has this.
		/// </summary>
		/// <value><c>true</c> if this instance has this; otherwise, <c>false</c>.</value>
		public bool HasThis
		{
			get { return hasThis; }
			protected set { hasThis = value; }
		}

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		/// <value>The parameters.</value>
		public SigType[] Parameters
		{
			get { return parameters; }
		}

		/// <summary>
		/// Gets the type of the return.
		/// </summary>
		/// <value>The type of the return.</value>
		public SigType ReturnType
		{
			get { return returnType; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodSignature"/> class.
		/// </summary>
		public MethodSignature()
		{
			//TODO: Remove this default constructor 
		}
		
		/// <summary>
		/// Loads the signature.
		/// </summary>
		/// <param name="reader">The reader.</param>
		public MethodSignature(SignatureReader reader)
			: base(reader)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VariableSignature"/> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		public MethodSignature(IMetadataProvider provider, TokenTypes token)
			: base(provider, token)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodSignature"/> class.
		/// </summary>
		/// <param name="returnType">Type of the return value.</param>
		/// <param name="parameters">The parameter types.</param>
		public MethodSignature(SigType returnType, SigType[] parameters)
		{
			if (returnType == null)
				throw new ArgumentNullException(@"returnType");
			if (parameters == null)
				throw new ArgumentNullException(@"parameters");

			this.callingConvention = CallingConvention.Default;
			this.hasExplicitThis = false;
			this.hasThis = false;
			this.parameters = parameters;
			this.returnType = returnType;
			this.genericParameterCount = 0;
		}

		//public MethodSignature(ISignatureContext context, MethodSignature signature, MethodSpecSignature specification)
		//{
		//    // NOT USED!!!
		//    if (context == null)
		//        throw new ArgumentNullException(@"context");
		//    if (signature == null)
		//        throw new ArgumentNullException(@"signature");
		//    if (specification == null)
		//        throw new ArgumentNullException(@"specification");

		//    this.callingConvention = signature.CallingConvention;
		//    this.hasExplicitThis = signature.HasExplicitThis;
		//    this.hasThis = signature.HasThis;
		//    this.genericParameterCount = 0;

		//    int length = signature.Parameters.Length;
		//    this.parameters = new SigType[length];
		//    for (int index = 0; index < length; index++)
		//    {
		//        this.parameters[index] = this.ApplySpecification(context, specification, signature.Parameters[index]);
		//    }
		//    this.returnType = this.ApplySpecification(context, specification, signature.ReturnType);
		//}

		//private SigType ApplySpecification(ISignatureContext context, MethodSpecSignature specification, SigType sigType)
		//{
		//    // NOT USED!!!
		//    SigType result = sigType;

		//    if (sigType is VarSigType)
		//    {
		//        result = context.GetGenericTypeArgument(((VarSigType)sigType).Index);
		//    }
		//    else if (sigType is MVarSigType)
		//    {
		//        result = specification.Types[((MVarSigType)sigType).Index];
		//    }

		//    Debug.WriteLine(String.Format(@"Replaced {0} by {1}.", sigType, result));
		//    return result;
		//}

		/// <summary>
		/// Parses the signature.
		/// </summary>
		/// <param name="reader">The reader.</param>
		protected sealed override void ParseSignature(SignatureReader reader)
		{
			byte value = reader.ReadByte();

			// Check for instance signature
			if (HAS_THIS == (value & HAS_THIS))
			{
				hasThis = true;
			}

			if (HAS_EXPLICIT_THIS == (value & HAS_EXPLICIT_THIS))
			{
				hasExplicitThis = true;
			}

			if (GENERIC == (value & GENERIC))
			{
				callingConvention = CallingConvention.Generic;
				genericParameterCount = reader.ReadCompressedInt32();
			}
			else if (VARARG == (value & VARARG))
			{
				callingConvention = CallingConvention.Vararg;
			}
			else if ((value & 0x1F) != 0x00)
			{
				throw new InvalidOperationException(@"Invalid method definition signature.");
			}

			// Number of parameters
			int paramCount = reader.ReadCompressedInt32();
			parameters = new SigType[paramCount];

			// Read the return type
			returnType = SigType.ParseTypeSignature(reader);

			// Read all parameters
			for (int i = 0; i < paramCount; i++)
				parameters[i] = SigType.ParseTypeSignature(reader);
		}

		/// <summary>
		/// 
		/// </summary>
		private const byte DEFAULT = 0x00;
		/// <summary>
		/// 
		/// </summary>
		private const byte VARARG = 0x05;
		/// <summary>
		/// 
		/// </summary>
		private const byte GENERIC = 0x10;
		/// <summary>
		/// 
		/// </summary>
		private const byte HAS_THIS = 0x20;
		/// <summary>
		/// 
		/// </summary>
		private const byte HAS_EXPLICIT_THIS = 0x40;

		/// <summary>
		/// Matcheses the specified other.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns></returns>
		public bool Matches(MethodSignature other)
		{
			if (object.ReferenceEquals(this, other))
				return true;

			// TODO: Check this to make sure it is correct
			if (other.GenericParameterCount != this.GenericParameterCount)
				return false;
			if (other.CallingConvention != this.CallingConvention)
				return false;
			if (other.HasThis != this.HasThis)
				return false;
			if (other.HasExplicitThis != this.HasExplicitThis)
				return false;
			if (this.Parameters.Length != other.Parameters.Length)
				return false;
			if (!this.ReturnType.Matches(other.ReturnType))
				return false;

			SigType[] thisParameters = this.Parameters;
			SigType[] otherParameters = other.Parameters;
			for (int i = 0; i < thisParameters.Length; i++)
			{
				if (!thisParameters[i].Matches(otherParameters[i]))
					return false;
			}

			return true;
		}
	}
}
