/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
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
		private CallingConvention _callingConvention;

        /// <summary>
        /// 
        /// </summary>
		private int _genericParameterCount;

        /// <summary>
        /// 
        /// </summary>
		private bool _hasExplicitThis;

        /// <summary>
        /// 
        /// </summary>
		private bool _hasThis;

        /// <summary>
        /// 
        /// </summary>
		private SigType[] _parameters;

        /// <summary>
        /// 
        /// </summary>
		private SigType _returnType;

        /// <summary>
        /// Gets the calling convention.
        /// </summary>
        /// <value>The calling convention.</value>
		public CallingConvention CallingConvention
		{
			get { return _callingConvention; }
		}

        /// <summary>
        /// Gets the generic parameter count.
        /// </summary>
        /// <value>The generic parameter count.</value>
		public int GenericParameterCount
		{
			get { return _genericParameterCount; }
		}

        /// <summary>
        /// Gets a value indicating whether this instance has explicit this.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has explicit this; otherwise, <c>false</c>.
        /// </value>
		public bool HasExplicitThis
		{
			get { return _hasExplicitThis; }
		}

        /// <summary>
        /// Gets a value indicating whether this instance has this.
        /// </summary>
        /// <value><c>true</c> if this instance has this; otherwise, <c>false</c>.</value>
		public bool HasThis
		{
			get { return _hasThis; }
		}

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
		public SigType[] Parameters
		{
			get { return _parameters; }
		}

        /// <summary>
        /// Gets the type of the return.
        /// </summary>
        /// <value>The type of the return.</value>
		public SigType ReturnType
		{
			get { return _returnType; }
		}

        /// <summary>
        /// Holds the token.
        /// </summary>
        TokenTypes token;

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <value>The token.</value>
        public TokenTypes Token
        {
            get { return this.token; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodSignature"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public MethodSignature(TokenTypes token)
        {
            this.token = token;
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

            this.token = TokenTypes.Assembly;
            this._callingConvention = CallingConvention.Default;
            this._hasExplicitThis = this._hasThis = false;
            this._parameters = parameters;
            this._returnType = returnType;
            this._genericParameterCount = 0;
        }

        /// <summary>
        /// Parses the specified provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public static MethodSignature Parse(IMetadataProvider provider, TokenTypes token)
        {
            byte[] buffer;
            int index = 0;
            provider.Read(token, out buffer);
            MethodSignature msig = new MethodSignature(token);
            msig.ParseSignature(buffer, ref index);
            Debug.Assert(index == buffer.Length, @"Signature parser didn't complete.");
            return msig;
        }

        /// <summary>
        /// Parses the signature.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
		protected sealed override void ParseSignature(byte[] buffer, ref int index)
		{
            // Check for instance signature
            if (HAS_THIS == (buffer[index] & HAS_THIS))
            {
                _hasThis = true;
            }
            if (HAS_EXPLICIT_THIS == (buffer[index] & HAS_EXPLICIT_THIS))
                _hasExplicitThis = true;
            if (GENERIC == (buffer[index] & GENERIC))
            {
                _callingConvention = CallingConvention.Generic;
                _genericParameterCount = Utilities.ReadCompressedInt32(buffer, ref index);
            }
            else if (VARARG == (buffer[index] & VARARG))
            {
                _callingConvention = CallingConvention.Vararg;
            }
            else if (0x00 != (buffer[index] & 0x1F))
            {
                throw new InvalidOperationException(@"Invalid method definition signature.");
            }
			
			index++;

            // Number of parameters
            int paramCount = Utilities.ReadCompressedInt32(buffer, ref index);
            _parameters = new SigType[paramCount];

            // Read the return type
            _returnType = SigType.ParseTypeSignature(buffer, ref index);

            // Read all parameters
            for (int i = 0; i < paramCount; i++)
                _parameters[i] = SigType.ParseTypeSignature(buffer, ref index);
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
