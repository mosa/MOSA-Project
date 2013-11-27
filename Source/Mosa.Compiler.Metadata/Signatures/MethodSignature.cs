/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	///
	/// </summary>
	public class MethodSignature : Signature
	{
		/// <summary>
		///
		/// </summary>
		//private CallingConvention callingConvention;
		private MethodCallingConvention methodCallingConvention;

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
		public MethodCallingConvention MethodCallingConvention
		{
			get { return methodCallingConvention; }
			protected set { methodCallingConvention = value; }
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
		/// <param name="reader">The reader.</param>
		public MethodSignature(SignatureReader reader)
			: base(reader)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodSignature"/> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="token">The token.</param>
		public MethodSignature(IMetadataProvider provider, HeapIndexToken token)
			: base(provider, token)
		{
		}

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
				methodCallingConvention = MethodCallingConvention.Generic;
				genericParameterCount = reader.ReadCompressedInt32();
			}
			else if (VARARG == (value & VARARG))
			{
				methodCallingConvention = MethodCallingConvention.VarArg;
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
	}
}