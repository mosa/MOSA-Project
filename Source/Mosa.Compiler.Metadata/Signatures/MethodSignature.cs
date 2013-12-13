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

namespace Mosa.Compiler.Metadata.Signatures
{
	/// <summary>
	///
	/// </summary>
	public class MethodSignature : Signature
	{
		#region Constants

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
		/// 
		/// </summary>
		private const byte C = 0x1;

		/// <summary>
		/// 
		/// </summary>			
		private const byte STDCALL = 0x2;

		/// <summary>
		/// 
		/// </summary>
		private const byte THISCALL = 0x3;

		/// <summary>
		/// 
		/// </summary>
		private const byte FASTCALL = 0x4;

		#endregion Constants

		/// <summary>
		/// Gets the calling convention.
		/// </summary>
		/// <value>The calling convention.</value>
		public MethodCallingConvention MethodCallingConvention { get; private set; }

		/// <summary>
		/// Gets the generic parameter count.
		/// </summary>
		/// <value>The generic parameter count.</value>
		public int GenericParameterCount { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this instance has explicit this.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has explicit this; otherwise, <c>false</c>.
		/// </value>
		public bool HasExplicitThis { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this instance has this.
		/// </summary>
		/// <value><c>true</c> if this instance has this; otherwise, <c>false</c>.</value>
		public bool HasThis { get; private set; }

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		/// <value>The parameters.</value>
		public SigType[] Parameters { get; private set; }

		/// <summary>
		/// Gets the type of the return.
		/// </summary>
		/// <value>The type of the return.</value>
		public SigType ReturnType { get; private set; }

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
				HasThis = true;
			}

			if (HAS_EXPLICIT_THIS == (value & HAS_EXPLICIT_THIS))
			{
				HasExplicitThis = true;
			}

			if (GENERIC == (value & GENERIC))
			{
				MethodCallingConvention = MethodCallingConvention.Generic;
				GenericParameterCount = reader.ReadCompressedInt32();
			}
			else if (VARARG == (value & VARARG))
			{
				MethodCallingConvention = MethodCallingConvention.VarArg;
			}
			else if (C == (value & C))
			{
			}
			else if (STDCALL == (value & STDCALL))
			{
			}
			else if (THISCALL == (value & THISCALL))
			{
			}
			else if (FASTCALL == (value & FASTCALL))
			{
			}
			else if ((value & 0x1F) != 0x00)
			{
				throw new InvalidOperationException(@"Invalid method definition signature.");
			}

			// Number of parameters
			int paramCount = reader.ReadCompressedInt32();
			Parameters = new SigType[paramCount];

			// Read the return type
			ReturnType = SigType.ParseTypeSignature(reader);

			// Read all parameters
			for (int i = 0; i < paramCount; i++)
			{
				Parameters[i] = SigType.ParseTypeSignature(reader);
			}
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(base.ToString() + " ");
			sb.Append("Has This/ThisExplicit: ");
			sb.Append(HasThis.ToString());
			sb.Append("/");
			sb.Append(HasExplicitThis.ToString());

			sb.Append(" RetType: ");
			sb.Append(ReturnType.ToString());

			if (Parameters.Length != 0)
			{
				sb.Append(" [ ");

				foreach (var param in Parameters)
				{
					sb.Append(param.ToString());
					sb.Append(", ");
				}

				sb.Length = sb.Length - 2;

				sb.Append(" ]");
			}

			return sb.ToString();
		}
	}
}